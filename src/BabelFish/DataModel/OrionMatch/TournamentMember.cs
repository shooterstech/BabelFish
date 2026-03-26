namespace Scopos.BabelFish.DataModel.OrionMatch {


    public class TournamentMember {

        /// <summary>
        /// The Unique Match ID that is a Member of the Tournament.
        /// </summary>
        [G_NS.JsonProperty( Order = 1 )]
        [G_NS.JsonConverter( typeof( G_BF_NS_CONV.MatchIdConverter ) )]
        [G_STJ_SER.JsonConverter( typeof( G_BF_STJ_CONV.MatchIdConverter ) )]
        public MatchID MatchId { get; set; }

        /// <summary>
        /// The name of the Match that is a Member of the Tournament.
        /// </summary>
        [G_NS.JsonProperty( Order = 2 )]
        public string MatchName { get; set; }

        /// <summary>
        /// The Match ID of the Tournament that this Match is a member of.
        /// </summary>
        [G_NS.JsonIgnore]
        public Tournament Tournament { get; set; }


        [G_NS.JsonProperty( Order = 4 )]
        public ApprovalStatus ApprovalStatus { get; set; } = ApprovalStatus.UNKNOWN;

    }
}
