using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using ShootersTech.BabelFish.Helpers;

namespace ShootersTech.BabelFish.DataModel.Clubs {
    /// <summary>
    /// Describes an Orion license an Orion user may have.
    /// </summary>
    public class ClubLicense {

        public ClubLicense() { }

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
        /// The date this license expires. Formatted as yyyy-MM-dd
        /// </summary>
        /// <example>2001-01-01</example>
        public string ExpirationDate { get; set; } = DateTime.Today.ToString( DateTimeFormats.DATE_FORMAT );

        /// <summary>
        /// The type of license this is.
        /// </summary>
        public ClubLicenseType LicenseType { get; set; } = ClubLicenseType.INDIVIDUAL;

        /// <summary>
        /// The complete, multi-line next of the license file. An empty string means the license file is either not genratated or it's expired.
        /// </summary>
        public string LicenseFile { get; set; } = string.Empty;

        /// <summary>
        /// Notes the Shooter's Tech support team took pertaining to this license.
        /// </summary>
        public List<string> Notes { get; set; } = new List<string>();

        /// <summary>
        /// A short, 6 to 8 character random code the existing user may use to install this sub license. It is valid once and then only until the download date.
        /// </summary>
        /// <example>ABCDEFG</example>
        public string DownloadCode { get; set; } = string.Empty;

        /// <summary>
        /// The date that the DownloadCode is valid until. Formatted as yyyy-MM-dd
        /// </summary>
        /// <example>2001-01-01</example>
        public string DownloadDate { get; set; } = DateTime.Today.ToString( DateTimeFormats.DATE_FORMAT );

        /// <summary>
        /// The list of capabilities this license includes.
        /// </summary>
        public List<ClubLicenseCapability> Capabilities { get; set; } = new List<ClubLicenseCapability>();
    }

    [JsonConverter( typeof( StringEnumConverter ) )]
    public enum ClubLicenseCapability { VISSCANNER, SCORECARD, CMPUPLOAD, PRIVILEGED };


    [JsonConverter( typeof( StringEnumConverter ) )]
    public enum ClubLicenseType { INDIVIDUAL, HOME, SITE, TEMPORARY };
}
