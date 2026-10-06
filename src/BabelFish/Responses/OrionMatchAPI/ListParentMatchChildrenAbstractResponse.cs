using Scopos.BabelFish.APIClients;
using Scopos.BabelFish.DataModel.OrionMatch;
using Scopos.BabelFish.Requests.OrionMatchAPI;

namespace Scopos.BabelFish.Responses.OrionMatchAPI {
    public abstract class ListParentMatchChildrenAbstractResponse :
        Response<MatchChildListWrapper>,
        ITokenResponse<ListParentMatchChildrenAbstractRequest> {

        public ListParentMatchChildrenAbstractResponse() : base() {
        }

        /// <summary>
        /// Facade function that returns the same as this.Value.MatchChildList.
        /// </summary>
        public MatchChildList MatchChildList {
            get { return Value.MatchChildList; }
        }

        /// <inheritdoc/>
        public ListParentMatchChildrenAbstractRequest GetNextRequest() {
            if (!HasMoreItems) {
                throw new NoMoreItemsException( "GetNextRequest() can not return a new request object because there are no more items to return. Always check .HasMoreItems before calling .GetNextRequest()." );
            }

            var nextRequest = (ListParentMatchChildrenAbstractRequest)Request.Copy();
            nextRequest.Token = Value.MatchChildList.NextToken;
            return nextRequest;
        }

        /// <inheritdoc />
        public bool HasMoreItems {
            get {
                return HasOkStatusCode && !string.IsNullOrEmpty( Value.MatchChildList.NextToken );
            }
        }
    }
}
