import traceback
from common.CommonFunctions import CommonFunctions
from athenadb.SQLDatabaseManager import SQLDatabaseManager
from common.CommonExceptions import *

common = CommonFunctions()
dbManager = SQLDatabaseManager(common)


def Authorize(license_number):
    """
    Check that the user is authorized to delete this tournament.
    They are authorized if they are the Admin or Member of the Orion Account.

    If the user is not authorized, throws a NotAuthorized exception.
    """
    caller = common.GetUsername()

    allowedRoles = ["Admin", "Member"]
    if common.UserHasOrionAcctAuthorizationRole(license_number, allowedRoles):
        return True

    #should we allow support team to delete tounraments?
    # if common.IsUserInGroup("Superusers") or common.IsUserInGroup("SupportTeam") or common.IsUserInGroup("Development"):
    #     return True

    raise NotAuthorized("Caller does not have permission to delete the selected tournament.")

#subdomain AUTHAPI /tournament/{tournament-id} DELETE
def lambda_handler(event, context):
    try:
        common.Init(event)
        dbManager.UpdateConnection()

        if not common.IsAuthenticatedUser():
            raise NotAuthorized("Caller must be an authenticated user to create a tournament.")

        #should this be list to allow multiple deltes in one call?
        tournamentId = common.GetArgumentStringV2("tournament-id", required=True)

        sqlTournamentLookup = """
        SELECT owner_id
        FROM tournament_expanded
        WHERE tournament_id = %s
        """
        sqlTournamentResults = dbManager.SelectQuery(sqlTournamentLookup, (tournamentId,), formatted = False)
        if len(sqlTournamentResults) == 0:
            raise MatchNotFound(f"The Tournament identified with {tournamentId} could not be found")

        license_number = sqlTournamentResults[0][0]
        Authorize(license_number)

        deleteSql = """
        DELETE
        FROM match_base
        WHERE match_id = %s
        """
        dbManager.ModifyQuery(deleteSql, (tournamentId,))

        response = {
            "DeleteTournamentResponse":{
                "TournamentId": tournamentId,
                "LicenseNumber": license_number
            }
        }
        common.AddMessage("The tournament has been deleted")
        common.AddBodyToResponse(response)
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
