using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Scopos.BabelFish.Runtime.Authentication;
using Scopos.BabelFish.DataModel.OrionMatch;

namespace Scopos.BabelFish.Requests.OrionMatchAPI {
    public class GetMatchAuthenticatedRequest : Request {

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="matchid">Must be in the Match ID format. e.g. 1.2.123456789.1</param>
        /// <param name="credentials"></param>
        /// <exception cref="FormatException">Thrown if matchid is not in the Match ID format</exception>
        public GetMatchAuthenticatedRequest( string matchid, UserAuthentication credentials ) : base( "GetMatchDetail", credentials ) {

            MatchID = new MatchID( matchid );
        }

        /// <summary>
        /// Consructor
        /// </summary>
        /// <param name="matchid"></param>
        /// <param name="credentials"></param>
        public GetMatchAuthenticatedRequest( MatchID matchid, UserAuthentication credentials ) : base( "GetMatchDetail", credentials ) {

            MatchID =matchid;
        }

        public MatchID MatchID { get; set; }

        /// <inheritdoc />
        public override string RelativePath {
            get { return $"/match/{MatchID}"; }
        }
    }
}
