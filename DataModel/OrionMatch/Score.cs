using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BabelFish.DataModel.OrionMatch {
    [Serializable]
    public class Score {

        public Score() {
            I = 0;
            X = 0;
            D = 0;
            S = 0;
            A = 0;
            N = 0;
        }

        /// <summary>
        /// Integer Score
        /// </summary>
        public int I { get; set; }

        /// <summary>
        /// Number of inner tens
        /// </summary>
        public int X { get; set; }

        /// <summary>
        /// Decimal Score
        /// </summary>
        public float D { get; set; }

        /// <summary>
        /// Rulebook specified Score
        /// </summary>
        public float S { get; set; }

        /// <summary>
        /// Average SHot Fired
        /// </summary>
        public float A { get; set; }

        /// <summary>
        /// Number of shots fired
        /// </summary>
        public int N { get; set; }

        //The definition of the target should be done within the definition of the event
    }
}
