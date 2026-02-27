using System.ComponentModel;
using Scopos.BabelFish.APIClients;

namespace Scopos.BabelFish.DataModel.Definitions {

    /// <summary>
    /// A "SimpleCOF" defines the type of StageStyles that make it up and the number of 
    /// shots per StageStyle. It does not define any structure, time limites, ranking rules
    /// etc. 
    /// </summary>
    [Serializable]
    public class SimpleCOF : ITelerikBindModel, IReconfigurableRulebookObject, IGetCourseOfFireDefinition {
        /// <summary>
        /// Public constructor
        /// </summary>
        public SimpleCOF() { }

        /// <summary>
        /// The set name of the Course of Fire definition that this simple COF is emulating.
        /// </summary>
		[G_STJ_SER.JsonPropertyOrder( 1 )]
        [G_NS.JsonProperty( Order = 1 )]
        public SetName CourseOfFireDef { get; set; } = SetName.DEFAULT;

        /// <summary>
        /// Components, roughly, describe the stages of this SimpleCOF.
        /// </summary>
		[G_STJ_SER.JsonPropertyOrder( 2 )]
        [G_NS.JsonProperty( Order = 2 )]
        public List<SimpleCOFComponent> Components { get; set; } = new List<SimpleCOFComponent>();

        /// <summary>
        /// The ScoreFormat to use (e.g. Events or Shots), that is defined by the parent's Event Style .ScoreFormatCollectionDef, to use when displaying scores with this SimpleCOFComponent.
        /// </summary>
        /// <remarks>When displaying scores, the user chooses the ScoreFormatName (e.g. Decimal or Integer) to use to format scores.</remarks>
        [G_NS.JsonProperty( Order = 3, DefaultValueHandling = Newtonsoft.Json.DefaultValueHandling.Include )]
        [DefaultValue( "Events" )]
        public string ScoreFormat { get; set; } = "Events";

        /// <summary>
        /// Helper function that retreives the common name of the referrenced COURSE OF FIRE. 
        /// <para>The method relies on the COURSE OF FIRE being loaded into the DefinitionCache (which
        /// is usually the case). If it is not, the string "Default" is instead returned.</para>
        /// </summary>
        [G_STJ_SER.JsonPropertyOrder( 10 )]
        [G_NS.JsonIgnore]
        [G_NS.JsonProperty( Order = 10 )]
        public string Name {
            get {
                if (DefinitionCache.TryGetCourseOfFireDefinition( this.CourseOfFireDef, out CourseOfFire cof )) {
                    return cof.CommonName;
                }

                return "Default";
            }
        }

        /// <inheritdoc/>
        public override string ToString() {
            return $"SimpleCOF for {CourseOfFireDef}";
        }

        /// <inheritdoc/>
        public async Task<CourseOfFire> GetCourseOfFireDefinitionAsync() {
            return await DefinitionCache.GetCourseOfFireDefinitionAsync( CourseOfFireDef );
        }

        /// <inheritdoc/>
        [G_STJ_SER.JsonIgnore]
        [G_NS.JsonIgnore]
        public string TextField {
            get {
                return CourseOfFireDef.ProperName;
            }
        }

        /// <inheritdoc/>
        [G_STJ_SER.JsonIgnore]
        [G_NS.JsonIgnore]
        public string ValueField {
            get {
                return CourseOfFireDef.ToString();
            }
        }

        /// <inheritdoc/>
		[G_STJ_SER.JsonPropertyOrder( 100 )]
        [G_NS.JsonProperty( Order = 100 )]
        [DefaultValue( "" )]
        public string Comment { get; set; } = string.Empty;
    }

    /// <summary>
    /// A SimpleCOFComponent is one Stage make up of a SimpleCOF. 
    /// </summary>
    public class SimpleCOFComponent : IGetStageStyleDefinition, IReconfigurableRulebookObject {

        /// <summary>
        /// SetName of a StageStyle to include. Value must be a memember of the parent EventStyle
        /// objects .StageStyles list. 
        /// </summary>
        [G_NS.JsonProperty( Order = 1, DefaultValueHandling = Newtonsoft.Json.DefaultValueHandling.Include )]
        public SetName StageStyleDef { get; set; } = SetName.DEFAULT;

        /// <summary>
        /// The number of shots that are fired for this stage of a event.
        /// </summary>
        [G_NS.JsonProperty( Order = 3, DefaultValueHandling = Newtonsoft.Json.DefaultValueHandling.Include )]
        public int Shots { get; set; } = 10;

        /// <summary>
        /// The ScoreComponent to use, from the score of this StageStyle, to calculate the 
        /// special sum score of the EventStyle. 
        /// </summary>
        [G_NS.JsonProperty( Order = 5, DefaultValueHandling = Newtonsoft.Json.DefaultValueHandling.Include )]
        [DefaultValue( ScoreComponent.S )]
        public ScoreComponent ScoreComponent { get; set; } = ScoreComponent.S;

        /// <inheritdoc/>
        [G_NS.JsonProperty( Order = 100 )]
        [DefaultValue( "" )]
        public string Comment { get; set; } = string.Empty;

        /// <inheritdoc/>
        /// <exception cref="ArgumentException">Thrown if the value of .StageStyle could not be parsed. Which shouldn't happen.</exception>
        public async Task<StageStyle> GetStageStyleDefinitionAsync() {

            return await DefinitionCache.GetStageStyleDefinitionAsync( StageStyleDef );

        }

        /// <inheritdoc/>
        public override string ToString() {
            return $"SimpleCOFComponent for {StageStyleDef}";
        }
    }
}
