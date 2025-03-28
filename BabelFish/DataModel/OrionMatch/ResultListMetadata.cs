using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Scopos.BabelFish.DataModel.OrionMatch {

	/// <summary>
	/// Used to describe the match that a ResultEvent was shot at. Instead of each ResultEvent including 
	/// each of these fields, the ResultEvent references a MatchID, that these fields may be looked up.
	/// Thus, hopefully, saving space in the already very long Result List.
	/// </summary>
	public class ResultListMetadata {

        /// <summary>
        /// Unique ID for the match.
        /// Field may possible be redundant
        /// </summary>
        [G_STJ_SER.JsonPropertyOrder( 1 )]
        [G_NS.JsonProperty( Order = 1 )]
        public string MatchID { get; set; } = string.Empty;

        /// <summary>
        /// The Owner of this data. e.g. OrionAcct001234
        /// </summary>
        [G_STJ_SER.JsonPropertyOrder( 2 )]
        [G_NS.JsonProperty( Order = 2 )]
        public string OwnerId { get; set; } = string.Empty;

        /// <summary>
        /// String holding the software (Orion Scoring System) and Version number of the software.
        /// </summary>
        [G_STJ_SER.JsonPropertyOrder( 3 )]
        [G_NS.JsonProperty( Order = 3 )]
        public string Creator { get; set; } = string.Empty;

		private ResultStatus localStatus = ResultStatus.FUTURE;

        /// <summary>
        /// FUTURE, INTERMEDIATE, UNOFFICIAL, OFFICIAL
        /// </summary>
        [G_STJ_SER.JsonPropertyOrder( 4 )]
        [G_NS.JsonProperty( Order = 4, DefaultValueHandling = G_NS.DefaultValueHandling.Populate )]
        [DefaultValue( ResultStatus.FUTURE )]
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
        /// UTC time of last update
        /// </summary>
        [G_STJ_SER.JsonConverter( typeof( G_BF_STJ_CONV.ScoposDateTimeConverter ) )]
        [G_STJ_SER.JsonPropertyOrder( 5 )]
        [G_NS.JsonConverter( typeof( G_BF_NS_CONV.DateTimeConverter ) )]
        [G_NS.JsonProperty( Order = 4 )]
        public DateTime LastUpdated { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// The location where the match was shot. Usually city and state, e.g. Minden, NE
        /// </summary>
        [G_STJ_SER.JsonPropertyOrder( 6 )]
        [G_NS.JsonProperty( Order = 6 )]
        public string MatchLocation { get; set; } = string.Empty;

        /// <summary>
        /// Start date for the ResultList of the Match. Used to guage what the Status of the Result list is.
        /// need defaults?
        /// </summary>
        [G_STJ_SER.JsonConverter( typeof( G_BF_STJ_CONV.ScoposDateOnlyConverter ) )]
        [G_STJ_SER.JsonPropertyOrder( 7 )]
        [G_NS.JsonConverter( typeof( G_BF_NS_CONV.DateConverter ) )]
        [G_NS.JsonProperty( Order = 7 )]
        public DateTime StartDate { get; set; } = DateTime.Today;

        /// <summary>
        /// End date for the ResultList of the Match. Used to guage what the Status of the ResultList is.
        /// need defaults?
        /// </summary>
        [G_STJ_SER.JsonConverter( typeof( G_BF_STJ_CONV.ScoposDateOnlyConverter ) )]
        [G_STJ_SER.JsonPropertyOrder( 8 )]
        [G_NS.JsonConverter( typeof( G_BF_NS_CONV.DateConverter ) )]
        [G_NS.JsonProperty( Order = 8 )]
        public DateTime EndDate { get; set; } = DateTime.Today;

        /// <summary>
        /// Name of the TargetCollection used in this match.
        /// </summary>
        [G_STJ_SER.JsonPropertyOrder( 9 )]
        [G_NS.JsonProperty( Order = 9 )]
        public string TargetCollectionName { get; set; }

        /// <summary>
        /// If projected scores are included, ProjectionMadeBy says who made the projection.
        /// </summary>
        [G_STJ_SER.JsonPropertyOrder( 10 )]
        [G_NS.JsonProperty( Order = 10 )]
        [DefaultValue( "" )]
        public string ProjectionMadeBy { get; set; } = string.Empty;

		/// <summary>
		/// The name of the SegmentGroup that the competition is currently in (based on the Course of Fire's Range Script).
		/// </summary>
        [G_STJ_SER.JsonPropertyOrder( 11 )]
        [G_NS.JsonProperty( Order = 11 )]
        [DefaultValue( "" )]
        public string SegmentGroupName { get; set; } = string.Empty;

        /// <summary>
        /// The type of target system that was used to score shots.
        /// </summary>
        [G_STJ_SER.JsonPropertyOrder( 12 )]
        [G_NS.JsonProperty( Order = 12 )]
        [DefaultValue( ScoringSystem.UNKNOWN )]
        public ScoringSystem ScoringSystem { get; set; } = ScoringSystem.UNKNOWN;

        /// <summary>
        /// The name of the scoring system in use. An empty string means unknown or a mix of differnt Scoring Systems.
        /// </summary>
        [G_STJ_SER.JsonPropertyOrder( 13 )]
        [G_NS.JsonProperty( Order = 13 )]
        [DefaultValue( "" )]
        public string ScoringSystemName { get; set; } = string.Empty;
    }
}
