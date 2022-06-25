using Newtonsoft.Json.Linq;
using ShootersTech.Responses;
using ShootersTech.DataModel.ScoreHistory;

namespace ShootersTech.Responses.ScoreAPI
{
    public class GetScoreHistoryResponse : Response<ScoreHistory>
    {

        public T ScoreHistory
        {
            get { return Value; }
        }
    }
}
