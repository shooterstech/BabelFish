using Scopos.BabelFish.DataModel.OrionMatch;
using Scopos.BabelFish.Requests.OrionMatchAPI;

namespace Scopos.BabelFish.Responses.OrionMatchAPI {
    public class CreateTournamentAuthenticatedResponse : Response<TournamentWrapper> {

        public CreateTournamentAuthenticatedResponse( CreateTournamentAuthenticatedRequest request ) : base() {
            this.Request = request;
        }

        /// <summary>
        /// Facade function that returns the same as this.Value.Tournament.
        /// </summary>
        public Tournament Tournament {
            get { return Value.Tournament; }
        }
    }
}
