using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scopos.BabelFish.DataModel.OrionMatch;

namespace Scopos.BabelFish.Requests.OrionMatchAPI {
    public class GetMatchPublicRequest : GetMatchAbstractRequest {

        public GetMatchPublicRequest( MatchID matchId ) : base( "GetMatchDetail", matchId ) {
        }
    }
}
