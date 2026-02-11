using System.Globalization;
using System.Runtime.Serialization;
using Scopos.BabelFish.DataModel.Common;
using Location = Scopos.BabelFish.DataModel.Common.Location;

namespace Scopos.BabelFish.DataModel.OrionMatch {
    [Serializable]
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
        public MatchID? MatchID { get; set; }

        public MatchID? ParentID { get; set; }

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
        public string OwnerName { get; set; } = string.Empty;

        public string ClubURL { get; set; } = string.Empty;

        /// <summary>
        /// The full URL for the owner's club URL.
        /// </summary>
        /// <example>https://rezults.scopos.tech/club/westpotomac/</example>
        public string ClubURLRezults {
            get {
                return $"https://rezults.scopos.tech/club/{ClubURL}/";
            }
        }


        /// <summary>
        /// Start Date of the Match. Formatted as YYYY-MM-dd.
        /// To retreive the Start Date as a DateTime use SetEndDate().
        /// </summary>
        public string StartDate { get; set; } = string.Empty;

        /// <summary>
        /// Returns the value of .StartDate as a DateTime instnace.
        /// </summary>
        /// <returns></returns>
        public DateTime GetStartDate() {

            try {
                DateTime startDate = DateTime.ParseExact( StartDate, DateTimeFormats.DATE_FORMAT, CultureInfo.InvariantCulture );
                return startDate;
            } catch {
                //Shouldn't ever reach here. Choosing not to throw an exception as it's really unlikely.
                return DateTime.Today;
            }
        }

        /// <summary>
        /// End Date of the Match. Formatted as YYYY-MM-dd.
        /// To retreive the End Date as a DateTime use GetEndDate().
        /// </summary>
        public string EndDate { get; set; } = string.Empty;

        /// <summary>
        /// Returns the value of .EndDate as a DateTime instance.
        /// </summary>
        /// <returns></returns>
        public DateTime GetEndDate() {

            try {
                DateTime endDate = DateTime.ParseExact( EndDate, DateTimeFormats.DATE_FORMAT, CultureInfo.InvariantCulture );
                return endDate;
            } catch {
                //Shouldn't ever reach here. Choosing not to throw an exception as it's really unlikely.
                return DateTime.Today;
            }
        }

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

        /// <summary>
        /// Returns a boolean indicating if the match is scheduled to take place in the future. Is based off of the match's .StartDate
        /// </summary>
        public bool IsInFuture {
            get {
                return GetStartDate() > DateTime.Today;
            }
        }

        /// <summary>
        /// Returns a boolean indicating if the match schedule is in the past (has completed). Is based off of the match's .EndDate
        /// </summary>
        public bool IsInThePast {
            get {
                return GetEndDate() < DateTime.Today;
            }
        }

        /// <summary>
        /// Returns a boolean indicating if the match is scheduled to be going on today. Is based off of both the .StartDate and .EndDate.
        /// </summary>
        public bool IsGoingOnNow {
            get {
                return !IsInFuture && !IsInThePast;
            }
        }

        public bool IsVirtualMatch {
            get {
                try {
                    return this.MatchID.VirtualMatch;
                } catch (Exception ex) {
                    return false;
                }
            }
        }

        public override string ToString() {
            StringBuilder foo = new StringBuilder();
            foo.Append( "MatchAbbr for " );
            foo.Append( MatchName );
            return foo.ToString();
        }
    }
}
