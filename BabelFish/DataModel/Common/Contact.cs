using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Scopos.BabelFish.DataModel.Common {
    /// <summary>
    /// Abbreviated information about a person
    /// </summary>
    public class Contact {
        /*
         * DEVELOPERS NOTE: EKA 2023-03-04
         * Previously named Person. Changed to Contact to highlight it is meant to be abbreviated. And to provid clarity vs Match objects Individual and Participant..
         */

        public Contact() {

        }

        /// <summary>
        /// The display name for this person.
        /// </summary>
        /// <example>Martin Martie McMartin</example>
        [DefaultValue( "" )]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// The email address for this person.
        /// </summary>
        [DefaultValue( "" )]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// The phone number for this person.
        /// </summary>
        [DefaultValue( "" )]
        public string Phone { get; set; } = string.Empty;

        public override string ToString() {
            return Name;
        }
    }
}
