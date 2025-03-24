using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Text;

namespace Scopos.BabelFish.DataModel.OrionMatch {
    /// <summary>
    /// Object that holds the RemarkName and Reason for a remark, if needed.
    /// This is mostly notation on the participants status within a match.
    /// </summary>
    [Serializable]
    public class Remark {
        /// <summary>
        /// this would be the name of the remark being given, DNS, DSQ, Eliminated, etc.
        /// </summary>
        [JsonConverter( typeof( StringEnumConverter ) )]
        [JsonProperty( DefaultValueHandling = DefaultValueHandling.Populate )]
        public ParticipantRemark ParticipantRemark { get; set; }

        /// <summary>
        /// A reason for a remark is not always given, but should be when the remark is assigned by the RSO
        /// </summary>
        public string Reason { get; set; } = string.Empty;

        /// <summary>
        /// the current status of the remark, it is possible to have previously been on the bubble, but now you aren't.
        /// </summary>
        [JsonConverter( typeof( StringEnumConverter ) )]
        [JsonProperty( DefaultValueHandling = DefaultValueHandling.Populate )]
        public RemarkVisibility Visibility { get; set; } = RemarkVisibility.SHOW;

        /// <inheritdoc />
        public override string ToString() {
            return ParticipantRemark.Description();
        }
    }
}