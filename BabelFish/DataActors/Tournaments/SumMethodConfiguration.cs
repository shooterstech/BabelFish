using System;
using System.Collections.Generic;
using System.Text;

namespace Scopos.BabelFish.DataActors.Tournaments {
    public class SumMethodConfiguration : MergeConfiguration {

        public SumMethodConfiguration() : base() { 
        
        }

        /// <summary>
        /// If true, an event representing each participants high score will be included.
        /// </summary>
        public bool IncludeHighScoreEvent { get; set; } = false;

        /// <summary>
        /// If true, an event representing each particiants average score will be included.
        /// </summary>
        public bool IncludeAverageScoreEvent { get; set; } = false;
    }
}
