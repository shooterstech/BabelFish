using System.ComponentModel;
using Scopos.BabelFish.DataModel.Athena;

namespace Scopos.BabelFish.DataModel.Definitions {

    /// <summary>
    /// CalculationVariables provide additional information that <see cref="ShotMapper""/> uses to calculate the <see cref="Score"/> of an <see cref="Event"/>.
    /// <para>The Type of <see cref="CalculationVariable"/> is dependent on the value of the Event's <see cref="Calculation"/>.
    /// For example, if the Event's Calculation is AVERAGE, then the CalculationVariable should be of type INTEGER, specifying the number of shots to average together when calculating the score for this Event.</para>
    /// </summary>
    public abstract class CalculationVariable : IReconfigurableRulebookObject {

        /// <summary>
        /// Concrete class identifier. 
        /// </summary>

        [G_NS.JsonProperty( Order = 1, DefaultValueHandling = Newtonsoft.Json.DefaultValueHandling.Include )]
        public CalculationVariableType VariableType { get; set; } = CalculationVariableType.INTEGER;

        /// <inheritdoc/>
        [DefaultValue( "" )]
        [G_NS.JsonProperty( Order = 100 )]
        public string Comment { get; set; } = string.Empty;
    }

    /// <summary>
    /// Specifies a hard coded variable of type integer.
    /// </summary>
	public class CalculationVariableInteger : CalculationVariable {

        /// <summary>
        /// Constructor.
        /// </summary>
		public CalculationVariableInteger() : base() {
            this.VariableType = CalculationVariableType.INTEGER;
        }

        /// <summary>
        /// The integer value of this variable. 
        /// </summary>
		[G_NS.JsonProperty( Order = 2, DefaultValueHandling = Newtonsoft.Json.DefaultValueHandling.Include )]
        public int Value { get; set; } = 0;
    }

    /// <summary>
    /// Specifies a hard coded variable of type float.
    /// </summary>
	public class CalculationVariableFloat : CalculationVariable {

        /// <summary>
        /// Constructor
        /// </summary>
		public CalculationVariableFloat() : base() {
            this.VariableType = CalculationVariableType.FLOAT;
        }

        /// <summary>
        /// The float value of this variable.
        /// </summary>
		[G_NS.JsonProperty( Order = 2, DefaultValueHandling = Newtonsoft.Json.DefaultValueHandling.Include )]
        public float Value { get; set; } = 0;
    }

    /// <summary>
    /// Specifies a hard coded string value.
    /// </summary>
	public class CalculationVariableString : CalculationVariable {

        /// <summary>
        /// Constructor
        /// </summary>
		public CalculationVariableString() : base() {
            this.VariableType = CalculationVariableType.STRING;
        }

        /// <summary>
        /// The stirng value of this variable.
        /// </summary>
		[G_NS.JsonProperty( Order = 2, DefaultValueHandling = Newtonsoft.Json.DefaultValueHandling.Include )]
        public string Value { get; set; } = string.Empty;
    }

    /// <summary>
    /// Specifies a score component to use in the calculation of the score for an Event.
    /// <para>For example, when an <see cref="Event"/> <see cref="Event.Calculation"/> is set SUM, CalculationVariableScoreComponent
    /// may be used to specify how the Special Sum (S) <see cref="Score"/> component is calculated.</para>
    /// </summary>
	public class CalculationVariableScoreComponent : CalculationVariable {

        /// <summary>
        /// Constructor
        /// </summary>
		public CalculationVariableScoreComponent() : base() {
            this.VariableType = CalculationVariableType.SCORE;
        }

        /// <summary>
        /// The ScoreComponent to use in the calculation of the <see cref="Score"/> for an Event.
        /// </summary>
		[G_NS.JsonProperty( Order = 2, DefaultValueHandling = Newtonsoft.Json.DefaultValueHandling.Include )]
        public ScoreComponent Value { get; set; } = ScoreComponent.I;
    }
}
