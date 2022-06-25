using System;
using System.Collections.Generic;
using System.Text;

namespace ShootersTech.DataModel.ScoreHistory {

    /// <summary>
    /// Represents an athletes averaged score over a given time span.
    /// </summary>
    public class AveragedScore {

        /// <summary>
        /// Public consructor
        /// </summary>
        public AveragedScore() { }

        /// <summary>
        /// Average number of inner tens.
        /// </summary>
        public float X { get; set; }

        /// <summary>
        /// Averaged decimal score value
        /// </summary>
        public float D { get; set; }

        /// <summary>
        /// Averaged integer score value
        /// </summary>
        public float I { get; set; }
    }
}
