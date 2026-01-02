using System;
using System.Collections.Generic;
using System.Text;
using Scopos.BabelFish.DataModel.Definitions;
using Scopos.BabelFish.DataModel.OrionMatch;
using Scopos.BabelFish.Runtime.Authentication;

namespace Scopos.BabelFish.Requests.OrionMatchAPI {
    public abstract class GetResultListAbstractRequest : Request, ITokenRequest {

        public GetResultListAbstractRequest( string operationId, MatchID matchId, string resultListName ) : base( operationId ) { 
            this.MatchID = matchId;
            this.ResultListName = resultListName;
        
        }

        public GetResultListAbstractRequest( string operationId, MatchID matchId, string resultListName, UserAuthentication credentials ) : base( operationId, credentials ) {
            this.MatchID = matchId;
            this.ResultListName = resultListName;
        }

        /// <summary>
        /// Factory method to return a concrete Get Result List Public Request or a Get Result List Authenticated Request object, based on the value of credentials.
        /// </summary>
        /// <param name="matchId"></param>
        /// <param name="resultListName"></param>
        /// <param name="credentials"></param>
        /// <returns></returns>
        public static GetResultListAbstractRequest Factory( MatchID matchId, string resultListName, UserAuthentication credentials = null ) {
            if ( credentials == null ) {
                return new GetResultListPublicRequest( matchId, resultListName );
            } else {
                return new GetResultListAuthenticatedRequest( matchId, resultListName, credentials );
            }
        }

        public MatchID MatchID { get; set; }

        public string ResultListName { get; set; } = string.Empty;

        /// <inheritdoc />
        public string Token { get; set; } = string.Empty;

        /// <inheritdoc />
        public int Limit { get; set; }

        /// <inheritdoc />
        public override string RelativePath {
            get { return $"/match/{MatchID}/result-list/{ResultListName}"; }
        }

    }
}
