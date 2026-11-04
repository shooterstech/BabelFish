using System.ComponentModel;
using Scopos.BabelFish.DataModel.Image;

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
        /// Gets or sets the relative URL path for the account resource. For the full path, use the <see cref="AccountUrl"/> property.
        /// </summary>
        [G_STJ_SER.JsonPropertyOrder( 5 )]
        [G_NS.JsonProperty( Order = 5 )]
        public string AccountUrlPath { get; set; } = string.Empty;

        /// <summary>
        /// The full URL to their public page on Rezults. For example https://rezults.scopos.tech/z/erik/.
        /// <para>An empty string is returned if the AccountUrlPath is not set or not public.</para>
        /// </summary>
        [G_STJ_SER.JsonIgnore]
        [G_NS.JsonIgnore]
        public string AccountUrl {
            get {
                if (string.IsNullOrWhiteSpace( this.AccountUrlPath )) {
                    return string.Empty;
                } else {
                    return $"https://rezults.scopos.tech/z/{this.AccountUrlPath}/";
                }
            }
        }

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

        /// <summary>
        /// The public profile image for this contact. Value may be null if the user has not uploaded a profile image.
        /// </summary>
        [G_STJ_SER.JsonPropertyOrder( 15 )]
        [G_NS.JsonProperty( Order = 15 )]
        public ScoposImageAbbr? ProfileImage { get; set; } = null;

        /// <summary>
        /// The UUID formatted Scopos Account user id for this contact.
        /// </summary>
        [DefaultValue( "" )]
        [G_STJ_SER.JsonPropertyOrder( 50 )]
        [G_NS.JsonProperty( Order = 50 )]
        public string UserId { get; set; } = string.Empty;

        /// <inheritdoc />
        public override string ToString() {
            return DisplayName;
        }
    }
}
