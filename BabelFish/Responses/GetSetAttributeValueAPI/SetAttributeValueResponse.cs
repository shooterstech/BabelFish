using BabelFish.DataModel.GetSetAttributeValue;

namespace BabelFish.Responses.GetSetAttributeValueAPI
{
    //TODO: Update to multiples List<SetAttributeValue> after it's working....
    public class SetAttributeValueResponse : Response<SetAttributeValueWrapper>
    {
        /// <summary>
        /// Facade function that returns the same as this.Value
        /// </summary>
        public List<SetAttributeValue> SetAttributeValues
        {
            get { return Value.SetAttributeValues; }
        }
    }
}