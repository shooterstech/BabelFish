using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Scopos.BabelFish.DataModel.Definitions {
    [Serializable]
    public class EventStyle : Definition, ICopy<EventStyle> {


        public EventStyle() : base() {
            Type = DefinitionType.EVENTSTYLE;

            //Don't initialize EventStyles or StageStyles, since one of these values has to be null.
        }

        [OnDeserialized]
        internal new void OnDeserializedMethod(StreamingContext context) {
            base.OnDeserializedMethod(context);

            //Don't initialize EventStyles or StageStyles, since one of these values as to be null.
        }

        public EventStyle Copy()
        {
            EventStyle e = new EventStyle();
            this.Copy(e);
            if (this.EventStyles != null)
            {
                foreach (var es in this.EventStyles)
                {
                    e.EventStyles.Add(es);
                }
            }
            if (this.StageStyles != null)
            {
                foreach (var es in this.StageStyles)
                {
                    e.StageStyles.Add(es);
                }
            }
            if (this.RelatedEventStyles != null)
            {
                foreach (var es in this.RelatedEventStyles)
                {
                    e.RelatedEventStyles.Add(es);
                }
            }
            if (this.SimpleCOFs != null)
            {
                foreach (var es in this.SimpleCOFs)
                {
                    e.SimpleCOFs.Add(es.Copy());
                }
            }
            return e;
        }

        /// <summary>
        /// An ordered list of EVENT STYLEs that comprise this EVENT STYLE. Each Event Style is listed by its SetName.
        /// Either EVENT STYLEs or StageStyles, but not both, is required. If included at least one EVENT STYLE must be listed
        /// </summary>
        public List<string>? EventStyles { get; set; } = new List<string> ();

        /// <summary>
        /// An ordered list of STAGE STYLEs that comprise the EVENT STYLE. Each STAGE STYLE is listed by its SetName.
        /// Either EventStyles or StageStyles, but not both, is required. If included at least one STAGE STYLE must be listed.
        /// </summary>
        public List<string>? StageStyles { get; set; } = new List<string>();

        /// <summary>
        /// A list (order is inconsequential) of other EVENT STYLEs that are similar to this EVENT STYLE.
        /// </summary>
        public List<string> RelatedEventStyles { get; set; } = new List<string>();

        /// <summary>
        /// A list of SimpleCOF. This lists the common ways to displaying scores from this EVENT STYLE.
        /// </summary>
        public List<SimpleCOF> SimpleCOFs { get; set; } = new List<SimpleCOF>();

    }
}
