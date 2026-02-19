using Scopos.BabelFish.APIClients;
using Scopos.BabelFish.DataModel.Common;
using Location = Scopos.BabelFish.DataModel.Common.Location;

namespace Scopos.BabelFish.DataModel.OrionMatch {

    /// <summary>
    /// A MatchAbbr object contains pertainent but not complete information about an Orion match. Intended
    /// to be returned as a list from a search. The information in a MatchAbbr instance can then be used
    /// to look up details of a match using the <seealso cref="OrionMatchAPIClient.GetMatchPublicAsync(Requests.OrionMatchAPI.GetMatchPublicRequest)"/>
    /// (or similiar ) API call.
    /// </summary>
    [Serializable]
    public class MatchAbbr {

        /// <summary>
        /// Public constructor.
        /// </summary>
        public MatchAbbr() {

        }

        /// <summary>
        /// Unique <seealso cref="Scopos.BabelFish.DataModel.OrionMatch.MatchID"/> for the competition.
        /// May use this value with other OrionMatchAPI calls including 
        /// <seealso cref="OrionMatchAPIClient.GetMatchPublicAsync(Requests.OrionMatchAPI.GetMatchPublicRequest)"/>.
        /// </summary>
        public MatchID MatchID { get; set; }

        /// <summary>
        /// If this is a Virtual Match, this is the <seealso cref="Scopos.BabelFish.DataModel.OrionMatch.MatchID"/> of the parent match.
        /// </summary>
        [G_NS.JsonIgnore]
        public MatchID ParentID {
            get {
                return this.MatchID.GetParentMatchID();
            }
        }

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

        /// <summary>
        /// The url folder path for the owner of this match. For the complete url see <seealso cref="ClubURLRezults"/>
        /// </summary>
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
        [G_STJ_SER.JsonConverter( typeof( G_BF_STJ_CONV.ScoposDateOnlyConverter ) )]
        [G_NS.JsonConverter( typeof( G_BF_NS_CONV.DateConverter ) )]
        public DateTime StartDate { get; set; }

        /// <summary>
        /// End Date of the Match. Formatted as YYYY-MM-dd.
        /// To retreive the End Date as a DateTime use GetEndDate().
        /// </summary>
        [G_STJ_SER.JsonConverter( typeof( G_BF_STJ_CONV.ScoposDateOnlyConverter ) )]
        [G_NS.JsonConverter( typeof( G_BF_NS_CONV.DateConverter ) )]
        public DateTime EndDate { get; set; }

        /// <summary>
        /// The SetName of the course of fire shot in this match.
        /// </summary>
        [Obsolete( "Starting with Orion 3.0 matches may have multiple courses of fire. This property will be replaced with a list of COURSE OF FIRE set names." )]
        public string CourseOfFireDef { get; set; } = string.Empty;

        /// <summary>
        /// Where this match took place.
        /// </summary>
        public Location Location { get; set; }

        /// <summary>
        /// A list of contacts for this match, such as the match director.
        /// </summary>
        public List<Contact> MatchContacts { get; set; }

        /// <summary>
        /// A list of scoring systems used in this match.
        /// </summary>
        [Obsolete( "Starting with Orion 3.0 matches may have multiple courses of fire and thus multiple EVENT STYLES. This property will be replaced with ... something, not sure what yet." )]
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
                return this.StartDate > DateTime.Today;
            }
        }

        /// <summary>
        /// Returns a boolean indicating if the match schedule is in the past (has completed). Is based off of the match's .EndDate
        /// </summary>
        public bool IsInThePast {
            get {
                return this.EndDate < DateTime.Today;
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

        /// <summary>
        /// Helper method that returns a boolean indicating if this match is a virtual match.
        /// </summary>
        [Obsolete( "Starting with Orion 3.0 all matches will be Virtual Matches. So this property will no longer be needed." )]
        public bool IsVirtualMatch {
            get {
                return this.MatchID.VirtualMatch;
            }
        }

        /// <inheritdoc/>
        public override string ToString() {
            return $"MatchAbbr for {MatchName}";
        }
    }
}
