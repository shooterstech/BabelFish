using Scopos.BabelFish.Runtime.Authentication;

namespace Scopos.BabelFish.Requests.OrionMatchAPI {
    /// <summary>
    /// Shared generation options for match and league-game RangeReporter requests.
    /// </summary>
    public abstract class RangeReportGenerationAuthenticatedRequest : Request {

        protected RangeReportGenerationAuthenticatedRequest(
            string operationId,
            UserAuthentication credentials ) : base( operationId, credentials ) {
            HttpMethod = HttpMethod.Post;
            SubDomain = APIClients.APISubDomain.AUTHAPI;
            Timeout = 3 * 60;
        }

        public int CourseOfFireId { get; set; } = 1;

        /// <summary>
        /// Requests three or four evenly distributed milestones. Mutually exclusive with
        /// <see cref="ShotMilestoneCounts"/>.
        /// </summary>
        public int? MilestoneCount { get; set; }

        public List<int>? ShotMilestoneCounts { get; set; }

        public List<string> UserContext { get; set; } = new List<string>();

        public bool DryRun { get; set; } = false;

        protected Dictionary<string, List<string>> BuildGenerationQueryParameters() {
            if (CourseOfFireId <= 0) {
                throw new ArgumentOutOfRangeException( nameof( CourseOfFireId ), "The course of fire id must be a positive integer." );
            }

            if (MilestoneCount.HasValue && MilestoneCount.Value != 3 && MilestoneCount.Value != 4) {
                throw new ArgumentOutOfRangeException( nameof( MilestoneCount ), "Milestone count must be either 3 or 4." );
            }

            if (ShotMilestoneCounts != null && ShotMilestoneCounts.Any( count => count <= 0 )) {
                throw new ArgumentOutOfRangeException( nameof( ShotMilestoneCounts ), "Shot milestone counts must be positive integers." );
            }

            if (MilestoneCount.HasValue && ShotMilestoneCounts != null && ShotMilestoneCounts.Count > 0) {
                throw new ArgumentException( "Milestone count and shot milestone counts are mutually exclusive." );
            }

            Dictionary<string, List<string>> parameterList = new Dictionary<string, List<string>> {
                { "course-of-fire-id", new List<string> { CourseOfFireId.ToString() } }
            };

            if (MilestoneCount.HasValue) {
                parameterList.Add( "milestone-count", new List<string> { MilestoneCount.Value.ToString() } );
            }

            if (ShotMilestoneCounts != null && ShotMilestoneCounts.Count > 0) {
                parameterList.Add( "shot-milestones", new List<string> { string.Join( ",", ShotMilestoneCounts ) } );
            }

            if (UserContext != null && UserContext.Count > 0) {
                if (UserContext.Any( string.IsNullOrWhiteSpace )) {
                    throw new ArgumentException( "User context entries must be non-empty strings.", nameof( UserContext ) );
                }

                parameterList.Add( "user-context", UserContext );
            }

            if (DryRun) {
                parameterList.Add( "dry-run", new List<string> { DryRun.ToString() } );
            }

            return parameterList;
        }
    }
}
