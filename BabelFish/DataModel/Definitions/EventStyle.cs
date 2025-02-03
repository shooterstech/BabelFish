using System.Runtime.Serialization;
using Scopos.BabelFish.DataActors.Specification.Definitions;

namespace Scopos.BabelFish.DataModel.Definitions {
    /// <summary>
    /// An Event Style defines a type of shooting.
    /// <para>When defining Event Styles the following principles should be employed:</para>
    /// <list type="bullet">
    /// <item>Attributes should not be included in defining an Event Style unless the Attribute refers to different equipment classes. For example, in the ISSF Men’s and Women’s air rifle should not be two different Event Styles since both events are ran by the exact same rules. However, in three-position air rifle there should be separate Event Styles for Sporter and Precision since they represent fundamentally different equipment.</item>
    /// <item>Team events should not have their own Event Styles if the individual and team event have the exact same Stage Styles.</item>
    /// <item>Events that could be shot at similar but different distances should not be given different Event Styles. Unless again they represent different equipment classes or the difference in distance is sufficiently large. For example shooting at 50m is close enough to 50yds. that the two may be given the same Event Style. However, shooting at 50ft since it is indoors and 1/3rd the distance would be a different Event Style.</item>
    /// </list>
    /// </summary>
    [Serializable]
    public class EventStyle : Definition {

        /// <summary>
        /// Public constructor
        /// </summary>
        public EventStyle() : base() {
            Type = DefinitionType.EVENTSTYLE;

            //Don't initialize EventStyles or StageStyles, since one of these values has to be null.
        }

        [OnDeserialized]
        internal new void OnDeserializedMethod(StreamingContext context) {
            base.OnDeserializedMethod(context);

            //Don't initialize EventStyles or StageStyles, since one of these values as to be null.
        }

        /// <summary>
        /// An ordered list of EVENT STYLEs that comprise this EVENT STYLE. Each Event Style is listed by its SetName.
        /// Either EVENT STYLEs or StageStyles, but not both, is required. If included at least one EVENT STYLE must be listed
        /// </summary>
		[G_STJ_SER.JsonPropertyOrder( 11 )]
        [G_NS.JsonProperty( Order = 11 )]
        public List<string>? EventStyles { get; set; } = new List<string> ();

        /// <summary>
        /// An ordered list of STAGE STYLEs that comprise the EVENT STYLE. Each STAGE STYLE is listed by its SetName.
        /// Either EventStyles or StageStyles, but not both, is required. If included at least one STAGE STYLE must be listed.
        /// </summary>
		[G_STJ_SER.JsonPropertyOrder( 12 )]
        [G_NS.JsonProperty( Order = 12 )]
        public List<string>? StageStyles { get; set; } = new List<string>();

        /// <summary>
        /// A list (order is inconsequential) of other EVENT STYLEs that are similar to this EVENT STYLE.
        /// </summary>
		[G_STJ_SER.JsonPropertyOrder( 13 )]
        [G_NS.JsonProperty( Order = 13 )]
        public List<string> RelatedEventStyles { get; set; } = new List<string>();

        /// <summary>
        /// A list of SimpleCOF. This lists the common ways to displaying scores from this EVENT STYLE.
        /// </summary>
		[G_STJ_SER.JsonPropertyOrder( 14 )]
        [G_NS.JsonProperty( Order = 14 )]
        public List<SimpleCOF> SimpleCOFs { get; set; } = new List<SimpleCOF>();

		/// <inheritdoc />
		public override async Task<bool> GetMeetsSpecificationAsync() {
			var validation = new IsEventStyleValid();

			var meetsSpecification = await validation.IsSatisfiedByAsync( this );
			SpecificationMessages = validation.Messages;

			return meetsSpecification;
		}

	}
}
