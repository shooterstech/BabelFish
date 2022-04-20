using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShootersTech.DataModel.Athena {
    public class Score {
        public Score() {

        }

        /// <summary>
        /// Number of inner tens.
        /// </summary>
        public int X { get; set; }

        /// <summary>
        /// Score in decimal value
        /// </summary>
        public float D { get; set; }

        /// <summary>
        /// Score in integer value
        /// </summary>
        public int I { get; set; }

        private float s = float.NaN;
        /// <summary>
        /// Special Sum score
        /// </summary>
        public float S {
            get {
                if (float.IsNaN(s))
                    return D;
                else
                    return s;
            }
            set {
                s = value;
            }
        }
    }
}
