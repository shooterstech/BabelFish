using Scopos.BabelFish.DataModel.OrionMatch;
using Scopos.BabelFish.Runtime.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scopos.BabelFish.Requests.OrionMatchAPI {
    public class GetSquaddingListPublicRequest : GetSquaddingListAbstractRequest {

        public GetSquaddingListPublicRequest(MatchID matchId, string squaddingEventName ) : base( "GetSquaddingList", matchId, squaddingEventName ) {
        }

        /// <inheritdoc />
        public override Request Copy() {
            var newRequest = new GetSquaddingListPublicRequest( MatchID, SquaddingEventName );
            newRequest.RelayName = this.RelayName;
            newRequest.Token = this.Token;
            newRequest.Limit = this.Limit;

            return newRequest;
        }
    }
}
