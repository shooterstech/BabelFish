from datetime import datetime
import traceback
from common.CommonFunctions import CommonFunctions
from athenadb.SQLDatabaseManager import SQLDatabaseManager
from common.CommonExceptions import *

common = CommonFunctions()
dbManager = SQLDatabaseManager(common)


"""
The calling parameters are 
- name
- owner_id
- visibility (optional, defaults to Private; options are "Public", "Protected", and "Private")
- show_on_search (optional, defaults to False)
- member_policy (optional, defaults to "INVITE"; options are "OPEN", "REQUEST", and "INVITE")

On call, 
Check that the caller is authorized to create a tournament for this owner_id (they must be an Admin or Member of the Orion Account associated with the owner_id)

Add an entry to match_base table as
- match_id: generated with NewTournamentId()   
- name: from the calling parameters
- owner_id: from the calling parameters
- visibility: from the calling parameters or defaulted to Private
- show_on_search: from the calling parameters (int(show_on_search)) or defaulted to 0

Add an entry to tournament table as
- tournament_id: same match_id previously generated
- member_policy: from the calling parameters or defaulted to "INVITE"
"""

def NewTournamentId(accountNumber, domain=1) :
	"""
	Returns a new MatchID for a new Tournament
	accountNumber is an int representing the license number for the EST Unit
	Use ESTLocal.LocalGetAccountNumber()
	"""

	#Only want two decimal places of the fractions of a second, normall %f would return 6 decimal places
	timeStamp = datetime.now().strftime("%Y%m%d%H%M%S%f")[:16]

	return "{}.{}.{}.2".format(domain, accountNumber, timeStamp)

def Authorize(license_number):
    """
    Check that the user is authorized to delete this tournament.
    They are authorized if they are the Admin or Member of the Orion Account.

    If the user is not authorized, throws a NotAuthorized exception.
    """
    allowedRoles = ["Admin", "Member"]
    if common.UserHasOrionAcctAuthorizationRole(license_number, allowedRoles):
        return True

    #shouldwe allow support team to delete tounraments?
    # if common.IsUserInGroup("Superusers") or common.IsUserInGroup("SupportTeam"):
    #     common.LogAlways("User {} is in Superusers or SupportTeam group, allowing tournament creation".format(common.GetUsername()))
    #     return True

    raise NotAuthorized("Caller does not have permission to create a tournament for the selected owner.")



#auth-api /tournament POST
def lambda_handler(event, context):
    try:
        common.Init(event)
        dbManager.UpdateConnection()

        name = common.GetArgumentStringV2("name", required=True) #Should we enforce unique tournament names per owner or just allow duplicates?
        ownerId = common.GetArgumentIntV2("owner-id", required=True)
        visibility = common.GetArgumentStringV2("visibility", defaultValue="Private")
        showOnSearch = common.GetArgumentBoolean("show-on-search")
        memberPolicy = common.GetArgumentStringV2("member-policy", defaultValue="INVITE")

        allowedVisibility = ["Public", "Protected", "Private"] #TODO use a string enum for this and member policy instead of hardcoding the options here and in the db schema
        if visibility not in allowedVisibility:
            raise InvalidOrMissingParameter(
                "Invalid visibility '{}'. Expected one of {}.".format(visibility, allowedVisibility)
            )

        allowedMemberPolicy = ["OPEN", "REQUEST", "INVITE"]
        if memberPolicy not in allowedMemberPolicy:
            raise InvalidOrMissingParameter(
                "Invalid member_policy '{}'. Expected one of {}.".format(memberPolicy, allowedMemberPolicy)
            )

        Authorize(ownerId)

        tournamentId = NewTournamentId(ownerId)

        createMatchBaseSql = """
        INSERT INTO match_base (match_id, name, owner_id, visibility, show_on_search)
        VALUES (%s, %s, %s, %s, %s)
        """
        dbManager.ModifyQuery(createMatchBaseSql, (tournamentId, name, ownerId, visibility, int(bool(showOnSearch))))

        createTournamentSql = """
        INSERT INTO tournament (tournament_id, member_policy)
        VALUES (%s, %s)
        """
        dbManager.ModifyQuery(createTournamentSql, (tournamentId, memberPolicy))

        response = { #return the created tournament object
            "Tournament": {
                "TournamentId": tournamentId,
                "TournamentName": name,
                "OwnerId": ownerId,
                "Visibility": visibility,
                "IncludeInSearchResults": bool(showOnSearch),
                "MemberPolicy": memberPolicy,
                "MergedResultLists": [],
                "Officials": [],
                "TournamentMembers": []
            }
        }
        common.AddBodyToResponse(response)
        return common.GetResponseBody()

    except InvalidOrMissingParameter as ex:
        common.SetStatusCode(400)
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
        # If we get here, something terrible has gone wrong
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
