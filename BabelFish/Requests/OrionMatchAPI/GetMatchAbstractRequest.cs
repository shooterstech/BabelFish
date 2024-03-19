using System;
using System.Collections.Generic;
using System.Text;
using Scopos.BabelFish.DataModel.OrionMatch;
using Scopos.BabelFish.Runtime.Authentication;

namespace Scopos.BabelFish.Requests.OrionMatchAPI {
    public abstract class GetMatchAbstractRequest : Request {

        public GetMatchAbstractRequest( string operationId, MatchID matchId ) : base( operationId ) {
            this.MatchID = matchId;
        }

        public GetMatchAbstractRequest( string operationId, MatchID matchId, UserAuthentication credentials ) : base( operationId, credentials) {
            this.MatchID = matchId;
        }

        public static GetMatchAbstractRequest Factory( MatchID matchID, UserAuthentication credentials = null ) {
            if ( credentials == null ) {
                return new GetMatchPublicRequest( matchID );
            } else {
                return new GetMatchAuthenticatedRequest( matchID, credentials );
            }
        }

        public MatchID MatchID { get; set; }

        /// <inheritdoc />
        public override string RelativePath {
            get { return $"/match/{MatchID}"; }
        }
    }
}
