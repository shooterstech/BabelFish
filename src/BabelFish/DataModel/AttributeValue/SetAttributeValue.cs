using Scopos.BabelFish.DataModel.Common;
using Scopos.BabelFish.DataModel.Definitions;

namespace Scopos.BabelFish.DataModel.AttributeValue {

    public class SetAttributeValueList : BaseClass {
        public List<SetAttributeValue> SetAttributeValues = new List<SetAttributeValue>();

        public override string ToString() {
            StringBuilder foo = new StringBuilder();
            foo.Append( "SetAttributeValue List" );
            return foo.ToString();
        }
    }

    public class SetAttributeValue {
        public SetAttributeValue() { }

        /// <summary>
        /// The SetName of the AttributeValue that was sent and we have a response about how successful the set-ing went.
        /// </summary>
        public SetName AttributeValue { get; set; } = new SetName();

        public string StatusCode { get; set; } = string.Empty;

        public List<string> Message { get; set; } = new List<string>();

        public override string ToString() {
            StringBuilder foo = new StringBuilder();
            foo.Append( "Set Attribute Value Response status for " );
            foo.Append( AttributeValue );
            return foo.ToString();
        }

    }
}
