using Scopos.BabelFish.APIClients;
using Scopos.BabelFish.DataModel.Clubs;
using Scopos.BabelFish.DataModel.OrionMatch;
using Scopos.BabelFish.DataModel.ScoposData;

namespace Scopos.BabelFish.Requests.ImageAPI {
    /// <summary>
    /// Defines the concrete <see cref="Request"/> class for the GetImages API call. This request is used to retrieve images from the Scopos Image API based on the specified image category and primary key.
    /// </summary>
    /// <remarks>Additional documentation for GetImagesRequest can be found at https://docs.google.com/document/d/17M5888Px6ztdQGH6M-5e5W_DfeVK6urVxRG1XVlQ7Oo/edit?usp=sharing</remarks>
    public class GetImagesRequest : Request, ITokenRequest {

        /// <summary>
        /// Initializes a new instance of the <see cref="GetImagesRequest"/> class with default values.
        /// <para>The request is set to not require credentials and is associated with the API subdomain.</para>
        /// </summary>
        public GetImagesRequest() : base( "GetImages" ) {
            this.RequiresCredentials = false;
            this.SubDomain = APISubDomain.API;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GetImagesRequest"/> class for a specific Orion Account (aka Club).
        /// </summary>
        /// <param name="club">The club detail object representing the Orion Account.</param>
        public GetImagesRequest( ClubDetail club ) : this() {
            this.ImageCategory = ImageCategory.CLUB;
            this.PrimaryKey = club.AccountNumber.ToString();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GetImagesRequest"/> class for a specific Orion Account (aka Club).
        /// </summary>
        /// <param name="club">The club abbreviation object representing the Orion Account.</param>
        public GetImagesRequest( ClubAbbr club ) : this() {
            this.ImageCategory = ImageCategory.CLUB;
            this.PrimaryKey = club.AccountNumber.ToString();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GetImagesRequest"/> class for a specific Match.
        /// </summary>
        /// <param name="matchId">The match ID representing the Match.</param>
        public GetImagesRequest( MatchID matchId ) : this() {
            this.ImageCategory = ImageCategory.MATCH;
            this.PrimaryKey = matchId.ToString();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GetImagesRequest"/> class for a specific Orion Account (aka Club) using the club account number.
        /// </summary>
        /// <param name="clubAccountNumber">The account number of the Orion Account (aka Club).</param>
        public GetImagesRequest( int clubAccountNumber ) : this() {
            this.ImageCategory = ImageCategory.CLUB;
            this.PrimaryKey = clubAccountNumber.ToString();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GetImagesRequest"/> class for a specific User.
        /// </summary>
        /// <param name="userId">The UUID formatted user ID of the User.</param>
        public GetImagesRequest( string userId ) : this() {
            this.ImageCategory = ImageCategory.USER;
            this.PrimaryKey = userId;
        }

        /// <summary>
        /// Gets or sets the category of the image.
        /// </summary>
        public ImageCategory ImageCategory { get; set; } = ImageCategory.CLUB;

        /// <summary>
        /// Value is dependent on the value of <see cref="ImageCategory"/>.
        /// <list type="bullet">
        /// <item>Club: The key will be the Orion Account (the owner) license number in string form.</item>
        /// <item>Match: The key will be the parent ID (which is a <see cref="MatchID"/>) in string form.</item>
        /// <item>League Team: The key will be the league ID (which is a <see cref="MatchID"/>) in string form.</item>
        /// <item>User: The key will be the UUID formatted user ID of the user who owns it.</item>
        /// </list>
        /// </summary>
        public string PrimaryKey { get; set; } = "";

        /// <summary>
        /// Value is dependent on the value of <see cref="ImageCategory"/> and <see cref="GroupKey"/>.
        /// <list type="bullet">
        /// <item>Club: Not used, should pass in an empty string.</item>
        /// <item>Match: Not used, should pass in an empty string.</item>
        /// <item>League Team:
        ///   <list type="bullet">
        ///   <item>Header: The SubKey will be the team id of the team the photo represents, in string form.</item>
        ///   <item>Profile: The SubKey will be the team id of the team the photo represents, in string form.</item>
        ///   <item>Bulk: The SubKey will be the parent id (which is a <see cref="MatchID"/>) of the game of which the photo was taken, in string form.</item>
        ///   </list>
        /// </item>
        /// <item>User: Not used, should pass in an empty string.</item>
        /// </list>
        /// </summary>
        public string SubKey { get; set; } = "";

        /// <inheritdoc />
        public string Token { get; set; }

        /// <inheritdoc />
        public int Limit { get; set; } = 0;

        /// <inheritdoc />
        public override Request Copy() {
            return new GetImagesRequest {
                ImageCategory = this.ImageCategory,
                PrimaryKey = this.PrimaryKey,
                SubKey = this.SubKey,
                Token = this.Token,
                Limit = this.Limit
            };
        }

        /// <inheritdoc />
        public override string RelativePath {
            get { return "/images"; }
        }

        /// <inheritdoc />
        public override Dictionary<string, List<string>> QueryParameters {
            get {

                Dictionary<string, List<string>> parameterList = new Dictionary<string, List<string>>();

                if (!string.IsNullOrEmpty( Token ))
                    parameterList.Add( "token", new List<string> { Token } );

                if (Limit > 0)
                    parameterList.Add( "limit", new List<string> { Limit.ToString() } );

                parameterList.Add( "image-category", new List<string>() { ImageCategory.Description() } );
                parameterList.Add( "primary-key", new List<string>() { PrimaryKey } );

                if (!string.IsNullOrEmpty( SubKey ))
                    parameterList.Add( "sub-key", new List<string>() { SubKey } );

                return parameterList;
            }
        }
    }
}
