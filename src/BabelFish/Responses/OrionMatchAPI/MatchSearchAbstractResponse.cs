using Scopos.BabelFish.APIClients;
using Scopos.BabelFish.DataModel.OrionMatch;
using Scopos.BabelFish.Requests.OrionMatchAPI;

namespace Scopos.BabelFish.Responses.OrionMatchAPI {
    [Obsolete( "Use one of the ListMatch() methods instead. Deprecated July 2026 with the BabelFish 2.0 release. This method will be removed in a future release." )]
    public abstract class MatchSearchAbstractResponse : Response<MatchSearchWrapper>, ITokenResponse<MatchSearchAbstractRequest> {

        public MatchSearchAbstractResponse() : base() {

        }

        /// <summary>
        /// Facade function that returns the same as this.Value
        /// </summary>
        public MatchSearchList MatchSearchList {
            get { return Value.MatchSearchList; }
        }

        /// <inheritdoc />
        public MatchSearchAbstractRequest GetNextRequest() {
            if (!this.HasMoreItems)
                throw new NoMoreItemsException( "GetNextRequest() can not return a new request object because there are no more items to return. Always check .HasMoreItems before calling .GetNextRequest()." );

            if (Request is MatchSearchPublicRequest) {
                var nextRequest = (MatchSearchPublicRequest)Request.Copy();
                nextRequest.Token = Value.MatchSearchList.NextToken;
                return nextRequest;
            } else if (Request is MatchSearchAuthenticatedRequest) {
                var nextRequest = (MatchSearchAuthenticatedRequest)Request.Copy();
                nextRequest.Token = Value.MatchSearchList.NextToken;
                return nextRequest;
            } else {
                throw new ArgumentException( $"Parameter Request is of unexpected type ${Request.GetType()}." );
            }
        }

        /// <inheritdoc />
        public bool HasMoreItems {
            get {
                return this.HasOkStatusCode && !string.IsNullOrEmpty( Value.MatchSearchList.NextToken );
            }
        }
    }
}
