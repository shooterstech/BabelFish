using System.ComponentModel;
using System.Runtime.Serialization;

namespace Scopos.BabelFish.DataModel.Definitions {
    public abstract class AbbreviatedFormatBase : IReconfigurableRulebookObject {

        public AbbreviatedFormatBase() { }

        /// <summary>
        /// The name of the top level event.
        /// </summary>
        [G_NS.JsonProperty( Order = 10 )]
        [DefaultValue( "" )]
        public string EventName { get; set; } = string.Empty;

        /// <summary>
        /// The name of the event to display to the athlete. If this value is null or an empty string, the value of EventName is used.
        /// </summary>
        [G_NS.JsonProperty( Order = 11 )]
        [DefaultValue( "" )]
        public string EventDisplayName { get; set; } = string.Empty;

		/// <inheritdoc/>
		[G_NS.JsonProperty( Order = 100 )]
		[DefaultValue( "" )]
		public string Comment { get; set; } = string.Empty;

	}

    /// <summary>
     /// An AbbreviatedFormat describes the format of a AbbreviatedResultCOF. These are used to display 
     /// event scores to the athlete within his or her Athena compliant EST Monitor and to spectators through EST Displays.
     /// </summary>
    public class AbbreviatedFormat : AbbreviatedFormatBase {

        public AbbreviatedFormat() : base() {

		}

		/// <summary>
		/// A unique name given to this AbbreviatedFormat.
		/// </summary>
		[G_NS.JsonProperty( Order = 1 )]
		[DefaultValue( "" )]
		public string FormatName { get; set; } = string.Empty;

		/// <summary>
		/// A list of child events who scores should be included in the resulting AbbreviatedResultCOF.
		/// Must be List<AbbreviatedFormat> or ...
		/// </summary>
		[G_NS.JsonProperty( Order = 20 )]
        [DefaultValue( null )]
        public List<AbbreviatedFormatChild> Children { get; set; } = new List<AbbreviatedFormatChild>();

        public bool ShouldSerializeChildren() {

            return Children != null && Children.Count > 0;
        }

		/// <inheritdoc/>
		public override string ToString() {
            if (FormatName != "")
                return $"{FormatName} for {EventName}";

            else
                return $"AbbreviatedFormat for {EventName}";

        }
    }
}
