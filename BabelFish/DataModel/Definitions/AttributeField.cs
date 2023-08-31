using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Scopos.BabelFish.Helpers;
using NLog;
using Newtonsoft.Json;
using Scopos.BabelFish.DataModel.AttributeValue;
using System.Configuration;
using Newtonsoft.Json.Linq;
using System.ComponentModel;

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

        private dynamic defaultValue = null;
        /// <summary>
        /// The default value for this field. It is the value assigned to the field if the user does not enter one. 
        /// </summary>
        [JsonIgnore]
        public dynamic DefaultValue { 
            get {
                //If defaultValue is null, it means it wasn't set as part of the definition. instead return a default value based on the value type
                if (defaultValue == null) {
                    switch( ValueType ) {
                        case ValueType.STRING:
                            if (MultipleValues)
                                defaultValue = new List<string>();
                            else
                                defaultValue = "";
                            break;

                        case ValueType.INTEGER:
                            if (MultipleValues)
                                defaultValue = new List<int>();
                            else 
                                defaultValue = 0;
                            break;

                        case ValueType.FLOAT:
                            if (MultipleValues)
                                defaultValue = new List<float>();
                            else
                                defaultValue = 0F;
                            break;

                        case ValueType.BOOLEAN:
                            //Booleans can not be multivalues
                            defaultValue = false;
                            break;

                        case ValueType.DATE:
                        case ValueType.DATE_TIME:
                            if (MultipleValues)
                                defaultValue = new List<DateTime>();
                            else
                                defaultValue = DateTime.UtcNow;
                            break;

                        case ValueType.TIME:
                            if (MultipleValues)
                                defaultValue = new List<TimeSpan>();
                            else
                                defaultValue = TimeSpan.Zero;
                            break;

                        case ValueType.SETNAME:
                            if (MultipleValues)
                                defaultValue = new List<SetName>();
                            else
                                defaultValue = SetName.Parse( "v1.0:orion:Profile Name" );
                            break;
                    }

                }

                return defaultValue;
            }
            set {
                if (value is JToken)
                    value = DeserializeFromJTokens( value );
            }
        }

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
            if (MultipleValues)
                return $"'{FieldName}' of list of type {ValueType}";
            else if (Key)
                return $"'{FieldName}' of type {ValueType} KEY VALUE";
            else
                return $"'{FieldName}' of type {ValueType}";
        }

        internal dynamic DeserializeFromJTokens(JToken value) {
            switch (ValueType) {
                case ValueType.STRING:
                    if (MultipleValues)
                        return value.ToObject<List<string>>();
                    else
                        return (string) value;

                case ValueType.BOOLEAN:
                    //Attribute definitions don't allow lists of booleans
                    return (bool) value;

                case ValueType.DATE:
                    if (MultipleValues)
                        return value.ToObject<List<DateTime>>();
                    else
                        return (DateTime) value;

                case ValueType.DATE_TIME:
                    if (MultipleValues)
                        return value.ToObject<List<DateTime>>();
                    else
                        return (DateTime)value;

                case ValueType.TIME:
                    if (MultipleValues)
                        return value.ToObject<List<TimeSpan>>();
                    else
                        return (TimeSpan)value;

                case ValueType.INTEGER:
                    if (MultipleValues)
                        return value.ToObject<List<int>>();
                    else
                        return (int) value;

                case ValueType.FLOAT:
                    if (MultipleValues)
                        return value.ToObject<List<float>>();
                    else

                        return (float) value;

                default:
                    //Shouldn't ever get here.
                    logger.Error( $"Unexpected ValueType '{ValueType}'." );
                    return DefaultValue;
            }
        }

        /// <summary>
        /// Returns a boolean indicating if the passed in value passes all validation tests for thei field.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool ValidateFieldValue( dynamic value ) {
            //TODO: This just checks the type of the value. It does not check against the validation rules.

            Type t = value.GetType();

            switch (ValueType) {
                case ValueType.STRING:
                    if (MultipleValues)
                        return value is List<string>;
                    else
                        return value is string;

                case ValueType.BOOLEAN:
                    //Attribute definitions don't allow lists of booleans
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

                        return value is float 
                            || value is double 
                            || value is decimal 
                            || value is int;

                case ValueType.SETNAME:
                    if (MultipleValues)
                        return value is List<SetName>;
                    else
                        return value is SetName;

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
                    var myList = new List<string>();
                    switch (ValueType) {
                        case ValueType.DATE:
                            foreach (var item in value) {
                                DateTime dateValue = (DateTime)item;
                                myList.Add( dateValue.ToString( DateTimeFormats.DATE_FORMAT ) );
                            }
                            return myList;

                        case ValueType.TIME:
                            foreach (var item in value) {
                                TimeSpan timeValue = (TimeSpan)item;
                                myList.Add( timeValue.ToString( DateTimeFormats.TIME_FORMAT ) );
                            }
                            return myList;

                        case ValueType.DATE_TIME:
                            foreach (var item in value) {
                                DateTime dateTimeValue = (DateTime)item;
                                myList.Add( dateTimeValue.ToString( DateTimeFormats.DATETIME_FORMAT ) );
                            }
                            return myList;

                        default:
                            return value;
                    }

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

        internal dynamic DeserializeFieldValue( dynamic value) {
            try {
                if ( MultipleValues) {
                    switch (ValueType) {
                        case ValueType.DATE:
                            var dateList = new List<DateTime>();
                            foreach( var item in value)
                                dateList.Add( DateTime.ParseExact( (string)item, DateTimeFormats.DATE_FORMAT, DateTimeFormats.CULTURE ) );
                            return dateList;
                        case ValueType.TIME:
                            var timeList = new List<TimeSpan>();
                            foreach (var item in value)
                                timeList.Add( TimeSpan.ParseExact( (string)item, DateTimeFormats.TIME_FORMAT, DateTimeFormats.CULTURE ) );
                            return timeList;
                        case ValueType.DATE_TIME:
                            var dateTimeList = new List<DateTime>();
                            foreach (var item in value)
                                dateTimeList.Add( DateTime.ParseExact( (string)item, DateTimeFormats.DATETIME_FORMAT, DateTimeFormats.CULTURE ) );
                            return dateTimeList;
                        default:
                            return value;
                    }
                } else {
                    switch (ValueType) {
                        case ValueType.DATE:
                            DateTime dateValue = DateTime.ParseExact( (string)value, DateTimeFormats.DATE_FORMAT, DateTimeFormats.CULTURE );
                            return dateValue;
                        case ValueType.TIME:
                            TimeSpan timeValue = TimeSpan.ParseExact( (string)value, DateTimeFormats.TIME_FORMAT, DateTimeFormats.CULTURE );
                            return timeValue;
                        case ValueType.DATE_TIME:
                            DateTime dateTimeValue = DateTime.ParseExact( (string)value, DateTimeFormats.DATETIME_FORMAT, DateTimeFormats.CULTURE );
                            return dateTimeValue;
                        default:
                            return value;
                    }

                }

            } catch (Exception ex) {
                //Presumable this would be a casting exception.
                var msg = $"Unable to deserialize '{value}' from a string. Was expected a ValueType of '{ValueType}'.";
                throw new AttributeValueValidationException( msg, ex, logger );
            }
        }

    }
}
