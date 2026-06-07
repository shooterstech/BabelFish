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

        public override string ToString() {
            return $"Sublicense {SubLicense}";
        }
    }


}
