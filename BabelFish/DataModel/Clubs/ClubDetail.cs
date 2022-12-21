using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Text;
using Newtonsoft.Json.Serialization;
using ShootersTech.BabelFish.Helpers;

namespace ShootersTech.BabelFish.DataModel.Clubs {
    /// <summary>
    /// Complete data about an Orion club account.
    /// </summary>
    public class ClubDetail {

        public ClubDetail() { }

        [OnDeserialized]
        internal void OnDeserialized( StreamingContext context ) {
            if (AdministratorList == null)
                AdministratorList = new List<Person>();
            if (Notes == null)
                Notes = new List<string>();
            if (LicenseList == null)
                LicenseList = new List<ClubLicense>();
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
        [DefaultValue( "" )]
        public string OwnerId { get; set; } = string.Empty;

        /// <summary>
        /// The list of people who are Administrators for this club.
        /// </summary>
        [DefaultValue( "" )]
        public List<Person> AdministratorList { get; set; } = new List<Person>();

        /// <summary>
        /// The email address of the club. May in fact be the email address of the administrator.
        /// </summary>
        [DefaultValue( "" )]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// The phone number of the club. May in fact be the phone number of the club's administrator.
        /// </summary>
        [DefaultValue( "" )]
        public string Phone { get; set; } = string.Empty;

        /// <summary>
        /// The city and state (and maybe country) where the club is from.
        /// </summary>
        /// <example>Axtell, NE</example>
        [DefaultValue( "" )]
        public string Hometown { get; set; } = string.Empty;

        /// <summary>
        /// The date the orion account was created. Formatted as yyyy-MM-dd
        /// </summary>
        /// <example>2001-01-01</example>
        [DefaultValue( "2001-01-01" )]
        public string MemberSince { get; set; } = DateTime.Today.ToString( DateTimeFormats.DATE_FORMAT );

        /// <summary>
        /// The URL path in www.shooterstech.net/clubs/{path} linking to their team page.
        /// </summary>
        /// <example>northeast</example>
        [DefaultValue( "" )]
        public string URLPath { get; set; } = string.Empty;

        /// <summary>
        /// The x-api-key for use by this Club.
        /// </summary>
        public string ApiKey { get; set; } = string.Empty;

        /// <summary>
        /// A list of notes, written by the Shooter's Tech support team pertaining to this Orion Club.
        /// </summary>
        public List<string> Notes { get; set; } = new List<string>();

        /// <summary>
        /// The list of Orion Licenses this Club has. Most Clubs will have exactly one license. 
        /// </summary>
        public List<ClubLicense> LicenseList { get; set; } = new List<ClubLicense>();

        public override string ToString() {
            return $"{Name} {OwnerId}";
        }
    }
}
