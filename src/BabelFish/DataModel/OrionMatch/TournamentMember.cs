namespace Scopos.BabelFish.DataModel.OrionMatch {


    public class TournamentMember : MatchAbbr {

        #region Private Variables

        #endregion

        #region Constructors, Factory Methods, and Initialization Methods

        #endregion

        #region Event Handlers

        #endregion

        #region Data Model Properties
        [G_NS.JsonProperty( Order = 50 )]
        public ApprovalStatus ApprovalStatus { get; set; } = ApprovalStatus.UNKNOWN;

        #endregion

        #region Helper Properties
        /// <summary>
        /// Tournament that this Match is a member of. Backpointer populated on deserialization.
        /// </summary>
        [G_NS.JsonIgnore]
        public Tournament Tournament { get; set; }


        [G_NS.JsonIgnore]
        public MatchID? TournamentId => Tournament?.TournamentId;

        #endregion

        #region Methods

        #endregion

    }
}
