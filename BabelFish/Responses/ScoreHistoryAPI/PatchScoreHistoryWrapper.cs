using System;
using System.Collections.Generic;
using System.Text;
using Scopos.BabelFish.DataModel.ScoreHistory;
using Scopos.BabelFish.DataModel;


namespace Scopos.BabelFish.Responses.ScoreHistoryAPI
{
    public class PatchScoreHistoryWrapper : BaseClass
    {

        public ScoreHistoryPostEntry ScoreHistoryPatch { get; set; }
    }
}
