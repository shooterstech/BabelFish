using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Scopos.BabelFish.Converters.Microsoft;
using Scopos.BabelFish.DataModel.Common;
using Scopos.BabelFish.DataModel.Image;

namespace Scopos.BabelFish.DataModel.Clubs {
    /// <summary>
    /// Complete data about an Orion club account.
    /// </summary>
    public class ClubDetail :
        G_STJ_SER.IJsonOnDeserialized,
        G_STJ_SER.IJsonOnDeserializing {

        #region Private and Protected Fields
        private static Logger _logger = LogManager.GetCurrentClassLogger();
        protected bool _ignoreEvents = false;
        private DateTime _memberSince = DateTime.Today;
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
        [G_NS.JsonIgnore]
        [G_STJ_SER.JsonIgnore]
        public static readonly List<VisibilityOption> VisibilityOptions = new List<VisibilityOption>() { VisibilityOption.PROTECTED, VisibilityOption.PUBLIC };
        #endregion 

        #region Constructors, Factory Methods, and Initialization
        public ClubDetail() {
        }

        /// <summary>
        /// Gets called by System.Text.Json when an instance is deserialized.
        /// </summary>
        public void OnDeserialized() {
            AddressList ??= new List<ClubAddress>();
            AdministratorList ??= new List<Contact>();
            Options ??= new List<ClubOptions>();
            NamespaceList ??= new List<NamespaceDetail>();
            TeamList ??= new List<ClubTeam>();

            // Set the backwards pointer for each Address List
            foreach (var address in AddressList) {
                if (address == null) continue;
                address.Club = this;
            }

            // Set the backwards pointer for each Contact
            foreach (var contact in ContactList) {
                if (contact == null) continue;
                contact.Club = this;
            }

            // Set the backwards pointer for each License
            foreach (var license in LicenseList) {
                if (license == null) continue;
                license.Club = this;
            }

            // Set the backwards pointer for each Club Team
            foreach (var team in TeamList) {
                if (team == null) continue;
                team.Club = this;
            }

            _ignoreEvents = false;
        }

        /// <summary>
        /// Gets called by NewtonSoft.Json's deserializer.
        /// </summary>
        /// <param name="context"></param>
        [OnDeserialized]
        internal void OnDeserialized( StreamingContext context ) => OnDeserialized();

        [OnDeserializing]
        internal void OnDeserializing( StreamingContext context ) => OnDeserializing();

        /// <summary>
        /// Gets called before System.Text.Json deserializes an instance of this class.
        /// Sets _ignoreEvents to true to prevent events from firing during deserialization.
        /// </summary>
        public void OnDeserializing() {
            _ignoreEvents = true;
        }
        #endregion

        #region Data Property Members
        /// <summary>
        /// The orion account number, usually 4 digits.
        /// </summary>
        /// <example>1234</example>
        [DefaultValue( 0 )]
        [Range( 0, 999999, ErrorMessage = "AccountNumber must be between 0 and 999999." )]
        public int AccountNumber { get; set; }

        /// <summary>
        /// The name of the club or individual who own's this Orion license.
        /// </summary>
        /// <example>Northeast High School</example>
        [DefaultValue( "" )]
        [Required]
        [StringLength( 128, ErrorMessage = "Name cannot be longer than 128 characters." )]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Formatted string identifying this is an Orion account and the Account Number.
        /// </summary>
        /// <example>OrionAcct001234</example>
        public string OwnerId {
            get {
                return $"OrionAcct{AccountNumber:D6}";
            }
        }

        /// <summary>
        /// Specifies if this is a INDIVIDUAL, HOME, or SITE license. This is used to determine what features are available to the club.
        /// </summary>
        public ClubLicenseType AccountType { get; set; } = ClubLicenseType.INDIVIDUAL;

        /// <summary>
        /// The list of people who are Administrators for this club.
        /// </summary>
        [DefaultValue( "" )]
        public List<Contact> AdministratorList { get; set; } = new List<Contact>();

        /// <summary>
        /// The email address of the club. May in fact be the email address of the administrator.
        /// </summary>
        [DefaultValue( "" )]
        [Obsolete( "Replaced with ContactList. June 2026." )]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// The phone number of the club. May in fact be the phone number of the club's administrator.
        /// </summary>
        [DefaultValue( "" )]
        [Obsolete( "Replaced with ContactList. June 2026." )]
        public string Phone { get; set; } = string.Empty;

        /// <summary>
        /// Street addresses associated with this club. This may include the club's physical location, mailing address, or the address of the club's administrator.
        /// <para>Unless you are a deserializer, the expected way to add a new ClubAddress is use <see cref="AddClubAddressAsync()"/> or
        /// <see cref="ClubAddress.CreateAsync(ClubDetail)"/></para>
        /// <para>The expected way to remove a ClubAddress is use <see cref="RemoveClubAddress(ClubAddress)"/></para>
        /// </summary>
        public List<ClubAddress> AddressList { get; set; } = new List<ClubAddress>();

        /// <summary>
        /// The list of contact methods for this club. This may include phone numbers, email addresses, and social media links.
        /// <para>Unless you are a deserializer, the expected way to add a new ClubContact is use <see cref="ClubContact.CreateAsync(ClubDetail, ClubContactType)"/></para>  
        /// </summary>
        public List<ClubContact> ContactList { get; set; } = new List<ClubContact>();

        /// <summary>
        /// The list of Club Teams that this Club has defined.
        /// <para>Unless you are a deserializer, the expected way to add a new ClubTeam is use <see cref="AddClubTeamAsync()"/> or
        /// <see cref="ClubTeam.CreateAsync(ClubDetail)"/></para>
        /// <para>The expected way to remove a ClubTeam is use <see cref="RemoveClubTeam(ClubTeam)"/></para>
        /// </summary>
        public List<ClubTeam> TeamList { get; set; } = new List<ClubTeam>();

        /// <summary>
        /// Tries to get a contact from the ContactList by its ClubContactType (e.g., phone number, email, etc.). Returns true if found, false otherwise.
        /// </summary>
        /// <param name="contactType">The type of contact to search for.</param>
        /// <param name="contact">The contact found, or null if not found.</param>
        /// <returns>True if a contact of the specified type is found, false otherwise.</returns>
        public bool TryGetContact( ClubContactType contactType, out ClubContact? contact ) {
            if (this.ContactList is null) {
                contact = null;
                return false;
            }

            contact = this.ContactList.FirstOrDefault( c => c.ContactType == contactType );
            return contact != null;
        }

        /// <summary>
        /// Returns the hometown of the club, which is determined by the first address in the AddressList that has IsRange set to true.
        /// If no such address exists, it will return the first physical address. If neither exists, it will return an empty string.
        /// </summary>
        /// <example>Axtell, NE</example>
        [DefaultValue( "" )]
        public string Hometown {
            get {
                // Find the ClubAddress with IsRange set to true.
                var rangeAddress = AddressList.FirstOrDefault( a => a.IsRange );
                if (rangeAddress != null) {
                    return StringFormatting.Hometown( rangeAddress.City, rangeAddress.State, rangeAddress.CountryCode );
                }

                // If no range address is found, find the first physical address.
                var physicalAddress = AddressList.FirstOrDefault( a => a.IsPhysical );
                if (physicalAddress != null) {
                    return StringFormatting.Hometown( physicalAddress.City, physicalAddress.State, physicalAddress.CountryCode );
                }

                // While it would be unusual, a Club is not required to have any addresses, so if none are found, return an empty string.
                return string.Empty;
            }
        }

        [Obsolete( "Replaced with AddressList. June 2026." )]
        public string Street1 { get; set; }

        [Obsolete( "Replaced with AddressList. June 2026." )]
        public string Street2 { get; set; }

        [Obsolete( "Replaced with AddressList. June 2026." )]
        public string City { get; set; } = string.Empty;

        [Obsolete( "Replaced with AddressList. June 2026." )]
        public string State { get; set; } = string.Empty;

        [Obsolete( "Replaced with AddressList. June 2026." )]
        public string PostalCode { get; set; } = string.Empty;

        [Obsolete( "Replaced with AddressList. June 2026." )]
        public string Country { get; set; } = string.Empty;

        /// <summary>
        /// The date the orion account was created. Formatted as a string yyyy-MM-dd.
        /// To get/set MemberSince date as a DateTime object use GetMemberSince() or SetMemberSince().
        /// </summary>
        /// <example>2001-01-01</example>
        [G_STJ_SER.JsonConverter( typeof( ScoposDateOnlyConverter ) )]
        [G_NS.JsonConverter( typeof( G_BF_NS_CONV.DateConverter ) )]
        public DateTime MemberSince { get; set; }

        /// <summary>
        /// The relative URL path in rezults.scopos.tech/clubs/{path}/ linking to their team page.
        /// <para>The full URL can be obtained from the <see cref="Url"/> property.</para>
        /// <para>Warning, not all Club Pages are publicly accessible.</para>
        /// </summary>
        /// <example>northeast</example>
        [DefaultValue( "" )]
        [Required]
        [StringLength( 45, ErrorMessage = "URLPath cannot be longer than 45 characters." )]
        public string URLPath { get; set; } = string.Empty;

        /// <summary>
        /// The full URL to the club's team page on Rezults. This is constructed from the base URL and the club's URLPath.
        /// <para>Warning, not all Club Pages are publicly accessible.</para>
        /// </summary>
        [G_NS.JsonIgnore]
        [G_STJ_SER.JsonIgnore]
        public string Url {
            get {
                return $"https://rezults.scopos.tech/club/{URLPath}/";
            }
        }


        /// <summary>
        /// Gets or sets the visibility level that controls who can view this address.
        /// The set value must be contained in <see cref="VisibilityOptions"/>. If not, the most restrictive option (the first in the list) will be used instead.
        /// </summary>
        [G_STJ_SER.JsonPropertyOrder( 3 )]
        [G_NS.JsonProperty( Order = 3, DefaultValueHandling = G_NS.DefaultValueHandling.Include )]
        public VisibilityOption Visibility {
            get => _visibility;
            set {
                if (!VisibilityOptions.Contains( value ))
                    _visibility = VisibilityOptions[0];

                _visibility = value;
            }
        }

        /// <summary>
        /// The x-api-key for use by this Club.
        /// <para>As of June 2026 returns a fake placeholder value.</para>
        /// </summary>
        [Obsolete( "No longer used as of Orion version 2.25.7" )]
        public string ApiKey { get { return "1234567890ABCDEFG"; } }

        /// <remarks>
        /// Should not be returned as part of the REST API response, in either public or authenticated calls.
        /// <para>As of June 2026 returns a fake placeholder value.</para>
        /// </remarks>
        [Obsolete( "No longer used as of Orion version 2.25.7 / June 2026" )]
        public string ApiKeyId { get { return "1234567890ABCDEFG"; } }

        /// <remarks>
        /// Should not be returned as part of the REST API response, in either public or authenticated calls.
        /// </remarks>
        public string AWSAccessKeyId { get; set; } = string.Empty;

        /// <remarks>
        /// Should not be returned as part of the REST API response, in either public or authenticated calls.
        /// </remarks>
        public string AWSSecretAccessKey { get; set; } = string.Empty;

        /// <remarks>
        /// Should not be returned as part of the REST API response, in either public or authenticated calls.
        /// </remarks>
        [Obsolete( "No longer used as of June 2026" )]
        public string AWSRegion { get; set; } = string.Empty;

        /// <summary>
        /// A list of notes, written by the Shooter's Tech support team pertaining to this Orion Club.
        /// </summary>
        [Obsolete( "No longer used as of June 2026" )]
        public List<string> Notes { get; set; } = new List<string>();

        /// <summary>
        /// The list of Orion Licenses this Club has. Most Clubs will have exactly one license. Orion at Home accounts can have exactly one.
        /// </summary>
        public List<ClubLicense> LicenseList { get; set; } = new List<ClubLicense>();

        /// <summary>
        /// A list of optional services this club has subscribed to.
        /// </summary>
        [Obsolete( "No longer used as of June 2026" )]
        public List<ClubOptions> Options { get; set; } = new List<ClubOptions> { };

        /// <summary>
        /// If the club is authroized to composed Definition files, this is the list of namespaces that the club has been authorized to use.
        /// If the club is not authorized to compose Definition files, this list will be empty.
        /// </summary>
        public List<NamespaceDetail> NamespaceList { get; set; } = new List<NamespaceDetail> { };

        /// <summary>
        /// The club's header photo. This is a user-friendly image that can be displayed in the UI to represent the club.
        /// </summary>
        public ScoposImageAbbr HeaderPhoto { get; set; } = new ScoposImageAbbr();

        /// <summary>
        /// The club's profile photo. This is a user-friendly image that can be displayed in the UI to represent the club.
        /// </summary>
        public ScoposImageAbbr ProfilePhoto { get; set; } = new ScoposImageAbbr();

        #endregion

        #region Events

        [G_NS.JsonIgnore]
        EventHandler<EventArgs<ClubAddress>> OnClubAddressAdded;

        [G_NS.JsonIgnore]
        EventHandler<EventArgs<ClubAddress>> OnClubAddressRemoved;

        [G_NS.JsonIgnore]
        EventHandler<EventArgs<ClubTeam>> OnClubTeamAdded;

        [G_NS.JsonIgnore]
        EventHandler<EventArgs<ClubTeam>> OnClubTeamRemoved;

        #endregion 

        #region Helper Properties

        /// <summary>
        /// Helper property to determine if this club may add another license. This is true if the club has no licenses or if the club has an Individual license. It is false if the club has a Site or Home license.
        /// </summary>
        [G_NS.JsonIgnore]
        [G_STJ_SER.JsonIgnore]
        public bool MayAddLicense {
            get {
                // Site Licenses and Home Licenses can only have one license. Individual Licenses can have multiple licenses.
                if (this.AccountType == ClubLicenseType.SITE || this.AccountType == ClubLicenseType.HOME) {
                    return LicenseList.Count < 1;
                }

                return true;
            }
        }

        /// <summary>
        /// Helper property to determine the expiration date of this club's list of Orion licenses.
        /// Home licenses do not expire, so this will return DateTime.MaxValue. If the club has no licenses, this will return DateTime.MinValue.
        /// Otherwise, it will return the expiration date of the license that expires furthest in the future.
        /// </summary>
        [G_NS.JsonIgnore]
        [G_STJ_SER.JsonIgnore]
        public DateTime ExpirationDate {
            get {
                // If the AccountType is HOME their is no expiration date, so return DateTime.MaxValue.
                if (this.AccountType == ClubLicenseType.HOME) {
                    return DateTime.MaxValue;
                }

                // If we get here, the AccountType is SITE or INDIVIDUAL

                // If they do not have any licenses, return DateTime.MinValue.
                if (LicenseList.Count == 0) {
                    return DateTime.MinValue;
                }

                // All licenses where Renew is true, should have the same expiration date.
                // This should be enforced when the license is created.

                // The expiration date is the expiration date furthest in the future.
                return LicenseList.Max( l => l.ExpirationDate );
            }
        }

        /// <summary>
        /// The list of roles that may be assigned to members of this Orion Account. The list of roles that is returned
        /// depends on the AccountType. For SITE and INDIVIDUAL accounts, all roles except HOME_USER are available. For HOME accounts,
        /// only the HOME_USER role is available. If the AccountType is not set to a valid value, an empty list is returned.
        /// </summary>
        /// <returns></returns>
        public List<ClubAuthorizationRole> GetApplicableAuthorizationRoles() {
            if (this.AccountType == ClubLicenseType.INDIVIDUAL || this.AccountType == ClubLicenseType.SITE) {
                return new List<ClubAuthorizationRole>() { ClubAuthorizationRole.ADMIN, ClubAuthorizationRole.MANAGER, ClubAuthorizationRole.MEMBER, ClubAuthorizationRole.COACH, ClubAuthorizationRole.TECHNICAL_OFFICER, ClubAuthorizationRole.PAYER };
            } else if (this.AccountType == ClubLicenseType.HOME) {
                return new List<ClubAuthorizationRole>() { ClubAuthorizationRole.HOME_USER };
            } else {
                // Would only get here if the AccountType is the deprecated ClubLicenseType.TEMPORARY or if the AccountType is not set to a valid value. In either case, return an empty list.
                _logger.Warn( $"GetApplicableAuthorizationRoles() called for club {this.OwnerId} with unknown AccountType {this.AccountType}. Returning HOME_USER role." );
                return new List<ClubAuthorizationRole>();
            }
        }

        #endregion

        #region Methods
        /// <summary>
        /// Creates and adds a new <see cref="ClubAddress"/> for the current club.
        /// <para>This method is the same as <see cref="ClubAddress.CreateAsync(ClubDetail)"/>.</para>
        /// </summary>
        /// <returns>The ClubAddress that was created.</returns>
        public async Task<ClubAddress> AddClubAddressAsync() {
            // Although this method is currently synchronous in its implementation, it is defined as CreateAsync() to allow for future enhancements
            // that may involve asynchronous operations (e.g., database calls, API requests) during the creation process.
            // By giving this Async name now, we can avoid breaking changes in the future if such enhancements are needed.

            ClubAddress clubAddress = new ClubAddress();
            clubAddress.Club = this;
            clubAddress.Club.AddressList ??= new List<ClubAddress>();
            clubAddress.Club.AddressList.Add( clubAddress );

            if (!_ignoreEvents) {
                OnClubAddressAdded?.Invoke( this, new EventArgs<ClubAddress>( clubAddress ) );
            }

            return clubAddress;
        }

        /// <summary>
        /// Removes the specified club address from the address list.
        /// </summary>
        /// <remarks>Raises <c>OnClubAddressRemoved</c> with the removed address when removal succeeds and
        /// event dispatching is enabled.</remarks>
        /// <param name="clubAddress">The club address to remove.</param>
        /// <returns><see langword="true"/> if the address was removed; otherwise, <see langword="false"/> when <paramref
        /// name="clubAddress"/> is <see langword="null"/> or not found in the address list.</returns>
        public bool RemoveClubAddress( ClubAddress clubAddress ) {
            if (clubAddress is null) {
                _logger.Warn( "RemoveClubAddress() called with null clubAddress." );
                return false;
            }

            if (this.AddressList is null || !this.AddressList.Contains( clubAddress )) {
                _logger.Warn( $"RemoveClubAddress() called for club {this.OwnerId} with clubAddress {clubAddress}. Address not found in AddressList." );
                return false;
            }

            this.AddressList.Remove( clubAddress );

            if (!_ignoreEvents) {
                OnClubAddressRemoved?.Invoke( this, new EventArgs<ClubAddress>( clubAddress ) );
            }

            return true;
        }

        /// <summary>
        /// Creates and adds a new <see cref="ClubTeam"/> for the current club.
        /// <para>This method is the same as <see cref="ClubTeam.CreateAsync(ClubDetail)"/>.</para>
        /// </summary>
        /// <returns>The ClubTeam that was created.</returns>
        public async Task<ClubTeam> AddClubTeamAsync() {
            // Although this method is currently synchronous in its implementation, it is defined as CreateAsync() to allow for future enhancements
            // that may involve asynchronous operations (e.g., database calls, API requests) during the creation process.
            // By giving this Async name now, we can avoid breaking changes in the future if such enhancements are needed.

            ClubTeam clubTeam = new ClubTeam();
            clubTeam.Club = this;
            clubTeam.Club.TeamList ??= new List<ClubTeam>();
            clubTeam.Club.TeamList.Add( clubTeam );

            if (!_ignoreEvents) {
                OnClubTeamAdded?.Invoke( this, new EventArgs<ClubTeam>( clubTeam ) );
            }

            return clubTeam;
        }

        /// <summary>
        /// Removes the specified club team from the team list.
        /// </summary>
        /// <remarks>Raises <c>OnClubTeamRemoved</c> after a successful removal when event notifications
        /// are enabled.</remarks>
        /// <param name="clubTeam">The club team to remove.</param>
        /// <returns><see langword="true"/> if the team was removed; otherwise, <see langword="false"/> when <paramref
        /// name="clubTeam"/> is <see langword="null"/>, the team list is unavailable, or the team is not in the list.</returns>
        public bool RemoveClubTeam( ClubTeam clubTeam ) {
            if (clubTeam is null) {
                _logger.Warn( "RemoveClubTeam() called with null clubTeam." );
                return false;
            }

            if (this.TeamList is null || !this.TeamList.Contains( clubTeam )) {
                _logger.Warn( $"RemoveClubTeam() called for club {this.OwnerId} with clubTeam {clubTeam}. Team not found in TeamList." );
                return false;
            }

            this.TeamList.Remove( clubTeam );

            if (!_ignoreEvents) {
                OnClubTeamRemoved?.Invoke( this, new EventArgs<ClubTeam>( clubTeam ) );
            }

            return true;
        }

        /// <summary>
        /// Returns true if this club's team page should be visible to the public. This is true if the club has set its
        /// Visibility to PUBLIC and has at least one valid license.
        /// </summary>
        /// <returns></returns>
        public bool IsPublicUrlPageVisible() {
            // EKA NOTE: As of October 2026, no longer considering if this is a home account. Both club and home accounts can have public pages if they have a valid license and have set their visibility to PUBLIC.
            return Visibility == VisibilityOption.PUBLIC
                && LicenseList.Any( l => l.ExpirationDate >= DateTime.Today );
        }

        /// <summary>
        /// Returns true if Club Members, Admins, and Managers should be able to see the Club page page even if the club has not set its Visibility to PUBLIC.
        /// This is true if the club has at least one valid license.
        /// </summary>
        /// <returns></returns>
        public bool IsProtectedUrlPageVisible() {
            // EKA NOTE: As of October 2026, no longer considering if this is a home account. Both club and home accounts can have protected pages if they have a valid license.
            return LicenseList.Any( l => l.ExpirationDate >= DateTime.Today );
        }

        /// <inheritdoc />
        public override string ToString() {
            return $"{Name} {OwnerId}";
        }
        #endregion
    }
}
