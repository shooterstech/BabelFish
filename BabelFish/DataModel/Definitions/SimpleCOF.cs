using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scopos.BabelFish.DataModel;
using Scopos.BabelFish.APIClients;
using Newtonsoft.Json;
using System.ComponentModel;

namespace Scopos.BabelFish.DataModel.Definitions {

    /// <summary>
    /// A "SimpleCOF" defines the type of StageStyles that make it up and the number of 
    /// shots per StageStyle. It does not define any structure, time limites, ranking rules
    /// etc. 
    /// </summary>
    [Serializable]
    public class SimpleCOF :ITelerikBindModel, ICopy<SimpleCOF>, IReconfigurableRulebookObject, IGetCourseOfFireDefinition
    {
        /// <summary>
        /// Public constructor
        /// </summary>
        public SimpleCOF() { }

        /// <inheritdoc/>
        public SimpleCOF Copy() {
            SimpleCOF scof = new SimpleCOF();
            scof.Name = this.Name;
            scof.CourseOfFireDef = this.CourseOfFireDef;
            if (this.Components != null) {
                foreach (var c in this.Components) {
                    scof.Components.Add( c.Copy() );
                }
            }
            scof.Comment = this.Comment;
            return scof;
        }

        /// <summary>
        /// The set name of the Course of Fire definition that this simple COF is emulating.
        /// </summary>
        [JsonProperty( Order = 11)]
        public string CourseOfFireDef { get; set; } = string.Empty;

        /// <summary>
        /// Components, roughly, describe the stages of this SimpleCOF.
        /// </summary>
        [JsonProperty( Order = 12 )]
        public List<SimpleCOFComponent> Components { get; set; } = new List<SimpleCOFComponent>();


        [Obsolete( "Use CourseOfFireDef instead." )]
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
        [JsonProperty(Order = 99, DefaultValueHandling = DefaultValueHandling.Ignore)]
        [DefaultValue("")]
        public string Comment { get; set; } = string.Empty;
    }

    /// <summary>
    /// A SimpleCOFComponent is one Stage make up of a SimpleCOF. 
    /// </summary>
    public class SimpleCOFComponent: IGetStageStyleDefinition, ICopy<SimpleCOFComponent>, IReconfigurableRulebookObject
    {
        /// <inheritdoc/>
        public SimpleCOFComponent Copy()
        {
            SimpleCOFComponent scofc = new SimpleCOFComponent();
			scofc.StageStyleDef = this.StageStyleDef;
            scofc.Shots = this.Shots;
            scofc.ScoreConfigName = this.ScoreConfigName;
            scofc.Comment = this.Comment;
            scofc.ScoreFormat = this.ScoreFormat;
            return scofc;
        }

        /// <summary>
        /// SetName of a StageStyle to include. Value must be a memember of the parent EventStyle
        /// objects .StageStyles list. 
        /// </summary>
        [JsonProperty( Order = 11 )]
        public string StageStyleDef { get; set; } = string.Empty;

        /// <summary>
        /// Same as StageStyleDef property. 
        /// </summary>
        [JsonProperty( Order = 11 )]
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
        [JsonProperty( Order = 12 )]
        public int Shots { get; set; } = 10;

        /// <summary>
        /// The ScoreConfigName to use, that is defined by the parent's COF's .ScoreFormatCollectionDef, to use when displaying scores with this SimpleCOFComponent.
        /// </summary>
        [DefaultValue( "Integer" )]
        [JsonProperty( DefaultValueHandling = DefaultValueHandling.Include, Order = 13 )]
        public string ScoreConfigName { get; set; } = "Integer";

        /// <summary>
        /// The ScoreComponent to use, from the score of this StageStyle, to calculate the 
        /// special sum score of the EventStyle. 
        /// </summary>
        [DefaultValue( ScoreComponent.S )]
        [JsonProperty( DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate, Order = 13 )]
        public ScoreComponent ScoreComponent { get; set; } = ScoreComponent.S;

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

        [Obsolete("Removed in favor of ScoreConfigDefault.")]
        public string ScoreFormat { get; set; } = string.Empty;

        /// <inheritdoc/>
        [JsonProperty(Order = 99, DefaultValueHandling = DefaultValueHandling.Ignore)]
        [DefaultValue("")]
        public string Comment { get; set; } = string.Empty;
    }
}
