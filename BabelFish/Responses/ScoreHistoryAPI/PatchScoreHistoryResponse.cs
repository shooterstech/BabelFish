using Scopos.BabelFish.DataModel.ScoreHistory;
using Scopos.BabelFish.Requests.ScoreHistoryAPI;
namespace Scopos.BabelFish.Responses.ScoreHistoryAPI {
    public class PatchScoreHistoryResponse : Response<PatchScoreHistoryWrapper> {

        public PatchScoreHistoryResponse( PatchScoreHistoryRequest request) {
            this.Request = request;
        }

        public ScoreHistoryPostEntry ScoreHistoryPatch //use same wrapper as post score history
        {
            get { return Value.ScoreHistoryPatch; }
        }

    }
}
