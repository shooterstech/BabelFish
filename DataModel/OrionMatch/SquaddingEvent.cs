using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BabelFish.DataModel.OrionMatch {
    [Serializable]
    public class SquaddingEvent {

        public SquaddingEvent() {
            TargetStages = new List<OrionTargetStage>();
        }

        public SquaddingEvent(string name, string description) {
            this.Name = name;
            this.Description = description;
            this.TargetStages = new List<OrionTargetStage>();
            this.ImageCaptureOrderByParticipant = false;
            this.EnableImageCapture = true;
        }

        /// <summary>
        /// A unique name giving to the Event. 
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// A Human readable description of the event.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// An in order list of stages that targets have to be photographed. 
        /// </summary>
        public List<OrionTargetStage> TargetStages { get; set; }

        /// <summary>
        /// If set to true, then the ImageCaptureList should be by Relay, then Firing Point, then OrionTargetStage.SortOrder (ignoring the RelayReset option)
        /// Default is false. 
        /// </summary>
        public bool ImageCaptureOrderByParticipant { get; set; }

        public bool EnableImageCapture { get; set; }
    }
}
