import json
import traceback
from common.CommonFunctions import CommonFunctions
from athenadb.SQLDatabaseManager import SQLDatabaseManager
from common.CommonExceptions import *

common = CommonFunctions()
dbManager = SQLDatabaseManager(common)

ALLOWED_MATCH_ROLES = ["Match Director", "Technical Officer", "Registration", "Range Officer", "Stat Officer"]
ALLOWED_ORION_ROLES = ["Admin", "Member"]

"""
DeleteTournamentMember
/tournament/{tournament-id}/member [DELETE]

match-id will be passed in as a query parameter

Retrieve the tournament with 
SELECT tournament_id, name, owner_id, start_date, end_date, visibility, show_on_search, member_policy
        FROM tournament_expanded
        WHERE tournament_id = %s
If the tournament doesn't exist, throw a MatchNotFound exception


Authenticate that the user is authorized to delete this match from the tournament. They are authorized if they have permissions for the tournament OR the match, regardless of member policy. 
Throw a NotAuthorized exception if the user doesn't have permissions for either. (use existing IsAuthorizedForTournament and IsAuthorizedForMatch functions)

Delete enrty from tournament_member table as
DELETE from tournament_member where tournament_id = %s and match_id = %s
"""


def IsAuthorizedForTournament(tournament_id, owner_id):
    if common.UserHasOrionAcctAuthorizationRole(owner_id, ALLOWED_ORION_ROLES):
        return True
    if common.UserHasMatchAuthorizationRole(tournament_id, ALLOWED_MATCH_ROLES):
        return True
    return False


def IsAuthorizedForMatch(match_id, owner_id):
    if common.UserHasOrionAcctAuthorizationRole(owner_id, ALLOWED_ORION_ROLES):
        return True
    if common.UserHasMatchAuthorizationRole(match_id, ALLOWED_MATCH_ROLES):
        return True
    return False


# auth-api /tournament/{tournament-id}/member DELETE
def lambda_handler(event, context):
    try:
        common.Init(event)
        dbManager.UpdateConnection()

        if not common.IsAuthenticatedUser():
            raise NotAuthorized("Caller must be an authenticated user to delete a tournament member.")

        tournamentId = common.GetArgumentStringV2("tournament-id", required=True)
        matchId = common.GetArgumentStringV2("match-id", required=True)

        sqlTournamentLookup = """
        SELECT tournament_id, name, owner_id, start_date, end_date, visibility, show_on_search, member_policy
        FROM tournament_expanded
        WHERE tournament_id = %s
        """
        tournamentRows = dbManager.SelectQuery(sqlTournamentLookup, (tournamentId,))
        if len(tournamentRows) == 0:
            raise MatchNotFound(f"The Tournament identified with {tournamentId} could not be found")
        tournament = tournamentRows[0]

        sqlMatchLookup = """
        SELECT match_id, owner_id
        FROM match_base
        WHERE match_id = %s
        """
        matchRows = dbManager.SelectQuery(sqlMatchLookup, (matchId,))
        if len(matchRows) == 0:
            raise MatchNotFound(f"The Match identified with {matchId} could not be found")
        match = matchRows[0]

        tournamentAuthorized = IsAuthorizedForTournament(tournamentId, int(tournament["owner_id"]))
        matchAuthorized = IsAuthorizedForMatch(matchId, int(match["owner_id"]))
        if not tournamentAuthorized and not matchAuthorized:
            raise NotAuthorized("Caller does not have permission to delete this match from the selected tournament.")

        sqlExistingRelationship = """
        SELECT tournament_id
        FROM tournament_member
        WHERE tournament_id = %s AND match_id = %s
        """
        existingRelationship = dbManager.SelectQuery(sqlExistingRelationship, (tournamentId, matchId))
        if len(existingRelationship) == 0:
            raise NotFound(f"The match {matchId} is not a member of tournament {tournamentId}.")

        deleteSql = """
        DELETE FROM tournament_member
        WHERE tournament_id = %s AND match_id = %s
        """
        dbManager.ModifyQuery(deleteSql, (tournamentId, matchId))
        common.LogAlways(f"Removed match {matchId} from tournament {tournamentId}.")

        response = {
            "TournamentMember": {
                "TournamentId": tournamentId,
                "MatchId": matchId,
                "ApprovalStatus": "DELETED"
            }
        }
        common.AddMessage("The match has been removed from the tournament.")
        common.AddBodyToResponse(response)
        return common.GetResponseBody()

    except InvalidOrMissingParameter as ex:
        common.SetStatusCode(400)
        common.AddMessage(str(ex))
        return common.GetResponseBody()

    except InvalidRelationship as ex:
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
