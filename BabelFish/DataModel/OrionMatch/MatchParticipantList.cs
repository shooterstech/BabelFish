using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.Serialization;
using System.Text;
using NLog;
using System.Text.Json;
using Scopos.BabelFish.Converters;
using System.Text.Json.Serialization;

namespace Scopos.BabelFish.DataModel.OrionMatch {
	/// <summary>
	/// A response object, describing a list of Match Participants in a match. Match Participants, in this API call, include athletes (competitors), match officials, and coaches. They do not include Teams.
	/// </summary>
	[Serializable]
	public class MatchParticipantList : ITokenItems<MatchParticipant> {

        private Logger logger = LogManager.GetCurrentClassLogger();

        public MatchParticipantList() {
            Items = new List<MatchParticipant>();
        }

        [OnDeserialized]
        internal void OnDeserialized( StreamingContext context ) {
            if (Items == null)
                Items = new List<MatchParticipant>();
        }

        /// <summary>
        /// Formatted as a string, the Match ID that this squadding list is from.
        /// Use GetMatchID() to return the value as a MatchID object.
        /// </summary>
        public string MatchID { get; set; }

        /// <summary>
        /// Start date for the ResultList of the Match. Used to guage what the Status of the Result list is.
        /// need defaults?
        /// </summary>
        [JsonConverter( typeof( ScoposDateOnlyConverter ) )]
        public DateTime StartDate { get; set; } = DateTime.Today;

        /// <summary>
        /// End date for the ResultList of the Match. Used to guage what the Status of the ResultList is.
        /// need defaults?
        /// </summary>
        [JsonConverter( typeof( ScoposDateOnlyConverter ) )]
        public DateTime EndDate { get; set; } = DateTime.Today;

        /// <summary>
        /// The Match ID that this squadding list is from.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="FormatException">Thrown if unable to parse the property MatchID into a MatchID object.</exception>
        public MatchID GetMatchID() {
            try {
                return new MatchID( MatchID );
            } catch (Exception ex) {
                //Probable either a FormatException or a NullValueException
                var msg = $"Can not parse MatchID values of '{MatchID}'. Received error {ex}.";
                logger.Error( msg, ex );
                throw new FormatException( msg );
            }
        }

        /// <summary>
        /// If this squadding list a Virtual Match, this is the Parent ID of the match. If this is a local match, then this 
        /// value will be the same as MatchID.
        /// </summary>
        public string ParentID { get; set; }

        public MatchID GetParentID() {
            //NOTE that I am using the GetMatchID value to calculate the value for ParentID. This *should* be the
            //same as the ParentID property.
            return GetMatchID().GetParentMatchID();
        }

        /// <summary>
        /// The name of the match that this squadding list is from.
        /// </summary>
        public string MatchName { get; set; }

        /// <summary>
        /// List of MatchParticipant (e.g. Individuals and their attributes, roles, and result cofs). 
        /// </summary>
        public List<MatchParticipant> Items { get; set; }

        /// <inheritdoc />
        public string NextToken { get; set; } = string.Empty;

        /// <inheritdoc />
        public int Limit { get; set; } = 50;

        /// <inheritdoc />
        public bool HasMoreItems {
            get {
                return !string.IsNullOrEmpty( NextToken );
            }
        }

        public override string ToString() {
            return $"MatchParticipantList with {Items.Count} items";
        }
    }
}
