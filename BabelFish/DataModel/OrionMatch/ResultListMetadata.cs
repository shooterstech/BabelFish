using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Scopos.BabelFish.Converters.Microsoft;

namespace Scopos.BabelFish.DataModel.OrionMatch {

	/// <summary>
	/// Used to describe the match that a ResultEvent was shot at. Instead of each ResultEvent including 
	/// each of these fields, the ResultEvent references a MatchID, that these fields may be looked up.
	/// Thus, hopefully, saving space in the already very long Result List.
	/// </summary>
	public class ResultListMetadata {

        /// <summary>
        /// UTC time of last update
        /// </summary>
        [G_STJ_SER.JsonConverter( typeof( Scopos.BabelFish.Converters.Microsoft.ScoposDateTimeConverter ) )]
        [G_NS.JsonConverter( typeof( G_BF_NS_CONV.DateTimeConverter ) )]
        public DateTime LastUpdated { get; set; } = DateTime.UtcNow;

		/// <summary>
		/// The location where the match was shot. Usually city and state, e.g. Minden, NE
		/// </summary>
		public string MatchLocation { get; set; } = string.Empty;

		/// <summary>
		/// Unique ID for the match.
		/// Field may possible be redundant
		/// </summary>
		public string MatchID { get; set; } = string.Empty;

		/// <summary>
		/// String holding the software (Orion Scoring System) and Version number of the software.
		/// </summary>
		public string Creator { get; set; } = string.Empty;

		/// <summary>
		/// The Owner of this data. e.g. OrionAcct001234
		/// </summary>
		public string OwnerId { get; set; } = string.Empty;

		private ResultStatus localStatus = ResultStatus.FUTURE;

		/// <summary>
		/// FUTURE, INTERMEDIATE, UNOFFICIAL, OFFICIAL
		/// </summary>
		
		[DefaultValue( ResultStatus.FUTURE )]
		[JsonInclude]
		public ResultStatus Status {
			get {
				if (EndDate < DateTime.Today) {
					localStatus = ResultStatus.OFFICIAL;
					return localStatus;
				} else {
					return localStatus;
				}
			}
			set {
				localStatus = value;
			}
		}

		/// <summary>
		/// Name of the TargetCollection used in this match.
		/// </summary>
		public string TargetCollectionName { get; set; }

        /// <summary>
        /// Start date for the ResultList of the Match. Used to guage what the Status of the Result list is.
        /// need defaults?
        /// </summary>
        [G_STJ_SER.JsonConverter( typeof( ScoposDateOnlyConverter ) )]
        [G_NS.JsonConverter( typeof( G_BF_NS_CONV.DateConverter ) )]
        public DateTime StartDate { get; set; } = DateTime.Today;

        /// <summary>
        /// End date for the ResultList of the Match. Used to guage what the Status of the ResultList is.
        /// need defaults?
        /// </summary>
        [G_STJ_SER.JsonConverter( typeof( ScoposDateOnlyConverter ) )]
        [G_NS.JsonConverter( typeof( G_BF_NS_CONV.DateConverter ) )]
        public DateTime EndDate { get; set; } = DateTime.Today;

        /// <summary>
        /// If projected scores are included, ProjectionMadeBy says who made the projection.
        /// </summary>
        [DefaultValue( "" )]
        public string ProjectionMadeBy { get; set; } = string.Empty;

		/// <summary>
		/// The name of the SegmentGroup that the competition is currently in (based on the Course of Fire's Range Script).
		/// </summary>
		[DefaultValue( "" )]
		public string SegmentGroupName { get; set; } = string.Empty;
    }
}
