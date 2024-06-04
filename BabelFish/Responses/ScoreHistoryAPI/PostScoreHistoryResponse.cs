using Scopos.BabelFish.DataModel.ScoreHistory;
using Scopos.BabelFish.Requests.ScoreHistoryAPI;
namespace Scopos.BabelFish.Responses.ScoreHistoryAPI {
    public class PostScoreHistoryResponse : Response<PostScoreHistoryWrapper> {

        public PostScoreHistoryResponse( PostScoreHistoryRequest request) {
            this.Request = request;
        }

        public ScoreHistoryPostEntry ScoreHistoryPost
        {
            get { return Value.ScoreHistoryPost; }
        }

    }
}
