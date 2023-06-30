using System;
using System.Collections.Generic;
using System.Text;
using Scopos.BabelFish.DataModel.ScoreHistory;
using Scopos.BabelFish.DataModel;


namespace Scopos.BabelFish.Responses.ScoreHistoryAPI {
    public class PostScoreHistoryWrapper : BaseClass {

        public ScoreHistoryPostEntry PostScoreHistory { get; set; }
    }
}
