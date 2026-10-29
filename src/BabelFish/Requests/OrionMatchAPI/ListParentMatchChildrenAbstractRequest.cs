using Scopos.BabelFish.DataModel.OrionMatch;
using Scopos.BabelFish.Runtime.Authentication;

namespace Scopos.BabelFish.Requests.OrionMatchAPI {
    public abstract class ListParentMatchChildrenAbstractRequest : Request, ITokenRequest {

        protected ListParentMatchChildrenAbstractRequest( string operationId, MatchID parentMatchId ) : base( operationId ) {
            ParentMatchId = parentMatchId ?? throw new ArgumentNullException( nameof( parentMatchId ) );
        }

        protected ListParentMatchChildrenAbstractRequest( string operationId, UserAuthentication credentials, MatchID parentMatchId ) : base( operationId, credentials ) {
            ParentMatchId = parentMatchId ?? throw new ArgumentNullException( nameof( parentMatchId ) );
        }

        public static ListParentMatchChildrenAbstractRequest Factory( MatchID parentMatchId, UserAuthentication credentials = null ) {
            if (credentials == null) {
                return new ListParentMatchChildrenPublicRequest( parentMatchId );
            } else {
                return new ListParentMatchChildrenAuthenticatedRequest( credentials, parentMatchId );
            }
        }

        /// <summary>
        /// The Match ID of the parent match whose children should be listed.
        /// </summary>
        public MatchID ParentMatchId { get; set; }

        /// <inheritdoc />
        public string Token { get; set; } = string.Empty;

        /// <inheritdoc />
        public int Limit { get; set; } = 50;

        /// <inheritdoc />
        public override string RelativePath {
            get {
                if (ParentMatchId == null) {
                    throw new ArgumentNullException( nameof( ParentMatchId ), "The parent match id must be set to list child matches." );
                }

                return $"/match/{ParentMatchId}/children";
            }
        }

        /// <inheritdoc />
        public override Dictionary<string, List<string>> QueryParameters {
            get {
                Dictionary<string, List<string>> parameterList = new Dictionary<string, List<string>>();

                if (Limit > 0) {
                    parameterList.Add( "limit", new List<string> { Limit.ToString() } );
                }

                if (!string.IsNullOrWhiteSpace( Token )) {
                    parameterList.Add( "token", new List<string> { Token } );
                }

                return parameterList;
            }
        }
    }
}
