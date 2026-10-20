using System.ComponentModel;

namespace Scopos.BabelFish.DataModel.Common {
    /// <summary>
    /// Abbreviated information about a person
    /// </summary>
    public class Contact {
        /*
         * DEVELOPERS NOTE: EKA 2023-03-04
         * Previously named Person. Changed to Contact to highlight it is meant to be abbreviated. And to provid clarity vs Match objects Individual and Participant.
         */

        public Contact() {

        }

        /// <summary>
        /// The display name for this person.
        /// </summary>
        /// <example>Martin Martie McMartin</example>
        [DefaultValue( "" )]
        [G_STJ_SER.JsonPropertyOrder( 2 )]
        [G_NS.JsonProperty( Order = 2 )]
        public string DisplayName { get; set; } = string.Empty;

        /// <summary>
        /// The full URL Path to their public page on Rezults. For example https://rezults.com/z/erik/
        /// </summary>
        [G_STJ_SER.JsonPropertyOrder( 5 )]
        [G_NS.JsonProperty( Order = 5 )]
        public string AccountUrl { get; set; } = string.Empty;

        /// <summary>
        /// The primary email address for this person.
        /// </summary>
        [DefaultValue( "" )]
        [G_STJ_SER.JsonPropertyOrder( 10 )]
        [G_NS.JsonProperty( Order = 10 )]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// The primary phone number for this person.
        /// </summary>
        [DefaultValue( "" )]
        [G_STJ_SER.JsonPropertyOrder( 11 )]
        [G_NS.JsonProperty( Order = 11 )]
        public string Phone { get; set; } = string.Empty;

        /// <inheritdoc />
        public override string ToString() {
            return DisplayName;
        }
    }
}
