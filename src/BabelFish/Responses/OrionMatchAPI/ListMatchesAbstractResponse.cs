using Scopos.BabelFish.APIClients;
using Scopos.BabelFish.DataModel.OrionMatch;
using Scopos.BabelFish.Requests.OrionMatchAPI;

namespace Scopos.BabelFish.Responses.OrionMatchAPI {
    public abstract class ListMatchesAbstractResponse : Response<MatchListWrapper>, ITokenResponse<ListMatchesAbstractRequest> {

        public ListMatchesAbstractResponse() : base() {
        }

        /// <summary>
        /// Facade function that returns the same as this.Value.MatchList.
        /// </summary>
        public MatchList MatchList {
            get { return Value.MatchList; }
        }

        /// <inheritdoc />
        public ListMatchesAbstractRequest GetNextRequest() {
            if (!HasMoreItems) {
                throw new NoMoreItemsException( "GetNextRequest() can not return a new request object because there are no more items to return. Always check .HasMoreItems before calling .GetNextRequest()." );
            }

            if (Request is ListMatchesPublicRequest) {
                var nextRequest = (ListMatchesPublicRequest)Request.Copy();
                nextRequest.Token = Value.MatchList.NextToken;
                return nextRequest;
            } else if (Request is ListMatchesAuthenticatedRequest) {
                var nextRequest = (ListMatchesAuthenticatedRequest)Request.Copy();
                nextRequest.Token = Value.MatchList.NextToken;
                return nextRequest;
            } else {
                throw new ArgumentException( $"Parameter Request is of unexpected type ${Request.GetType()}." );
            }
        }

        /// <inheritdoc />
        public bool HasMoreItems {
            get {
                return HasOkStatusCode && !string.IsNullOrEmpty( Value.MatchList.NextToken );
            }
        }
    }
}
