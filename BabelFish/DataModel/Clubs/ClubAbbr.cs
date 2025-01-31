using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using NLog;
using Location = Scopos.BabelFish.DataModel.Common.Location;

namespace Scopos.BabelFish.DataModel.Clubs {
    /// <summary>
    /// Abbreviated data about an Orion club account.
    /// </summary>
    public class ClubAbbr : IComparable<ClubAbbr>
    {
        public int CompareTo(ClubAbbr other)
        {
            int compare = this.AccountNumber.CompareTo(other.AccountNumber);
            if (compare != 0)
                return compare;

            compare = this.Name.CompareTo(other.Name);
            if (compare != 0)
                return compare;

            compare = this.IsCurrentlyShooting.CompareTo(other.IsCurrentlyShooting);
            if (compare != 0)
                return compare;

            return this.AccountNumber.CompareTo(other.AccountNumber);
        }

        private Logger logger = LogManager.GetCurrentClassLogger();
        private DateTime memberSince = DateTime.Today;

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
        public string OwnerId { 
            get {
                return $"OrionAcct{AccountNumber:D6}";
            }
        }

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
        /// The date the orion account was created. 
        /// </summary>
        /// <example>2001-01-01</example>
        [G_STJ_SER.JsonConverter( typeof( G_BF_STJ_CONV.ScoposDateOnlyConverter ) )]
        [G_NS.JsonConverter( typeof( G_BF_NS_CONV.DateConverter ) )]
        public DateTime MemberSince { get; set; }

        /// <summary>
        /// The date and time of the last shot this club fired that was publicly displayed.
        /// </summary>
        /// <example>2001-01-01</example>
        [G_STJ_SER.JsonConverter( typeof( Scopos.BabelFish.Converters.Microsoft.ScoposDateTimeConverter ) )]
        [G_NS.JsonConverter( typeof( G_BF_NS_CONV.DateTimeConverter ) )]
        public DateTime LastPublicShot { get; set; }

        /// <summary>
        /// bool set by LastPublicShot, if shot within the last 15 minutes, it is true.
        /// </summary>
        /// <example>false</example>
        [DefaultValue(false)]
        public bool IsCurrentlyShooting { get { return LastPublicShot.AddMinutes(15) >= DateTime.UtcNow; }  }

        /// <summary>
        /// The URL path in www.Scopos.net/clubs/{path} linking to their team page.
        /// </summary>
        /// <example>northeast</example>
        [DefaultValue( "" )]
        public string URLPath { get; set; } = String.Empty;

        public Location Location { get; set; }

        public override string ToString() {
            return $"{Name} {OwnerId}";
        }

    }
}
