using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Scopos.BabelFish.Helpers;
using Scopos.BabelFish.DataModel.AttributeValue;
using Scopos.BabelFish.DataModel.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using NLog;

namespace Scopos.BabelFish.DataModel.OrionMatch {

    [Serializable]
    public class League {

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
        /// Name of the Match
        /// </summary>
        [JsonProperty( Order = 7 )]
        public string LeagueName {
            get; set;
        } = string.Empty;


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

        /// <summary>
        /// Unique LeagueID for this League.
        /// 
        /// This is a required field.
        /// </summary>
        [JsonProperty( Order = 11 )]
        public string LeagueID { get; set; } = string.Empty;

        public int LeagueNetworkID { get; set; }

        public string LeagueNetworkName { get; set; }

        public int SeasonID { get; set; }

        public string SeasonName { get; set; }

        public string MatchType { get { return "League"; } }

		/// <summary>
		/// Contact information for the match, i.e. person's name, phone, email.
		/// </summary>
		public Contact LeagueAdministrator { get; set; } = new Contact();

		/// <summary>
		/// Start Date of the Match. Formatted as YYYY-MM-dd
		/// </summary>
		[JsonProperty( Order = 10 )]
        public string StartDate { get; set; } = string.Empty;

        /// <summary>
        /// Returns the value of .StartDate property as a DateTime object. If .StartDate can not
        /// be parsed returns .Today as the value.
        /// </summary>
        /// <returns></returns>
        public DateTime GetStartDate() {
            try {
                return DateTime.ParseExact( StartDate, DateTimeFormats.DATE_FORMAT, CultureInfo.InvariantCulture );
            } catch (Exception ex) {
                logger.Error( ex, $"Unable to parse StartDate with value '{StartDate}' as a DateTime from League ID {LeagueID}." );
                return DateTime.Today;
            }
        }

        /// <summary>
        /// End Date of the Match. Formatted as YYYY-MM-dd
        /// </summary>
        [JsonProperty( Order = 13 )]
        public string EndDate { get; set; } = string.Empty;

        /// <summary>
        /// Returns the value of .StartDate property as a DateTime object. If .StartDate can not
        /// be parsed returns .Today as the value.
        /// </summary>
        /// <returns></returns>
        public DateTime GetEndDate() {
            try {
                return DateTime.ParseExact( EndDate, DateTimeFormats.DATE_FORMAT, CultureInfo.InvariantCulture );
            } catch (Exception ex) {
                logger.Error( ex, $"Unable to parse EndDate with value '{EndDate}' as a DateTime from League ID {LeagueID}." );
                return DateTime.Today;
            }
        }

        public LeagueConfiguration Configuration { get; set; }

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