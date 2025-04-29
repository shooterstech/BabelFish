using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Runtime.Serialization;

namespace Scopos.BabelFish.DataModel.Definitions {

    /// <summary>
    /// Defines which shots should be displayed within the EST Athlete Monitor and EST Spectator Display during a Segment.
    /// </summary>
    public class ShowInSegment: IReconfigurableRulebookObject {

        /// <summary>
        /// Public constructor
        /// </summary>
        public ShowInSegment() {

        }

        [OnDeserialized]
        internal new void OnDeserializedMethod( StreamingContext context ) {

            if (StageLabel == null)
                StageLabel = new List<string>();
        }

        /// <summary>
        /// Display shots who's StageLabel is within this list. StageLabels are traditionally defined by a single character.
        /// </summary>
        public List<string> StageLabel { get; set; } = new List<string>();

		/// <summary>
		/// Display shots that are either competition shots (non sighters), sighters, or both.
		/// </summary>
		[G_NS.JsonProperty( DefaultValueHandling = Newtonsoft.Json.DefaultValueHandling.Include )]
		public CompetitionType Competition { get; set; } = CompetitionType.BOTH;

        /// <summary>
        /// Must be one of the following values
        /// ALL
        /// STRING (default)
        /// Past(n), where n is an integer
        /// </summary>
        [DefaultValue( "STRING" )]
        public string ShotPresentation { get; set; } = "STRING";

        /// <inheritdoc/>
        [DefaultValue( "" )]
        public string Comment { get; set; } = string.Empty;

        /// <inheritdoc/>
        public override string ToString() {
            return $"Show {Competition} for {StageLabel}";
        }

    }
}