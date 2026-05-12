namespace Scopos.BabelFish.DataModel.OrionMatch {

    /// <summary>
    /// ResultListMember describes one of the ResultLists to pull data from within a <see cref="MergedResultList"/>.
    /// <para>Result Lists are uniquely identified through a combination of the MatchId, CourseOfFireId, and Result List name.</para>
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
        /// The Course of Fire Id, unique within the Match. 
        /// </summary>
        [G_NS.JsonProperty( Order = 2 )]
        public int CourseOfFireId { get; set; } = 1;

        /// <summary>
        /// The name of the Result List that is a Member of the Merged Result List.
        /// </summary>
        [G_NS.JsonProperty( Order = 3 )]
        public string ResultName { get; set; } = string.Empty;

        /// <summary>
        /// The string to use as the column header.
        /// </summary>
        [G_NS.JsonProperty( Order = 4 )]
        public string HeaderName { get; set; } = string.Empty;
    }
}
