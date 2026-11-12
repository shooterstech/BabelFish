using Scopos.BabelFish.DataModel.OrionMatch;
using Scopos.BabelFish.Runtime.Authentication;

namespace Scopos.BabelFish.Requests.OrionMatchAPI {
    /// <summary>
    /// Shared league-week identity for RangeReporter recap requests.
    /// </summary>
    public abstract class LeagueRecapRangeReportRequest : Request {

        protected LeagueRecapRangeReportRequest( string operationId ) : base( operationId ) {
        }

        protected LeagueRecapRangeReportRequest(
            string operationId,
            UserAuthentication credentials ) : base( operationId, credentials ) {
        }

        public MatchID? LeagueId { get; set; }

        public DateTime StartDate { get; set; } = DateTime.MinValue;

        public DateTime EndDate { get; set; } = DateTime.MinValue;

        public override string RelativePath {
            get {
                if (LeagueId == null) {
                    throw new ArgumentNullException( nameof( LeagueId ), "The league id must be set for a league recap range report." );
                }

                return $"/range-reporter/league/{LeagueId}/recap";
            }
        }

        protected Dictionary<string, List<string>> BuildLeagueWeekQueryParameters() {
            if (StartDate == DateTime.MinValue) {
                throw new ArgumentException( "The start date must be set for a league recap range report.", nameof( StartDate ) );
            }

            if (EndDate == DateTime.MinValue) {
                throw new ArgumentException( "The end date must be set for a league recap range report.", nameof( EndDate ) );
            }

            if (EndDate.Date < StartDate.Date) {
                throw new ArgumentException( "The end date must be on or after the start date.", nameof( EndDate ) );
            }

            return new Dictionary<string, List<string>> {
                { "start-date", new List<string> { StartDate.ToString( DateTimeFormats.DATE_FORMAT ) } },
                { "end-date", new List<string> { EndDate.ToString( DateTimeFormats.DATE_FORMAT ) } }
            };
        }
    }
}
