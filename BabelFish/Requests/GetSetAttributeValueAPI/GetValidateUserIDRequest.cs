using Scopos.BabelFish.Helpers;

namespace Scopos.BabelFish.Requests.GetSetAttributeValueAPI
{
    public class GetValidateUserIDRequest : Request
    {
        public GetValidateUserIDRequest() 
        {
            WithAuthentication = true;
        }

        /// <summary>
        /// UserId to lookup
        /// </summary>
        public string UserID { get; set; } = string.Empty;

        /// <inheritdoc />
        public override string RelativePath
        {
            get { return $"/users/{UserID}"; }
        }

        public override APIStage ApiStage
        {  
            get { return APIStage.PRODUCTION; } 
        }
    }
}