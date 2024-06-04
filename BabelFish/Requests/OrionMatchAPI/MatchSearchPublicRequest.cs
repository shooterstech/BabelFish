using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scopos.BabelFish.Requests.OrionMatchAPI {
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
