using System.ComponentModel;
using System.Runtime.Serialization;
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
		/// The type of target this is. Either a scoring ring target, hit miss target, or a test.
		/// </summary>
		[G_STJ_SER.JsonPropertyOrder( 11 )]
		[G_NS.JsonProperty( Order = 11, DefaultValueHandling = Newtonsoft.Json.DefaultValueHandling.Include )]
		public TargetModel TargetModel { get; set; } = TargetModel.SCORING_RING;

        /// <summary>
        /// An ordered list, highest to lowest, of the scoring rings on the target.
        /// </summary>
        /// <remarks>Intended for TargetType = SCORING_RING </remarks>
		[G_STJ_SER.JsonPropertyOrder( 12 )]
        [G_NS.JsonProperty( Order = 12 )]
        public List<ScoringRing> ScoringRings { get; set; } = new List<ScoringRing>();

		/// <summary>
		/// Definition of the inner ten ring (sometimes called 'X's or "Center Tens").
		/// </summary>
		/// <remarks>Intended for TargetType = SCORING_RING </remarks>
		[G_STJ_SER.JsonPropertyOrder( 13 )]
        [G_NS.JsonProperty( Order = 13 )]
        public ScoringRing InnerTen { get; set; } = new ScoringRing();

		/// <summary>
		/// An ordered list, largest to smallest, of the aiming marks on the physical target.
		/// </summary>
		/// <remarks>Intended for TargetType = SCORING_RING or HIT_MISS </remarks>
		[G_STJ_SER.JsonPropertyOrder( 14 )]
        [G_NS.JsonProperty( Order = 14 )]
        public List<AimingMark> AimingMarks { get; set; } = new List<AimingMark>();

		/// <summary>
		/// The background color of the target.
		/// </summary>
		/// <remarks>Intended for TargetType = SCORING_RING or HIT_MISS </remarks>
		[G_STJ_SER.JsonPropertyOrder( 15 )]
        [G_NS.JsonProperty( Order = 15, DefaultValueHandling = Newtonsoft.Json.DefaultValueHandling.Include )]
        public AimingMarkColor BackgroundColor { get; set; } = AimingMarkColor.WHITE;

		/// <summary>
		/// The maximum zoom that this target should be displayed in a square user interface window.
		/// The width that is shown (on the physical target) would be the width of the widest scoirng ring
		/// divided by the MaxZoom. So on an Air Rifle target, the width of the 1 ring is 45.5mm. If MaxZoom
		/// is 4.5, then 45.5 / 4.5 = 10.1mm of the target would be shown (which is about the width of the 
		/// 8 ring.
		/// </summary>
		/// <remarks>Intended for TargetType = SCORING_RING </remarks>
		[G_STJ_SER.JsonPropertyOrder( 16 )]
		[G_NS.JsonProperty( Order = 16, DefaultValueHandling = Newtonsoft.Json.DefaultValueHandling.Include )]
		[DefaultValue( 4.5f )]
		public float MaxZoom { get; set; } = 4.5f;

		/// <summary>
		/// The expected distance that this target is shot at. Measured in mm.
		/// </summary>
		/// <remarks>Intended for TargetType = SCORING_RING </remarks>
		[G_STJ_SER.JsonPropertyOrder( 20 )]
        [G_NS.JsonProperty( Order = 20 )]
        [DefaultValue( 10000 )]
        public int? Distance { get; set; } = null;

		/// <summary>
		/// The maximum value that can be awarded to this test.
		/// </summary>
		/// <remarks>Intended for TargetType = TEST </remarks>
		[G_STJ_SER.JsonPropertyOrder( 20 )]
		[G_NS.JsonProperty( Order = 20 )]
		[DefaultValue( 10000 )]
		public int? MaxValue { get; set; } = 100;

		/// <inheritdoc />
		public override async Task<bool> GetMeetsSpecificationAsync() {
            var validation = new IsTargetValid();

			var meetsSpecification = await validation.IsSatisfiedByAsync( this );
			SpecificationMessages = validation.Messages;

			return meetsSpecification;
		}
	}
}
