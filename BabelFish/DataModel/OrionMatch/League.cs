using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Scopos.BabelFish.Converters;
using Scopos.BabelFish.DataModel.AttributeValue;
using Scopos.BabelFish.DataModel.Common;
using Scopos.BabelFish.Responses.OrionMatchAPI;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using NLog;

namespace Scopos.BabelFish.DataModel.OrionMatch {

    [Serializable]
    public class League: LeagueBase {

        private string parentId = "";
        private Logger logger = LogManager.GetCurrentClassLogger();

        public League() { }

        
        /// <summary>
        /// After an object is deserialized form JSON,
        /// adds defaults to empty properties
        /// </summary>
        /// <param name="context"></param>
        [OnDeserialized()]
        public void OnDeserialized( StreamingContext context ) {

        }

        /// <summary>
        /// The list of Events in the Match that have Result Lists associated with them.
        /// </summary>
        [JsonProperty( Order = 3 )]
        public List<ResultEventAbbr> ResultEvents { get; set; } = new List<ResultEventAbbr>();

        /// <summary>
        /// Sets the public visibility for the match. Valid values are
        /// Private : No visibility
        /// Protected : Only athletes, their coaches, and match officials may see
        /// Club : Same as participant, but also includes all club members of the sponsoring club
        /// Public : Everyone may view
        /// </summary>
        [JsonProperty( Order = 8 )]
        [JsonConverter( typeof( StringEnumConverter ) )]
        public VisibilityOption Visibility { get; set; } = VisibilityOption.PRIVATE;

        /// <summary>
        /// The orion account or at home account who owns this match.
        /// </summary>
        /// <example>OrionAcct000001 or AtHomeAcct123456</example>
        [JsonProperty( Order = 9 )]
        public string OwnerId { get; set; } = string.Empty;


        public string MatchType { get { return "League"; } }

		/// <summary>
		/// Contact information for the match, i.e. person's name, phone, email.
		/// </summary>
		public Contact LeagueAdministrator { get; set; } = new Contact();

        [JsonConverter( typeof( DateConverter ) )]
        public DateTime StartDate { get; set; }

        [JsonConverter( typeof( DateConverter ) )]
        public DateTime EndDate { get; set; }

		/// <summary>
		/// String holding the software (Orion Scoring System) and Version number of the software.
		/// </summary>
		public string Creator { get; set; }

		public string Boilerplate { get; set; } = string.Empty;

        public override string ToString() {
            StringBuilder foo = new StringBuilder();
            foo.Append( "LeagueDetail for " );
            foo.Append( LeagueName );
            return foo.ToString();
        }

        /// <summary>
        /// UTC time the match data was last updated.
        /// </summary>
        [JsonIgnore]
        public DateTime LastUpdated { get; private set; } = DateTime.UtcNow;
    }
}