using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scopos.BabelFish.DataModel;
using Scopos.BabelFish.APIClients;
using System.ComponentModel;

namespace Scopos.BabelFish.DataModel.Definitions {

    /// <summary>
    /// A "SimpleCOF" defines the type of StageStyles that make it up and the number of 
    /// shots per StageStyle. It does not define any structure, time limites, ranking rules
    /// etc. 
    /// </summary>
    [Serializable]
    public class SimpleCOF :ITelerikBindModel, IReconfigurableRulebookObject, IGetCourseOfFireDefinition
    {
        /// <summary>
        /// Public constructor
        /// </summary>
        public SimpleCOF() { }

        /// <summary>
        /// The set name of the Course of Fire definition that this simple COF is emulating.
        /// </summary>
		[G_STJ_SER.JsonPropertyOrder( 1 )]
        [G_NS.JsonProperty( Order = 1 )]
        public string CourseOfFireDef { get; set; } = string.Empty;

        /// <summary>
        /// Components, roughly, describe the stages of this SimpleCOF.
        /// </summary>
		[G_STJ_SER.JsonPropertyOrder( 2 )]
        [G_NS.JsonProperty( Order = 2 )]
        public List<SimpleCOFComponent> Components { get; set; } = new List<SimpleCOFComponent>();


        [Obsolete( "Use CourseOfFireDef instead." )]
        [G_STJ_SER.JsonPropertyOrder( 10 )]
        [G_NS.JsonProperty( Order = 10 )]
        public string Name { get; set; } = string.Empty;

        /// <inheritdoc/>
        public override string ToString() {
			return $"SimpleCOF for {CourseOfFireDef}";
		}

        /// <inheritdoc/>
        public async Task<CourseOfFire> GetCourseOfFireDefinitionAsync() {

            SetName cofSetName = SetName.Parse( CourseOfFireDef );
            return await DefinitionCache.GetCourseOfFireDefinitionAsync( cofSetName );
        }

        /// <inheritdoc/>
        [G_STJ_SER.JsonIgnore]
        [G_NS.JsonIgnore]
        public string TextField {
            get {
				SetName setName;
				if (SetName.TryParse( CourseOfFireDef, out setName )) {
					return setName.ProperName;
				} else {
					return "Unknown";
				}
			}
        }

        /// <inheritdoc/>
        [G_STJ_SER.JsonIgnore]
        [G_NS.JsonIgnore]
        public string ValueField {
			get {
				SetName setName;
				if (SetName.TryParse( CourseOfFireDef, out setName )) {
					return setName.ToString();
				} else {
					return "Unknown";
				}
			}
		}

        /// <inheritdoc/>
		[G_STJ_SER.JsonPropertyOrder( 100 )]
        [G_NS.JsonProperty( Order = 100 )]
        [DefaultValue("")]
        public string Comment { get; set; } = string.Empty;
    }

    /// <summary>
    /// A SimpleCOFComponent is one Stage make up of a SimpleCOF. 
    /// </summary>
    public class SimpleCOFComponent: IGetStageStyleDefinition, IReconfigurableRulebookObject
    {

        /// <summary>
        /// SetName of a StageStyle to include. Value must be a memember of the parent EventStyle
        /// objects .StageStyles list. 
        /// </summary>
        [G_NS.JsonProperty( Order = 1, DefaultValueHandling = Newtonsoft.Json.DefaultValueHandling.Include )]
        public string StageStyleDef { get; set; } = string.Empty;

        /// <summary>
        /// Same as StageStyleDef property. 
        /// </summary>
        [G_NS.JsonProperty( Order = 2, DefaultValueHandling = Newtonsoft.Json.DefaultValueHandling.Include )]
        [Obsolete( "Use StageStyleDef instead.")]
        public string StageStyle { 
            get {
                return this.StageStyleDef;
            }
            set {
                if (string.IsNullOrEmpty( this.StageStyleDef) ) {
                    this.StageStyleDef = value;
                }
            }
        }

        /// <summary>
        /// The number of shots that are fired for this stage of a event.
        /// </summary>
        [G_NS.JsonProperty( Order = 3, DefaultValueHandling = Newtonsoft.Json.DefaultValueHandling.Include )]
        public int Shots { get; set; } = 10;

        /// <summary>
        /// The ScoreConfigName to use, that is defined by the parent's COF's .ScoreFormatCollectionDef, to use when displaying scores with this SimpleCOFComponent.
        /// </summary>
        [G_NS.JsonProperty( Order = 4, DefaultValueHandling = Newtonsoft.Json.DefaultValueHandling.Include )]
        [DefaultValue( "Integer" )]
        public string ScoreConfigName { get; set; } = "Integer";

        /// <summary>
        /// The ScoreComponent to use, from the score of this StageStyle, to calculate the 
        /// special sum score of the EventStyle. 
        /// </summary>
        [G_NS.JsonProperty( Order = 5, DefaultValueHandling = Newtonsoft.Json.DefaultValueHandling.Include )]
        [DefaultValue( ScoreComponent.S )]
        public ScoreComponent ScoreComponent { get; set; } = ScoreComponent.S;

        [G_NS.JsonProperty( Order = 10 )]
        [Obsolete( "Removed in favor of ScoreConfigName." )]
        [DefaultValue( "" )]
        public string ScoreFormat { get; set; } = string.Empty;

        /// <inheritdoc/>
        [G_NS.JsonProperty( Order = 100 )]
        [DefaultValue( "" )]
        public string Comment { get; set; } = string.Empty;

        /// <inheritdoc/>
        /// <exception cref="ArgumentException">Thrown if the value of .StageStyle could not be parsed. Which shouldn't happen.</exception>
        public async Task<StageStyle> GetStageStyleDefinitionAsync() {

            SetName stageStyleSetName = SetName.Parse( StageStyleDef );
            return await DefinitionCache.GetStageStyleDefinitionAsync( stageStyleSetName );

        }

        /// <inheritdoc/>
        public override string ToString() {
            return $"SimpleCOFComponent for {StageStyleDef}";
        }
    }
}
