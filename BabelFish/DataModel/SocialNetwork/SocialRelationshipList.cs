using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;
using Scopos.BabelFish.Converters;
using Scopos.BabelFish.DataModel.Definitions;

namespace Scopos.BabelFish.DataModel.SocialNetwork {

    /// <summary>
    /// Represents the data returned by a Social Network API call.
    /// </summary>
    public class SocialRelationshipList : ITokenItems<SocialRelationship> {

        public SocialRelationshipList() { }

        public bool AsActive { get; set; } = false;
        public bool AsPassive { get; set; } = false;
        public bool IncomingRequests { get; set; } = false;
        public bool OutgoingRequests { get; set; } = false;

        public List<SocialRelationship> Items { get; set; } = new List<SocialRelationship>();

        /// <inheritdoc />
		[JsonConverter( typeof( NextTokenConverter ) )]
        public string NextToken { get; set; } = string.Empty;

        /// <inheritdoc />
        public int Limit { get; set; } = 50;

        /// <inheritdoc />
        public bool HasMoreItems {
            get {
                return !string.IsNullOrEmpty( NextToken );
            }
        }

        public override string ToString() {
            return $"SocialRelationshipList with {Items.Count} items";
        }

    }
}
