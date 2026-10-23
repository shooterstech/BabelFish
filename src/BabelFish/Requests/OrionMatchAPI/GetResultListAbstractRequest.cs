using System;
using System.Collections.Generic;
using System.Globalization;
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

        /// <summary>
        /// The course of fire containing this result list. Defaults to the legacy course, 1.
        /// </summary>
        public int CourseOfFireId { get; set; } = 1;

        /// <inheritdoc />
        public override Dictionary<string, List<string>> QueryParameters {
            get {
                if (CourseOfFireId <= 0) {
                    throw new ArgumentOutOfRangeException( nameof( CourseOfFireId ), "The course of fire id must be a positive integer." );
                }

                return new Dictionary<string, List<string>> {
                    { "course-of-fire-id", new List<string> { CourseOfFireId.ToString( CultureInfo.InvariantCulture ) } }
                };
            }
        }

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
