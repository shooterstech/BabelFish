using System.Text;
using Scopos.BabelFish.DataModel.OrionMatch;
using Scopos.BabelFish.Runtime.Authentication;

namespace Scopos.BabelFish.Requests.OrionMatchAPI {
    public class PatchMatchChildAuthenticatedRequest : Request {

        public PatchMatchChildAuthenticatedRequest( UserAuthentication credentials ) : base( "PatchMatchChild", credentials ) {
            HttpMethod = new HttpMethod( "PATCH" );
            SubDomain = APIClients.APISubDomain.AUTHAPI;
        }

        public PatchMatchChildAuthenticatedRequest( UserAuthentication credentials, MatchChild matchChild ) : this( credentials ) {
            MatchChild = matchChild ?? throw new ArgumentNullException( nameof( matchChild ) );
        }

        /// <summary>
        /// The Match ID of the parent match containing the child match, inferred from <see cref="MatchChild"/>.
        /// </summary>
        public MatchID ParentMatchId {
            get {
                return MatchChild?.ParentID ?? MatchID.DEFAULT;
            }
        }

        /// <summary>
        /// The Match ID of the child match being updated, inferred from <see cref="MatchChild"/>.
        /// </summary>
        public MatchID ChildMatchId {
            get {
                return MatchChild?.MatchID ?? MatchID.DEFAULT;
            }
        }

        /// <summary>
        /// The updated child match object to submit as the request body.
        /// </summary>
        public MatchChild MatchChild { get; set; } = new MatchChild();

        /// <inheritdoc />
        public override string RelativePath {
            get {
                if (MatchChild == null) {
                    throw new ArgumentNullException( nameof( MatchChild ), "The match child must be set to patch a child match." );
                }

                if (MatchChild.ParentID == null || MatchChild.ParentID.IsDefault) {
                    throw new ArgumentNullException( nameof( MatchChild.ParentID ), "The match child parent id must be set to patch a child match." );
                }

                if (MatchChild.MatchID == null || MatchChild.MatchID.IsDefault) {
                    throw new ArgumentNullException( nameof( MatchChild.MatchID ), "The match child match id must be set to patch a child match." );
                }

                return $"/match/{ParentMatchId}/children/{ChildMatchId}";
            }
        }

        /// <inheritdoc />
        public override StringContent PostParameters {
            get {
                if (MatchChild == null) {
                    throw new ArgumentNullException( nameof( MatchChild ), "The match child must be set to patch a child match." );
                }

                var jsonAsString = G_NS.JsonConvert.SerializeObject( MatchChild, SerializerOptions.NewtonsoftJsonSerializer );
                return new StringContent( jsonAsString, Encoding.UTF8, "application/json" );
            }
        }
    }
}
