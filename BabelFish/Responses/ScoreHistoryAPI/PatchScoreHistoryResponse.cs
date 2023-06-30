using Scopos.BabelFish.DataModel.ScoreHistory;
using Scopos.BabelFish.Requests.ScoreHistoryAPI;
namespace Scopos.BabelFish.Responses.ScoreHistoryAPI {
    public class PatchScoreHistoryResponse : Response<PostScoreHistoryWrapper> {

        public PatchScoreHistoryResponse( PatchScoreHistoryRequest request) {
            this.Request = request;
        }

        public ScoreHistoryPostEntry PatchScoreHistory //use same wrapper as post score history
        {
            get { return Value.PostScoreHistory; }
        }

    }
}
