using System.Text;
using Scopos.BabelFish.DataModel.OrionMatch;
using Scopos.BabelFish.Runtime.Authentication;

namespace Scopos.BabelFish.Requests.OrionMatchAPI {
    public class PostMatchParentAuthenticatedRequest : Request {

        public PostMatchParentAuthenticatedRequest( UserAuthentication credentials ) : base( "PostMatchParent", credentials ) {
            HttpMethod = HttpMethod.Post;
            SubDomain = APIClients.APISubDomain.AUTHAPI;
        }

        public PostMatchParentAuthenticatedRequest( UserAuthentication credentials, Match match ) : this( credentials ) {
            Match = match ?? throw new ArgumentNullException( nameof( match ) );
        }

        /// <summary>
        /// Parent match object to upload.
        /// </summary>
        public Match Match { get; set; } = new Match();

        /// <inheritdoc />
        public override string RelativePath {
            get { return "/match"; }
        }

        /// <inheritdoc />
        public override StringContent PostParameters {
            get {
                if (Match == null) {
                    throw new ArgumentNullException( nameof( Match ), "The match must be set to post a parent match." );
                }

                if (Match.MatchID == null || Match.MatchID.IsDefault) {
                    throw new ArgumentNullException( nameof( Match.MatchID ), "The match id must be set to post a parent match." );
                }

                if (!Match.MatchID.VirtualMatchParent) {
                    throw new ArgumentException( "PostMatchParent can only upload virtual parent matches.", nameof( Match.MatchID ) );
                }

                if (string.IsNullOrWhiteSpace( Match.OwnerId )) {
                    throw new ArgumentNullException( nameof( Match.OwnerId ), "The match owner id must be set to post a parent match." );
                }

                Match.CheckSum = Match.CalculateChecksum().ToString();
                var jsonAsString = G_NS.JsonConvert.SerializeObject( Match, SerializerOptions.NewtonsoftJsonSerializer );
                return new StringContent( jsonAsString, Encoding.UTF8, "application/json" );
            }
        }
    }
}
