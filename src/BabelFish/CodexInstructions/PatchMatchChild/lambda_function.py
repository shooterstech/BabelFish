import traceback
from datetime import datetime, timezone
from common import CommonFunctions
from common.CommonExceptions import *
from boto3.dynamodb.conditions import Key
from botocore.exceptions import ClientError
from pydantic import ValidationError
from common.pydantic.MatchChild import MatchChild

common = CommonFunctions()
MATCH_ID_INDEX = "MATCH-MatchID-index"
MATCH_ID_KEY = "MATCH_MatchID"
CHILD_EDIT_FIELDS = {"Name", "Location", "Officials"}
PARENT_EDIT_FIELDS = {"ApprovalStatus"}
SYSTEM_MANAGED_FIELDS = {"LastUpdated"}

"""
Parameters:
- match-id (path param): the match_id of the parent match of the child match to update
- child-match-id (path param): the match_id of the child match to update
Request body is a json object in the format of the MatchChild model defined in common/pydantic/MatchChild.py (no computed fields)

Retrieve the match child via dynamodb by querying the MATCH-MatchID-Index index where MATCH_MatchID = child-match-id
If the match child does not exist, return a 404 error "No match child found with match_id {child-match-id}"
If the match child exists, check if the ParentID of the match child matches the match-id path param. 
    If it does not match, return a 404 error "No match child found with match_id {child-match-id} and parent_match_id {match-id}"

Check the permissions of the user for both the parent match ("match_parent.invite") and child match ("match.edit"). 

If the user has permissions for the child match, they are allowed to edit any of the following fields:
- Name
- Location
- Officials

If the user has permissions for the parent match, they are allowed to edit any of the following fields:
- ApprovalStatus

(If the user has both permissions, they can edit all of the above fields)

If the user tries to edit a field they do not have permissions for, add a warning message and ignore the change.
NOTE: Not throwing an error because some BabelFish defaults can be out of sync when location is empty, 
    so patching with the same created match child object would result in a failed request due to lack of permissions to edit Location. 

Modify the match child and save the updated match child to dynamodb. The LastUpdated field should be updated to the current timestamp in UTC.
"""


def GetMatchChild(dynamoTable, childMatchId):
    response = dynamoTable.query(
        IndexName=MATCH_ID_INDEX,
        KeyConditionExpression=Key(MATCH_ID_KEY).eq(childMatchId),
    )
    items = response.get("Items", [])
    if items:
        if len(items) > 1:
            common.LogAlways(
                "Warning: Multiple match children found with match_id {}. This should not happen. Returning the first match child.".format(
                    childMatchId
                )
            )
        return items[0]

    raise MatchNotFound(
        "No match child found with match_id {}".format(childMatchId)
    )


def ValidateRequestBody(body):
    if not isinstance(body, dict):
        raise InvalidOrMissingParameter(
            "Request body must be a JSON object in the MatchChild format."
        )

    try:
        matchChild = MatchChild.model_validate(body)
    except ValidationError as ex:
        raise InvalidOrMissingParameter(
            "Request body is not a valid MatchChild: {}".format(ex)
        )

    return matchChild.model_dump(mode="json")


def GetIncludedModelFields(body):
    includedFields = set()
    for fieldName, fieldInfo in MatchChild.model_fields.items():
        if fieldName in body or fieldInfo.alias in body:
            includedFields.add(fieldName)
    return includedFields


def _ComparableValue(value):
    if isinstance(value, datetime):
        return value.strftime(common.DATETIMEFORMAT)
    return value


def GetChangedFields(existingMatchChild, requestedMatchChild, includedFields):
    changedFields = []
    for fieldName in includedFields:
        if fieldName in SYSTEM_MANAGED_FIELDS:
            continue

        if _ComparableValue(existingMatchChild.get(fieldName)) != _ComparableValue(
            requestedMatchChild.get(fieldName)
        ):
            changedFields.append(fieldName)

    return sorted(changedFields)


def GetAuthorizedChangedFields(parentMatchId, childMatchId, changedFields):
    hasChildPermission, _ = common.UserHasPermissionsForMatch(
        childMatchId,
        ["match.edit"],
    )
    hasParentPermission, _ = common.UserHasPermissionsForMatch(
        parentMatchId,
        ["match_parent.invite"],
    )

    authorizedChangedFields = []
    warningMessages = []
    for fieldName in changedFields:
        hasFieldPermission = False
        if fieldName in CHILD_EDIT_FIELDS:
            hasFieldPermission = hasChildPermission
        elif fieldName in PARENT_EDIT_FIELDS:
            hasFieldPermission = hasParentPermission

        if hasFieldPermission:
            authorizedChangedFields.append(fieldName)
        else:
            warningMessages.append(
                "Warning: User does not have permissions to edit field {}. Ignoring requested change.".format(
                    fieldName
                )
            )

    return authorizedChangedFields, warningMessages


def UpdateMatchChild(dynamoTable, existingMatchChild, requestedMatchChild, changedFields):
    updateFields = sorted(set(changedFields) | {"LastUpdated"})
    requestedMatchChild["LastUpdated"] = datetime.now(timezone.utc).strftime(
        common.DATETIMEFORMAT
    )

    expressionNames = {}
    expressionValues = {}
    setExpressions = []
    for i, fieldName in enumerate(updateFields):
        nameKey = "#field{}".format(i)
        valueKey = ":value{}".format(i)
        expressionNames[nameKey] = fieldName
        expressionValues[valueKey] = requestedMatchChild[fieldName]
        setExpressions.append("{} = {}".format(nameKey, valueKey))

    try:
        response = dynamoTable.update_item(
            Key={
                "AccountNumber": existingMatchChild["AccountNumber"],
                "UniqueID": existingMatchChild["UniqueID"],
            },
            UpdateExpression="SET {}".format(", ".join(setExpressions)),
            ExpressionAttributeNames=expressionNames,
            ExpressionAttributeValues=expressionValues,
            ConditionExpression="attribute_exists(AccountNumber) AND attribute_exists(UniqueID)",
            ReturnValues="ALL_NEW",
        )
    except ClientError as ex:
        if ex.response["Error"]["Code"] == "ConditionalCheckFailedException":
            raise MatchNotFound(
                "No match child found with match_id {}, this should never happen.".format(
                    existingMatchChild.get("MatchID")
                )
            )
        raise

    return response["Attributes"]


#PATCH: /match/{match-id}/children/{child-match-id}
def lambda_handler(event, context):
    try:
        common.Init(event, loadUserInformation=False)

        if not common.IsAuthenticatedUser():
            raise NotAuthorized(
                "Caller must be an authenticated user to update a child match."
            )

        parentMatchId = common.GetArgumentStringV2("match-id", required=True)
        childMatchId = common.GetArgumentStringV2("child-match-id", required=True)
        body = common.GetBody()
        requestedMatchChild = ValidateRequestBody(body)
        includedFields = GetIncludedModelFields(body)

        dynamoTable = common.GetDBOrion()
        existingMatchChild = GetMatchChild(dynamoTable, childMatchId)

        if existingMatchChild.get("ParentID") != parentMatchId:
            raise MatchNotFound(
                "No match child found with match_id {} and parent_match_id {}".format(
                    childMatchId,
                    parentMatchId,
                )
            )

        changedFields = GetChangedFields(
            existingMatchChild,
            requestedMatchChild,
            includedFields,
        )
        authorizedChangedFields, warningMessages = GetAuthorizedChangedFields(
            parentMatchId,
            childMatchId,
            changedFields,
        )
        for warningMessage in warningMessages:
            common.AddMessage(warningMessage)

        updatedMatchChild = UpdateMatchChild(
            dynamoTable,
            existingMatchChild,
            requestedMatchChild,
            authorizedChangedFields,
        )
        common.LogOnError(
            "Updated child match {} for parent match {}. Changed fields: {}.".format(
                childMatchId,
                parentMatchId,
                ", ".join(authorizedChangedFields)
                if authorizedChangedFields
                else "LastUpdated",
            )
        )

        common.AddBodyToResponse({"MatchChild": updatedMatchChild})
        return common.GetResponseBody()

    except InvalidOrMissingParameter as ex:
        common.SetStatusCode(400)
        common.AddMessage(str(ex))
        return common.GetResponseBody()

    except MatchNotFound as ex:
        common.SetStatusCode(404)
        common.AddMessage(str(ex))
        return common.GetResponseBody()

    except NotAuthorized as ex:
        common.SetStatusCode(401)
        common.AddMessage(str(ex))
        return common.GetResponseBody()

    except OrionException as ex:
        common.SetStatusCode(400)
        common.AddMessage(str(ex))
        return common.GetResponseBody()

    except Exception as ex:
        try:
            common.LogAlways("Danger, Will Robinson!")
            common.Error(ex)
            common.Error(traceback.format_exc())
            common.SetStatusCode(500)
            common.AddMessage("Internal Server Error")
            return common.GetResponseBody()
        except Exception as ex:
            print(ex)
            print(traceback.format_exc())
            response = {
                "statusCode": 500,
                "headers": {},
                "body": "{\n    \"Title\": \"\",\n    \"Message\": [\n        \"Internal Server Error\"\n    ],\n    \"ResponseCodes\": [\n        \"ServerError\"\n    ]\n}",
            }
            return response
