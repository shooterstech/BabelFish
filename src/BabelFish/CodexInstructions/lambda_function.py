import traceback
from common.CommonFunctions import CommonFunctions
from athenadb.SQLDatabaseManager import SQLDatabaseManager
from common.ContinuationTokenManager import ContinuationTokenManager
from common.ContinuationToken import ContinuationTokenException
from common.CommonExceptions import *

common = CommonFunctions()
dbManager = SQLDatabaseManager(common)
continuationManager = ContinuationTokenManager(common)

MAX_LIMIT = 100
DEFAULT_LIMIT = 10
VALID_VISIBILITIES = {"public": "Public", "protected": "Protected", "private": "Private"}
VALID_MEMBER_POLICIES = {"OPEN", "INVITE", "REQUEST"}
VALID_MEMBER_APPROVAL_STATUS = {"APPROVED", "PENDING", "REJECTED"}
VALID_SEARCH_PRESETS = {"incoming-invites", "outgoing-requests"}

"""
/tournament [GET]
TournamentSearch
parameters:
- limit (optional, default 10): number of tournaments to return
- token (optional): continuation token to retrieve next set of results
- name (optional): filter tournaments by name (partial match)
- owner-id (optional str): filter tournaments by owner id
- license-number (optional str): filter tournaments by license number of the organizer
- visibility (optional str, default Public): filter tournaments by visibility (options are Public, Private, Protected). If the visibility is not Public, the calling user must have the "tournament.read" permission for the owner-id of the tournament to see it in search results.
- member-policy (optional str): filter tournaments by member policy (options are OPEN, INVITE, REQUEST) (note this is NOT a member-has parameter)
- member-has-owner-id (optional string): filter tournaments that contain a match with this owner id as a member
- member-has-match-id (optional string): filter tournaments that contain a match with this match id as a member
- member-has-approval-status (optional string): filter tournaments that contain a match with this approval status as a member. If combined with member-has-owner-id, only matches with the specified owner id AND approval status will be considered (options are APPROVED, PENDING, REJECTED)
- search-preset (optional string): filter tournaments by a preset group of conditions. If included, override the specified parameters Options are:
    - "incoming-invites": Requires member-has-owner-id to be set. Filter by member-policy=INVITE and member-has-approval-status=PENDING for matches with the specified owner id
    - "outgoing-requests": Requires member-has-owner-id to be set. Filter by member-policy=REQUEST and member-has-approval-status=PENDING for matches with the specified owner id 

Ensure to not return tournaments with visibility not Public unless the calling user has tournament.read permissions for the owner-id of the tournament. 
    
See other lambda functions in tournaments subdir for the SQL table structure. 

Note that each member-has parameter should be an AND of a single match member. For example, if member-has-owner-id and member-has-approval-status are both provided, only matches that satisfy both conditions will be considered when filtering tournaments.

Use ContinuationTokenManager to manage continuation tokens for paginating results. See other lambda functions that have used ContinuationTokenManager for examples of how to use it.
"""


def ValidateLimit(limitInput):
    if limitInput < 1:
        return DEFAULT_LIMIT

    if limitInput > MAX_LIMIT:
        return MAX_LIMIT

    return limitInput


def ValidateVisibility(visibilityInput):
    if not visibilityInput:
        return "Public"

    normalized = visibilityInput.strip().lower()
    if normalized not in VALID_VISIBILITIES:
        raise InvalidOrMissingParameter(
            "Invalid visibility '{}'. Expected one of {}.".format(
                visibilityInput,
                list(VALID_VISIBILITIES.values()),
            )
        )

    return VALID_VISIBILITIES[normalized]


def ValidateMemberPolicy(memberPolicyInput):
    if not memberPolicyInput:
        return ""

    memberPolicy = memberPolicyInput.strip().upper()
    if memberPolicy not in VALID_MEMBER_POLICIES:
        raise InvalidOrMissingParameter(
            "Invalid member-policy '{}'. Expected one of {}.".format(
                memberPolicyInput,
                sorted(list(VALID_MEMBER_POLICIES)),
            )
        )

    return memberPolicy


def ValidateMemberApprovalStatus(statusInput):
    if not statusInput:
        return ""

    status = statusInput.strip().upper()
    if status not in VALID_MEMBER_APPROVAL_STATUS:
        raise InvalidOrMissingParameter(
            "Invalid member-has-approval-status '{}'. Expected one of {}.".format(
                statusInput,
                sorted(list(VALID_MEMBER_APPROVAL_STATUS)),
            )
        )

    return status


def ValidateSearchPreset(searchPreset):
    if not searchPreset:
        return ""

    normalized = searchPreset.strip().lower()
    if normalized not in VALID_SEARCH_PRESETS:
        raise InvalidOrMissingParameter(
            "Invalid search-preset '{}'. Expected one of {}.".format(
                searchPreset,
                sorted(list(VALID_SEARCH_PRESETS)),
            )
        )

    return normalized




def GetLicenseNumberFromOwnerId(ownerId):
    if not ownerId or not ownerId.startswith("OrionAcct"):
        return None

    tail = ownerId.replace("OrionAcct", "", 1)
    if not tail.isdigit():
        return None

    return int(tail)


def ApplySearchPreset(searchPreset, memberHasOwnerId, memberPolicy, memberHasApprovalStatus):
    if not searchPreset:
        return memberPolicy, memberHasApprovalStatus

    if not memberHasOwnerId:
        raise InvalidOrMissingParameter(
            "search-preset '{}' requires member-has-owner-id to be included.".format(
                searchPreset
            )
        )

    if searchPreset == "incoming-invites":
        return "INVITE", "PENDING"

    if searchPreset == "outgoing-requests":
        return "REQUEST", "PENDING"

    return memberPolicy, memberHasApprovalStatus


def BuildSearchQuery(
    name,
    ownerId,
    licenseNumber,
    visibility,
    showOnSearch,
    memberPolicy,
    memberHasOwnerId,
    memberHasMatchId,
    memberHasApprovalStatus,
):
    sqlValues = []
    whereClauses = ["te.visibility = %s"]
    sqlValues.append(visibility)

    if showOnSearch is not None:
        whereClauses.append("te.show_on_search = %s")
        sqlValues.append(int(bool(showOnSearch)))

    if name:
        whereClauses.append("te.name LIKE %s")
        sqlValues.append("%{}%".format(name))

    if ownerId:
        whereClauses.append("te.owner_id = %s")
        sqlValues.append(ownerId)

    if licenseNumber is not None and licenseNumber > 0:
        whereClauses.append("te.license_number = %s")
        sqlValues.append(licenseNumber)

    if memberPolicy:
        whereClauses.append("te.member_policy = %s")
        sqlValues.append(memberPolicy)

    hasMemberFilters = bool(memberHasOwnerId or memberHasMatchId or memberHasApprovalStatus)
    if hasMemberFilters:
        memberWhereClauses = ["tm.tournament_id = te.tournament_id"]
        joinClause = ""
        if memberHasOwnerId:
            joinClause = "JOIN match_base mm ON tm.match_id = mm.match_id"
            memberWhereClauses.append("mm.owner_id = %s")
            sqlValues.append(memberHasOwnerId)

        if memberHasMatchId:
            memberWhereClauses.append("tm.match_id = %s")
            sqlValues.append(memberHasMatchId)

        if memberHasApprovalStatus:
            memberWhereClauses.append("tm.approval_status = %s")
            sqlValues.append(memberHasApprovalStatus)

        whereClauses.append(
            """
            EXISTS (
                SELECT 1
                FROM tournament_member tm
                {}
                WHERE {}
            )
            """.format(joinClause, " AND ".join(memberWhereClauses))
        )

    sql = """
    SELECT
        te.tournament_id,
        te.name,
        te.owner_id,
        te.license_number,
        te.start_date,
        te.end_date,
        te.visibility,
        te.show_on_search,
        te.member_policy
    FROM tournament_expanded te
    WHERE {}
    ORDER BY te.start_date DESC, te.name
    """.format(" AND ".join(whereClauses))

    return sql, sqlValues


def GetRelevantTournamentMembers(
    tournamentIds,
    memberHasOwnerId,
    memberHasMatchId,
    memberHasApprovalStatus,
):
    if not tournamentIds:
        return {}

    placeholders = ", ".join(["%s"] * len(tournamentIds))
    whereClauses = ["tm.tournament_id IN ({})".format(placeholders)]
    values = list(tournamentIds)

    if memberHasOwnerId:
        whereClauses.append("mb.owner_id = %s")
        values.append(memberHasOwnerId)

    if memberHasMatchId:
        whereClauses.append("tm.match_id = %s")
        values.append(memberHasMatchId)

    if memberHasApprovalStatus:
        whereClauses.append("tm.approval_status = %s")
        values.append(memberHasApprovalStatus)

    sql = """
    SELECT
        tm.tournament_id,
        tm.match_id,
        mb.name AS match_name,
        tm.approval_status
    FROM tournament_member tm
    LEFT JOIN match_base mb ON tm.match_id = mb.match_id
    WHERE {}
    ORDER BY tm.tournament_id, mb.name, tm.match_id
    """.format(" AND ".join(whereClauses))

    rows = dbManager.SelectQuery(sql, values)
    membersByTournament = {}
    for row in rows:
        tournamentId = row["tournament_id"]
        if tournamentId not in membersByTournament:
            membersByTournament[tournamentId] = []

        membersByTournament[tournamentId].append(
            {
                "MatchId": row["match_id"],
                "MatchName": row["match_name"],
                "ApprovalStatus": row["approval_status"],
            }
        )

    return membersByTournament


def UserCanReadTournament(tournamentRow, ownerReadPermissionCache):
    visibility = tournamentRow.get("visibility", "Public")
    if visibility == "Public":
        return True

    licenseNumber = tournamentRow.get("license_number")
    if licenseNumber is None or licenseNumber == 0:
        licenseNumber = GetLicenseNumberFromOwnerId(tournamentRow.get("owner_id")) #currently only club users can own/create tournaments

    if licenseNumber is None:
        return False

    licenseNumber = int(licenseNumber)
    if licenseNumber not in ownerReadPermissionCache:
        hasPermissions, _ = common.UserHasPermissionsForOrionAcct(
            licenseNumber, ["tournament.read"]
        )
        ownerReadPermissionCache[licenseNumber] = hasPermissions

    return ownerReadPermissionCache[licenseNumber]


def FormatTournament(tournamentRow, tournamentMembers=None):
    formattedTournament = {
        "MatchType": "Tournament",
        "MatchId": tournamentRow["tournament_id"],
        "TournamentId": tournamentRow["tournament_id"],
        "MatchName": tournamentRow["name"],
        "TournamentName": tournamentRow["name"],
        "OwnerId": tournamentRow["owner_id"],
        "StartDate": str(tournamentRow["start_date"]),
        "EndDate": str(tournamentRow["end_date"]),
        "Visibility": tournamentRow["visibility"],
        "IncludeInSearchResults": bool(tournamentRow["show_on_search"]),
        "MemberPolicy": tournamentRow["member_policy"],
        "MergedResultLists": [],
        "Officials": [],
        "TournamentMembers": [],
        "Abbreviated": True
    }
    if tournamentMembers is not None:
        formattedTournament["TournamentMembers"] = tournamentMembers

    return formattedTournament


def lambda_handler(event, context):
    try:
        common.Init(event)
        dbManager.UpdateConnection()

        limit = ValidateLimit(common.GetArgumentIntV2("limit", DEFAULT_LIMIT))
        continuationToken = common.GetArgumentStringV2("token")
        calculate = True

        if continuationToken:
            try:
                fullTournamentList = continuationManager.GetCachedResponse(continuationToken)
                calculate = False
                common.LogAlways("TournamentSearch using cached continuation token response.")
            except KeyError:
                common.LogOnError("could not find cached continuation key, regenerating")

        if calculate:
            name = common.GetArgumentStringV2("name")
            ownerId = common.GetArgumentStringV2("owner-id")
            licenseNumber = common.GetArgumentIntV2("license-number")
            visibility = ValidateVisibility(common.GetArgumentStringV2("visibility"))
            memberPolicy = ValidateMemberPolicy(common.GetArgumentStringV2("member-policy"))
            memberHasOwnerId = common.GetArgumentStringV2("member-has-owner-id")
            memberHasMatchId = common.GetArgumentStringV2("member-has-match-id")
            showOnSearch = common.GetArgumentBoolean("show-on-search", None)
            memberHasApprovalStatus = ValidateMemberApprovalStatus(
                common.GetArgumentStringV2("member-has-approval-status")
            )
            searchPreset = ValidateSearchPreset(common.GetArgumentStringV2("search-preset"))

            memberPolicy, memberHasApprovalStatus = ApplySearchPreset(
                searchPreset,
                memberHasOwnerId,
                memberPolicy,
                memberHasApprovalStatus,
            )
            hasMemberFilters = bool(
                memberHasOwnerId or memberHasMatchId or memberHasApprovalStatus
            )

            common.LogAlways(
                "TournamentSearch request filters: visibility={}, owner-id={}, license-number={}, member-policy={}, member-has-owner-id={}, member-has-match-id={}, search-preset={}".format(
                    visibility,
                    ownerId,
                    licenseNumber,
                    memberPolicy,
                    memberHasOwnerId,
                    memberHasMatchId,
                    searchPreset,
                )
            )

            searchSql, searchValues = BuildSearchQuery(
                name=name,
                ownerId=ownerId,
                licenseNumber=licenseNumber,
                visibility=visibility,
                showOnSearch=showOnSearch,
                memberPolicy=memberPolicy,
                memberHasOwnerId=memberHasOwnerId,
                memberHasMatchId=memberHasMatchId,
                memberHasApprovalStatus=memberHasApprovalStatus,
            )
            print("SEARCH SQL:\n", searchSql)
            print("SEARCH VALUES:", searchValues)
            searchResults = dbManager.SelectQuery(searchSql, searchValues)

            ownerReadPermissionCache = {} #cache to avoid redundant permission checks for multiple tournaments with the same owner
            visibleTournamentRows = []
            for row in searchResults:
                if not UserCanReadTournament(row, ownerReadPermissionCache):
                    continue

                visibleTournamentRows.append(row)

            relevantMembersByTournament = {}
            if hasMemberFilters:
                visibleTournamentIds = [row["tournament_id"] for row in visibleTournamentRows]
                relevantMembersByTournament = GetRelevantTournamentMembers( #Get all members for the visible tournaments that satisfy the member filters, to avoid doing separate queries for each tournament when formatting the response
                    tournamentIds=visibleTournamentIds,
                    memberHasOwnerId=memberHasOwnerId,
                    memberHasMatchId=memberHasMatchId,
                    memberHasApprovalStatus=memberHasApprovalStatus,
                )

            fullTournamentList = []
            for row in visibleTournamentRows:
                tournamentMembers = None
                if hasMemberFilters:
                    tournamentMembers = relevantMembersByTournament.get(
                        row["tournament_id"], []
                    )
                fullTournamentList.append(
                    FormatTournament(row, tournamentMembers=tournamentMembers)
                )

            common.LogAlways(
                "TournamentSearch found {} tournaments before pagination.".format(
                    len(fullTournamentList)
                )
            )

        pagedItems, nextToken = continuationManager.GetLimitedResponse(
            fullTournamentList,
            limit,
            continuationToken,
        )

        response = {
            "TournamentSearchList": {
                "TotalCount": len(fullTournamentList),
                "Items": pagedItems,
            }
        }
        if nextToken:
            response["TournamentSearchList"]["NextToken"] = nextToken

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

    except ContinuationTokenException as ex:
        common.SetStatusCode(400)
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
