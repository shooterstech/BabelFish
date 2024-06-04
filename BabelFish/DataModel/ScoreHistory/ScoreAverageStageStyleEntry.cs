using System;
using System.Collections.Generic;
using System.Text;

namespace Scopos.BabelFish.DataModel.ScoreHistory {
    public class ScoreAverageStageStyleEntry : ScoreAverageEntry {

        public const int CONCRETE_CLASS_ID = 1;

        public ScoreAverageStageStyleEntry() {
            this.ConcreteClassId = CONCRETE_CLASS_ID;
        }

        /// <summary>
        /// String, formatted as a SetName, representing the Stage Style this ScoreAverageEntry represents
        /// </summary>
        public string StageStyleDef { get; set; }

        public override string ToString()
        {
            StringBuilder foo = new StringBuilder();
            foo.Append("StageStyle: ");
            foo.Append(base.StartDate.ToString("MM/dd/yyyy"));
            foo.Append(" - ");
            foo.Append(base.UserId);
            foo.Append(" - ");
            foo.Append(StageStyleDef);
            return foo.ToString();
        }
    }
}
