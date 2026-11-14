using Scopos.BabelFish.APIClients;
using Scopos.BabelFish.DataModel.OrionMatch;
using Scopos.BabelFish.Requests.OrionMatchAPI;

namespace Scopos.BabelFish.Responses.OrionMatchAPI {
    public abstract class TournamentSearchAbstractResponse : Response<TournamentSearchWrapper>, ITokenResponse<TournamentSearchAbstractRequest> {

        public TournamentSearchAbstractResponse() : base() {
        }

        /// <summary>
        /// Facade function that returns the same as this.Value.TournamentSearchList.
        /// </summary>
        public TournamentSearchList TournamentSearchList {
            get { return Value.TournamentSearchList; }
        }

        /// <inheritdoc/>
        public TournamentSearchAbstractRequest GetNextRequest() {
            if (!this.HasMoreItems)
                throw new NoMoreItemsException( "GetNextRequest() can not return a new request object because there are no more items to return. Always check .HasMoreItems before calling .GetNextRequest()." );

            if (Request is TournamentSearchPublicRequest) {
                var nextRequest = (TournamentSearchPublicRequest)Request.Copy();
                nextRequest.Token = Value.TournamentSearchList.NextToken;
                return nextRequest;
            } else if (Request is TournamentSearchAuthenticatedRequest) {
                var nextRequest = (TournamentSearchAuthenticatedRequest)Request.Copy();
                nextRequest.Token = Value.TournamentSearchList.NextToken;
                return nextRequest;
            } else {
                throw new ArgumentException( $"Parameter Request is of unexpected type ${Request.GetType()}." );
            }
        }

        /// <inheritdoc />
        public bool HasMoreItems {
            get {
                return this.HasOkStatusCode && !string.IsNullOrEmpty( Value.TournamentSearchList.NextToken );
            }
        }
    }
}
