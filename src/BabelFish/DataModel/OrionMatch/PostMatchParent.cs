namespace Scopos.BabelFish.DataModel.OrionMatch {
    public class PostMatchParent {

        /// <summary>
        /// The Match ID of the uploaded parent match.
        /// </summary>
        [G_STJ_SER.JsonConverter( typeof( G_BF_STJ_CONV.MatchIdConverter ) )]
        [G_NS.JsonConverter( typeof( G_BF_NS_CONV.MatchIdConverter ) )]
        public MatchID MatchID { get; set; } = MatchID.DEFAULT;

        /// <summary>
        /// S3 key where the uploaded match object was written.
        /// </summary>
        public string S3Key { get; set; } = string.Empty;
    }
}
