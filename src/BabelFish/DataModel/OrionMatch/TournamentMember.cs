namespace Scopos.BabelFish.DataModel.OrionMatch {


    public class TournamentMember : MatchAbbr {

        /// <summary>
        /// Tournament that this Match is a member of. Backpointer populated on deserialization.
        /// </summary>
        [G_NS.JsonIgnore]
        public Tournament Tournament { get; set; }

        public MatchID? TournamentId => Tournament?.TournamentId;


        [G_NS.JsonProperty( Order = 1 )] 
        public ApprovalStatus ApprovalStatus { get; set; } = ApprovalStatus.UNKNOWN;

    }
}
