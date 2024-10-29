using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scopos.BabelFish.DataModel.Definitions {
    /// <summary>
    /// An Aiming Mark object defines the dimension, shape, and color of the mark an athlete aims on on a TARGET.
    /// </summary>
    [Serializable]
    public class ScoringRing : ScoringShapeDimension, ICopy<ScoringRing> {

        /// <summary>
        /// Public Constructor
        /// </summary>
        public ScoringRing() { }

        public ScoringRing Copy() {

            ScoringRing copy = new ScoringRing();
            copy.Shape = this.Shape;
            copy.Value = this.Value;
            copy.Comment = this.Comment;
            copy.Dimension = this.Dimension;

            return copy;
        }


        /// <summary>
        /// The value, in points, that the athelte earns by hitting this scoring ring.
        /// </summary>
        public int Value { get; set; } = 0;
    }
}
