using Scopos.BabelFish.APIClients;
using Scopos.BabelFish.DataModel.OrionMatch;
using Scopos.BabelFish.Requests.OrionMatchAPI;

namespace Scopos.BabelFish.Responses.OrionMatchAPI {
    public class ListMatchesAuthenticatedResponse : Response<MatchListWrapper>, ITokenResponse<ListMatchesAuthenticatedRequest> {

        public ListMatchesAuthenticatedResponse( ListMatchesAuthenticatedRequest request ) : base() {
            Request = request;
        }

        /// <summary>
        /// Facade function that returns the same as this.Value.MatchList.
        /// </summary>
        public MatchList MatchList {
            get { return Value.MatchList; }
        }

        /// <inheritdoc />
        public ListMatchesAuthenticatedRequest GetNextRequest() {
            if (!HasMoreItems) {
                throw new NoMoreItemsException( "GetNextRequest() can not return a new request object because there are no more items to return. Always check .HasMoreItems before calling .GetNextRequest()." );
            }

            var nextRequest = (ListMatchesAuthenticatedRequest)Request.Copy();
            nextRequest.Token = Value.MatchList.NextToken;
            return nextRequest;
        }

        /// <inheritdoc />
        public bool HasMoreItems {
            get {
                return HasOkStatusCode && !string.IsNullOrEmpty( Value.MatchList.NextToken );
            }
        }
    }
}
