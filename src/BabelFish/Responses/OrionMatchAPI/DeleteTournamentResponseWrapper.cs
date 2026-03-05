using Scopos.BabelFish.DataModel;
using Scopos.BabelFish.DataModel.OrionMatch;

namespace Scopos.BabelFish.Responses.OrionMatchAPI {
    public class DeleteTournamentResponseWrapper : BaseClass {

        public DeleteTournamentResponse DeleteTournamentResponse { get; set; } = new DeleteTournamentResponse();
    }
}
