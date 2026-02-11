using Scopos.BabelFish.Runtime.Authentication;

namespace Scopos.BabelFish.Requests.OrionMatchAPI {
    public abstract class MatchSearchAbstractRequest : Request, ITokenRequest {

        public MatchSearchAbstractRequest( string operationId ) : base( operationId ) {

        }

        public MatchSearchAbstractRequest( string operationId, UserAuthentication credentials ) : base( operationId ) {

        }
        public static MatchSearchAbstractRequest Factory( UserAuthentication credentials = null ) {
            if (credentials == null) {
                return new MatchSearchPublicRequest();
            } else {
                return new MatchSearchAuthenticatedRequest( credentials );
            }
        }

        /// <summary>
        /// Distance in miles to search.
        /// The default is 100 miles.
        /// </summary>
        public int Distance { get; set; } = 500;

        /// <summary>
        /// The start date of the match dates to search.
        /// The default value is the first day of the current month.
        /// </summary>
        public DateTime StartDate { get; set; } = new DateTime( DateTime.Today.Year, DateTime.Today.Month, 1 ).AddDays( -14 );

        /// <summary>
        /// The end date of the match dates to search.
        /// The default value is the last day of the current month.
        /// </summary>
        public DateTime EndDate { get; set; } = new DateTime( DateTime.Today.Year, DateTime.Today.Month, 1 ).AddDays( 0 );

        /// <summary>
        /// The shooting style to search or unassigned for all.
        /// The default value is Air Rifle.
        /// </summary>
        [Obsolete( "To be replaced with Event Style" )]
        public List<string> ShootingStyle { get; set; } = new List<string>();

        /// <summary>
        /// The Logitude of an area to search.
        /// If > default of 0, Latitude must also be > 0.
        /// </summary>
        public double Longitude { get; set; } = -84.5063057;

        /// <summary>
        /// The Latitude of an area to search.
        /// If > default of 0, Longitude must also be > 0.
        /// </summary>
        public double Latitude { get; set; } = 38.0394328;

        /// <inheritdoc />
        public string Token { get; set; } = string.Empty;

        /// <summary>
        /// The maximum number of search results to return. 
        /// </summary>
        public int Limit { get; set; } = 50;

        public override Dictionary<string, List<string>> QueryParameters {
            get {
                if (Latitude == 0 || Longitude == 0)
                    throw new RequestException( "Longitude and Latitude are required and must be > 0." );

                Dictionary<string, List<string>> parameterList = new Dictionary<string, List<string>>();
                parameterList.Add( "distance", new List<string>() { Distance.ToString() } );
                parameterList.Add( "start-date", new List<string>() { StartDate.ToString( DateTimeFormats.DATE_FORMAT ) } );
                parameterList.Add( "end-date", new List<string>() { EndDate.ToString( DateTimeFormats.DATE_FORMAT ) } );
                parameterList.Add( "shooting-style", ShootingStyle );
                parameterList.Add( "longitude", new List<string>() { Longitude.ToString() } );
                parameterList.Add( "latitude", new List<string>() { Latitude.ToString() } );
                parameterList.Add( "limit", new List<string>() { Limit.ToString() } );
                if (!string.IsNullOrEmpty( Token ))
                    parameterList.Add( "token", new List<string>() { Token } );

                return parameterList;
            }
        }

    }
}
