using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Scopos.BabelFish.Converters;

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
        [JsonConverter( typeof( Scopos.BabelFish.Converters.DateTimeConverter ) )]
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
		[JsonConverter( typeof( StringEnumConverter ) )]
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
		/// Name of the TargetCollection used in this match.
		/// </summary>
		public string TargetCollectionName { get; set; }

        /// <summary>
        /// Start date for the ResultList of the Match. Used to guage what the Status of the Result list is.
        /// need defaults?
        /// </summary>
        [JsonConverter( typeof( DateConverter ) )]
        public DateTime StartDate { get; set; } = DateTime.Today;

        /// <summary>
        /// End date for the ResultList of the Match. Used to guage what the Status of the ResultList is.
        /// need defaults?
        /// </summary>
        [JsonConverter( typeof( DateConverter ) )]
        public DateTime EndDate { get; set; } = DateTime.Today;
    }
}
