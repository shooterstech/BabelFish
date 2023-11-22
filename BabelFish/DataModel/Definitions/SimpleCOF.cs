using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scopos.BabelFish.DataModel;

namespace Scopos.BabelFish.DataModel.Definitions {

    /// <summary>
    /// A "SimpleCOF" defines the type of StageStyles that make it up and the number of 
    /// shots per StageStyle. It does not define any structure, time limites, ranking rules
    /// etc. 
    /// </summary>
    [Serializable]
    public class SimpleCOF :ITelerikBindModel {

        public SimpleCOF() { }

        [Obsolete( "Use CourseOfFireDef instead." )]
        public string Name { get; set; }

        /// <summary>
        /// The Course of Fire definition that this simple COF is emulating.
        /// </summary>
        public string CourseOfFireDef { get; set; }

        public List<SimpleCOFComponent> Components { get; set; } = new List<SimpleCOFComponent>();

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
	}

    public class SimpleCOFComponent {
        public string StageStyle { get; set; } = string.Empty;

        public int Shots { get; set; } = 0;

        public string ScoreFormat { get; set; } = string.Empty;
    }
}
