import json
import traceback
from common.CommonFunctions import CommonFunctions
from athenadb.SQLDatabaseManager import SQLDatabaseManager
from common.CommonExceptions import *

common = CommonFunctions()
dbManager = SQLDatabaseManager(common)

VALID_MEMBER_APPROVAL_STATUS = {"APPROVED", "PENDING", "REJECTED"}

"""
PatchTournamentMember Lambda Function
/tournament/{tournament-id}/member [PATCH]

params:
- tournament-id (path)
- match-id (string)
- approval-status (string): The new approval status for the tournament member, either "APPROVED", "PENDING", or "REJECTED".

retrieve the tournament and match information using something like
SELECT t.tournament_id, t.name, t.owner_id, t.start_date, t.end_date, t.visibility, t.show_on_search, t.member_policy, tm.match_id, tm.approval_status
        FROM tournament_expanded t
        LEFT JOIN tournament_member tm ON tm.tournament_id = t.tournament_id
        WHERE t.tournament_id = %s
        AND tm.match_id = %s

If the tournament exists but the match is not a member of the tournament, throw a MatchNotFound exception.
If the tournament doesn't exist, throw a TournamentNotFound exception. 

Look at the tournament's member policy to check authorization; if the member policy is:
- INVITE (must be accepted on the match side): The calling user must have tournament.join permissions for the match id of the member
- REQUEST (must be accepted on the tournament side): The calling user must have tournament.add_member permissions for the tournament id 
- OPEN: raise InvalidMatch since match members are automatically APPROVED so this call should not be necessary
throw NotAuthorized if the calling user does not have permissions

update the entry in tournament_member setting approval_status to approval-status

return the updated tournament member object (see other lambdas in tournaments subdir)
"""



def ValidateApprovalStatus(status_input):
    status = (status_input or "").strip().upper()
    if status not in VALID_MEMBER_APPROVAL_STATUS:
        raise InvalidOrMissingParameter(
            "Invalid approval-status '{}'. Expected one of {}.".format(
                status_input,
                sorted(list(VALID_MEMBER_APPROVAL_STATUS)),
            )
        )
    return status


# auth-api /tournament/{tournament-id}/member PATCH
def lambda_handler(event, context):
    try:
        common.Init(event)
        dbManager.UpdateConnection()

        if not common.IsAuthenticatedUser():
            raise NotAuthorized(
                "Caller must be an authenticated user to patch a tournament member."
            )

        tournamentId = common.GetArgumentStringV2("tournament-id", required=True)
        matchId = common.GetArgumentStringV2("match-id", required=True)
        approvalStatus = ValidateApprovalStatus(
            common.GetArgumentStringV2("approval-status", required=True)
        )

        sqlTournamentAndMemberLookup = """
        SELECT
            t.tournament_id,
            t.name,
            t.owner_id,
            t.start_date,
            t.end_date,
            t.visibility,
            t.show_on_search,
            t.member_policy,
            tm.match_id,
            tm.approval_status
        FROM tournament_expanded t
        LEFT JOIN tournament_member tm
            ON tm.tournament_id = t.tournament_id
            AND tm.match_id = %s
        WHERE t.tournament_id = %s
        """
        tournamentMemberRows = dbManager.SelectQuery(
            sqlTournamentAndMemberLookup, (matchId, tournamentId)
        )
        if len(tournamentMemberRows) == 0:
            raise TournamentNotFound(
                f"The Tournament identified with {tournamentId} could not be found."
            )

        tournamentMember = tournamentMemberRows[0]
        if not tournamentMember.get("match_id"):
            raise MatchNotFound(
                f"The Match identified with {matchId} is not a member of tournament {tournamentId}."
            )

        memberPolicy = str(tournamentMember.get("member_policy", "INVITE")).upper()
        if memberPolicy == "INVITE": # this is an invite that needs to be approved by the match people
            hasPermissions, _ = common.UserHasPermissionsForMatch(
                matchId, ["tournament.join"], throw=True
            )
            
        elif memberPolicy == "REQUEST": # this is a request to join that needs to be approved by the tournament people
            hasPermissions, _ = common.UserHasPermissionsForMatch(
                tournamentId, ["tournament.add_member"], throw=True
            )
            
            
        elif memberPolicy == "OPEN":
            raise InvalidMatch(
                "The selected tournament has OPEN membership. Members are auto-approved and do not require approval-status updates."
            )
        else:
            raise InvalidOrMissingParameter(
                "Invalid member_policy '{}'. Expected one of ['OPEN', 'REQUEST', 'INVITE'].".format(
                    memberPolicy
                )
            )

        updateMemberSql = """
        UPDATE tournament_member
        SET approval_status = %s
        WHERE tournament_id = %s
            AND match_id = %s
        """
        dbManager.ModifyQuery(updateMemberSql, (approvalStatus, tournamentId, matchId))
        common.LogAlways(
            "Updated tournament member: tournament {} match {} approval_status {}.".format(
                tournamentId, matchId, approvalStatus
            )
        )

        response = {
            "TournamentMember": {
                "TournamentId": tournamentId,
                "MatchId": matchId,
                "ApprovalStatus": approvalStatus,
            }
        }
        common.AddBodyToResponse(response)
        return common.GetResponseBody()

    except InvalidOrMissingParameter as ex:
        common.SetStatusCode(400)
        common.AddMessage(str(ex))
        return common.GetResponseBody()

    except InvalidMatch as ex:
        common.SetStatusCode(400)
        common.AddMessage(str(ex))
        return common.GetResponseBody()

    except TournamentNotFound as ex:
        common.SetStatusCode(404)
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


