using Scopos.BabelFish.Runtime.Authentication;

namespace Scopos.BabelFish.Requests.OrionMatchAPI {
    /// <summary>
    /// Abstract request class for asking SCOPOS' REST API to do a search for matches.
    /// <para>Callers can do a search by location, date, and club owner of the match.</para>
    /// </summary>
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
        /// <para>When performing a location search, Longitude, Latitude, and Distance are required parameters.</para>
        /// </summary>
        public int? Distance { get; set; } = null;

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
        public DateTime? EndDate { get; set; } = new DateTime( DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day ).AddDays( 0 );

        /// <summary>
        /// The shooting style to search or unassigned for all.
        /// The default value is empty List
        /// </summary>
        [Obsolete( "To be replaced with Event Style" )]
        public List<string> ShootingStyle { get; set; } = new List<string>();

        /// <summary>
        /// The Logitude of an area to search.
        /// <para>When performing a location search, Longitude, Latitude, and Distance are required parameters.</para>
        /// </summary>
        public double? Longitude { get; set; } = null;

        /// <summary>
        /// The Latitude of an area to search.
        /// <para>When performing a location search, Longitude, Latitude, and Distance are required parameters.</para>
        /// </summary>
        public double? Latitude { get; set; } = null;

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

        /// <inheritdoc />
        public override Dictionary<string, List<string>> QueryParameters {
            get {
                //If the user specified one of Longitude, Latitude, or Distance, they must specify all three.
                bool validationSearchArgumentError = new[] { Longitude is not null, Latitude is not null, Distance is not null }.Count( x => x ) is 1 or 2;
                if (validationSearchArgumentError) {
                    throw new ArgumentNullException( $"When performing a Location searcch Longitude, Latitude, and Distance must all be specified (not null values)." );
                }

                Dictionary<string, List<string>> parameterList = new Dictionary<string, List<string>>();
                parameterList.Add( "distance", new List<string>() { Distance != null ? Distance.ToString() : "" } );
                parameterList.Add( "start-date", new List<string>() { StartDate != null ? ((DateTime)StartDate).ToString( DateTimeFormats.DATE_FORMAT ) : "" } );
                parameterList.Add( "end-date", new List<string>() { EndDate != null ? ((DateTime)EndDate).ToString( DateTimeFormats.DATE_FORMAT ) : "" } );
                parameterList.Add( "shooting-style", ShootingStyle != null ? ShootingStyle : new List<string>() { "" } );
                parameterList.Add( "longitude", new List<string>() { Longitude != null ? Longitude.ToString() : "" } );
                parameterList.Add( "latitude", new List<string>() { Latitude != null ? Latitude.ToString() : "" } );
                parameterList.Add( "owner-id", new List<string>() { OwnerId != null ? OwnerId.ToString() : "" } );
                parameterList.Add( "limit", new List<string>() { Limit.ToString() } );
                if (!string.IsNullOrEmpty( Token ))
                    parameterList.Add( "token", new List<string>() { Token } );

                return parameterList;
            }
        }

    }
}
