using Scopos.BabelFish.DataModel.Common;

namespace Scopos.BabelFish.DataModel.Clubs {
    /// <summary>
    /// Represents a contact method for a Club (aka Orion Account). Such as their phone number, email address, or social media link. 
    /// </summary>
    public class ClubContact : IOnCloned {

        #region Private and Protected Fields

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
        /// <returns>A task that represents the asynchronous operation. The task result contains the newly created <see cref="ClubContact"/>.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the provided <paramref name="club"/> is null.</exception>
        public static Task<ClubContact> CreateAsync( ClubDetail club ) {
            // Although this method is currently synchronous in its implementation, it is defined as CreateAsync() to allow for future enhancements
            // that may involve asynchronous operations (e.g., database calls, API requests) during the creation process.
            // By giving this Async name now, we can avoid breaking changes in the future if such enhancements are needed.

            if (club is null)
                throw new ArgumentNullException( nameof( club ), "ClubDetail cannot be null when creating a ClubContact." );

            ClubContact clubContact = new ClubContact();
            clubContact.Club = club;
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
        public ClubContactType ContactType { get; set; }

        /// <summary>
        /// The actual contact information (e.g., the phone number or email address).
        /// <para>An empty string value means the contact information is not provided.</para>
        /// </summary>
        public string ContactValue { get; set; } = string.Empty;

        /// <summary>
        /// Indicates whether the contact information is visible to the public or private to the club. The default value is PRIVATE.
        /// </summary>
        public VisibilityOption Visibility { get; set; } = VisibilityOption.PRIVATE;

        #endregion

        #region Helper Properties
        /// <summary>
        /// Backwards reference to the ClubDetail that this contact belongs to. This property is set when the ClubContact is created using the <see cref="CreateAsync(ClubDetail)"/> method.
        /// </summary>
        [G_NS.JsonIgnore]
        [G_STJ_SER.JsonIgnore]
        public ClubDetail Club { get; set; }
        #endregion

        #region Methods

        /// <summary>
        /// Helper property to return the list of valid VisibilityOption values. This is not returned as part of the REST API response, but is provided for ease of use in client applications.
        /// </summary>
        [G_NS.JsonIgnore]
        [G_STJ_SER.JsonIgnore]
        public static List<VisibilityOption> VisibilityOptions { get; private set; } = new List<VisibilityOption>() { VisibilityOption.PRIVATE, VisibilityOption.PUBLIC };

        /// <summary>
        /// Validates the contact information based on the specified contact type. Returns a <see cref="ClubContactValidation"/> object indicating whether the contact information is valid and providing a message if it is not.
        /// </summary>
        /// <returns></returns>
        public ClubContactValidation Validate() {
            ClubContactValidation validation = new ClubContactValidation();

            //An empty string is considered valid, as it indicates that the contact information is not provided. Validation is only performed when a value is present.
            if (string.IsNullOrWhiteSpace( ContactValue )) {
                validation.IsValid = true;
                validation.Message = string.Empty;
                return validation;
            }

            switch (ContactType) {
                case ClubContactType.PHONE_NUMBER:
                    // Simple phone number validation (can be improved with regex)
                    if (!System.Text.RegularExpressions.Regex.IsMatch( ContactValue, @"^\+?[0-9\s\-()]+$" )) {
                        validation.IsValid = false;
                        validation.Message = "Invalid phone number format.";
                        return validation;
                    }
                    break;
                case ClubContactType.EMAIL:
                    // Simple email validation (can be improved with regex)
                    if (!System.Text.RegularExpressions.Regex.IsMatch( ContactValue, @"^[^@\s]+@[^@\s]+\.[^@\s]+$" )) {
                        validation.IsValid = false;
                        validation.Message = "Invalid email address format.";
                        return validation;
                    }
                    break;
                default:
                    // For all other contact types, we will assume they are URLs and validate accordingly.
                    // Simple URL validation (can be improved with regex)
                    if (!Uri.TryCreate( ContactValue, UriKind.Absolute, out Uri? uriResult ) ||
                        (uriResult.Scheme != Uri.UriSchemeHttp && uriResult.Scheme != Uri.UriSchemeHttps)) {
                        validation.IsValid = false;
                        validation.Message = $"Invalid URL format for {ContactType.Description()}.";
                        return validation;
                    }
                    break;
            }
            // If all checks pass
            validation.IsValid = true;
            return validation;
        }
        #endregion
    }

    public class ClubContactValidation {
        /// <summary>
        /// Indicates whether the contact information is valid according to the validation rules for the specified contact type.
        /// </summary>
        public bool IsValid { get; set; }

        /// <summary>
        /// Provides a message describing the validation result. If the contact information is invalid, this message will contain details about why it is invalid.
        /// Value is an empty string if the contact information is valid.
        /// </summary>
        public string Message { get; set; } = string.Empty;
    }
}
