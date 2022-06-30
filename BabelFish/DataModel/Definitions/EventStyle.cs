using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ShootersTech.DataModel.Definitions {
    [Serializable]
    public class EventStyle : Definition {


        public EventStyle() : base() {
            Type = DefinitionType.EVENTSTYLE;

            //Don't initialize EventStyles or StageStyles, since one of these values as to be null.
        }

        [OnDeserialized]
        internal new void OnDeserializedMethod(StreamingContext context) {
            base.OnDeserializedMethod(context);

            //Don't initialize EventStyles or StageStyles, since one of these values as to be null.
        }

        public List<string>? EventStyles { get; set; }

        public List<string>? StageStyles { get; set; }

        public List<string> RelatedEventStyles { get; set; } = new List<string>();

        public List<SimpleCOF> SimpleCOFs { get; set; } = new List<SimpleCOF>();

    }
}
