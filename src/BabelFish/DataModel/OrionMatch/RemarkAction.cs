namespace Scopos.BabelFish.DataModel.OrionMatch {
    /// <summary>
    /// Object that holds the RemarkName and Reason for a remark, if needed.
    /// This is mostly notation on the participants status within a match.
    /// </summary>
    [Serializable]
    public class RemarkAction : ICheckSum {
        /// <summary>
        /// this would be the name of the remark being given, DNS, DSQ, Eliminated, etc.
        /// </summary>
        [G_NS.JsonConverter( typeof( G_NS_CONV.StringEnumConverter ) )]
        [G_NS.JsonProperty( Order = 1, DefaultValueHandling = G_NS.DefaultValueHandling.Populate )]
        public ParticipantRemark ParticipantRemark { get; set; }

        /// <summary>
        /// the current status of the remark, it is possible to have previously been on the bubble, but now you aren't.
        /// </summary>
        [G_NS.JsonConverter( typeof( G_NS_CONV.StringEnumConverter ) )]
        [G_NS.JsonProperty( Order = 2, DefaultValueHandling = G_NS.DefaultValueHandling.Populate )]
        public RemarkVisibility Visibility { get; set; } = RemarkVisibility.SHOW;

        /// <summary>
        /// A human readable reason for this remark. Not required, but recommended.
        /// </summary>
        [G_NS.JsonProperty( Order = 3 )]
        public string Reason { get; set; } = string.Empty;

        /// <summary>
        /// The UTC time that this remark was applied.
        /// </summary>
        [G_STJ_SER.JsonConverter( typeof( G_BF_STJ_CONV.ScoposDateTimeConverter ) )]
        [G_NS.JsonConverter( typeof( G_BF_NS_CONV.DateTimeConverter ) )]
        [G_NS.JsonProperty( Order = 4 )]
        public DateTime AppliedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// RemarkActions that are added through CommandAutomationRemark must have a unique identifier 
        /// (to identify what automation added it). 
        /// </summary>
        [G_NS.JsonProperty( Order = 5 )]
        public int ActionId { get; set; } = 0;

        /// <inheritdoc />
        public override string ToString() {
            return $"{ParticipantRemark.Description()} {Visibility.Description()}";
        }

        /// <inheritdoc />
        /// <remarks>Choosing not to include CheckSum in the serialized value, as it is not a top level document.</remarks>
        [G_NS.JsonIgnore]
        [G_STJ_SER.JsonIgnore]
        public string CheckSum { get; set; } = string.Empty;


        /// <inheritdoc />
        public ulong CalculateChecksum() {
            var combined = $"{ParticipantRemark}|{Visibility}|{Reason}|{AppliedAt.ToString( Helpers.DateTimeFormats.DATETIME_FORMAT )}|{ActionId}";

            return Helpers.Common.Md5ToUlong( combined );
        }
    }
}
