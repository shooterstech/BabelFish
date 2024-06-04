using Newtonsoft.Json;
using Scopos.BabelFish.DataModel.ScoreHistory;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Include)]
        public int X { get; set; } = 0;

        /// <summary>
        /// Score in decimal value
        /// </summary>
        [JsonProperty( DefaultValueHandling = DefaultValueHandling.Include )]
        public float D { get; set; } = 0;

        /// <summary>
        /// Score in integer value
        /// </summary>
        [JsonProperty( DefaultValueHandling = DefaultValueHandling.Include )]
        public int I { get; set; } = 0;


        /// <summary>
        /// Special Sum score. Usually used in an Event to add the Integer value
        /// from one child Event with the Decimal value from a different child Event.
        /// ONly applicable to Scores from Event Stypes == EVENT. 
        /// </summary>
        [JsonProperty( DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate )]
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
        [JsonProperty( DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate )]
        public float J { get; set; } = 0;

        /// <summary>
        /// Special use case score. Value is displayed to one decimal place. Known to be used to hold an averaged decimal score, or in Group Mode to display the Roundness of the shot group.
        /// </summary>
        [JsonProperty( DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate )]
        public float K { get; set; } = 0;

        /// <summary>
        /// Special use case score. Value is displayed to one decimal place. Known to be used to hold an averaged inner ten score, or in Group Mode to display the distance the center of the group is from the center of the target. 
        /// </summary>
        [JsonProperty( DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate )]
        public float L { get; set; } = 0;

        /// <summary>
        /// Returns a boolean indicating if this Score is 0 (all values are zero). 
        /// </summary>
        [JsonIgnore]
        public bool IsZero {
            get {
                return (X == 0 && D == 0 && I == 0 && S == 0 && J == 0 && K == 0 && L == 0);
            }
        }

        [JsonIgnore]
        public int NumShotsFired { get; set; } = 0;

        public AveragedScore GetAvgShotFired()
        {
            if (NumShotsFired == 0) return new AveragedScore();
            return new AveragedScore
            {
                X = ((float) X) / NumShotsFired,
                D = D / NumShotsFired,
                I = ((float)I) / NumShotsFired
            };
        }

        public static Score operator +( Score left, Score right)
        {
            return new Score
            {
                X = left.X + right.X,
                D = left.D + right.D,
                I = left.I + right.I,
                S = left.S + right.S,
                J = left.J + right.J,
                K = left.K + right.K,
                L = left.L + right.L,
                NumShotsFired = left.NumShotsFired + right.NumShotsFired,
            };
        }


    }
}