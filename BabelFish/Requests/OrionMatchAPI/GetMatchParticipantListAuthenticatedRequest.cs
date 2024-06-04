using Scopos.BabelFish.DataModel.OrionMatch;
using Scopos.BabelFish.Helpers;
using Scopos.BabelFish.Runtime.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scopos.BabelFish.Requests.OrionMatchAPI {
    public class GetMatchParticipantListAuthenticatedRequest : GetMatchParticipantListAbstractRequest {


        public GetMatchParticipantListAuthenticatedRequest( MatchID matchId, UserAuthentication credentials ) : base( "GetMatchParticipantList", matchId, credentials ) {
        }

        /// <inheritdoc />
        public override Request Copy() {
            GetMatchParticipantListAuthenticatedRequest newRequest = new GetMatchParticipantListAuthenticatedRequest( MatchID, Credentials );
            newRequest.Role = this.Role; 
            newRequest.Token = this.Token;
            return newRequest;
        }
    }
}
