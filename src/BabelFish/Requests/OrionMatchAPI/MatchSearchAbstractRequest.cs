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
        /// The default is 500 miles.
        /// </summary>
        public int? Distance { get; set; } = 500;

        /// <summary>
        /// The start date of the match dates to search.
        /// The default value is the first day of the current month.
        /// if null, call will return today, one year ago.
        /// </summary>
        public DateTime? StartDate { get; set; } = new DateTime( DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day ).AddDays( -365 );

        /// <summary>
        /// The end date of the match dates to search.
        /// The default value is the last day of the current month.
        /// if null, call will return today
        /// </summary>
        public DateTime? EndDate { get; set; } = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day).AddDays( 0 );

        /// <summary>
        /// The shooting style to search or unassigned for all.
        /// The default value is empty List
        /// </summary>
        [Obsolete( "To be replaced with Event Style" )]
        public List<string> ShootingStyle { get; set; } = new List<string>();

        /// <summary>
        /// The Logitude of an area to search.
        /// If > default of 0, Latitude must also be > 0.
        /// if null, no location will be specificied.
        /// </summary>
        public double? Longitude { get; set; } = -84.5063057;

        /// <summary>
        /// The Latitude of an area to search.
        /// If > default of 0, Longitude must also be > 0.
        /// if null, no location will be specificied.
        /// </summary>
        public double? Latitude { get; set; } = 38.0394328;

        /// <summary>
        /// Owner ID should be in the form of OrionAccount00XXXX. default is empty string
        /// if null, all matches will be returned
        /// </summary>
        public string? OwnerId { get; set; } = string.Empty;

        /// <inheritdoc />
        public string Token { get; set; } = string.Empty;

        /// <summary>
        /// The maximum number of search results to return. 
        /// </summary>
        public int Limit { get; set; } = 50;

        public override Dictionary<string, List<string>> QueryParameters {
            get {
                Dictionary<string, List<string>> parameterList = new Dictionary<string, List<string>>();
                parameterList.Add("distance", new List<string>() { Distance != null ? Distance.ToString() : "" } );
                parameterList.Add( "start-date", new List<string>() { StartDate != null ? ((DateTime)StartDate).ToString( DateTimeFormats.DATE_FORMAT ) : "" } );
                parameterList.Add( "end-date", new List<string>() { EndDate != null ? ((DateTime)EndDate).ToString( DateTimeFormats.DATE_FORMAT ) : "" } );
                parameterList.Add( "shooting-style", ShootingStyle != null ? ShootingStyle : new List<string>() { "" } );
                parameterList.Add( "longitude", new List<string>() { Longitude != null ? Longitude.ToString() : "" } );
                parameterList.Add( "latitude", new List<string>() { Latitude != null ? Latitude.ToString() : "" } );
                parameterList.Add( "owner-id", new List<string>() { OwnerId != null ? OwnerId.ToString() : "" });
                parameterList.Add( "limit", new List<string>() { Limit.ToString() } );
                if (!string.IsNullOrEmpty( Token ))
                    parameterList.Add( "token", new List<string>() { Token } );

                return parameterList;
            }
        }

    }
}
