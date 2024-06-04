using Scopos.BabelFish.DataModel.ScoreHistory;
using Scopos.BabelFish.Requests.ScoreHistoryAPI;
namespace Scopos.BabelFish.Responses.ScoreHistoryAPI
{

    public class ScoreHistoryDeleteBody
    {
        public ScoreHistoryDeleteBody() { }
        public string ResultCOFID { get; set; }
        public string MatchID { get; set; }
    }

    public class DeleteScoreHistoryResponse : Response<DeleteScoreHistoryWrapper>
    {

        public DeleteScoreHistoryResponse(DeleteScoreHistoryRequest request)
        {
            this.Request = request;
        }

        public ScoreHistoryDeleteBody ScoreHistoryDelete
        {
            get { return Value.ScoreHistoryDelete; }
        }

    }
}
