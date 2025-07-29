using BabelFish.DataModel.Athena;
using Scopos.BabelFish.DataModel.Definitions;

namespace Scopos.BabelFish.DataModel.Athena {
    [Serializable]
    public class Score {

        private float s = float.NaN;
        public Score() {

        }

        /// <summary>
        /// Number of inner tens.
        /// </summary>
        [G_STJ_SER.JsonInclude]
        [G_NS.JsonProperty( DefaultValueHandling = Newtonsoft.Json.DefaultValueHandling.Include )]
        public int X { get; set; } = 0;

        /// <summary>
        /// Score in decimal value
        /// </summary>
        [G_STJ_SER.JsonInclude]
        [G_NS.JsonProperty( DefaultValueHandling = Newtonsoft.Json.DefaultValueHandling.Include )]
        public float D { get; set; } = 0;

        /// <summary>
        /// Score in integer value
        /// </summary>
        [G_STJ_SER.JsonInclude]
        [G_NS.JsonProperty( DefaultValueHandling = Newtonsoft.Json.DefaultValueHandling.Include )]
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

        /// <summary>
        /// Returns a boolean indicating if this Score is 0 (all values are zero). 
        /// </summary>
        [G_STJ_SER.JsonIgnore]
        public bool IsZero {
            get {
                return (X == 0 && D == 0 && I == 0 && S == 0 && J == 0 && K == 0 && L == 0);
            }
        }

        /// <summary>
        /// This field is used internally to BabelFish only. Its value is neither written to or read from JSON.
        /// </summary>
        [G_STJ_SER.JsonIgnore]
        public int NumShotsFired { get; set; } = 0;

        public AveragedScore GetAvgShotFired() {
            if (NumShotsFired == 0) return new AveragedScore();
            return new AveragedScore {
                X = ((float)X) / NumShotsFired,
                D = D / NumShotsFired,
                I = ((float)I) / NumShotsFired
            };
        }

        public static Score operator +( Score left, Score right ) {
            return new Score {
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

        public void Add( Score right, ScoreComponent s ) {
            this.X += right.X;
            this.D += right.D;
            this.I += right.I;
            this.J += right.J;
            this.K += right.K;
            this.L += right.L;
            this.NumShotsFired += right.NumShotsFired;

            //The S is for speical sum
            switch (s) {
                case ScoreComponent.I:
                    this.S += right.I;
                    break;
                case ScoreComponent.X:
                    this.S += right.X;
                    break;
                case ScoreComponent.D:
                    this.S += right.D;
                    break;
                case ScoreComponent.S:
                    this.S += right.S;
                    break;
                case ScoreComponent.J:
                    this.S += right.J;
                    break;
                case ScoreComponent.K:
                    this.S += right.K;
                    break;
                case ScoreComponent.L:
                    this.S += right.L;
                    break;
            }
        }

		public override string ToString() {
			return ToString( ScoreComponent.D );
		}

        public string ToString( ScoreComponent scoreComponent ) {

            switch (scoreComponent) {
                case ScoreComponent.D:
                default:
                    return this.D.ToString( "F1" );
				case ScoreComponent.I:
					return this.I.ToString();
				case ScoreComponent.X:
                    return this.X.ToString();
                case ScoreComponent.S:
                    return this.S.ToString( "F1" );
                case ScoreComponent.J:
                    return this.J.ToString( "F5" );
                case ScoreComponent.K:
                    return this.K.ToString( "F5" );
                case ScoreComponent.L:
                    return this.L.ToString( "F5" );
            }
        }

        public string ToString( ScoreFormatCollection scoreFormatDefinition, string scoreConfigName ) {
            foreach (var scoreConfig in scoreFormatDefinition.ScoreConfigs) {
                if (scoreConfig.ScoreConfigName == scoreConfigName) {
                    return this.ToString( scoreConfig.ScoreComponent );
                }
            }

            //Shouldn't ever get here, but just in case
            return this.ToString( ScoreComponent.D );
        }

        public float GetScoreComponentScore( ScoreComponent scoreComponent ) {

			switch (scoreComponent) {
				case ScoreComponent.D:
				default:
					return this.D;
				case ScoreComponent.I:
					return this.I;
				case ScoreComponent.X:
					return this.X;
				case ScoreComponent.S:
					return this.S;
				case ScoreComponent.J:
					return this.J;
				case ScoreComponent.K:
					return this.K;
				case ScoreComponent.L:
					return this.L;
			}

		}
	}
}