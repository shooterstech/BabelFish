using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scopos.BabelFish.Requests.OrionMatchAPI {
    public class GetResultListPublicRequest : Request, ITokenRequest {
        public GetResultListPublicRequest( string matchid = "", string listname = "" ) : base( "GetResultList" ) {
            MatchID = matchid;
            ResultListName = listname;
        }

        /// <inheritdoc />
        public override Request Copy() {
            var newRequest = new GetResultListPublicRequest( MatchID, ResultListName );
            newRequest.Token = this.Token;
            newRequest.Limit = this.Limit;

            return newRequest;
        }

        public string MatchID { get; set; } = string.Empty;

        public string ResultListName { get; set; } = string.Empty;

        /// <inheritdoc />
        public string Token { get; set; } = string.Empty;

        /// <inheritdoc />
        public int Limit { get; set; }

        /// <inheritdoc />
        public override string RelativePath {
            get { return $"/match/{MatchID}/result-list/{ResultListName}"; }
        }

        /// <inheritdoc />
        public override Dictionary<string, List<string>> QueryParameters {
            get {

                Dictionary<string, List<string>> parameterList = new Dictionary<string, List<string>>();

                if (!string.IsNullOrEmpty( Token )) {
                    parameterList.Add( "token", new List<string> { Token } );
                }

                if (Limit > 0)
                    parameterList.Add( "limit", new List<string> { Limit.ToString() } );

                return parameterList;
            }
        }
    }
}
