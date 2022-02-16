using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BabelFish.DataModel.Definitions {
    [Serializable]
    public class EventStyle : Definition {


        public EventStyle() : base() {
            Type = Definition.DefinitionType.EVENTSTYLE;

            //Don't initialize EventStyles or StageStyles, since one of these values as to be null.

            RelatedEventStyles = new List<string>();
            SimpleCOFs = new List<SimpleCOF>();
        }

        [OnDeserialized]
        internal new void OnDeserializedMethod(StreamingContext context) {
            base.OnDeserializedMethod(context);

            //Don't initialize EventStyles or StageStyles, since one of these values as to be null.

            if (RelatedEventStyles == null)
                RelatedEventStyles = new List<string>();

            if (SimpleCOFs == null)
                SimpleCOFs = new List<SimpleCOF>();
        }

        public List<string> EventStyles { get; set; }

        public List<string> StageStyles { get; set; }

        public List<string> RelatedEventStyles { get; set; }

        public List<SimpleCOF> SimpleCOFs { get; set; }

    }
}
