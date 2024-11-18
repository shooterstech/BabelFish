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
    public class AbbreviatedFormat : IReconfigurableRulebookObject, ICopy<AbbreviatedFormat> {

        public AbbreviatedFormat() {

        }

        /// <inheritdoc />
        public AbbreviatedFormat Copy() {
            AbbreviatedFormat copy = new AbbreviatedFormat();
            copy.FormatName = this.FormatName;
            copy.EventName = this.EventName;
            copy.EventDisplayName = this.EventDisplayName;
            copy.Comment = this.Comment;
            if (this.Children != null) {
                foreach( var c in this.Children) {
                    copy.Children.Add(c.Copy());
                }
            }

            return copy;
        }

        [OnDeserialized]
        internal void OnDeserializedMethod( StreamingContext context ) {
            if (FormatName == null)
                FormatName = "";

            if (EventName == null)
                EventName = "";

            if (Children == null)
                Children = new List<AbbreviatedFormat>();
        }

        /// <summary>
        /// A unique name given to this AbbreviatedFormat.
        /// </summary>
        [JsonProperty( Order = 1 )]
        [DefaultValue( "" )]
        public string FormatName { get; set; } = string.Empty;

        /// <summary>
        /// The name of the top level event.
        /// </summary>
        [JsonProperty( Order = 2 )]
        [DefaultValue( "" )]
        public string EventName { get; set; } = string.Empty;

        /// <summary>
        /// The name of the event to display to the athlete
        /// </summary>
        [DefaultValue( "" )]
        [JsonProperty( Order = 3 )]
        public string EventDisplayName { get; set; } = string.Empty;

        /// <summary>
        /// A list of child events who scores should be included in the resulting AbbreviatedResultCOF.
        /// Must be List<AbbreviatedFormat> or ...
        /// </summary>
        [JsonProperty(Order = 4)]
        [DefaultValue(null)]
        public List<AbbreviatedFormat> Children { get; set; } = new List<AbbreviatedFormat>();

        /// <inheritdoc/>
        [JsonProperty( Order = 99, DefaultValueHandling = DefaultValueHandling.Ignore )]
        [DefaultValue( "" )]
        public string Comment { get; set; } = string.Empty;

        public override string ToString() {
            if (FormatName != "")
                return $"{FormatName} for {EventName}";

            else
                return $"AbbreviatedFormat for {EventName}";

        }
    }
}
