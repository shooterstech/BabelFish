using System.ComponentModel;

namespace Scopos.BabelFish.DataModel.Definitions {
    public abstract class CalculationVariable : IReconfigurableRulebookObject {

		[G_NS.JsonProperty( Order = 1, DefaultValueHandling = Newtonsoft.Json.DefaultValueHandling.Include )]
		public CalculationVariableType VariableType { get; set; } = CalculationVariableType.INTEGER;

		/// <inheritdoc/>
		[DefaultValue( "" )]
		[G_NS.JsonProperty( Order = 100 )]
		public string Comment { get; set; } = string.Empty;
	}

	public class CalculationVariableInteger : CalculationVariable {

		public CalculationVariableInteger() : base() { 
			this.VariableType = CalculationVariableType.INTEGER;
		}

		[G_NS.JsonProperty( Order = 2, DefaultValueHandling = Newtonsoft.Json.DefaultValueHandling.Include )]
		public int Value { get; set; } = 0;
	}

	public class CalculationVariableFloat : CalculationVariable {

		public CalculationVariableFloat() : base() {
			this.VariableType = CalculationVariableType.FLOAT;
		}

		[G_NS.JsonProperty( Order = 2, DefaultValueHandling = Newtonsoft.Json.DefaultValueHandling.Include )]
		public float Value { get; set; } = 0;
	}

	public class CalculationVariableString : CalculationVariable {

		public CalculationVariableString() : base() {
			this.VariableType = CalculationVariableType.STRING;
		}

		[G_NS.JsonProperty( Order = 2, DefaultValueHandling = Newtonsoft.Json.DefaultValueHandling.Include )]
		public string Value { get; set; } = string.Empty;
	}

	public class CalculationVariableScoreComponent : CalculationVariable {

		public CalculationVariableScoreComponent() : base() {
			this.VariableType = CalculationVariableType.SCORE;
		}

		[G_NS.JsonProperty( Order = 2, DefaultValueHandling = Newtonsoft.Json.DefaultValueHandling.Include )]
		public ScoreComponent Value { get; set; } = ScoreComponent.I;
	}
}
