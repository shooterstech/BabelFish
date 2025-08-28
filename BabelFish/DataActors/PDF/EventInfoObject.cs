using Scopos.BabelFish.DataModel.Athena.Shot;
using Scopos.BabelFish.DataModel.Definitions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Scopos.BabelFish.DataActors.PDF
{
    public class EventInfoObject
    {
        public EventComposite eventComposite { get; set; }

        public byte[] ObjectImage { get; set; }

        public GroupAnalysisMaths GroupMaths { get; set; }

        public List<Shot> ShotList { get; set; }

        public string ScoreFormatted { get; set; }

        public string EventLabel { get; set; }

        public Target TargetDef { get; set; }
    }
}
