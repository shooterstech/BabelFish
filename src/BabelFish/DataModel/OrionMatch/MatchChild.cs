using Scopos.BabelFish.DataModel.Common;

namespace Scopos.BabelFish.DataModel.OrionMatch {

    /// <summary>
    /// A MatchChild object contains the API representation of a virtual child match belonging to a parent match.
    /// </summary>
    [Serializable]
    public class MatchChild {

        /// <summary>
        /// Legacy account identifier for the owner of this child match.
        /// </summary>
        public string AccountNumber { get; set; } = string.Empty;

        /// <summary>
        /// The account identifier that owns this child match.
        /// </summary>
        public string OwnerId { get; set; } = string.Empty;

        /// <summary>
        /// Unique MatchID for the child match.
        /// </summary>
        [G_STJ_SER.JsonConverter( typeof( G_BF_STJ_CONV.MatchIdConverter ) )]
        [G_NS.JsonConverter( typeof( G_BF_NS_CONV.MatchIdConverter ) )]
        public MatchID MatchID { get; set; } = MatchID.DEFAULT;

        /// <summary>
        /// Duplicate of <see cref="MatchID"/> included by the API for DynamoDB lookup compatibility.
        /// </summary>
        [G_STJ_SER.JsonConverter( typeof( G_BF_STJ_CONV.MatchIdConverter ) )]
        [G_NS.JsonConverter( typeof( G_BF_NS_CONV.MatchIdConverter ) )]
        public MatchID MATCH_MatchID { get; set; } = MatchID.DEFAULT;

        /// <summary>
        /// DynamoDB unique identifier for the match object.
        /// </summary>
        public string UniqueID { get; set; } = string.Empty;

        /// <summary>
        /// MatchID of the parent match.
        /// </summary>
        [G_STJ_SER.JsonConverter( typeof( G_BF_STJ_CONV.MatchIdConverter ) )]
        [G_NS.JsonConverter( typeof( G_BF_NS_CONV.MatchIdConverter ) )]
        public MatchID ParentID { get; set; } = MatchID.DEFAULT;

        /// <summary>
        /// User-facing child match name.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Alias for <see cref="Name"/> that matches the terminology used by other match models.
        /// </summary>
        [G_STJ_SER.JsonIgnore]
        [G_NS.JsonIgnore]
        public string MatchName {
            get { return Name; }
        }

        /// <summary>
        /// Where this child match takes place.
        /// </summary>
        public Location Location { get; set; } = new Location();

        /// <summary>
        /// Officials assigned to this child match.
        /// </summary>
        public List<object> Officials { get; set; } = new List<object>();

        public VisibilityOption Visibility { get; set; } = VisibilityOption.PRIVATE;

        /// <summary>
        /// Approval status of this child match within the parent match.
        /// </summary>
        public ApprovalStatus ApprovalStatus { get; set; } = ApprovalStatus.UNKNOWN;

        /// <summary>
        /// Data model version written by the API.
        /// </summary>
        public string JSONVersion { get; set; } = Helpers.Common.DATA_MODEL_VERSION;

        /// <summary>
        /// UTC time the child match object was last updated.
        /// </summary>
        [G_STJ_SER.JsonConverter( typeof( G_BF_STJ_CONV.ScoposDateTimeConverter ) )]
        [G_NS.JsonConverter( typeof( G_BF_NS_CONV.DateTimeConverter ) )]
        public DateTime LastUpdated { get; set; } = DateTime.UtcNow;

        public override string ToString() {
            return $"MatchChild {Name} ({MatchID})";
        }
    }
}
