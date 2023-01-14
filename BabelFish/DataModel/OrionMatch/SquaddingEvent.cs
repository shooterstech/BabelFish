using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Scopos.BabelFish.DataModel.OrionMatch {
    [Serializable]
    public class SquaddingEvent {

        public SquaddingEvent() {
            TargetStages = new List<OrionTargetStage>();
        }

        public SquaddingEvent(string name) {
            this.Name = name;
            this.TargetStages = new List<OrionTargetStage>();
            this.ImageCaptureOrderByParticipant = false;
            this.EnableImageCapture = true;
        }

        /// <summary>
        /// If set to true, then the ImageCaptureList should be by Relay, then Firing Point, then OrionTargetStage.SortOrder (ignoring the RelayReset option)
        /// Default is false. 
        /// </summary>
        public bool ImageCaptureOrderByParticipant { get; set; }

        /// <summary>
        /// An in order list of stages that targets have to be photographed. 
        /// </summary>
        public List<OrionTargetStage> TargetStages { get; set; }

        /// <summary>
        /// A unique name giving to the Event. 
        /// </summary>
        public string Name { get; set; }

        public bool EnableImageCapture { get; set; }
    }
}
