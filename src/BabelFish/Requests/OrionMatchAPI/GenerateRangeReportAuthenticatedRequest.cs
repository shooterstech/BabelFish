using Scopos.BabelFish.DataModel.OrionMatch;
using Scopos.BabelFish.Runtime.Authentication;

namespace Scopos.BabelFish.Requests.OrionMatchAPI {
    public class GenerateRangeReportAuthenticatedRequest : Request {

        public GenerateRangeReportAuthenticatedRequest( UserAuthentication credentials ) : base( "GenerateRangeReport", credentials ) {
            HttpMethod = HttpMethod.Post;
            SubDomain = APIClients.APISubDomain.AUTHAPI;
            Timeout = 3 * 60;
        }

        public GenerateRangeReportAuthenticatedRequest(
            MatchID matchId,
            string resultListName,
            UserAuthentication credentials ) : this( credentials ) {
            MatchId = matchId;
            ResultListName = resultListName;
        }

        public MatchID? MatchId { get; set; }

        public string ResultListName { get; set; } = string.Empty;

        public int CourseOfFireId { get; set; } = 1;

        public string? MilestoneStrategy { get; set; }

        public List<int>? ShotMilestoneCounts { get; set; }

        public int? ExpectedShots { get; set; }

        public string? SnapshotOrderBy { get; set; }

        public List<string> UserContext { get; set; } = new List<string>();

        public bool DryRun { get; set; } = false;

        /// <inheritdoc />
        public override string RelativePath {
            get { return "/range-reporter"; }
        }

        /// <inheritdoc />
        public override Dictionary<string, List<string>> QueryParameters {
            get {
                if (MatchId == null) {
                    throw new ArgumentNullException( nameof( MatchId ), "The match id must be set to generate a range report." );
                }

                if (string.IsNullOrWhiteSpace( ResultListName )) {
                    throw new ArgumentNullException( nameof( ResultListName ), "The result list name must be set to generate a range report." );
                }

                if (CourseOfFireId <= 0) {
                    throw new ArgumentOutOfRangeException( nameof( CourseOfFireId ), "The course of fire id must be a positive integer." );
                }

                Dictionary<string, List<string>> parameterList = new Dictionary<string, List<string>> {
                    { "match-id", new List<string> { MatchId.ToString() } },
                    { "result-list-name", new List<string> { ResultListName } },
                    { "course-of-fire-id", new List<string> { CourseOfFireId.ToString() } }
                };

                if (!string.IsNullOrWhiteSpace( MilestoneStrategy )) {
                    parameterList.Add( "milestone-strategy", new List<string> { MilestoneStrategy } );
                }

                if (ShotMilestoneCounts != null && ShotMilestoneCounts.Count > 0) {
                    if (ShotMilestoneCounts.Any( count => count <= 0 )) {
                        throw new ArgumentOutOfRangeException( nameof( ShotMilestoneCounts ), "Shot milestone counts must be positive integers." );
                    }

                    parameterList.Add( "shot-milestones", new List<string> { string.Join( ",", ShotMilestoneCounts ) } );
                }

                if (ExpectedShots.HasValue) {
                    if (ExpectedShots.Value <= 0) {
                        throw new ArgumentOutOfRangeException( nameof( ExpectedShots ), "Expected shots must be a positive integer." );
                    }

                    parameterList.Add( "expected-shots", new List<string> { ExpectedShots.Value.ToString() } );
                }

                if (!string.IsNullOrWhiteSpace( SnapshotOrderBy )) {
                    parameterList.Add( "snapshot-order-by", new List<string> { SnapshotOrderBy } );
                }

                if (UserContext != null && UserContext.Count > 0) {
                    parameterList.Add( "user-context", UserContext );
                }

                if (DryRun) {
                    parameterList.Add( "dry-run", new List<string> { DryRun.ToString() } );
                }

                return parameterList;
            }
        }
    }
}
