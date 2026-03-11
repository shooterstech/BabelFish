using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using Scopos.BabelFish.Converters.Microsoft;

namespace Scopos.BabelFish.DataModel.OrionMatch {
    /// <summary>
    /// A response object, describing a list of Match Participants in a match. Match Participants, in this API call, include athletes (competitors), match officials, and coaches. They do not include Teams.
    /// </summary>
    [Serializable]
    public class MatchParticipantList : ITokenItems<MatchParticipant> {

        private Logger _logger = LogManager.GetCurrentClassLogger();

        public MatchParticipantList() {
            Items = new List<MatchParticipant>();
        }

        [OnDeserialized]
        internal void OnDeserialized( StreamingContext context ) {
            if (Items == null)
                Items = new List<MatchParticipant>();
        }

        /// <summary>
        /// The name of the match that this squadding list is from.
        /// </summary>
        [G_NS.JsonProperty( Order = 1 )]
        public string MatchName { get; set; }

        /// <summary>
        /// The Match ID that this squadding list is from. 
        /// </summary>
        [G_NS.JsonProperty( Order = 2 )]
        public MatchID MatchID { get; set; }

        /// <summary>
        /// Start date for underlining <see cref="Match"/>.
        /// </summary>
        [G_NS.JsonProperty( Order = 3 )]
        [G_STJ_SER.JsonConverter( typeof( ScoposDateOnlyConverter ) )]
        [G_NS.JsonConverter( typeof( G_BF_NS_CONV.DateConverter ) )]
        public DateTime StartDate { get; set; } = DateTime.Today;

        /// <summary>
        /// End date for underlining <see cref="Match"/>.
        /// need defaults?
        /// </summary>
        [G_NS.JsonProperty( Order = 4 )]
        [G_STJ_SER.JsonConverter( typeof( ScoposDateOnlyConverter ) )]
        [G_NS.JsonConverter( typeof( G_BF_NS_CONV.DateConverter ) )]
        public DateTime EndDate { get; set; } = DateTime.Today;

        /// <summary>
        /// List of MatchParticipant (e.g. Individuals and their attributes, roles, and result cofs). 
        /// </summary>
        [G_NS.JsonProperty( Order = 10 )]
        public List<MatchParticipant> Items { get; set; }

        /// <inheritdoc />
		[JsonConverter( typeof( NextTokenConverter ) )]
        [G_NS.JsonProperty( Order = 50 )]
        public string NextToken { get; set; } = string.Empty;

        /// <inheritdoc />
        [G_NS.JsonProperty( Order = 51 )]
        public int Limit { get; set; } = 50;

        /// <inheritdoc />
        [G_NS.JsonProperty( Order = 52 )]
        public bool HasMoreItems {
            get {
                return !string.IsNullOrEmpty( NextToken );
            }
        }

        /// <inheritdoc />
        public override string ToString() {
            return $"MatchParticipantList from {MatchName} with {Items.Count} participants.";
        }
    }
}
