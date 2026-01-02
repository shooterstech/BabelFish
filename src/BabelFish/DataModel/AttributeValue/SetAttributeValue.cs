using System.Text;
using Scopos.BabelFish.DataModel;

namespace Scopos.BabelFish.DataModel.AttributeValue {

    [Serializable]
    public class SetAttributeValueList : BaseClass
    {
        public List<SetAttributeValue> SetAttributeValues = new List<SetAttributeValue>();

        public override string ToString()
        {
            StringBuilder foo = new StringBuilder();
            foo.Append("SetAttributeValue List");
            return foo.ToString();
        }
    }

    [Serializable]
    public class SetAttributeValue
    {
        public SetAttributeValue() { }

        /// <summary>
        /// The SetName of the AttributeValue that was sent and we have a response about how successful the set-ing went.
        /// </summary>
        public string AttributeValue { get; set; } = string.Empty;

        public string StatusCode { get; set; } = string.Empty;

        public List<string> Message { get; set; } = new List<string>();

        public override string ToString()
        {
            StringBuilder foo = new StringBuilder();
            foo.Append("Set Attribute Value Response status for ");
            foo.Append(AttributeValue);
            return foo.ToString();
        }

    }
}