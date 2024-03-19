using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Scopos.BabelFish.Runtime.Authentication;
using Scopos.BabelFish.DataModel.OrionMatch;

namespace Scopos.BabelFish.Requests.OrionMatchAPI {
    public class GetMatchAuthenticatedRequest : GetMatchAbstractRequest {

        /// <summary>
        /// Consructor
        /// </summary>
        /// <param name="matchId"></param>
        /// <param name="credentials"></param>
        public GetMatchAuthenticatedRequest( MatchID matchId, UserAuthentication credentials ) : base( "GetMatchDetail", matchId, credentials ) {
        }
    }
}
