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
AddTournamentMember
/tournament/{tournament-id}/member [POST]

Retrieve the tournament with 
SELECT tournament_id, name, owner_id, start_date, end_date, visibility, show_on_search, member_policy
        FROM tournament_expanded
        WHERE tournament_id = %s
If the tournament doesn't exist, throw a MatchNotFound exception

Retrieve the match with
SELECT match_id, name, owner_id, show_on_search
FROM match_base
WHERE match_id = %s AND visibility = "Public"
If the match doesn't exist, throw a MatchNotFound exception

Authenticate that the user is authorized to add this match to the tournament based on the member_policy:
- INVTE: only Admins or Members of the Orion Account or match authorization roles of Match Director, Technical Officer, Registration, Range Officer, or Stat Officer for the tournament (NOT the match being added)
- REQUEST: only Admins or Members of the Orion Account associated with the added match or match authorization roles of Match Director, Technical Officer, Registration, Range Officer, or Stat Officer for match being added

If the user has authorization for both the tournament and the match, add the match to the tournament with "APPROVED" status. If the user only has authorization for one of them, add the match to the tournament with "PENDING" status. If the user doesn't have authorization for either, throw a NotAuthorized exception.

Add an entry to the tournament_member table as
- tournament_id: from the calling parameters
- match_id: from calling params ( the match_id to add to the tournament)
- approval_status: described above, either "APPROVED" or "PENDING"

return the tournament object with the new match included in the list of matches. The tournament object should be in the same format as the GetTournament endpoint, with an additional field for each match in the tournament indicating whether that match is "APPROVED" or "PENDING" in the tournament.
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




# auth-api /tournament/{tournament-id}/member POST
def lambda_handler(event, context):
    try:
        common.Init(event)
        dbManager.UpdateConnection()

        if not common.IsAuthenticatedUser():
            raise NotAuthorized("Caller must be an authenticated user to add a tournament member.")

        tournamentId = common.GetArgumentStringV2("tournament-id", required=True)
        matchId = common.GetArgumentStringV2("match-id", required=True)

        if tournamentId == matchId:
            raise InvalidOrMissingParameter("A tournament may not be added as its own member.")

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
        SELECT match_id, name, owner_id, show_on_search
        FROM match_base
        WHERE match_id = %s AND visibility = "Public"
        """
        matchRows = dbManager.SelectQuery(sqlMatchLookup, (matchId,))
        if len(matchRows) == 0:
            raise MatchNotFound(f"The Match identified with {matchId} could not be found")
        match = matchRows[0]

        tournamentAuthorized = IsAuthorizedForTournament(tournamentId, int(tournament["owner_id"]))
        matchAuthorized = IsAuthorizedForMatch(matchId, int(match["owner_id"]))
        memberPolicy = str(tournament.get("member_policy", "INVITE")).upper()

        if memberPolicy == "INVITE":
            if not tournamentAuthorized:
                raise NotAuthorized("Caller does not have permission to invite this match to the selected tournament.")
        elif memberPolicy == "REQUEST":
            if not matchAuthorized:
                raise NotAuthorized("Caller does not have permission to request entry into the selected tournament.")
        elif memberPolicy == "OPEN": #Not used currently 
            if not tournamentAuthorized and not matchAuthorized:
                raise NotAuthorized("Caller does not have permission to add this match to the selected tournament.")
        else:
            raise InvalidOrMissingParameter(
                "Invalid member_policy '{}'. Expected one of ['OPEN', 'REQUEST', 'INVITE'].".format(memberPolicy)
            )

        if tournamentAuthorized and matchAuthorized:
            approvalStatus = "APPROVED"
        elif tournamentAuthorized or matchAuthorized:
            approvalStatus = "PENDING"
        else:
            raise NotAuthorized("Caller does not have permission to add this match to the selected tournament.")

        checkExistingSql = """
        SELECT tournament_id
        FROM tournament_member
        WHERE tournament_id = %s AND match_id = %s
        """
        existing = dbManager.SelectQuery(checkExistingSql, (tournamentId, matchId))
        if len(existing) > 0:
            raise InvalidRelationship(f"The match {matchId} is already a member of tournament {tournamentId}.")

        insertSql = """
        INSERT INTO tournament_member (tournament_id, match_id, approval_status)
        VALUES (%s, %s, %s)
        """
        dbManager.ModifyQuery(insertSql, (tournamentId, matchId, approvalStatus))
        common.LogAlways(
            f"Added match {matchId} to tournament {tournamentId} with approval status {approvalStatus}."
        )

        response = {
            "TournamentMember": {
                "TournamentId": tournamentId,
                "MatchId": matchId,
                "ApprovalStatus": approvalStatus
            }
        }
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
