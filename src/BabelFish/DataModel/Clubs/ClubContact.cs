using System.ComponentModel.DataAnnotations;
using Scopos.BabelFish.DataModel.Common;

namespace Scopos.BabelFish.DataModel.Clubs {
    /// <summary>
    /// Represents a contact method for a Club (aka Orion Account). Such as their phone number, email address, or social media link. 
    /// </summary>
    public class ClubContact : IOnCloned, IValidatableObject {

        #region Private and Protected Fields
        private string _contactValue = string.Empty;
        private VisibilityOption _visibility = VisibilityOption.PUBLIC;


        /// <summary>
        /// Helper property to return the list of valid values for <see cref="Visibility"/>.
        /// <list type="bullet">
        /// <item>
        /// <description>PROTECTED: May be seen by Club Members.</description>
        /// </item>
        /// <item>
        /// <description>PUBLIC: May be seen by anyone.</description>
        /// </item>
        /// </list>
        /// </summary>
        public static readonly List<VisibilityOption> VisibilityOptions = new List<VisibilityOption>() { VisibilityOption.PROTECTED, VisibilityOption.PUBLIC };
        #endregion

        #region Constructors, Factory Methods, and Initialization
        /// <summary>
        /// Default Constructor.
        /// <para>Initializes a new instance of the <see cref="ClubContact"/> class with default values.</para>
        /// <para>Unless you happen to be a deserializer, it is recommended to use the <see cref="CreateAsync"/> method to create instances.</para>
        /// </summary>
        public ClubContact() {
        }

        /// <summary>
        /// Creates a new instance of <see cref="ClubContact"/> and associates it with the provided <see cref="ClubDetail"/>.
        /// This method ensures that the new ClubContact is properly linked to the club's contact list.
        /// </summary>
        /// <param name="club">The club to associate with the new contact.</param>
        /// <param name="contactType">The type of contact information (e.g., phone number, email, etc.).</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the newly created <see cref="ClubContact"/>.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the provided <paramref name="club"/> is null.</exception>
        /// <exception cref="InvalidOperationException">Thrown when the provided <paramref name="club"/> already has a contact of the same <paramref name="contactType"/>.</exception>
        public static Task<ClubContact> CreateAsync( ClubDetail club, ClubContactType contactType ) {
            // Although this method is currently synchronous in its implementation, it is defined as CreateAsync() to allow for future enhancements
            // that may involve asynchronous operations (e.g., database calls, API requests) during the creation process.
            // By giving this Async name now, we can avoid breaking changes in the future if such enhancements are needed.

            if (club is null)
                throw new ArgumentNullException( nameof( club ), "ClubDetail cannot be null when creating a ClubContact." );

            // Throw an error if the ClubDetail already has a contact of the same type. This prevents duplicate contact types for a single club.
            if (club.ContactList != null && club.ContactList.Any( c => c.ContactType == contactType ))
                throw new InvalidOperationException( $"A contact of type {contactType} already exists for this club." );

            ClubContact clubContact = new ClubContact();
            clubContact.Club = club;
            clubContact.ContactType = contactType;
            clubContact.Club.ContactList ??= new List<ClubContact>();
            clubContact.Club.ContactList.Add( clubContact );

            return Task.FromResult( clubContact );
        }

        /// <summary>
        /// Sets the Club property on the cloned instance to match the source instance. This ensures that when a ClubContact is cloned, it remains associated with the same ClubDetail as the original.
        /// </summary>
        /// <param name="source">The original object that was cloned.</param>
        public void OnCloned( object source ) {

            if (source is ClubContact sourceContact) {
                this.Club = sourceContact.Club;
            }
        }

        #endregion

        #region Data Model Properties

        /// <summary>
        /// The type of contact information (e.g., phone number, email, etc.).
        /// </summary>
        [G_STJ_SER.JsonPropertyOrder( 1 )]
        [G_NS.JsonProperty( Order = 1 )]
        public ClubContactType ContactType { get; set; }

        /// <summary>
        /// The actual contact information (e.g., the phone number or email address).
        /// <para>An empty string value means the contact information is not provided.</para>
        /// </summary>
        /// <remarks>This property is validated based on the <see cref="ContactType"/>, via the IValidatableObject interface in the <see cref="Validate(ValidationContext)"/> method.
        /// For example, if the contact type is EMAIL, the value must be a valid email address.</remarks>
        [G_STJ_SER.JsonPropertyOrder( 2 )]
        [G_NS.JsonProperty( Order = 2 )]
        public string ContactValue {
            get {
                return this._contactValue;
            }
            set {
                if (string.IsNullOrWhiteSpace( value )) {
                    this._contactValue = string.Empty;
                } else {
                    this._contactValue = value.Trim();
                }
            }
        }


        /// <summary>
        /// Gets or sets the visibility level that controls who can view this address.
        /// The set value must be contained in <see cref="VisibilityOptions"/>. If not, the most restrictive option (the first in the list) will be used instead.
        /// </summary>
        [G_STJ_SER.JsonPropertyOrder( 3 )]
        [G_NS.JsonProperty( Order = 3 )]
        public VisibilityOption Visibility {
            get => _visibility;
            set {
                if (!VisibilityOptions.Contains( value ))
                    _visibility = VisibilityOptions[0];

                _visibility = value;
            }
        }

        /// <summary>
        /// Gets or sets the date and time this ClubContACT was last updated.
        /// </summary>
        [G_STJ_SER.JsonPropertyOrder( 13 )]
        [G_NS.JsonProperty( Order = 13 )]
        public DateTime LastUpdated { get; set; }

        #endregion

        #region Helper Properties
        /// <summary>
        /// Backwards reference to the ClubDetail that this contact belongs to. This property is set when the ClubContact is created using the <see cref="CreateAsync(ClubDetail, ClubContactType)"/> method.
        /// </summary>
        [G_NS.JsonIgnore]
        [G_STJ_SER.JsonIgnore]
        public ClubDetail Club { get; set; }

        /// <summary>
        /// Returns sample placeholder text for the contact value based on the contact type.
        /// </summary>
        [G_NS.JsonIgnore]
        [G_STJ_SER.JsonIgnore]
        public string Placeholder {
            get {
                switch (this.ContactType) {
                    case ClubContactType.PHONE_NUMBER:
                        return "+1 (703) 596-0099";
                    case ClubContactType.EMAIL:
                        return "support@scopos.tech";
                    case ClubContactType.WEBSITE:
                        return "https://rezults.scopos.tech/";
                    case ClubContactType.FACEBOOK:
                        return "scoposrezults";
                    case ClubContactType.INSTAGRAM:
                        return "scopos.rezults";
                    case ClubContactType.YOU_TUBE:
                        return "@ScoposRezults";
                    default:
                        // Default placeholder for social media or other contact types
                        return "scopos";
                }
            }
        }

        /// <summary>
        /// The value to display for read only purposes. Will usually be the same as ContactValue, but for website URLs we will remove the "https://" or "http://" prefix to make it cleaner when displayed.
        /// </summary>
        [G_NS.JsonIgnore]
        [G_STJ_SER.JsonIgnore]
        public string DisplayValue {
            get {
                if (this.ContactType == ClubContactType.WEBSITE && !string.IsNullOrWhiteSpace( ContactValue )) {
#if NETSTANDARD2_1
                    // Remove "https://" or "http://" prefix for cleaner display of website URLs
                    return ContactValue.Replace( "https://", "", StringComparison.OrdinalIgnoreCase )
                                       .Replace( "http://", "", StringComparison.OrdinalIgnoreCase );
#else
                    // Remove "https://" or "http://" prefix for cleaner display of website URLs
                    return ContactValue.Replace( "https://", "" )
                                       .Replace( "http://", "" );
#endif
                }

                return ContactValue;
            }
        }

        /// <summary>
        /// Helper property, returns true if the ContactValue is known (i.e., not null or whitespace), false otherwise.
        /// </summary>
        [G_NS.JsonIgnore]
        [G_STJ_SER.JsonIgnore]
        public bool HasValue {
            get {
                return !string.IsNullOrWhiteSpace( ContactValue );
            }
        }

        /// <summary>
        /// Returns true if the ContactType is one that we can represent as a URL link (e.g., WEBSITE, FACEBOOK, INSTAGRAM) and the ContactValue is known, false otherwise.
        /// </summary>
        [G_NS.JsonIgnore]
        [G_STJ_SER.JsonIgnore]
        public bool HasUrlPath {
            get {
                if (string.IsNullOrWhiteSpace( ContactValue )) {
                    return false;
                }

                switch (this.ContactType) {
                    case ClubContactType.EMAIL:
                    case ClubContactType.WEBSITE:
                    case ClubContactType.FACEBOOK:
                    case ClubContactType.INSTAGRAM:
                    case ClubContactType.X:
                    case ClubContactType.TIKTOK:
                    case ClubContactType.YOU_TUBE:
                    case ClubContactType.LINKEDIN:
                    case ClubContactType.SNAPCHAT:
                        return true;
                    default:
                        return false;
                }
            }
        }

        /// <summary>
        /// Returns the url path to use for this contact when the ContactType is one that we can represent as a URL link (e.g., WEBSITE, FACEBOOK, INSTAGRAM).
        /// For example, if the ContactType is FACEBOOK and the ContactValue is "scoposrezults", this property will return "https://www.facebook.com/scoposrezults".
        /// If the ContactType is not a URL type, this property will return empty string.
        /// </summary>
        [G_NS.JsonIgnore]
        [G_STJ_SER.JsonIgnore]
        public string UrlPath {
            get {
                if (!HasUrlPath || string.IsNullOrWhiteSpace( ContactValue )) {
                    return string.Empty;
                }

                switch (this.ContactType) {
                    case ClubContactType.WEBSITE:
                        // For website contact type, we will assume the ContactValue is already a full URL and return it as is.
                        return ContactValue;
                    case ClubContactType.EMAIL:
                        // For email contact type, we will return a "mailto:" link.
                        return $"mailto:{ContactValue}";
                    case ClubContactType.FACEBOOK:
                        return $"https://www.facebook.com/{ContactValue}";
                    case ClubContactType.INSTAGRAM:
                        return $"https://www.instagram.com/{ContactValue}";
                    case ClubContactType.X:
                        return $"https://x.com/{ContactValue}";
                    case ClubContactType.TIKTOK:
                        return $"https://www.tiktok.com/{ContactValue}";
                    case ClubContactType.YOU_TUBE:
                        return $"https://www.youtube.com/{ContactValue}";
                    case ClubContactType.LINKEDIN:
                        return $"https://www.linkedin.com/company/{ContactValue}";
                    case ClubContactType.SNAPCHAT:
                        return $"https://www.snapchat.com/add/{ContactValue}";
                    default:
                        return string.Empty;
                }
            }
        }
        #endregion

        #region Methods

        /// <summary>
        /// Implements the IValidatableObject interface. Validates the contact information based on the specified contact type. 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ValidationResult> Validate( ValidationContext validationContext ) {

            // If the ContactValue is null or whitespace, we consider it valid (as it means no contact information is provided).
            if (string.IsNullOrWhiteSpace( ContactValue )) {
                ;
            } else {

                switch (ContactType) {
                    case ClubContactType.PHONE_NUMBER:
                        // Simple phone number validation (can be improved with regex)
                        if (!System.Text.RegularExpressions.Regex.IsMatch( ContactValue, @"^\+?[0-9\s\-()]+$" )) {
                            yield return new ValidationResult( "Invalid phone number format.", new[] { nameof( ContactValue ) } );
                        }
                        break;
                    case ClubContactType.EMAIL:
                        // Simple email validation (can be improved with regex)
                        if (!System.Text.RegularExpressions.Regex.IsMatch( ContactValue, @"^[^@\s]+@[^@\s]+\.[^@\s]+$" )) {
                            yield return new ValidationResult( "Invalid email format.", new[] { nameof( ContactValue ) } );
                        }
                        break;
                    case ClubContactType.WEBSITE:
                        // For all other contact types, we will assume they are URLs and validate accordingly.
                        // Simple URL validation (can be improved with regex)
                        string contactValueToValidate = ContactValue;

                        // The value must start with "https://" or "http://" but the error message will only mention "https://" to encourage secure URLs. We will allow "http://" for validation purposes.
                        if (!Uri.TryCreate( contactValueToValidate, UriKind.Absolute, out Uri? uriResult )
                            || (uriResult.Scheme != Uri.UriSchemeHttp && uriResult.Scheme != Uri.UriSchemeHttps)
                            || string.IsNullOrWhiteSpace( uriResult.Host )) {
                            yield return new ValidationResult( $"Invalid URL format for {ContactType.Description()}. Value must start with https://", new[] { nameof( ContactValue ) } );
                        }
                        break;
                    case ClubContactType.YOU_TUBE:
                        // For YouTube validation, the channel name must start with an @, e.g., "@ScoposRezults". If it does not, we will return a validation error.
                        if (!ContactValue.StartsWith( "@", StringComparison.Ordinal )) {
                            yield return new ValidationResult( $"Invalid YouTube channel name format. Value must start with @, for example @ScoposRezults.", new[] { nameof( ContactValue ) } );
                        }
                        break;
                    case ClubContactType.TIKTOK:
                        // For TikTok validation, the channel name must start with an @, e.g., "@ScoposRezults". If it does not, we will return a validation error.
                        if (!ContactValue.StartsWith( "@", StringComparison.Ordinal )) {
                            yield return new ValidationResult( $"Invalid TikTok channel name format. Value must start with @, for example @ScoposRezults.", new[] { nameof( ContactValue ) } );
                        }
                        break;
                    default:
                        // For any other contact types (which would be the social media addresses), we will not perform any specific validation.
                        break;
                }
            }
        }
        #endregion
    }
}
