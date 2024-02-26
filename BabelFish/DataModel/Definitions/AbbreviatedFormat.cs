using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Scopos.BabelFish.DataModel.Definitions {
    /// <summary>
    /// An AbbreviatedFormat describes the format of a AbbreviatedResultCOF. These are used to display 
    /// event scores to the athlete within his or her Athena compliant EST Monitor and to spectators through EST Displays.
    /// </summary>
    public class AbbreviatedFormat  {

        private List<string> validationErrorList = new List<string>();

        public AbbreviatedFormat() {
            FormatName = "";
            EventName = "";
            //Children = new List<AbbreviatedFormat>();

        }

        /// <summary>
        /// A unique name given to this AbbreviatedFormat.
        /// </summary>
        [JsonProperty(Order=1)]
        [DefaultValue("")]
        public string FormatName { get; set; }

        /// <summary>
        /// The name of the top level event.
        /// </summary>
        [JsonProperty(Order = 2)]
        [DefaultValue("")]
        public string EventName { get; set; }

        /// <summary>
        /// The name of the event to display to the athlete
        /// </summary>
        [DefaultValue("")]
        [JsonProperty(Order = 3)]
        public string EventDisplayName { get; set; }

        /// <summary>
        /// A list of child events who scores should be included in the resulting AbbreviatedResultCOF.
        /// Must be List<AbbreviatedFormat> or ...
        /// </summary>
        [JsonProperty(Order = 4)]
        [DefaultValue(null)]
        public List<AbbreviatedFormat> Children { get; set; }

        /// <summary>
        /// Human Readable score format string. defaults to decimal single value.
        /// </summary>
        [JsonProperty(Order = 5)]
        [DefaultValue("0 - 0")]
        public string ScoreFormatted { get; set; }

        /// <summary>
        /// available shot attribute list.
        /// </summary>
        [JsonProperty(Order = 6)]
        public List<string> AttributeList { get; set; } = new List<string>();


        [OnDeserialized]
        internal void OnDeserializedMethod(StreamingContext context) {
            if (FormatName == null)
                FormatName = "";

            if (EventName == null)
                EventName = "";

            if (Children == null)
                Children = new List<AbbreviatedFormat>();
        }

        public override string ToString() {
            if (FormatName != "")
                return $"{FormatName} for {EventName}";

            else
                return $"AbbreviatedFormat for {EventName}";

        }
    }
}
