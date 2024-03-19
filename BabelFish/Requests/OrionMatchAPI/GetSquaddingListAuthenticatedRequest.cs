using Scopos.BabelFish.DataModel.OrionMatch;
using Scopos.BabelFish.Runtime.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scopos.BabelFish.Requests.OrionMatchAPI {
    public class GetSquaddingListAuthenticatedRequest : GetSquaddingListAbstractRequest {

        public GetSquaddingListAuthenticatedRequest( MatchID matchId, string squaddingEventName, UserAuthentication credentials ) : base( "GetSquaddingList", matchId, squaddingEventName, credentials ) {
        }

        /// <inheritdoc />
        public override Request Copy() {
            var newRequest = new GetSquaddingListAuthenticatedRequest( MatchID, SquaddingEventName, Credentials );
            newRequest.RelayName = this.RelayName;
            newRequest.Token = this.Token;
            newRequest.Limit = this.Limit;

            return newRequest;
        }
    }
}
