using Scopos.BabelFish.DataModel.OrionMatch;
using Scopos.BabelFish.Runtime.Authentication;

namespace Scopos.BabelFish.Requests.OrionMatchAPI {
    public abstract class GetLeagueRecapRangeReportAbstractRequest : LeagueRecapRangeReportRequest {

        protected GetLeagueRecapRangeReportAbstractRequest( string operationId ) : base( operationId ) {
        }

        protected GetLeagueRecapRangeReportAbstractRequest(
            string operationId,
            UserAuthentication credentials ) : base( operationId, credentials ) {
        }

        public static GetLeagueRecapRangeReportAbstractRequest Factory(
            MatchID leagueId,
            DateTime startDate,
            DateTime endDate,
            UserAuthentication? credentials = null ) {
            if (credentials == null) {
                return new GetLeagueRecapRangeReportPublicRequest( leagueId, startDate, endDate );
            } else {
                return new GetLeagueRecapRangeReportAuthenticatedRequest(
                    leagueId,
                    startDate,
                    endDate,
                    credentials );
            }
        }

        public override Dictionary<string, List<string>> QueryParameters {
            get { return BuildLeagueWeekQueryParameters(); }
        }
    }
}
