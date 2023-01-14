using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.Serialization;
using System.Text;
using BabelFish.DataModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using NLog;
using Scopos.BabelFish.Helpers;

namespace Scopos.BabelFish.DataModel.Clubs {
    /// <summary>
    /// Describes an Orion license an Orion user may have.
    /// </summary>
    public class ClubLicense : IObjectRelationalMapper {

        private Logger logger = LogManager.GetCurrentClassLogger();
        private DateTime expirationDate = DateTime.Today;
        private DateTime downloadDate = DateTime.Today;

        public ClubLicense() {
            NewRecord = true;
        }

        [OnDeserialized]
        internal void OnDeserialized( StreamingContext context ) {
            NewRecord = false; //If this object is being deserialized, we can assume it is an existing record.
            if (Notes == null)
                Notes = new List<string>();
            if (Capabilities == null)
                Capabilities = new List<ClubLicenseCapability>();
        }

        /// <summary>
        /// Human name of the admin in charge of this license.
        /// </summary>
        public string User { get; set; } = string.Empty;

        public int LicenseNumber { get; set; } = 0;

        /// <summary>
        /// Typically a single character, uniquely identifying a single license an Orion Club owns.
        /// </summary>
        /// <example>A</example>
        public string SubLicense { get; set; } = "A";

        /// <summary>
        /// The date this license expires. Formatted as yyyy-MM-dd. 
        /// To Get/Set ExpriationDate as a DateTime object use GetExpirationDate() or SetExpriationDate()
        /// </summary>
        /// <example>2001-01-01</example>
        public string ExpirationDate {
            get {
                return expirationDate.ToString( DateTimeFormats.DATE_FORMAT );
            }
            set {
                try {
                    //Test if we can parse the input without throwing an error.
                    var parsedDate = DateTime.ParseExact( value, DateTimeFormats.DATE_FORMAT, CultureInfo.InvariantCulture );
                    //If the input can be parsed, we can go ahead and set the value.
                    expirationDate = parsedDate;
                } catch (Exception ex) {
                    var msg = $"Unable to parse the input Date {value}.";
                    logger.Error( msg, ex );
                }
            }
        }

        public DateTime GetExpirationDate() {
            return expirationDate;
        }

        public void SetExpirationDate( DateTime expirationDate ) {
            this.expirationDate = expirationDate;
        }

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
        public string DownloadDate {
            get {
                return downloadDate.ToString( DateTimeFormats.DATE_FORMAT );
            }
            set {
                try {
                    //Test if we can parse the input without throwing an error.
                    var parsedDate = DateTime.ParseExact( value, DateTimeFormats.DATE_FORMAT, CultureInfo.InvariantCulture );
                    //If the input can be parsed, we can go ahead and set the value.
                    downloadDate = parsedDate;
                } catch (Exception ex) {
                    var msg = $"Unable to parse the input Date {value}.";
                    logger.Error( msg, ex );
                }
            }
        } 

        public DateTime GetDownloadDate() {
            return downloadDate;
        }

        public void SetDownloadDate( DateTime downloadDate ) {
            this.downloadDate = downloadDate;
        }

        /// <summary>
        /// The list of capabilities this license includes.
        /// </summary>
        public List<ClubLicenseCapability> Capabilities { get; set; } = new List<ClubLicenseCapability>();

        /// <inheritdoc />
        [JsonIgnore]
        public bool NewRecord { get; set; }

        public override string ToString() {
            return $"Sublicense {SubLicense}";
        }
    }


}
