using Scopos.BabelFish.DataModel.OrionMatch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scopos.BabelFish.Requests.OrionMatchAPI {
    public class GetResultListPublicRequest : GetResultListAbstractRequest {

        public GetResultListPublicRequest( MatchID matchId, string resultListName ) : base( "GetResultList", matchId, resultListName ) {
        }

        /// <inheritdoc />
        public override Request Copy() {
            var newRequest = new GetResultListPublicRequest( MatchID, ResultListName );
            newRequest.Token = this.Token;
            newRequest.Limit = this.Limit;
            newRequest.CourseOfFireId = this.CourseOfFireId;
            newRequest.Preliminary = this.Preliminary;
            newRequest.Credentials = this.Credentials;
            newRequest.IgnoreInMemoryCache = this.IgnoreInMemoryCache;
            newRequest.IgnoreFileSystemCache = this.IgnoreFileSystemCache;
            newRequest.Timeout = this.Timeout;
            newRequest.HeaderKeyValuePairs = new Dictionary<string, string>( this.HeaderKeyValuePairs );

            return newRequest;
        }

        /// <summary>
        /// If this is a public match and preliminary is true, then this GetResultList will return participants ranked and scored by their predictive results; 
        /// the predictive scores are based on a participant's score history and shots taken in the current match. 
        /// </summary>
        public bool Preliminary { get; set; }

        /// <inheritdoc />
        public override Dictionary<string, List<string>> QueryParameters {
            get {

                Dictionary<string, List<string>> parameterList = base.QueryParameters;

                if (!string.IsNullOrEmpty( Token )) {
                    parameterList.Add( "token", new List<string> { Token } );
                }

                if (Limit > 0)
                    parameterList.Add( "limit", new List<string> { Limit.ToString() } );

                if (Preliminary)
                    parameterList.Add("preliminary", new List<string> { Preliminary.ToString() } );

                return parameterList;
            }
        }
    }
}
