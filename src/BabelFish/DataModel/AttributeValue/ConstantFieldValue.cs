namespace Scopos.BabelFish.DataModel.AttributeValue {


    [G_STJ_SER.JsonConverter( typeof( G_BF_STJ_CONV.ConstantFieldValueListConverter ) )]
    [G_NS.JsonConverter( typeof( G_BF_NS_CONV.ConstantFieldValueListConverter ) )]
    public class ConstantFieldValueList : List<ConstantFieldValue> {

    }

    public class ConstantFieldValue {

        public ConstantFieldValue() { }

        public ConstantFieldValue( string fieldName, dynamic value ) {
            FieldName = fieldName;
            Value = value;
        }

        public string FieldName { get; set; } = string.Empty;

        public dynamic Value { get; set; }
    }
}
