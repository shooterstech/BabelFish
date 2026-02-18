using Scopos.BabelFish.Runtime.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scopos.BabelFish.Requests.OrionMatchAPI
{
    public class MatchSearchAuthenticatedRequest : MatchSearchAbstractRequest {
        /// <summary>
        /// Authenticated constructor. 
        /// User is encouraged (really you need to do this) to set the Request Properties at time of construction.
        /// </summary>
        public MatchSearchAuthenticatedRequest(UserAuthentication credentials) : base( "MatchSearch", credentials)  { 
            this.RequiresCredentials = true;
        }

        /// <inheritdoc />
        public override Request Copy() {
            var newRequest = new MatchSearchAuthenticatedRequest(Credentials);
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
        public override string RelativePath
        {
            get { return $"/match/search"; }
        }
    }
}
