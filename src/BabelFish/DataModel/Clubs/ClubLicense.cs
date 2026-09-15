using System.Runtime.Serialization;
using Version = Scopos.BabelFish.DataModel.Common.Version;

namespace Scopos.BabelFish.DataModel.Clubs {
    /// <summary>
    /// Describes an Orion license an Orion user may have.
    /// </summary>
    public class ClubLicense {

        private Logger _logger = LogManager.GetCurrentClassLogger();

        public ClubLicense() {
        }

        [OnDeserialized]
        internal void OnDeserialized( StreamingContext context ) {
            if (Notes == null)
                Notes = new List<string>();
            if (Capabilities == null)
                Capabilities = new List<ClubLicenseCapability>();
        }

        /// <summary>
        /// This is the preferred method for creating a new ClubLicense instance. It ensures that the license is properly
        /// associated with the provided ClubDetail and that the club has not exceeded its license limit.
        /// </summary>
        /// <param name="club"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        public static Task<ClubLicense> CreateAsync( ClubDetail club ) {
            // Although this method is currently synchronous in its implementation, it is defined as CreateAsync() to allow for future enhancements
            // that may involve asynchronous operations (e.g., database calls, API requests) during the creation process.
            // By giving this Async name now, we can avoid breaking changes in the future if such enhancements are needed.

            if (club is null)
                throw new ArgumentNullException( nameof( club ), "ClubDetail cannot be null when creating a ClubAddress." );

            if (!club.MayAddLicense)
                throw new InvalidOperationException( "The club has reached its license limit and cannot add more licenses." );

            ClubLicense clubLicense = new ClubLicense();
            var nextSubLicenseLetter = (char)('A' + club.LicenseList.Count);
            clubLicense.Club = club;
            clubLicense.LicenseType = club.AccountType;
            clubLicense.SubLicense = nextSubLicenseLetter.ToString();

            if (club.LicenseList.Count == 0) {
                // If this is the first license being created for the club, assign default capabilities and expiration date.
                clubLicense.Capabilities = new List<ClubLicenseCapability>() { ClubLicenseCapability.VIS_SCANNER };
                clubLicense.ExpirationDate = DateTime.Today.AddDays( 365 );
            } else {
                // If there are existing licenses, copy the capabilities and expiration date from the first license.
                clubLicense.Capabilities = club.LicenseList.First().Capabilities;
                clubLicense.ExpirationDate = club.LicenseList.First().ExpirationDate;
            }
            club.LicenseList ??= new List<ClubLicense>();
            club.LicenseList.Add( clubLicense );

            return Task.FromResult( clubLicense );
        }

        /// <summary>
        /// Sets the Club property on the cloned instance to match the source instance. This ensures that when a ClubAddress is cloned, it remains associated with the same ClubDetail as the original.
        /// </summary>
        /// <param name="source">The original object that was cloned.</param>
        public void OnCloned( object source ) {

            if (source is ClubAddress sourceAddress) {
                this.Club = sourceAddress.Club;
            }
        }

        /// <summary>
        /// Typically a single character, uniquely identifying a single license an Orion Club owns.
        /// </summary>
        /// <example>A</example>
        public string SubLicense { get; set; } = "A";

        /// <summary>
        /// The date this license expires.
        /// <para>By default, this is set to one year from the date of license generation, but it can be set to any date.</para>
        /// </summary>
        [G_STJ_SER.JsonConverter( typeof( G_BF_STJ_CONV.ScoposDateOnlyConverter ) )]
        [G_NS.JsonConverter( typeof( G_BF_NS_CONV.DateConverter ) )]
        public DateTime ExpirationDate { get; set; } = DateTime.Today.AddYears( 1 );

        /// <summary>
        /// The type of license this is.
        /// </summary>
        [Obsolete( "No longer used. As of BabelFish 2.0 the LicenseType is now a property of ClubDetail (Jun 2026). May be removed with the sql column is also removed." )]
        public ClubLicenseType LicenseType { get; set; } = ClubLicenseType.INDIVIDUAL;

        /// <summary>
        /// The complete, multi-line next of the license file. An empty string means the license file is either not genratated or it's expired.
        /// </summary>
        public string LicenseFile { get; set; } = string.Empty;

        /// <summary>
        /// Indicates that the customer wishes the license to be renewed when it expires. This is only a preference and does not guarantee renewal.
        /// If false, the license will not be renewed and will expire on the ExpirationDate. 
        /// </summary>
        public bool Renew { get; set; } = true;

        /// <summary>
        /// A license is considered cancelled, if renew if false and the ExpirationDate on this license is less than the calculated ExpirationDate on the owning ClubDetail.
        /// Which should be any license that was not set to renew, the last time the Club paid for their renewal licenses. 
        /// </summary>
        [G_NS.JsonIgnore]
        [G_STJ_SER.JsonIgnore]
        public bool Cancelled {
            get {
                // If Club is null, we cannot determine if the license is cancelled. The next best thing is if Renew is true, the license is not cancelled. If Renew is false and the ExpirationDate is in the past, we consider it cancelled.
                if (Club == null)
                    return Renew && ExpirationDate < DateTime.Today;

                return (!Renew && ExpirationDate < Club?.ExpirationDate);
            }
        }

        /// <summary>
        /// Notes the Shooter's Tech support team took pertaining to this license.
        /// </summary>
        [G_NS.JsonIgnore]
        [G_STJ_SER.JsonIgnore]
        [Obsolete( "Replaced with ScoposNoteService" )]
        public List<string> Notes { get; set; } = new List<string>();

        /// <summary>
        /// A short, 6 to 8 character random code the existing user may use to install this sub license. It is valid once and then only until the download date.
        /// </summary>
        /// <example>ABCDEFG</example>
        [Obsolete( "No longer used with BabelFish 2.0 / Orion 3.0" )]
        public string DownloadCode { get; set; } = string.Empty;

        /// <summary>
        /// The date that the DownloadCode is valid until. 
        /// </summary>
        [G_STJ_SER.JsonConverter( typeof( G_BF_STJ_CONV.ScoposDateOnlyConverter ) )]
        [G_NS.JsonConverter( typeof( G_BF_NS_CONV.DateConverter ) )]
        [Obsolete( "No longer used with BabelFish 2.0 / Orion 3.0" )]
        public DateTime DownloadDate { get; set; } = DateTime.Today;

        /// <summary>
        /// The list of capabilities this license includes.
        /// </summary>
        [Obsolete( "No longer used with BabelFish 2.0 / Orion 3.0" )]
        public List<ClubLicenseCapability> Capabilities { get; set; } = new List<ClubLicenseCapability>();

        /// <summary>
        /// The last time that this instance of Orion checked in, this is the version of Orion it was running.
        /// </summary>
        public Version FirmwareVersion { get; set; } = Version.Parse( "1.0.0.0" );

        /// <summary>
        /// The date and time that this instance of Orion last checked in.
        /// </summary>
        [G_STJ_SER.JsonConverter( typeof( G_BF_STJ_CONV.ScoposDateTimeConverter ) )]
        [G_NS.JsonConverter( typeof( G_BF_NS_CONV.DateTimeConverter ) )]
        public DateTime FirmwareDate { get; set; } = DateTime.MinValue;

        /// <summary>
        /// Backwards pointer to the owning ClubDetail. This is not serialized in the API response, but is provided for ease of use in client applications.
        /// </summary>
        [G_NS.JsonIgnore]
        [G_STJ_SER.JsonIgnore]
        public ClubDetail? Club { get; set; }

        public override string ToString() {
            return $"Sublicense {SubLicense}";
        }
    }


}
