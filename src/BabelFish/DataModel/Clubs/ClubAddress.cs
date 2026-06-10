using Scopos.BabelFish.DataModel.Common;
using Scopos.BabelFish.DataModel.OrionMatch;

namespace Scopos.BabelFish.DataModel.Clubs {

    /// <summary>
    /// Represents a street address assigned to an Orion Account (also known as a Club).
    /// </summary>
    public class ClubAddress : ICheckSum {

        #region Private and Public Fields
        private string _countryCode = "USA";
        private VisibilityOption _visibility = VisibilityOption.PUBLIC;
        private bool _isMailing = false;
        private bool _isPhysical;
        #endregion

        #region Constructors, Factory Methods, and Initialization
        /// <summary>
        /// Default Constructor.
        /// <para>Initializes a new instance of the <see cref="ClubAddress"/> class with default values.</para>
        /// <para>Unless you happen to be a deserializer, it is recommended to use the <see cref="CreateAsync"/> method to create instances.</para>
        /// </summary>
        public ClubAddress() {
        }

        /// <summary>
        /// Creates a new instance of <see cref="ClubAddress"/> and associates it with the provided <see cref="ClubDetail"/>.
        /// This method ensures that the new address is properly linked to the club's address list.
        /// </summary>
        /// <param name="club">The club to associate with the new address.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the newly created <see cref="ClubAddress"/>.</returns>
        public static async Task<ClubAddress> CreateAsync( ClubDetail club ) {
            // Although this method is currently synchronous in its implementation, it is defined as async to allow for future enhancements
            // that may involve asynchronous operations (e.g., database calls, API requests) during the creation process.
            // By marking it Async now, we can avoid breaking changes in the future if such enhancements are needed.

            ClubAddress clubAddress = new ClubAddress();
            clubAddress.Club = club;
            clubAddress.Club.AddressList.Add( clubAddress );

            return clubAddress;
        }
        #endregion

        #region Data Model Properties

        /// <summary>
        /// Gets or sets the unique identifier for this address record. A value of 0 means this address has not been saved
        /// to the database and does not have an assigned ID. Once saved, this value will be a positive integer that uniquely
        /// identifies this address within the system.
        /// </summary>
        [G_NS.JsonProperty( Order = 1 )]
        public int AddressId { get; set; } = 0;

        /// <summary>
        /// Gets or sets the recipient name associated with this address.
        /// </summary>
        [G_NS.JsonProperty( Order = 3 )]
        [Required]
        public string RecipientName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the first street address line.
        /// </summary>
        [G_NS.JsonProperty( Order = 4 )]
        [Required]
        public string Street1 { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the optional second street address line (for suite, unit, apartment, etc.).
        /// </summary>
        [G_NS.JsonProperty( Order = 5 )]
        public string? Street2 { get; set; }

        /// <summary>
        /// Gets or sets the city for this address.
        /// </summary>
        [G_NS.JsonProperty( Order = 6 )]
        [Required]
        public string City { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the state, province, or region for this address.
        /// </summary>
        [G_NS.JsonProperty( Order = 7 )]
        [Required]
        public string State { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the postal or ZIP code for this address.
        /// </summary>
        [G_NS.JsonProperty( Order = 8 )]
        [Required]
        public string PostalCode { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the 3-character country code, or an empty string.
        /// </summary>
        /// <exception cref="ArgumentException">
        /// Thrown when the value is not empty and not exactly 3 characters long.
        /// </exception>
        [G_NS.JsonProperty( Order = 9 )]
        [Required]
        public string CountryCode {
            get => _countryCode;
            set {
                var code = value ?? string.Empty;

                if (code.Length != 0 && code.Length != 3)
                    throw new ArgumentException( "CountryCode must be empty or exactly 3 characters.", nameof( value ) );

                _countryCode = code;
            }
        }

        /// <summary>
        /// Gets or sets the visibility level that controls who can view this address.
        /// If <see cref="VisibilityOptions"/> is null or empty, any defined <see cref="VisibilityOption"/> value is allowed.
        /// Otherwise, the value must be contained in <see cref="VisibilityOptions"/>.
        /// </summary>
        [G_NS.JsonProperty( Order = 10 )]
        public VisibilityOption Visibility {
            get => _visibility;
            set {
                if (VisibilityOptions != null && VisibilityOptions.Count > 0 && !VisibilityOptions.Contains( value ))
                    throw new ArgumentOutOfRangeException( nameof( value ), "Visibility must be one of the allowed VisibilityOptions values." );

                _visibility = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this address is used for mailing.
        /// <para>When set to <c>true</c>, all other addresses in <see cref="ClubDetail.AddressList"/> are set to <c>false</c>. Only one
        /// ClubAddress can be the mailing address at a time.</para>
        /// <para>A ClubAddress may be both the mailing and physical address simultaneously.</para>
        /// </summary>
        [G_NS.JsonProperty( Order = 11 )]
        public bool IsMailing {
            get => _isMailing;
            set {
                if (_isMailing == value)
                    return;

                _isMailing = value;

                if (value && Club?.AddressList != null) {
                    foreach (var address in Club.AddressList) {
                        if (address == null || ReferenceEquals( address, this ))
                            continue;

                        // Direct field assignment avoids unnecessary re-entry in setter logic
                        address._isMailing = false;
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this address is the physical location.
        /// <para>When set to <c>true</c>, all other addresses in <see cref="ClubDetail.AddressList"/> are set to <c>false</c>. Only one
        /// ClubAddress can be the physical address at a time.</para>
        /// <para>A ClubAddress may be both the mailing and physical address simultaneously.</para>
        /// </summary>
        [G_NS.JsonProperty( Order = 12 )]
        public bool IsPhysical {
            get => _isPhysical;
            set {
                if (_isPhysical == value)
                    return;

                _isPhysical = value;

                if (value && Club?.AddressList != null) {
                    foreach (var address in Club.AddressList) {
                        if (address == null || ReferenceEquals( address, this ))
                            continue;

                        // Direct field assignment avoids setter re-entry
                        address._isPhysical = false;
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets the date and time this address was last updated.
        /// </summary>
        [G_NS.JsonProperty( Order = 13 )]
        public DateTime LastUpdated { get; set; }

        #endregion

        #region Helper Properties
        /// <summary>
        /// Backwards pointer to the owning ClubDetail. This is not serialized in the API response, but is provided for ease of use in client applications.
        /// </summary>
        [G_NS.JsonIgnore]
        [G_STJ_SER.JsonIgnore]
        public ClubDetail? Club { get; set; }

        /// <summary>
        /// Helper property to return the list of valid VisibilityOption values. This is not returned as part of the REST API response, but is provided for ease of use in client applications.
        /// </summary>
        [G_NS.JsonIgnore]
        [G_STJ_SER.JsonIgnore]
        public static List<VisibilityOption> VisibilityOptions { get; private set; } = new List<VisibilityOption>() { VisibilityOption.PRIVATE, VisibilityOption.PUBLIC };

        /// <inheritdoc />
        /// <remarks>Choosing not to include CheckSum in the serialized value, as it is not a top level document.</remarks>
        [G_NS.JsonIgnore]
        [G_STJ_SER.JsonIgnore]
        public string CheckSum { get; set; }
        #endregion

        #region Methods
        /// <summary>
        /// Returns a string that represents the current Club ADdress. This is useful for debugging and logging purposes.
        /// </summary>
        public override string ToString() {
            return $"Club Address {Street1} for {Club?.Name ?? "Unknown Club"} (ID: {AddressId})";
        }

        /// <inheritdoc />
        public ulong CalculateChecksum() {
            // Combine relevant properties into a single string for hashing
            string combined = $"{Club?.AccountNumber}|{RecipientName}|{Street1}|{Street2}|{City}|{State}|{PostalCode}|{CountryCode}|{Visibility}|{IsMailing}|{IsPhysical}";

            var hash = Helpers.Common.Md5ToUlong( combined );
            return hash;
        }

        #endregion
    }
}
