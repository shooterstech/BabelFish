using System.ComponentModel;
using System.Runtime.Serialization;
using Scopos.BabelFish.DataActors.OrionMatch;
using Scopos.BabelFish.DataModel.OrionMatch;

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

		public abstract List<AbbreviatedFormatChild> GetCompiledAbbreviatedFormatChildren( IEventScoreProjection re );

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
		/// A list of (uncompiled) child events who scores should be included in the resulting AbbreviatedResultCOF.
		/// </summary>
        /// <remarks>
        /// To get the compiled list, use the method GetCompiledabbreviatedFormatChildren()</remarks>
		[G_NS.JsonProperty( Order = 20 )]
        [DefaultValue( null )]
        public List<AbbreviatedFormatChild> Children { get; set; } = new List<AbbreviatedFormatChild>();

        public bool ShouldSerializeChildren() {

            return Children != null && Children.Count > 0;
		}

		public override List<AbbreviatedFormatChild> GetCompiledAbbreviatedFormatChildren( IEventScoreProjection re ) {
			List<AbbreviatedFormatChild> list = new List<AbbreviatedFormatChild>();
            foreach (var child in Children) {
                list.AddRange( child.GetCompiledAbbreviatedFormatChildren( re ) );
            }

            return list;
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
