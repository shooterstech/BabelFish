using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scopos.BabelFish.DataModel.Athena {
    [Serializable]
    public class Score {

        private float s = float.NaN;
        public Score() {

        }

        /// <summary>
        /// Number of inner tens.
        /// </summary>
        public int X { get; set; } = 0;

        /// <summary>
        /// Score in decimal value
        /// </summary>
        public float D { get; set; } = 0;

        /// <summary>
        /// Score in integer value
        /// </summary>
        public int I { get; set; } = 0;


        /// <summary>
        /// Special Sum score. Usually used in an Event to add the Integer value
        /// from one child Event with the Decimal value from a different child Event.
        /// ONly applicable to Scores from Event Stypes == EVENT. 
        /// </summary>
        public float S {
            get {
                if (float.IsNaN( s ))
                    return D;
                else
                    return s;
            }
            set {
                s = value;
            }
        }

        /// <summary>
        /// Special use case score. Value is displayed to one decimal place. Known to be used to hold a averaged integer score, or in Group Mode to display the Area of the shot group. 
        /// </summary>
        public float J { get; set; } = 0;

        /// <summary>
        /// Special use case score. Value is displayed to one decimal place. Known to be used to hold an averaged decimal score, or in Group Mode to display the Roundness of the shot group.
        /// </summary>
        public float K { get; set; } = 0;

        /// <summary>
        /// Special use case score. Value is displayed to one decimal place. Known to be used to hold an averaged inner ten score, or in Group Mode to display the distance the center of the group is from the center of the target. 
        /// </summary>
        public float L { get; set; } = 0;

    }
}