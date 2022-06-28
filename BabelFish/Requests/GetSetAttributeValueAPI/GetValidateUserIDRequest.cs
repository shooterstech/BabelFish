using ShootersTech.Helpers;

namespace ShootersTech.Requests.GetSetAttributeValueAPI
{
    public class GetValidateUserIDRequest : Request
    {
        public GetValidateUserIDRequest(string userID)
        {
            WithAuthentication = true;
            UserID = userID;
        }

        private string UserID { get; set; } = string.Empty;

        /// <inheritdoc />
        public override string RelativePath
        {
            get { return $"/users/{UserID}"; }
        }

        public override APIStage ApiStage
        {  
            get { return APIStage.BETA; } 
        }
    }
}