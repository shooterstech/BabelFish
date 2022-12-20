using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using ShootersTech.BabelFish.Helpers;

namespace ShootersTech.BabelFish.DataModel.Clubs {
    /// <summary>
    /// Abbreviated data about an Orion club account.
    /// </summary>
    public class ClubAbbr {

        public ClubAbbr() { }

        /// <summary>
        /// The orion account number, usually 4 digits.
        /// </summary>
        /// <example>1234</example>
        [DefaultValue(0)]
        public int AccountNumber { get; set; } = 0;

        /// <summary>
        /// The name of the club or individual who own's this Orion license.
        /// </summary>
        /// <example>Northeast High School</example>
        [DefaultValue("")]
        public string Name { get; set; } = String.Empty;

        /// <summary>
        /// Formatted string identifying this is an Orion account and the Account Number.
        /// </summary>
        /// <example>OrionAcct001234</example>
        [DefaultValue( "" )]
        public string OwnerId { get; set; } = String.Empty;

        /// <summary>
        /// The primary point of contact (first and last name) for this Orion account.
        /// </summary>
        /// <example>Martin McMartin</example>
        [DefaultValue( "" )]
        public string Administrator { get; set; } = String.Empty;

        /// <summary>
        /// The email address of the club. May in fact be the email address of the administrator.
        /// </summary>
        [DefaultValue( "" )]
        public string Email { get; set; } = String.Empty;

        /// <summary>
        /// The phone number of the club. May in fact be the phone number of the club's administrator.
        /// </summary>
        [DefaultValue( "" )]
        public string Phone { get; set; } = String.Empty;

        /// <summary>
        /// The city and state (and maybe country) where the club is from.
        /// </summary>
        /// <example>Axtell, NE</example>
        [DefaultValue( "" )]
        public string Hometown { get; set; } = String.Empty;

        /// <summary>
        /// The date the orion account was created. Formatted as yyyy-MM-dd
        /// </summary>
        /// <example>2001-01-01</example>
        [DefaultValue( "" )]
        public string MemberSince { get; set; } = DateTime.Today.ToString( DateTimeFormats.DATE_FORMAT );

        /// <summary>
        /// The URL path in www.shooterstech.net/clubs/{path} linking to their team page.
        /// </summary>
        /// <example>northeast</example>
        [DefaultValue( "" )]
        public string URLPath { get; set; } = String.Empty;
    }
}
