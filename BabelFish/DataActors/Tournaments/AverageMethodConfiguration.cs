using System;
using System.Collections.Generic;
using System.Text;

namespace Scopos.BabelFish.DataActors.Tournaments {
    public class AverageMethodConfiguration : MergeConfiguration {

        public AverageMethodConfiguration() : base() {

            /*
             * NOTE: MergeConfiguration classes use the same concrete class identifier
             * as the cooresponding MergeMethod classes.
             */

            this.Method = "Average";
        }

        /// <summary>
        /// If true, an event representing each participants high score will be included.
        /// </summary>
        public bool AddHighScoreEvent { get; set; } = false;

        /// <summary>
        /// If true, scores with a DNF are excluded when calculating a participant's average.
        /// </summary>
        public bool ExcludeDNFFromAverage { get; set; } = false;
    }
}
