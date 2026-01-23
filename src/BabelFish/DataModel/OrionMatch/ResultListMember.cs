namespace Scopos.BabelFish.DataModel.OrionMatch {

    /// <summary>
    /// ResultListMember describes one of the ResultLists to pull data from within a <see cref="MergedResultList"/>.
    /// </summary>
    public class ResultListMember {

        /// <summary>
        /// The Unique Match ID that is a Member of the Merged Result List.
        /// </summary>
        [G_NS.JsonProperty( Order = 1 )]
        [G_NS.JsonConverter( typeof( G_BF_NS_CONV.MatchIdConverter ) )]
        [G_STJ_SER.JsonConverter( typeof( G_BF_STJ_CONV.MatchIdConverter ) )]
        public MatchID MatchId { get; set; }

        /// <summary>
        /// The name of the Result List that is a Member of the Merged Result List.
        /// </summary>
        [G_NS.JsonProperty( Order = 2 )]
        public string ResultName { get; set; }

        /// <summary>
        /// The string to use as the column header.
        /// </summary>
        [G_NS.JsonProperty( Order = 3 )]
        public string HeaderName { get; set; }
    }
}
