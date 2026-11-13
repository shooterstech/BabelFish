using System.Globalization;
using System.Text.RegularExpressions;
using Scopos.BabelFish.Runtime.Authentication;

namespace Scopos.BabelFish.Requests.NewsfeedAPI {
    /// <summary>Soft-deletes a post using server-side stored ownership and permissions.</summary>
    public class DeleteNewsfeedPostAuthenticatedRequest : Request {
        /// <summary>Constructs a request using the logged-in user's credentials.</summary>
        public DeleteNewsfeedPostAuthenticatedRequest( UserAuthentication credentials ) : base( "DeleteNewsfeedPost", credentials ) {
            HttpMethod = HttpMethod.Delete;
            IgnoreInMemoryCache = true;
            IgnoreFileSystemCache = true;
        }

        /// <summary>Constructs a deletion request for a decimal-string post identifier.</summary>
        public DeleteNewsfeedPostAuthenticatedRequest( string postId, UserAuthentication credentials ) : this( credentials ) {
            PostId = postId;
        }

        /// <summary>Required positive unsigned BIGINT, represented as a decimal string.</summary>
        public string PostId { get; set; } = string.Empty;

        /// <inheritdoc />
        public override string RelativePath {
            get {
                if (PostId == null || !Regex.IsMatch( PostId, @"\A[1-9][0-9]{0,19}\z" ) ||
                    !ulong.TryParse( PostId, NumberStyles.None, CultureInfo.InvariantCulture, out _ ))
                    throw new APIRequestParameterException( "PostId must be a positive unsigned BIGINT decimal string." );
                return $"/newsfeed/posts/{PostId}";
            }
        }
    }
}
