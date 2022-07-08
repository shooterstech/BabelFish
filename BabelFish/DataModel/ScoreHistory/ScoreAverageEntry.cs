using System;
using System.Collections.Generic;
using System.Text;

namespace ShootersTech.DataModel.ScoreHistory {
    public class ScoreAverageEntry : ScoreAverageBase {

        public ScoreAverageEntry() : base() {
        }

        public AveragedScore ScoreAverage { get; set; }
    }
}
