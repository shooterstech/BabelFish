using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Text;
using NLog;
using Scopos.BabelFish.Helpers;

namespace Scopos.BabelFish.DataModel.Clubs {
    /// <summary>
    /// Abbreviated data about an Orion club account.
    /// </summary>
    public class ClubAbbr {

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
        /// The date the orion account was created. Formatted as a string yyyy-MM-dd.
        /// To get/set MemberSince date as a DateTime object use GetMemberSince() or SetMemberSince().
        /// </summary>
        /// <example>2001-01-01</example>
        public string MemberSince { 
            get {
                return memberSince.ToString( DateTimeFormats.DATE_FORMAT );
            }
            set {
                try {
                    //Test if we can parse the input without throwing an error.
                    var parsedDate = DateTime.ParseExact( value, DateTimeFormats.DATE_FORMAT, CultureInfo.InvariantCulture );
                    //If the input can be parsed, we can go ahead and set the value.
                    memberSince = parsedDate;
                } catch (Exception ex) {
                    var msg = $"Unable to parse the input Date {value}.";
                    logger.Error( msg, ex );
                }
            }
        }

        /// <summary>
        /// Returns the date that this Club held an Orion License
        /// </summary>
        /// <returns></returns>
        public DateTime GetMemberSince() {
            return memberSince;
        }

        /// <summary>
        /// Sets the date that this Club has helf an Orion License
        /// </summary>
        /// <param name="memberSince"></param>
        public void SetMemberSince(DateTime memberSince) {
            this.memberSince = memberSince;
        }

        /// <summary>
        /// The URL path in www.Scopos.net/clubs/{path} linking to their team page.
        /// </summary>
        /// <example>northeast</example>
        [DefaultValue( "" )]
        public string URLPath { get; set; } = String.Empty;

        public override string ToString() {
            return $"{Name} {OwnerId}";
        }

    }
}
