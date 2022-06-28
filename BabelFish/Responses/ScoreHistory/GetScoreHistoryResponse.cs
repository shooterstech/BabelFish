using Newtonsoft.Json.Linq;
using ShootersTech.Responses;
using ShootersTech.DataModel.ScoreHistory;
using ShootersTech.Requests.ScoreHistoryAPI;

namespace ShootersTech.Responses.ScoreHistoryAPI {
    public class GetScoreHistoryResponse : Response<ScoreHistory<ScoreHistoryBase>>
    {

        public GetScoreHistoryResponse( GetScoreHistoryRequest request) {
            this.Request = request;
        }

        public ScoreHistory<ScoreHistoryBase> ScoreHistory
        {
            get { return Value; }
        }

        /// <inheritdoc/>
        protected override void ConvertBodyToValue() {

            /*
             * .Value can be one of four types. 
             *  1. ScoreHistoryEventStyleEntry
             *  2. ScoreHistoryStageStyleEntry
             *  3. ScoreHistoryEventStyleTimespan
             *  4. ScoreHisotryStageStyleTimespan
             *  The type depends on the structure of the returned data. Which
             *  is dependent on the parameters sent to the API.
             */

            var scoreHistoryReqeust = (GetScoreHistoryRequest)this.Request;

            if (scoreHistoryReqeust.Format == ScoreHistoryFormatOptions.DAY) {
                //The request was for each score fired, not a summation over a series of days.
                if (scoreHistoryReqeust.EventStyle != null ) {
                    var foo = Body.ToObject<ScoreHistory<ScoreHistoryEventStyleEntry>>();
                    Value = (ScoreHistory<ScoreHistoryBase>)foo;
                }
            } else {
                //The request was for a summation over a series of days. The data will be in the "Timespan" format
            }
        }
    }
}
