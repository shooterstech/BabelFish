using Scopos.BabelFish.DataModel.OrionMatch;
using Scopos.BabelFish.Requests.OrionMatchAPI;

namespace Scopos.BabelFish.Responses.OrionMatchAPI {
    public class AddTournamentMemberAuthenticatedResponse : Response<TournamentMemberWrapper> {

        public AddTournamentMemberAuthenticatedResponse( AddTournamentMemberAuthenticatedRequest request ) : base() {
            this.Request = request;
        }

        /// <summary>
        /// Facade function that returns the same as this.Value.TournamentMember.
        /// </summary>
        public TournamentMember TournamentMember {
            get { return Value.TournamentMember; }
        }
    }
}
