using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scopos.BabelFish.DataModel.OrionMatch;

namespace Scopos.BabelFish.Requests.OrionMatchAPI {
    public class GetMatchPublicRequest : Request {

        public GetMatchPublicRequest( string matchid ) : base( "GetMatchDetail" ) {
            MatchID = new MatchID( matchid );
        }

        public GetMatchPublicRequest( MatchID matchid ) : base( "GetMatchDetail" ) {
            MatchID = matchid;
        }

        public MatchID MatchID { get; set; }

        /// <inheritdoc />
        public override string RelativePath {
            get { return $"/match/{MatchID}"; }
        }
    }
}
