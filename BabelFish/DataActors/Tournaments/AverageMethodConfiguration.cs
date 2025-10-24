using System;
using System.Collections.Generic;
using System.Text;

namespace Scopos.BabelFish.DataActors.Tournaments {
    public class AverageMethodConfiguration : MergeConfiguration {

        public AverageMethodConfiguration() : base() { 
        
        }

        /// <summary>
        /// If true, an event representing each participants high score will be included.
        /// </summary>
        public bool IncludeHighScoreEvent { get; set; } = false;
    }
}
