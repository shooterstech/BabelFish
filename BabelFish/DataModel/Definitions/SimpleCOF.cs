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
    public class SimpleCOF :ITelerikBindModel, ICopy<SimpleCOF>, IReconfigurableRulebookObject
    {

        public SimpleCOF() { }

        [Obsolete( "Use CourseOfFireDef instead." )]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// The set name of the Course of Fire definition that this simple COF is emulating.
        /// </summary>
        public string CourseOfFireDef { get; set; } = string.Empty;

        public List<SimpleCOFComponent> Components { get; set; } = new List<SimpleCOFComponent>();

		public override string ToString() {
			return $"SimpleCOF for {CourseOfFireDef}";
		}

        public SimpleCOF Copy()
        {
            SimpleCOF scof = new SimpleCOF();
            scof.Name = this.Name;
            scof.CourseOfFireDef = this.CourseOfFireDef;
            if (this.Components != null)
            {
                foreach (var c in this.Components)
                {
                    scof.Components.Add(c.Copy());
                }
            }
            scof.Comment = this.Comment;
            return scof;
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

    public class SimpleCOFComponent: IGetStageStyleDefinition, ICopy<SimpleCOFComponent>, IReconfigurableRulebookObject
    {
		/// <summary>
		/// SetName of a StageStyle to include. Value must be a memember of the parent EventStyle
		/// objects .StageStyles list. 
		/// </summary>
		public string StageStyle { get; set; } = string.Empty;

		/// <summary>
		/// The number of shots that are fired for this stage of a event.
		/// </summary>
        public int Shots { get; set; } = 0;

		public override string ToString() {
			return $"SimpleCOFComponent for {StageStyle}";
		}

		/// <inheritdoc/>
		/// <exception cref="ArgumentException">Thrown if the value of .StageStyle could not be parsed. Which shouldn't happen.</exception>
		public async Task<StageStyle> GetStageStyleDefinitionAsync() {

			SetName stageStyleSetName = SetName.Parse( StageStyle );
			return await DefinitionCache.GetStageStyleDefinitionAsync( stageStyleSetName );

		}

        public SimpleCOFComponent Copy()
        {
            SimpleCOFComponent scofc = new SimpleCOFComponent();
			scofc.StageStyle = this.StageStyle;
            scofc.Shots = this.Shots;
            scofc.ScoreFormat = this.ScoreFormat;
            scofc.Comment = this.Comment;
            return scofc;
        }

        [Obsolete("Potentially obsolute, as we are allowing the user to specify the score format outside of the definition.")]
        public string ScoreFormat { get; set; } = string.Empty;

        /// <inheritdoc/>
        [JsonProperty(Order = 99, DefaultValueHandling = DefaultValueHandling.Ignore)]
        [DefaultValue("")]
        public string Comment { get; set; } = string.Empty;
    }
}
