using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BabelFish.DataModel.OrionMatch {
    [Serializable]
    [Obsolete("Use ShootersTech.DataModel.Athena.Shot.Shot instead.")]
    public class Score {

        public Score() { }

        /// <summary>
        /// Number of inner tens
        /// </summary>
        public int X { get; set; } = 0;

        /// <summary>
        /// Integer Score
        /// </summary>
        public int I { get; set; } = 0;

        /// <summary>
        /// Rulebook specified Score
        /// </summary>
        public float S { get; set; } = 0;

        /// <summary>
        /// Decimal Score
        /// </summary>
        public float D { get; set; } = 0;

        /// <summary>
        /// Average SHot Fired
        /// </summary>
        public float A { get; set; } = 0;

        /// <summary>
        /// Number of shots fired
        /// </summary>
        public int N { get; set; } = 0;

        /// <summary>
        /// Score variable. Meaning, we're not quite sure what value it is going to hold, if any.
        /// </summary>
        public float V { get; set; } = 0f;

        //The definition of the target should be done within the definition of the event
    }
}
