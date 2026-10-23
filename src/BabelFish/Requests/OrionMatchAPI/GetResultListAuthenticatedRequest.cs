using Scopos.BabelFish.DataModel.OrionMatch;
using Scopos.BabelFish.Runtime.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scopos.BabelFish.Requests.OrionMatchAPI {
    public class GetResultListAuthenticatedRequest : GetResultListAbstractRequest {
        public GetResultListAuthenticatedRequest( MatchID matchId, string resultListName, UserAuthentication credentials ) : base( "GetResultList", matchId, resultListName, credentials ) {
        }

        /// <inheritdoc />
        public override Request Copy() {
            var newRequest = new GetResultListAuthenticatedRequest( MatchID, ResultListName, Credentials );
            newRequest.Token = this.Token;
            newRequest.Limit = this.Limit;
            newRequest.CourseOfFireId = this.CourseOfFireId;
            newRequest.IgnoreInMemoryCache = this.IgnoreInMemoryCache;
            newRequest.IgnoreFileSystemCache = this.IgnoreFileSystemCache;
            newRequest.Timeout = this.Timeout;
            newRequest.HeaderKeyValuePairs = new Dictionary<string, string>( this.HeaderKeyValuePairs );

            return newRequest;
        }

        /// <inheritdoc />
        public override Dictionary<string, List<string>> QueryParameters {
            get {

                Dictionary<string, List<string>> parameterList = base.QueryParameters;

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
