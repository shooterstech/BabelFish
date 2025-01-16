using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.Serialization;
using System.Text;
using Scopos.BabelFish.DataModel;
using Scopos.BabelFish.DataModel.Common;
using NLog;
using Scopos.BabelFish.Helpers;
using System.Text.Json.Serialization;
using Scopos.BabelFish.Converters;

namespace Scopos.BabelFish.DataModel.Clubs
{
    /// <summary>
    /// Complete data about an Orion club account.
    /// </summary>
    public class ClubDetail : IObjectRelationalMapper {

        private Logger logger = LogManager.GetCurrentClassLogger();
        private DateTime memberSince = DateTime.Today;

        public ClubDetail() {
            NewRecord = true;
        }

        [OnDeserialized]
        internal void OnDeserialized( StreamingContext context ) {
            NewRecord = false; //If this object is being deserialized, we can assume it is an existing club.
            if (AdministratorList == null)
                AdministratorList = new List<Contact>();
            if (Notes == null)
                Notes = new List<string>();
            if (LicenseList == null)
                LicenseList = new List<ClubLicense>();
            if (Options == null)
                Options = new List<ClubOptions>();
            if (NamespaceList == null)
                NamespaceList = new List<NamespaceDetail>();
        }

        /// <summary>
        /// The orion account number, usually 4 digits.
        /// </summary>
        /// <example>1234</example>
        [DefaultValue( 0 )]
        public int AccountNumber { get; set; }

        /// <summary>
        /// The name of the club or individual who own's this Orion license.
        /// </summary>
        /// <example>Northeast High School</example>
        [DefaultValue( "" )]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Formatted string identifying this is an Orion account and the Account Number.
        /// </summary>
        /// <example>OrionAcct001234</example>
        public string OwnerId {
            get {
                return $"OrionAcct{AccountNumber:D6}";
            }
        } 

        /// <summary>
        /// The list of people who are Administrators for this club.
        /// </summary>
        [DefaultValue( "" )]
        public List<Contact> AdministratorList { get; set; } = new List<Contact>();

        /// <summary>
        /// The email address of the club. May in fact be the email address of the administrator.
        /// </summary>
        [DefaultValue( "" )]
        [Obsolete( "Soon to be replaced with v1.0:orion:Email Address" )]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// The phone number of the club. May in fact be the phone number of the club's administrator.
        /// </summary>
        [DefaultValue( "" )]
        [Obsolete("Soon to be replaced with v1.0:orion:Phone Number")]
        public string Phone { get; set; } = string.Empty;

        /// <summary>
        /// The city and state (and maybe country) where the club is from.
        /// </summary>
        /// <example>Axtell, NE</example>
        [DefaultValue( "" )]
        public string Hometown { get; set; } = string.Empty;

        [Obsolete( "Soon to be replaced with v1.0:orion:Address" )]
        public string Street1 { get; set; }

        [Obsolete( "Soon to be replaced with v1.0:orion:Address" )]
        public string Street2 { get; set; }

        [Obsolete( "Soon to be replaced with v1.0:orion:Address" )]
        public string City { get; set; } = string.Empty;

        [Obsolete( "Soon to be replaced with v1.0:orion:Address" )]
        public string State { get; set; } = string.Empty;

        [Obsolete( "Soon to be replaced with v1.0:orion:Address" )]
        public string PostalCode { get; set; } = string.Empty;

        [Obsolete( "Soon to be replaced with v1.0:orion:Address" )]
        public string Country { get; set; } = string.Empty;

        /// <summary>
        /// The date the orion account was created. Formatted as a string yyyy-MM-dd.
        /// To get/set MemberSince date as a DateTime object use GetMemberSince() or SetMemberSince().
        /// </summary>
        /// <example>2001-01-01</example>
        [JsonConverter( typeof( ScoposDateOnlyConverter ) )]
        public DateTime MemberSince { get; set; }

        /// <summary>
        /// The URL path in www.Scopos.net/clubs/{path} linking to their team page.
        /// </summary>
        /// <example>northeast</example>
        [DefaultValue( "" )]
        public string URLPath { get; set; } = string.Empty;

        /// <summary>
        /// The x-api-key for use by this Club.
        /// </summary>
        public string ApiKey { get; set; } = string.Empty;

        public string ApiKeyId { get; set; } = string.Empty;

        public string AWSAccessKeyId { get; set; } = string.Empty;

        public string AWSSecretAccessKey { get; set; } = string.Empty;

        public string AWSRegion { get; set; } = string.Empty;

        /// <summary>
        /// A list of notes, written by the Shooter's Tech support team pertaining to this Orion Club.
        /// </summary>
        public List<string> Notes { get; set; } = new List<string>();

        /// <summary>
        /// The list of Orion Licenses this Club has. Most Clubs will have exactly one license. 
        /// </summary>
        public List<ClubLicense> LicenseList { get; set; } = new List<ClubLicense>();

        /// <summary>
        /// A list of optional services this club has subscribed to.
        /// </summary>
        public List<ClubOptions> Options { get; set; } = new List<ClubOptions> { };

        public List<NamespaceDetail> NamespaceList { get; set; } = new List<NamespaceDetail> { };

        /// <inheritdoc />
        [JsonIgnore]
        public bool NewRecord { get; set; }

        public override string ToString() {
            return $"{Name} {OwnerId}";
        }
    }
}
