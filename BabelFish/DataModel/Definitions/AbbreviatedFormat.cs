using System.ComponentModel;
using System.Runtime.Serialization;

namespace Scopos.BabelFish.DataModel.Definitions {
    /// <summary>
    /// An AbbreviatedFormat describes the format of a AbbreviatedResultCOF. These are used to display 
    /// event scores to the athlete within his or her Athena compliant EST Monitor and to spectators through EST Displays.
    /// </summary>
    public class AbbreviatedFormat : IReconfigurableRulebookObject {

        public AbbreviatedFormat() {

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
        [G_NS.JsonProperty( Order = 1 )]
        [DefaultValue( "" )]
        public string FormatName { get; set; } = string.Empty;

        /// <summary>
        /// The name of the top level event.
        /// </summary>
        [G_NS.JsonProperty( Order = 2 )]
        [DefaultValue( "" )]
        public string EventName { get; set; } = string.Empty;

        /// <summary>
        /// The name of the event to display to the athlete
        /// </summary>
        [G_NS.JsonProperty( Order = 3 )]
        [DefaultValue( "" )]
        public string EventDisplayName { get; set; } = string.Empty;

        /// <summary>
        /// A list of child events who scores should be included in the resulting AbbreviatedResultCOF.
        /// Must be List<AbbreviatedFormat> or ...
        /// </summary>
        [G_NS.JsonProperty( Order = 4 )]
        [DefaultValue( null )]
        public List<AbbreviatedFormat> Children { get; set; } = new List<AbbreviatedFormat>();

        /// <inheritdoc/>
        [G_NS.JsonProperty( Order = 100 )]
        [DefaultValue( "" )]
        public string Comment { get; set; } = string.Empty;

        public bool ShouldSerializeChildren() {

            return Children != null && Children.Count > 0;
        }

        public override string ToString() {
            if (FormatName != "")
                return $"{FormatName} for {EventName}";

            else
                return $"AbbreviatedFormat for {EventName}";

        }
    }
}
