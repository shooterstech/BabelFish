using BabelFish.DataModel.GetSetAttributeValue;

namespace BabelFish.Responses.GetSetAttributeValueAPI
{
    //TODO: Update to multiples List<SetAttributeValue> after it's working....
    public class SetAttributeValueResponse : Response<SetAttributeValue>
    {
        /// <summary>
        /// Facade function that returns the same as this.Value
        /// </summary>
        public SetAttributeValue SetAttributeValue
        {
            get { return Value; }
        }
    }
}