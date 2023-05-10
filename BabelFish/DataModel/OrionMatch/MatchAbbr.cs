using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Scopos.BabelFish.DataModel.AttributeValue;
using Scopos.BabelFish.DataModel.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Scopos.BabelFish.DataModel.Athena.Shot;

namespace Scopos.BabelFish.DataModel.OrionMatch {
    public class MatchAbbr {

        public MatchAbbr() {

        }

        [OnDeserialized()]
        public void OnDeserialized( StreamingContext context ) {
            if (ScoringSystems == null)
                ScoringSystems = new List<string>();
            if (Location == null)
                Location = new Location();
            if (MatchContacts == null)
                MatchContacts = new List<Contact>();
        }

        /// <summary>
        /// Unique MatchID for the competition to get squadding for. Will match exactly (assuming no errors) of the MatchID in the GetMatchDetailRequest.
        /// </summary>
        public string MatchID { get; set; } = string.Empty;

        public string ParentID { get; set; } = string.Empty;

        /// <summary>
        /// External Result URL
        /// </summary>
        public string ResultURL { get; set; } = string.Empty;

        /// <summary>
        /// Name of the Match
        /// </summary>
        public string MatchName { get; set; } = string.Empty;

        /// <summary>
        /// The orion account or at home account who owns this match.
        /// </summary>
        /// <example>OrionAcct000001 or AtHomeAcct123456</example>
        public string OwnerId { get; set; } = string.Empty;

        /// <summary>
        /// Human readable name of the Owner, usually a school or club name.
        /// </summary>
        public string OwnerName { get; set;} = string.Empty;


        /// <summary>
        /// Start Date of the Match. Formatted as YYYY-MM-dd
        /// </summary>
        public string StartDate { get; set; } = string.Empty;

        /// <summary>
        /// End Date of the Match. Formatted as YYYY-MM-dd
        /// </summary>
        public string EndDate { get; set; } = string.Empty;

        public string CourseOfFireDef { get; set; } = string.Empty;

        public Location Location { get; set; }

        public List<Contact> MatchContacts { get; set; }

        /// <summary>
        /// A list of scoring systems used in this match.
        /// </summary>
        public List<string> ScoringSystems { get; set; } = new List<string>();

        /// <summary>
        /// The high level shooting style that this match was conducted under.
        /// </summary>
        public string ShootingStyle { get; set; } = string.Empty;

        public override string ToString() {
            StringBuilder foo = new StringBuilder();
            foo.Append( "MatchAbbr for " );
            foo.Append( MatchName );
            return foo.ToString();
        }
    }
}
