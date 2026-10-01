using Scopos.BabelFish.APIClients;
using Scopos.BabelFish.DataModel.Clubs;
using Scopos.BabelFish.DataModel.OrionMatch;
using Scopos.BabelFish.DataModel.ScoposData;

namespace Scopos.BabelFish.Requests.ImageAPI {
    public class GetImagesRequest : Request, ITokenRequest {

        public GetImagesRequest() : base( "GetImages" ) {
            this.RequiresCredentials = false;
            this.SubDomain = APISubDomain.API;
        }

        public GetImagesRequest( ClubDetail club ) : this() {
            this.ImageCategory = ImageCategory.CLUB;
            this.PrimaryKey = club.AccountNumber.ToString();
        }

        public GetImagesRequest( ClubAbbr club ) : this() {
            this.ImageCategory = ImageCategory.CLUB;
            this.PrimaryKey = club.AccountNumber.ToString();
        }

        public GetImagesRequest( MatchID matchId ) : this() {
            this.ImageCategory = ImageCategory.MATCH;
            this.PrimaryKey = matchId.ToString();
        }

        public GetImagesRequest( int clubAccountNumber ) : this() {
            this.ImageCategory = ImageCategory.CLUB;
            this.PrimaryKey = clubAccountNumber.ToString();
        }

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
        /// <item>League: The key will be the league ID (which is a <see cref="MatchID"/>) in string form.</item>
        /// <item>User: The key will be the UUID formatted user ID of the user who owns it.</item>
        /// </list>
        /// </summary>
        public string PrimaryKey { get; set; } = "";

        /// <summary>
        /// Value is dependent on the value of <see cref="ImageCategory"/> and <see cref="GroupKey"/>.
        /// <list type="bullet">
        /// <item>Club: Not used.</item>
        /// <item>Match: Not used.</item>
        /// <item>League:
        ///   <list type="bullet">
        ///   <item>Header: The SubKey will be the team id of the team the photo represents, in string form.</item>
        ///   <item>Profile: The SubKey will be the team id of the team the photo represents, in string form.</item>
        ///   <item>Bulk: The SubKey will be the parent id (which is a <see cref="MatchID"/>) of the game of which the photo was taken, in string form.</item>
        ///   </list>
        /// </item>
        /// <item>User: Not used.</item>
        /// </list>
        /// </summary>
        public string SubKey { get; set; } = "";

        /// <inheritdoc />
        public string Token { get; set; }

        /// <inheritdoc />
        public int Limit { get; set; } = 0;

        /// <inheritdoc />
        public override Request Copy() {
            var newRequest = new GetImagesRequest();
            newRequest.Token = this.Token;

            return newRequest;
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
