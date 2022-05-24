using BabelFish.DataModel.GetSetAttributeValue;

namespace BabelFish.Responses.GetSetAttributeValueAPI
{
    public class GetValidateUserIDResponse : Response<ValidateUserID>
    {
        /// <summary>
        /// Facade function that returns the same as this.Value
        /// </summary>
        public ValidateUserID ValidateUserID
        {
            get { return Value; }
        }
    }
}