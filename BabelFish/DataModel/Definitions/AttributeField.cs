using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;
using Newtonsoft.Json;

namespace Scopos.BabelFish.DataModel.Definitions {
    [Serializable]
    public class AttributeField {

        private static Logger logger= LogManager.GetCurrentClassLogger();

        public AttributeField() {
            Required = false;
            Key = false;
        }

        /// <summary>
        /// Name given to this field. It is unique within the parent ATTRIBUTE.
        /// </summary>
        public string FieldName { get; set; } = string.Empty;

        /// <summary>
        /// Human readable name for the field. This is the value that is displayed to users in a form entering ATTRIBUTE VALUES. In a Simple Attribute, it is customarily the same value as the ATTRIBUTE's DisplayName.
        /// </summary>
        public string DisplayName { get; set; } = string.Empty;

        /// <summary>
        /// Indicates if the value of the attribute must be chosen from a list, may be any value, of the there is a suggested list of values.
        /// </summary>
        public FieldType FieldType { get; set; } = FieldType.OPEN;

        /// <summary>
        /// The default value for this field. It is the value assigned to the field if the user does not enter one. 
        /// </summary>
        public dynamic DefaultValue { get; set; } = string.Empty;

        /// <summary>
        /// True if the user may enter multiple values in this field (in other words, its a list). False if the user may only enter one value.
        /// </summary>
        public bool MultipleValues { get; set; } = false;

        /// <summary>
        /// True if the user is required to enter a value. False if the user desn't have to. If the user doesn't have to, then the DefaultValue is applied.
        /// </summary>
        public bool Required { get; set; } = false;

        /// <summary>
        /// In an ATTRIBUTE that has MultipleValues set to true, Key determines the unique identifier in the list. Exactly one AttributeField within an ATTRIBUTE must have Key set to true.
        /// </summary>
        public bool Key { get; set; } = false;

        /// <summary>
        /// Human readable description of what this feild represents. 
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Validation rule that all must be true in order for the value to pass validation.
        /// </summary>
        public AttributeValidation Validation { get; set; } = new AttributeValidation();

        /// <summary>
        /// List of possible values, when FieldType is CLOSED or SUGGEST
        /// </summary>
        public List<AttributeValueOption> Values { get; set; } = new List<AttributeValueOption>();

        /// <summary>
        /// The type of data that this field will hold.
        /// </summary>
        public ValueType ValueType { get; set; } = ValueType.STRING;

        public override string ToString() {
            return $"{FieldName} of type {ValueType} Key: {Key}";
        }

        /// <summary>
        /// Returns a boolean indicating if the passed in value passes all validation tests for thei field.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool ValidateFieldValue( dynamic value ) {
            logger.Info( "ValidateFieldValue is not yet implemented. It is always returning true." );
            return true;
        }

    }
}
