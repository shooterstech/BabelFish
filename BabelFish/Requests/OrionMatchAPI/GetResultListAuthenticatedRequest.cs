using Scopos.BabelFish.DataModel.OrionMatch;
using Scopos.BabelFish.Runtime.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scopos.BabelFish.Requests.OrionMatchAPI {
    public class GetResultListAuthenticatedRequest : Request, ITokenRequest {
        public GetResultListAuthenticatedRequest( MatchID matchid, string listname, UserAuthentication credentials ) : base( "GetResultList", credentials ) {
            MatchID = matchid;
            ResultListName = listname;
        }

        /// <inheritdoc />
        public override Request Copy() {
            var newRequest = new GetResultListAuthenticatedRequest( MatchID, ResultListName, Credentials );
            newRequest.Token = this.Token;
            newRequest.Limit = this.Limit;

            return newRequest;
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
