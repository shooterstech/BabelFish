using Scopos.BabelFish.DataModel.AttributeValue;
using Scopos.BabelFish.Requests.AttributeValueAPI;

namespace Scopos.BabelFish.Responses.AttributeValueAPI
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