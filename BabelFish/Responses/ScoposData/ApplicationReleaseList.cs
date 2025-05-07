using Scopos.BabelFish.Converters.Microsoft;
using Scopos.BabelFish.DataModel;
using Scopos.BabelFish.DataModel.ScoposData;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Scopos.BabelFish.Responses.ScoposData
{
    public class ApplicationReleaseList : ITokenItems<ReleaseInfo>
    {
        public List<ReleaseInfo> Items { get; set; } = new List<ReleaseInfo>();

        /// <inheritdoc />
        [JsonConverter(typeof(NextTokenConverter))]
        public string NextToken { get; set; } = string.Empty;

        /// <inheritdoc />
        public int Limit { get; set; } = 50;

        /// <inheritdoc />
        public bool HasMoreItems
        {
            get
            {
                return !string.IsNullOrEmpty(NextToken);
            }
        }
    }
}
