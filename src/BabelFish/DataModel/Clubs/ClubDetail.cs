using System.ComponentModel;
using System.Text.Json.Serialization;
using Scopos.BabelFish.Converters.Microsoft;
using Scopos.BabelFish.DataModel.Common;

namespace Scopos.BabelFish.DataModel.Clubs {
    /// <summary>
    /// Complete data about an Orion club account.
    /// </summary>
    public class ClubDetail : IJsonOnDeserialized {

        #region Private and Protected Fields
        private static Logger _logger = LogManager.GetCurrentClassLogger();
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

        public void OnDeserialized() {
            AddressList ??= new List<ClubAddress>();
            AdministratorList ??= new List<Contact>();
            Options ??= new List<ClubOptions>();
            NamespaceList ??= new List<NamespaceDetail>();

            // Set the backwards pointer for each Address List
            foreach (var address in AddressList) {
                if (address == null) continue;
                address.Club = this;
            }
        }
        #endregion

        #region Data Property Members
        /// <summary>
        /// The orion account number, usually 4 digits.
        /// </summary>
        /// <example>1234</example>
        [DefaultValue( 0 )]
        public int AccountNumber { get; set; }

        /// <summary>
        /// The name of the club or individual who own's this Orion license.
        /// </summary>
        /// <example>Northeast High School</example>
        [DefaultValue( "" )]
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
        public ClubLicenseType LicenseType { get; set; } = ClubLicenseType.INDIVIDUAL;

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
        /// <para>Unless you are a deserializer, the expected way to add a new ClubAddress is use <see cref="ClubAddress.CreateAsync(ClubDetail)"/></para>
        /// </summary>
        public List<ClubAddress> AddressList { get; set; } = new List<ClubAddress>();

        /// <summary>
        /// The list of contact methods for this club. This may include phone numbers, email addresses, and social media links.
        /// <para>Unless you are a deserializer, the expected way to add a new ClubContact is use <see cref="ClubContact.CreateAsync(ClubDetail, ClubContactType)"/></para>  
        /// </summary>
        public List<ClubContact> ContactList { get; set; } = new List<ClubContact>();

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
        /// The URL path in www.Scopos.net/clubs/{path} linking to their team page.
        /// </summary>
        /// <example>northeast</example>
        [DefaultValue( "" )]
        public string URLPath { get; set; } = string.Empty;


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
        [Obsolete( "No longer used as of Orion version 2.25.7" )]
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
        public string AWSRegion { get; set; } = string.Empty;

        /// <summary>
        /// A list of notes, written by the Shooter's Tech support team pertaining to this Orion Club.
        /// </summary>
        public List<string> Notes { get; set; } = new List<string>();

        /// <summary>
        /// The list of Orion Licenses this Club has. Most Clubs will have exactly one license. 
        /// </summary>
        public List<ClubLicense> LicenseList { get; set; } = new List<ClubLicense>();

        /// <summary>
        /// A list of optional services this club has subscribed to.
        /// </summary>
        public List<ClubOptions> Options { get; set; } = new List<ClubOptions> { };

        public List<NamespaceDetail> NamespaceList { get; set; } = new List<NamespaceDetail> { };

        #endregion

        #region Helper Properties

        #endregion

        #region Methods
        /// <summary>
        /// Returns true if this club's team page should be visible to the public. This is true if the club has set its
        /// Visibility to PUBLIC and has at least one valid Orion for Clubs license.
        /// </summary>
        /// <returns></returns>
        public bool IsPublicUrlPageVisible() {
            return Visibility == VisibilityOption.PUBLIC && LicenseList.Any( l => (l.LicenseType == ClubLicenseType.INDIVIDUAL || l.LicenseType == ClubLicenseType.SITE) && l.ExpirationDate >= DateTime.Today );
        }

        /// <summary>
        /// Returns true if Club Members, Admins, and Managers should be able to see the Club page page even if the club has not set its Visibility to PUBLIC.
        /// This is true if the club has at least one valid Orion for Clubs license.
        /// <para>Would be false if all of their licenses have expired, or this is a Orion at Home account.</para>
        /// </summary>
        /// <returns></returns>
        public bool IsProtectedUrlPageVisible() {
            return LicenseList.Any( l => (l.LicenseType == ClubLicenseType.INDIVIDUAL || l.LicenseType == ClubLicenseType.SITE) && l.ExpirationDate >= DateTime.Today );
        }

        /// <inheritdoc />
        public override string ToString() {
            return $"{Name} {OwnerId}";
        }
        #endregion
    }
}
