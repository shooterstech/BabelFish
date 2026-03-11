using Scopos.BabelFish.APIClients;
using Scopos.BabelFish.DataModel.OrionMatch;
using Scopos.BabelFish.Requests.OrionMatchAPI;

namespace Scopos.BabelFish.Responses.OrionMatchAPI {
    public class TournamentSearchAuthenticatedResponse : Response<TournamentSearchWrapper>, ITokenResponse<TournamentSearchAuthenticatedRequest> {

        public TournamentSearchAuthenticatedResponse( TournamentSearchAuthenticatedRequest request ) : base() {
            Request = request;
        }

        /// <summary>
        /// Facade function that returns the same as this.Value.TournamentSearchList.
        /// </summary>
        public TournamentSearchList TournamentSearchList {
            get { return Value.TournamentSearchList; }
        }

        /// <inheritdoc/>
        public TournamentSearchAuthenticatedRequest GetNextRequest() {
            if (!this.HasMoreItems)
                throw new NoMoreItemsException( "GetNextRequest() can not return a new request object because there are no more items to return. Always check .HasMoreItems before calling .GetNextRequest()." );

            var nextRequest = (TournamentSearchAuthenticatedRequest)Request.Copy();
            nextRequest.Token = Value.TournamentSearchList.NextToken;
            return nextRequest;
        }

        /// <inheritdoc />
        public bool HasMoreItems {
            get {
                return this.HasOkStatusCode && !string.IsNullOrEmpty( Value.TournamentSearchList.NextToken );
            }
        }
    }
}
