using System.ComponentModel;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json.Serialization;
using NLog;
using Scopos.BabelFish.Converters;
using Scopos.BabelFish.DataModel.AttributeValue;
using Scopos.BabelFish.DataModel.Common;
using Scopos.BabelFish.Responses.OrionMatchAPI;

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

            if (ResultEvents == null)
                ResultEvents = new List<ResultEventAbbr>();

            if (Documents == null)
                Documents = new List<Document>();
        }

        /// <summary>
        /// The list of Events in the Match that have Result Lists associated with them.
        /// </summary>
        [JsonPropertyOrder ( 3 )]
        public List<ResultEventAbbr> ResultEvents { get; set; } = new List<ResultEventAbbr>();

        /// <summary>
        /// Sets the public visibility for the match. Valid values are
        /// Private : No visibility
        /// Protected : Only athletes, their coaches, and match officials may see
        /// Club : Same as participant, but also includes all club members of the sponsoring club
        /// Public : Everyone may view
        /// </summary>
        [JsonPropertyOrder ( 8 )]
        
        public VisibilityOption Visibility { get; set; } = VisibilityOption.PRIVATE;

        /// <summary>
        /// The orion account or at home account who owns this match.
        /// </summary>
        /// <example>OrionAcct000001 or AtHomeAcct123456</example>
        [JsonPropertyOrder ( 9 )]
        public string OwnerId { get; set; } = string.Empty;


        public string MatchType { get { return "League"; } }

		/// <summary>
		/// Contact information for the match, i.e. person's name, phone, email.
		/// </summary>
		public Contact LeagueAdministrator { get; set; } = new Contact();

        [JsonConverter( typeof( ScoposDateOnlyConverter ) )]
        public DateTime StartDate { get; set; }

        [JsonConverter( typeof( ScoposDateOnlyConverter ) )]
        public DateTime EndDate { get; set; }

        /// <summary>
        /// The full URL where the League's (small) icon image may be loaded from. 
        /// </summary>
        [DefaultValue( "" )]
        public string IconURL { get; set; } = string.Empty;

        /// <summary>
        /// The full URL where the League's (large) logo image may be loaded from. 
        /// </summary>
        [DefaultValue( "" )]
        public string LogoURL { get; set; } = string.Empty;

        /// <summary>
        /// String holding the software (Orion Scoring System) and Version number of the software.
        /// </summary>
        public string Creator { get; set; }

		public string Boilerplate { get; set; } = string.Empty;

        /// <summary>
        /// A list of documents associated with thsi league. Common documents might include
        /// the league program, or a range script. 
        /// </summary>
        public List<Document> Documents { get; set; } = new List<Document>();

        /// <summary>
        /// A Newtonsoft Conditional Property to only serialize Documents when it has one or more Document objects
        /// https://www.newtonsoft.com/json/help/html/ConditionalProperties.htm
        /// </summary>
        /// <returns></returns>
        public bool ShouldSerializeDocuments() {
            return (Documents != null && Documents.Count > 0);
        }

        public List<LeagueWeek> LeagueWeeks {
            get {
                List<LeagueWeek> weeks = new List<LeagueWeek>();

                DateTime beginingOfWeek = StartDate;
                DateTime endOfWeek = beginingOfWeek.AddDays( 6 );
                int weekNumber = 1;
                do {
                    LeagueWeek lw = new LeagueWeek() {
                        Name = $"Week {weekNumber}",
                        StartOfWeek = beginingOfWeek,
                        EndOfWeek = endOfWeek
                    };
                    weeks.Add( lw );

                    weekNumber++;
                    beginingOfWeek = endOfWeek.AddDays( 1 );
                    endOfWeek = beginingOfWeek.AddDays( 6 );
                } while ( beginingOfWeek <= EndDate );

                return weeks;
            }
        }

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