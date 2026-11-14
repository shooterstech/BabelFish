using Scopos.BabelFish.DataModel.OrionMatch;
using Scopos.BabelFish.Runtime.Authentication;

namespace Scopos.BabelFish.Requests.OrionMatchAPI {
    public class CreateMatchChildAuthenticatedRequest : Request {

        public CreateMatchChildAuthenticatedRequest( UserAuthentication credentials, MatchID parentMatchId, string ownerId, string name ) : base( "CreateMatchChild", credentials ) {
            HttpMethod = HttpMethod.Post;
            ParentMatchId = parentMatchId ?? throw new ArgumentNullException( nameof( parentMatchId ) );
            OwnerId = ownerId;
            Name = name;
            SubDomain = APIClients.APISubDomain.AUTHAPI;
        }

        /// <summary>
        /// The Match ID of the parent match where a new child match is being created.
        /// </summary>
        public MatchID ParentMatchId { get; set; }

        /// <summary>
        /// License number of the Orion account that owns the child match.
        /// </summary>
        public string OwnerId { get; set; } = string.Empty;

        /// <summary>
        /// User-facing name of the child match.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <inheritdoc />
        public override string RelativePath {
            get {
                if (ParentMatchId == null) {
                    throw new ArgumentNullException( nameof( ParentMatchId ), "The parent match id must be set to create a child match." );
                }

                return $"/match/{ParentMatchId}/children";
            }
        }

        /// <inheritdoc />
        public override Dictionary<string, List<string>> QueryParameters {
            get {
                if (string.IsNullOrWhiteSpace( OwnerId )) {
                    throw new ArgumentNullException( $"{nameof( OwnerId )} must be set to create a child match." );
                }

                if (string.IsNullOrWhiteSpace( Name )) {
                    throw new ArgumentNullException( $"{nameof( Name )} must be set to create a child match." );
                }

                return new Dictionary<string, List<string>> {
                    { "owner-id", new List<string> { OwnerId } },
                    { "name", new List<string> { Name } }
                };
            }
        }
    }
}
