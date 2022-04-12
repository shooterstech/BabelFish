using System.Text;
//using System.Text.Json; //COMMENT OUT FOR .NET Standard 2.0

namespace BabelFish.DataModel.GetSetAttributeValue {

    [Serializable]
    public class AttributeValueList
    {
        public List<AttributeValue> Attributes { get; set; } = new List<AttributeValue>();

        public override string ToString()
        {
            StringBuilder foo = new StringBuilder();
            foo.Append("Attribute List");
            return foo.ToString();
        }

    }
}
