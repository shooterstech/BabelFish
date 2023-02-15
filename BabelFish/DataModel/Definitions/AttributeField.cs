using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Scopos.BabelFish.Helpers;
using NLog;
using Newtonsoft.Json;
using Scopos.BabelFish.DataModel.AttributeValue;
using System.Configuration;

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
            //TODO: This just checks the type of the value. It does not check against the validation rules.

            switch (ValueType) {
                case ValueType.STRING:
                    if (MultipleValues)
                        return value is List<string>;
                    else
                        return value is string;

                case ValueType.BOOLEAN:
                    if (MultipleValues)
                        return value is List<bool>;
                    else
                        return value is bool;

                case ValueType.DATE:
                    if (MultipleValues)
                        return value is List<DateTime>;
                    else
                        return value is DateTime;

                case ValueType.DATE_TIME:
                    if (MultipleValues)
                        return value is List<DateTime>;
                    else
                        return value is DateTime;

                case ValueType.TIME:
                    if (MultipleValues)
                        return value is List<TimeSpan>;
                    else
                        return value is TimeSpan;

                case ValueType.INTEGER:
                    if (MultipleValues)
                        return value is List<int>;
                    else
                        return value is int;

                case ValueType.FLOAT:
                    if (MultipleValues)
                        return value is List<float> || value is List<double> || value is List<decimal> || value is int;
                    else

                        return value is float || value is double || value is decimal || value is int;

                default:
                    //Shouldn't ever get here.
                    logger.Error( $"Unexpected ValueType '{ValueType}'.");
                    return true;
            }
        }

        /// <summary>
        /// Some ValueTypes are not stored as the underlying value, but instead are stored as strings.
        /// This is because when the values are converted to json .
        /// It is a best practice to call ValidateFieldValue() with the value prior to calliing this function.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="AttributeValueValidationException">Thrown if the passed in value is not the expected type.</exception>
        internal dynamic SerializeFieldValue( dynamic value ) {
            //NOTE that this isn't a true serialization. as values of type int, floats, booleans just get passed through and not converted to strings.

            try {
                if (MultipleValues) {

                } else {
                    switch (ValueType) {
                        case ValueType.DATE:
                            DateTime dateValue = (DateTime)value;
                            return dateValue.ToString( DateTimeFormats.DATE_FORMAT );
                        case ValueType.TIME:
                            TimeSpan timeValue = (TimeSpan)value;
                            return timeValue.ToString( DateTimeFormats.TIME_FORMAT );
                        case ValueType.DATE_TIME:
                            DateTime dateTimeValue = (DateTime)value;
                            return dateTimeValue.ToString( DateTimeFormats.DATETIME_FORMAT );
                        default:
                            return value;
                    }
                }
            } catch ( Exception ex ) {
                //Presumable this would be a casting exception.
                Type dynamicType = value.GetType();
                var msg = $"Unable to serialize '{value}' to a string. Was expected a ValueType of '{ValueType}', instead got '{dynamicType}'.";
                throw new AttributeValueValidationException( msg, ex, logger );
            }
        }

    }
}
