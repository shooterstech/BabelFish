using Scopos.BabelFish.DataModel.Definitions;

namespace Scopos.BabelFish.DataModel.Athena {

    /// <summary>
    /// Represents an athletes averaged score over a given time span.
    /// </summary>
    public class AveragedScore {

        /// <summary>
        /// Public consructor
        /// </summary>
        public AveragedScore() { }

        public AveragedScore( Score score ) {
            X = score.X;
            D = score.D;
            I = score.I;
        }

        /// <summary>
        /// Average number of inner tens.
        /// </summary>
        public float X { get; set; } = 0.0f;

        /// <summary>
        /// Averaged decimal score value
        /// </summary>
        public float D { get; set; } = 0.0f;

        /// <summary>
        /// Averaged integer score value
        /// </summary>
        public float I { get; set; } = 0.0f;

        [G_STJ_SER.JsonIgnore]
        public bool IsZero {
            get {
                return X == 0 && D == 0 && I == 0;
            }
        }

        public static AveragedScore operator +( AveragedScore left, AveragedScore right ) {
            return new AveragedScore() {
                X = left.X + right.X,
                D = left.D + right.D,
                I = left.I + right.I
            };
        }

        public static AveragedScore operator /( AveragedScore left, float right ) {
            return new AveragedScore() {
                X = left.X / right,
                D = left.D / right,
                I = left.I / right
            };
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
			}

		}
	}
}
