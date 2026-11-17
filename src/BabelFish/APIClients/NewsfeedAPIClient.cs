using Scopos.BabelFish.Requests.NewsfeedAPI;
using Scopos.BabelFish.Responses.NewsfeedAPI;
using Scopos.BabelFish.Runtime.Authentication;

namespace Scopos.BabelFish.APIClients {
    /// <summary>Authenticated newsfeed post operations. Mutations are never cached.</summary>
    public class NewsfeedAPIClient : APIClient<NewsfeedAPIClient> {
        /// <summary>Uses the configured default API stage.</summary>
        public NewsfeedAPIClient() : base() {
            LocalStoreDirectory = null;
            IgnoreFileSystemCache = true;
            IgnoreInMemoryCache = true;
        }

        /// <summary>Uses the specified API stage.</summary>
        public NewsfeedAPIClient( APIStage apiStage ) : base( apiStage ) {
            LocalStoreDirectory = null;
            IgnoreFileSystemCache = true;
            IgnoreInMemoryCache = true;
        }

        /// <summary>
        /// Creates a post. Do not automatically retry an uncertain result: the server has no idempotency key.
        /// </summary>
        public async Task<CreateNewsfeedPostAuthenticatedResponse> CreateNewsfeedPostAuthenticatedAsync( CreateNewsfeedPostAuthenticatedRequest request ) {
            var response = new CreateNewsfeedPostAuthenticatedResponse( request );
            await CallAPIAsync( request, response ).ConfigureAwait( false );
            return response;
        }

        /// <summary>Creates a personal post, or a club post when a license number is supplied.</summary>
        public Task<CreateNewsfeedPostAuthenticatedResponse> CreateNewsfeedPostAuthenticatedAsync( string bodyText, UserAuthentication credentials, int? licenseNumber = null ) =>
            CreateNewsfeedPostAuthenticatedAsync( new CreateNewsfeedPostAuthenticatedRequest( bodyText, credentials, licenseNumber ) );

        /// <summary>Soft-deletes a post. Authorized retries retain the first deletion's audit fields.</summary>
        public async Task<DeleteNewsfeedPostAuthenticatedResponse> DeleteNewsfeedPostAuthenticatedAsync( DeleteNewsfeedPostAuthenticatedRequest request ) {
            var response = new DeleteNewsfeedPostAuthenticatedResponse( request );
            await CallAPIAsync( request, response ).ConfigureAwait( false );
            return response;
        }

        /// <summary>Soft-deletes the post identified by its decimal-string ID.</summary>
        public Task<DeleteNewsfeedPostAuthenticatedResponse> DeleteNewsfeedPostAuthenticatedAsync( string postId, UserAuthentication credentials ) =>
            DeleteNewsfeedPostAuthenticatedAsync( new DeleteNewsfeedPostAuthenticatedRequest( postId, credentials ) );
    }
}
