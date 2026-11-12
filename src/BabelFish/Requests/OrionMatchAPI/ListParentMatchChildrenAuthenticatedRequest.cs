using Scopos.BabelFish.DataModel.OrionMatch;
using Scopos.BabelFish.Runtime.Authentication;

namespace Scopos.BabelFish.Requests.OrionMatchAPI {
    public class ListParentMatchChildrenAuthenticatedRequest : ListParentMatchChildrenAbstractRequest {

        public ListParentMatchChildrenAuthenticatedRequest( UserAuthentication credentials, MatchID parentMatchId ) : base( "ListParentMatchChildren", credentials, parentMatchId ) {
        }

        /// <inheritdoc />
        public override Request Copy() {
            return new ListParentMatchChildrenAuthenticatedRequest( Credentials, ParentMatchId ) {
                Limit = Limit,
                Token = Token
            };
        }
    }
}
