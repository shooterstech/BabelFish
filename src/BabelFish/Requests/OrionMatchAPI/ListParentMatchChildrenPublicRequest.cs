using Scopos.BabelFish.DataModel.OrionMatch;

namespace Scopos.BabelFish.Requests.OrionMatchAPI {
    public class ListParentMatchChildrenPublicRequest : ListParentMatchChildrenAbstractRequest {

        public ListParentMatchChildrenPublicRequest( MatchID parentMatchId ) : base( "ListParentMatchChildren", parentMatchId ) {
        }

        /// <inheritdoc />
        public override Request Copy() {
            return new ListParentMatchChildrenPublicRequest( ParentMatchId ) {
                Limit = Limit,
                Token = Token
            };
        }
    }
}
