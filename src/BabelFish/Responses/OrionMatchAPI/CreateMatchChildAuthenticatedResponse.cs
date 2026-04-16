using Scopos.BabelFish.DataModel.OrionMatch;
using Scopos.BabelFish.Requests.OrionMatchAPI;

namespace Scopos.BabelFish.Responses.OrionMatchAPI {
    public class CreateMatchChildAuthenticatedResponse : Response<MatchChildWrapper> {

        public CreateMatchChildAuthenticatedResponse( CreateMatchChildAuthenticatedRequest request ) : base() {
            Request = request;
        }

        /// <summary>
        /// Facade function that returns the same as this.Value.MatchChild.
        /// </summary>
        public MatchChild MatchChild {
            get { return Value.MatchChild; }
        }
    }
}
