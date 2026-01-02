using Scopos.BabelFish.DataModel.OrionMatch;
using Scopos.BabelFish.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scopos.BabelFish.Requests.OrionMatchAPI {
    public class GetMatchParticipantListPublicRequest : GetMatchParticipantListAbstractRequest {

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="matchid"></param>
        public GetMatchParticipantListPublicRequest( MatchID matchId ) : base( "GetMatchParticipantList", matchId ) {
            MatchID = matchId;
        }

        public override Request Copy() {
            GetMatchParticipantListPublicRequest newRequest = new GetMatchParticipantListPublicRequest( MatchID );
            newRequest.Role = this.Role; 
            newRequest.Token = this.Token;
            return newRequest;
        }
    }
}
