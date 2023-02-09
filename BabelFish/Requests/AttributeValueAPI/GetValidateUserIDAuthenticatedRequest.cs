using Scopos.BabelFish.Helpers;

namespace Scopos.BabelFish.Requests.AttributeValueAPI {
    public class GetValidateUserIDAuthenticatedRequest : Request {
        public GetValidateUserIDAuthenticatedRequest() : base( "ValidateUserID" ) {
            RequiresCredentials = true;
        }

        /// <summary>
        /// UserId to lookup
        /// </summary>
        public string UserID { get; set; } = string.Empty;

        /// <inheritdoc />
        public override string RelativePath {
            get { return $"/users/{UserID}"; }
        }
    }
}