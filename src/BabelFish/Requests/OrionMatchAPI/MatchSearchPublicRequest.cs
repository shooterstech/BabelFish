namespace Scopos.BabelFish.Requests.OrionMatchAPI {


    [Obsolete( "Use one of the ListMatch() methods instead. Deprecated July 2026 with the BabelFish 2.0 release. This method will be removed in a future release." )]
    public class MatchSearchPublicRequest : MatchSearchAbstractRequest {
        /// <summary>
        /// Public constructor. 
        /// User is encouraged (really you need to do this) to set the Request Properties at time of construction.
        /// </summary>
        public MatchSearchPublicRequest() : base( "MatchSearch" ) { }

        /// <inheritdoc />
        public override Request Copy() {
            var newRequest = new MatchSearchPublicRequest();
            newRequest.StartDate = StartDate;
            newRequest.EndDate = EndDate;
            newRequest.ShootingStyle = ShootingStyle;
            newRequest.Longitude = Longitude;
            newRequest.Latitude = Latitude;
            newRequest.OwnerId = OwnerId;
            newRequest.Token = Token;
            newRequest.Distance = Distance;
            newRequest.Limit = Limit;

            return newRequest;
        }

        /// <inheritdoc />
        public override string RelativePath {
            get { return $"/match/search"; }
        }
    }
}
