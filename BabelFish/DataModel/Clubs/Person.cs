using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Scopos.BabelFish.DataModel.Clubs {
    /// <summary>
    /// Abbreviated information about a person
    /// </summary>
    public class Person {
        /*
         * DEVELOPERS NOTE: EKA 2022-12-20
         * A 'Person' is a fairly abstract data type. I'm incluidng it as part of the Scopos.BabelFish.DataModel.Clubs 
         * namespace as this matches the API Model. It is possible, and perhaps wise, that a more abstract data model is 
         * used in the future, one that is not specifically tied to .Clubs.
         */

        public Person() {

        }

        /// <summary>
        /// The given name, or sometimes called the first name (in western cultres), or sometimes called the familiar name, for this person.
        /// </summary>
        /// <example>Martin</example>
        [DefaultValue( "" )]
        public string GivenName { get; set; } = String.Empty;

        /// <summary>
        /// The middle name, for this person.
        /// </summary>
        /// <example>Martie</example>
        [DefaultValue( "" )]
        public string MiddleName { get; set; } = String.Empty;

        /// <summary>
        /// The family name, or sometimes called the last name (in western cultres) for this person.
        /// </summary>
        /// <example>McMartin</example>
        [DefaultValue( "" )]
        public string FamilyName { get; set; } = String.Empty;

        /// <summary>
        /// The display name for this person.
        /// </summary>
        /// <example>Martin Martie McMartin</example>
        [DefaultValue( "" )]
        public string DisplayName { get; set; } = String.Empty;

        /// <summary>
        /// The abbreviated or shorten display name for this person.
        /// </summary>
        /// <example>M McMartin</example>
        [DefaultValue( "" )]
        public string DisplayNameShort { get; set; } = String.Empty;

        /// <summary>
        /// The email address for this person.
        /// </summary>
        [DefaultValue( "" )]
        public string Email { get; set; } = String.Empty;

        /// <summary>
        /// The phone number for this person.
        /// </summary>
        [DefaultValue( "" )]
        public string Phone { get; set; } = String.Empty;

        public override string ToString() {
            return DisplayName;
        }
    }
}
