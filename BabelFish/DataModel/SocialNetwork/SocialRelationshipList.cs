using System;
using System.Collections.Generic;
using System.Text;
using Scopos.BabelFish.DataModel.Definitions;

namespace Scopos.BabelFish.DataModel.SocialNetwork {

    /// <summary>
    /// Represents the data returned by a GetScoreAverage API call.
    /// </summary>
    public class SocialRelationshipList : ITokenItems<SocialRelationship> {

        public SocialRelationshipList() { }

        public List<SocialRelationship> Items { get; set; } = new List<SocialRelationship>();

        /// <inheritdoc />
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
