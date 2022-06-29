using System;
using System.Collections.Generic;
using System.Text;

namespace ShootersTech.DataModel.ScoreHistory {
    public class ScoreAverageStageStyleEntry : ScoreAverageEntry {

        public const int CONCRETE_CLASS_ID = 1;

        public ScoreAverageStageStyleEntry() {
            this.ConcreteClassId = CONCRETE_CLASS_ID;
        }

    }
}
