using System.ComponentModel;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using Scopos.BabelFish.DataActors.Specification.Definitions;

namespace Scopos.BabelFish.DataModel.Definitions {

    /// <summary>
    /// A TARGET defines the appearance and scoring ring dimensions of a physical and scoring target.
    /// </summary>
    [Serializable]
    public class Target : Definition {

        /// <summary>
        /// Public constructor
        /// </summary>
        public Target() : base() {
            Type = DefinitionType.TARGET;
        }

        [OnDeserialized]
        internal new void OnDeserializedMethod(StreamingContext context) {
            base.OnDeserializedMethod(context);
        }

        /// <summary>
        /// An ordered list, highest to lowest, of the scoring rings on the target.
        /// </summary>
        [JsonPropertyOrder( 10)]
        public List<ScoringRing> ScoringRings { get; set; } = new List<ScoringRing>();

        /// <summary>
        /// Definition of the inner ten ring (sometimes called 'X's or "Center Tens").
        /// </summary>
        [JsonPropertyOrder( 11)]
        public ScoringRing InnerTen { get; set; } = new ScoringRing();

        /// <summary>
        /// An ordered list, largest to smallest, of the aiming marks on the physical target.
        /// </summary>
        [JsonPropertyOrder( 12)]
        public List<AimingMark> AimingMarks { get; set; } = new List<AimingMark>();

        /// <summary>
        /// The background color of the target.
        /// </summary>
        
        [JsonPropertyOrder( 13)]
        public AimingMarkColor BackgroundColor { get; set; } = AimingMarkColor.WHITE;

        /// <summary>
        /// The expected distance that this target is shot at. Measured in mm.
        /// </summary>
        [JsonPropertyOrder ( 14 )]
        [DefaultValue( 10000 )]
        public int Distance { get; set; } = 10000;

		/// <inheritdoc />
		public override async Task<bool> GetMeetsSpecificationAsync() {
            var validation = new IsTargetValid();

			var meetsSpecification = await validation.IsSatisfiedByAsync( this );
			SpecificationMessages = validation.Messages;

			return meetsSpecification;
		}
	}
}
