using System.ComponentModel;
using Scopos.BabelFish.DataModel.Definitions;
using Scopos.BabelFish.DataModel.OrionMatch;

namespace Scopos.BabelFish.DataModel.Athena {

    /// <summary>
    /// Represents the score a <see cref="Participant"/> has earned for an <see cref="Event"/>.
    /// <para>The Score class has a number of different properties to representing different ways to track the same scores (e.g. X, D, I, S, J, K, L).
    /// Which property is used to display the score for a given Event is determined by a <see cref="ScoreFormatCollection"/> ScoreFormatDefinition associated with the Event's EventType.
    /// </para>
    /// </summary>
    public class Score {

        #region Private Variables
        private float s = float.NaN;

        #endregion

        #region Constructors, factory methods, and initialization methods
        /// <summary>
        /// Constructor for Score class. Initializes all properties to 0.
        /// </summary>
        public Score() {

        }

        #endregion

        #region Data Model Properties
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
        [DefaultValue( 0 )]
        public float J { get; set; } = 0;

        /// <summary>
        /// Special use case score. Value is displayed to one decimal place. Known to be used to hold an averaged decimal score, or in Group Mode to display the Roundness of the shot group.
        /// </summary>
        [DefaultValue( 0 )]
        public float K { get; set; } = 0;

        /// <summary>
        /// Special use case score. Value is displayed to one decimal place. Known to be used to hold an averaged inner ten score, or in Group Mode to display the distance the center of the group is from the center of the target. 
        /// </summary>
        [DefaultValue( 0 )]
        public float L { get; set; } = 0;

        #endregion

        #region Helper Properties
        /// <summary>
        /// Returns a boolean indicating if this Score is 0 (all values are zero). 
        /// </summary>
        [G_STJ_SER.JsonIgnore]
        [G_NS.JsonIgnore]
        public bool IsZero {
            get {
                return (X == 0 && D == 0 && I == 0 && S == 0 && J == 0 && K == 0 && L == 0);
            }
        }

        /// <summary>
        /// This field is used internally to BabelFish only. Its value is neither written to or read from JSON.
        /// </summary>
        [G_STJ_SER.JsonIgnore]
        [G_NS.JsonIgnore]
        public int NumShotsFired { get; set; } = 0;

        #endregion

        #region Methods
        /// <summary>
        /// Newtonsoft.Json helper method to determine whether the J property should be serialized. We want to avoid serializing J
        /// if it's value is close to zero, NaN, or Infinity, as in those cases J is not being used to hold a meaningful score.
        /// </summary>
        /// <returns></returns>
        public bool ShouldSerializeJ() {
            return !(Math.Abs( J ) < .00001f || float.IsNaN( J ) || float.IsInfinity( J ));
        }

        /// <summary>
        /// Newtonsoft.json helper method to determine whether the K property should be serialized. We want to avoid serializing K
        /// if it's value is close to zero, NaN, or Infinity, as in those cases K is not being used to hold a meaningful score.
        /// </summary>
        /// <returns></returns>
        public bool ShouldSerializeK() {
            return !(Math.Abs( K ) < .00001f || float.IsNaN( K ) || float.IsInfinity( K ));
        }

        /// <summary>
        /// Newtonsoft.json helper method to determine whether the L property should be serialized. We want to avoid serializing L
        /// if it's value is close to zero, NaN, or Infinity, as in those cases L is not being used to hold a meaningful score.
        /// </summary>
        /// <returns></returns>
        public bool ShouldSerializeL() {
            return !(Math.Abs( L ) < .00001f || float.IsNaN( L ) || float.IsInfinity( L ));
        }

        /// <summary>
        /// Returns a variation of the Score class where the X, D, and I properties have been averaged by the number of shots fired (NumShotsFired). 
        /// </summary>
        /// <returns></returns>
        public AveragedScore GetAvgShotFired() {
            if (NumShotsFired == 0) return new AveragedScore();
            return new AveragedScore {
                X = ((float)X) / NumShotsFired,
                D = D / NumShotsFired,
                I = ((float)I) / NumShotsFired
            };
        }

        /// <summary>
        /// Method to turn a score to 0. Most often used in cases where the participant received a DSQ, and thus there score is 0. 
        /// </summary>
        public void MakeScoreZero() {
            X = 0;
            D = 0;
            I = 0;
            S = 0;
            J = 0;
            K = 0;
            L = 0;
        }

        /// <summary>
        /// Returns a string representation of the current object using the decimal score component format.
        /// </summary>
        public override string ToString() {
            return ToString( ScoreComponent.D );
        }

        /// <summary>
        /// Returns a string representation of the current object using the specified score component format. 
        /// </summary>
        /// <param name="scoreComponent">The score component to use for formatting.</param>
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

        /// <summary>
        /// Returns a string representing of this Score using the score format specified by the passed in ScoreFormatCollection and scoreConfigName.
        /// The method looks up the ScoreConfig in the ScoreFormatCollection with a ScoreConfigName matching the passed in scoreConfigName,
        /// then uses the ScoreComponent specified in that ScoreConfig to determine how to format the Score for the string representation.
        /// </summary>
        /// <param name="scoreFormatDefinition"></param>
        /// <param name="scoreConfigName"></param>
        /// <returns></returns>
        public string ToString( ScoreFormatCollection scoreFormatDefinition, string scoreConfigName ) {
            foreach (var scoreConfig in scoreFormatDefinition.ScoreConfigs) {
                if (scoreConfig.ScoreConfigName == scoreConfigName) {
                    return this.ToString( scoreConfig.ScoreComponent );
                }
            }

            //Shouldn't ever get here, but just in case
            return this.ToString( ScoreComponent.D );
        }

        /// <summary>
        /// Returns just the score component specified by the passed in ScoreComponent enum. E.g. if ScoreComponent.I is passed in, this method returns just the I property of this Score object.
        /// </summary>
        /// <param name="scoreComponent"></param>
        /// <returns></returns>
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

        #endregion

        #region Operator Overloads
        /// <summary>
        /// Operator overload for adding two scores together.
        /// <para>Each score component is added individually.</para>
        /// <para>To have control over how the S component is calculated, use the Add() method with a specific ScoreComponent.</para>
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Divide operator.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static Score operator /( Score left, int right ) {
            //EKA QUESTION: Oct 2025: Should the devide operator return a Score or an AveragedScore ? 

            if (left is null || left.IsZero || right == 0)
                return new Score();

            return new Score {
                X = left.X / right,
                D = left.D / right,
                I = left.I / right,
                S = left.S / right,
                J = left.J / right,
                K = left.K / right,
                L = left.L / right,
                NumShotsFired = left.NumShotsFired, //Its debatable how .NumberShotsFired should be calculated.
            };
        }

        /// <summary>
        /// Add method that uses the specified ScoreComponent to determine how to calculate the S property of this Score when adding the right Score to this Score.
        /// </summary>
        /// <param name="right"></param>
        /// <param name="s"></param>
        public void Add( Score right, ScoreComponent s ) {
            this.I += right.I;
            this.X += right.X;
            this.D = (float)Math.Round( this.D + right.D, 1 );
            // J, K, and L are all special use case score components that are only used in certain cases, and are not always used to hold meaningful scores. Thus, when adding them together we want to round the result to 5 decimal places.
            this.J = (float)Math.Round( this.J + right.J, 5 );
            this.K = (float)Math.Round( this.K + right.K, 5 );
            this.L = (float)Math.Round( this.L + right.L, 5 );
            this.NumShotsFired += right.NumShotsFired;

            // If S is NaN, that means it hasn't been set yet. Since we are about to assign it, we need to make sure to set it to 0 before we add to it
            // Otherwise .S will return the .D value when we go to add to it, which will throw off the score calculation.
            if (float.IsNaN( this.s ))
                this.s = 0;

            //The S is for speical sum
            switch (s) {
                case ScoreComponent.I:
                    this.S = (float)Math.Round( this.S + right.I, 5 );
                    break;
                case ScoreComponent.X:
                    this.S = (float)Math.Round( this.S + right.X, 5 );
                    break;
                case ScoreComponent.D:
                    this.S = (float)Math.Round( this.S + right.D, 5 );
                    break;
                case ScoreComponent.S:
                    this.S = (float)Math.Round( this.S + right.S, 5 );
                    break;
                case ScoreComponent.J:
                    this.S = (float)Math.Round( this.S + right.J, 5 );
                    break;
                case ScoreComponent.K:
                    this.S = (float)Math.Round( this.S + right.K, 5 );
                    break;
                case ScoreComponent.L:
                    this.S = (float)Math.Round( this.S + right.L, 5 );
                    break;
            }
        }

        #endregion
    }
}
