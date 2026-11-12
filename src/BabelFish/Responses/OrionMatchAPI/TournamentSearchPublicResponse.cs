using Scopos.BabelFish.Requests.OrionMatchAPI;

namespace Scopos.BabelFish.Responses.OrionMatchAPI {
    public class TournamentSearchPublicResponse : TournamentSearchAbstractResponse {

        public TournamentSearchPublicResponse( TournamentSearchPublicRequest request ) : base() {
            Request = request;
        }

        /// <inheritdoc />
        public new TournamentSearchPublicRequest GetNextRequest() {
            return (TournamentSearchPublicRequest)base.GetNextRequest();
        }
    }
}
