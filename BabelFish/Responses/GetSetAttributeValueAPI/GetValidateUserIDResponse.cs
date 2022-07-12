using ShootersTech.BabelFish.DataModel.GetSetAttributeValue;
using ShootersTech.BabelFish.Requests.GetSetAttributeValueAPI;

namespace ShootersTech.BabelFish.Responses.GetSetAttributeValueAPI
{
    public class GetValidateUserIDResponse : Response<ValidateUserID>
    {

        public GetValidateUserIDResponse( GetValidateUserIDRequest request ) : base() {
            this.Request = request;
        }

        /// <summary>
        /// Facade function that returns the same as this.Value
        /// </summary>
        public ValidateUserID ValidateUserID
        {
            get { return Value; }
        }
    }
}