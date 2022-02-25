using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace BabelFish.DataModel.Definitions {
    [Serializable]
    public class AttributeField {

        //TODO Convert these string constants into an ENUM
        public const string FIELDTYPE_OPEN = "OPEN";
        public const string FIELDTYPE_CLOSED = "CLOSED";
        public const string FIELDTYPE_SUGGEST = "SUGGEST";
        public const string FIELDTYPE_DERIVED = "DERIVED";

        public const string VALUETYPE_STRING = "STRING";
        public const string VALUETYPE_INTEGER = "INTEGER";
        public const string VALUETYPE_DATE = "DATE";
        public const string VALUETYPE_DATE_TIME = "DATE TIME";
        public const string VALUETYPE_FLOAT = "FLOAT";
        public const string VALUETYPE_BOOLEAN = "BOOLEAN";


        List<string> errorList = new List<string>();

        public AttributeField() {
            Required = false;
            Key = false;
        }

        public string FieldName { get; set; } = string.Empty;

        public string DisplayName { get; set; } = string.Empty;

        public string FieldType { get; set; } = string.Empty;

        public dynamic DefaultValue { get; set; } = string.Empty;

        public bool MultipleValues { get; set; } = false;

        public bool Required { get; set; } = false;

        public bool Key { get; set; } = false;

        public AttributeValidation Validation { get; set; } = new AttributeValidation();

        public List<AttributeValueOption> Values { get; set; } = new List<AttributeValueOption>();

        public string ValueType { get; set; } = string.Empty;

    }
}
