using Scopos.BabelFish.DataModel.OrionMatch;
using Scopos.BabelFish.Runtime.Authentication;

namespace Scopos.BabelFish.Requests.OrionMatchAPI {
    public class PatchTournamentMemberAuthenticatedRequest : Request {

        public PatchTournamentMemberAuthenticatedRequest( UserAuthentication credentials ) : base( "PatchTournamentMember", credentials ) {
            HttpMethod = new HttpMethod( "PATCH" );
            SubDomain = APIClients.APISubDomain.AUTHAPI;
        }


        /// <summary>
        /// The Match ID of the tournament where the member is being updated.
        /// </summary>
        public MatchID? TournamentId { get; set; } = null;

        /// <summary>
        /// The Match ID of the member match being updated.
        /// </summary>
        public MatchID? MatchId { get; set; } = null;

        /// <summary>
        /// The updated approval status of the tournament member.
        /// Valid values are APPROVED, PENDING, and REJECTED.
        /// </summary>
        public ApprovalStatus ApprovalStatus { get; set; } = ApprovalStatus.UNKNOWN;

        /// <inheritdoc />
        public override string RelativePath {
            get {
                if (TournamentId == null) {
                    throw new ArgumentNullException( nameof( TournamentId ), "The tournament id must be set to patch a tournament member." );
                }

                return $"/tournament/{TournamentId}/member";
            }
        }

        /// <inheritdoc />
        public override Dictionary<string, List<string>> QueryParameters {
            get {
                if (MatchId == null) {
                    throw new ArgumentNullException( nameof( MatchId ), "The match id must be set to patch a tournament member." );
                }

                if (ApprovalStatus != ApprovalStatus.APPROVED
                    && ApprovalStatus != ApprovalStatus.PENDING
                    && ApprovalStatus != ApprovalStatus.REJECTED) {
                    throw new ArgumentOutOfRangeException( nameof( ApprovalStatus ), "The approval status must be APPROVED, PENDING, or REJECTED." );
                }

                return new Dictionary<string, List<string>> {
                    { "match-id", new List<string> { MatchId.ToString() } },
                    { "approval-status", new List<string> { EnumHelper.MemberValue( ApprovalStatus ) } }
                };
            }
        }
    }
}
