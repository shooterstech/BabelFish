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
        /// The list of people who are Administrators for this club.
        /// </summary>
        [DefaultValue( "" )]
        public List<Contact> AdministratorList { get; set; } = new List<Contact>();

        /// <summary>
        /// The email address of the club. May in fact be the email address of the administrator.
        /// </summary>
        [DefaultValue( "" )]
        [Obsolete( "Soon to be replaced with v1.0:orion:Email Address" )]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// The phone number of the club. May in fact be the phone number of the club's administrator.
        /// </summary>
        [DefaultValue( "" )]
        [Obsolete( "Soon to be replaced with v1.0:orion:Phone Number" )]
        public string Phone { get; set; } = string.Empty;

        /// <summary>
        /// Street addresses associated with this club. This may include the club's physical location, mailing address, or the address of the club's administrator.
        /// <para>Unless you are a deserializer, the expected way to add a new ClubAddress is use <see cref="ClubAddress.CreateAsync(ClubDetail)"/></para>
        /// </summary>
        public List<ClubAddress> AddressList { get; set; } = new List<ClubAddress>();

        /// <summary>
        /// The city and state (and maybe country) where the club is from.
        /// </summary>
        /// <example>Axtell, NE</example>
        [DefaultValue( "" )]
        public string Hometown { get; set; } = string.Empty;

        [Obsolete( "Soon to be replaced with v1.0:orion:Address" )]
        public string Street1 { get; set; }

        [Obsolete( "Soon to be replaced with v1.0:orion:Address" )]
        public string Street2 { get; set; }

        [Obsolete( "Soon to be replaced with v1.0:orion:Address" )]
        public string City { get; set; } = string.Empty;

        [Obsolete( "Soon to be replaced with v1.0:orion:Address" )]
        public string State { get; set; } = string.Empty;

        [Obsolete( "Soon to be replaced with v1.0:orion:Address" )]
        public string PostalCode { get; set; } = string.Empty;

        [Obsolete( "Soon to be replaced with v1.0:orion:Address" )]
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
        /// Gets or sets the visibility of this club's team page on rezults.scopos.net. It is the responsibility of a
        /// Club's administrators or manager to set this value appropriately.
        /// <para>If PRIVATE, the team page will not be visible to the public.</para>
        /// <para>If PUBLIC, the team page will likely be visible to the public. In order to be visility, the Club must
        /// have a valid Orion for Clubs license. Check <see cref="IsPublicUrlPageVisible"/> to learn if the Club
        /// passes these tests.</para>
        /// </summary>
        [G_NS.JsonProperty( DefaultValueHandling = G_NS.DefaultValueHandling.Include )]
        public VisibilityOption Visibility { get; set; } = VisibilityOption.PRIVATE;
        /// <summary>
        /// The x-api-key for use by this Club.
        /// </summary>
        public string ApiKey { get; set; } = string.Empty;

        /// <remarks>
        /// Should not be returned as part of the REST API response, in either public or authenticated calls.
        /// </remarks>
        public string ApiKeyId { get; set; } = string.Empty;

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
        /// <summary>
        /// Helper property to return the list of valid VisibilityOption values. This is not returned as part of the REST API response, but is provided for ease of use in client applications.
        /// </summary>
        [G_NS.JsonIgnore]
        [G_STJ_SER.JsonIgnore]
        public static List<VisibilityOption> VisibilityOptions { get; private set; } = new List<VisibilityOption>() { VisibilityOption.PRIVATE, VisibilityOption.PUBLIC };
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

        /// <inheritdoc />
        public override string ToString() {
            return $"{Name} {OwnerId}";
        }
        #endregion
    }
}
