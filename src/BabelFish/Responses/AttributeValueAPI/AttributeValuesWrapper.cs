using Scopos.BabelFish.DataModel;
using Scopos.BabelFish.DataModel.Definitions;

namespace Scopos.BabelFish.Responses.AttributeValueAPI {
    public class AttributeValuesWrapper : BaseClass {

        public AttributeValuesWrapper() {
            AttributeValues = new Dictionary<SetName, AttributeValueDataPacketAPIResponse>();
        }

        /// <summary>
        /// The Key is the set name of the attribute value.
        /// The Value is the server response (which includes the AttributeValue's value).
        /// </summary>
        public Dictionary<SetName, AttributeValueDataPacketAPIResponse> AttributeValues { get; set; }
    }
}
