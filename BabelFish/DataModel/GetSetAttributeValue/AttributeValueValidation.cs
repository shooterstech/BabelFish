using System.Text.RegularExpressions;
using ShootersTech.DataModel.Definitions;

namespace ShootersTech.DataModel.GetSetAttributeValue
{
    internal class AttributeValueValidation
    {

        public string lastException { get; private set; } = string.Empty;
        /// <summary>
        /// Helper function to validate setting Field Data
        /// https://support.orionscoringsystem.com/index.html?definition-attribute.html
        /// </summary>
        /// <returns>true or false</returns>
        public bool ValidateSetFieldData(AttributeField definitionToValidate, string fieldName, object fieldValue, string fieldKey = "")
        {
            if (definitionToValidate != null)
            {
                // fieldName exists in Definition
                if (definitionToValidate.FieldName != fieldName)
                {
                    lastException = "Validation Field Name mismatch";
                    return false;
                }

                // fieldValue matches Definition Type
                Type checkType = GetDefinitionFieldValueType(definitionToValidate, fieldName);
                if (!this.ValidFieldValueType(checkType, fieldValue))
                {
                    lastException = $"Field Value cannot be cast to Definition Field Type of {checkType.ToString()}";
                    return false;
                }

                // Required values when FieldType = CLOSED
                if (Enum.TryParse<Helpers.DefinitionFieldTypeEnums>(
                    definitionToValidate.FieldType
                    , out Helpers.DefinitionFieldTypeEnums enumMatch))
                {
                    if (enumMatch == Helpers.DefinitionFieldTypeEnums.CLOSED)
                    {
                        var allowedValues = definitionToValidate.Values.Select(x => x.Value).ToList();
                        bool found = false;
                        foreach (var val in allowedValues)
                        {
                            if (val.ToString() == fieldValue.ToString())
                                found = true;
                        }
                        if ( !found )
                        {
                            lastException = $"Value {fieldValue} not in allowed list of values: {String.Join(", ", allowedValues.ToList())}";
                            return false;
                        }
                    }
                }
                else if ( checkType == typeof(String) )
                    return false;

                // fieldValue Validation
                if (definitionToValidate.Validation != null)
                {
                    Definitions.AttributeValidation fieldNameValidation = definitionToValidate.Validation;

                    // MIN value
                    if (fieldNameValidation.MinValue != null)
                    {
                        switch (definitionToValidate.ValueType)
                        {
                            case "DATE":
                                if ( DateTime.Parse(fieldValue.ToString()).Date < DateTime.Parse(fieldNameValidation.MinValue.ToString()))
                                {
                                    lastException = definitionToValidate.Validation.ErrorMessage;
                                    return false;
                                }
                                break;
                            default:
                                break;
                        }
                    }
                    
                    // MAX value
                    if (fieldNameValidation.MaxValue != null)
                    {
                        switch (definitionToValidate.ValueType)
                        {
                            case "DATE":
                                if (DateTime.Parse(fieldValue.ToString()).Date > DateTime.Parse((string)fieldNameValidation.MaxValue.ToString()))
                                {
                                    lastException = definitionToValidate.Validation.ErrorMessage;
                                    return false;
                                }
                                break;
                            default:
                                break;
                        }
                    }

                    // REGEX
                    if (!string.IsNullOrEmpty(fieldNameValidation.Regex))
                    {
                        Regex regex = new Regex(fieldNameValidation.Regex);
                        Match match = regex.Match(fieldValue.ToString());
                        if (!match.Success)
                        {
                            lastException = definitionToValidate.Validation.ErrorMessage;
                            return false;
                        }
                    }
                }
            }
            else
            {
                lastException = "Definition not found.";
                return false;
            }

            // Made it through the gauntlet unscathed!!
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="castType"></param>
        /// <param name="fieldValue"></param>
        /// <returns></returns>
        public bool ValidFieldValueType(Type castType, object fieldValue)
        {
            if (Helpers.SettingsHelper.ConvertSettingsType(castType, fieldValue.ToString()) != null)
                return true;
            else
                return false;
        }

        /// <summary>
        /// typeof(Type) for Field from AttributeDefinition
        /// </summary>
        /// <param name="fieldName">Field Name string from GetDefintionFields()</param>
        /// <returns>Type from AttributeDefinition</returns>
        public Type GetDefinitionFieldValueType(AttributeField fieldDefinition, string fieldName)
        {
            string typeToCheck = fieldDefinition.ValueType;
            return ConvertValueTypeToType(typeToCheck);
        }

        /// <summary>
        /// Convert Definition string type value to Type
        /// https://support.orionscoringsystem.com/string-formatting-value-type.html
        /// </summary>
        /// <param name="typeToConvert"></param>
        /// <returns></returns>
        // TODO: generic enough to put in Helpers????
        public Type ConvertValueTypeToType(string typeToConvert)
        {
            switch (typeToConvert)
            {
                case "STRING":
                    return typeof(string);
                case "BOOLEAN":
                    return typeof(Boolean);
                case "DATE":
                    return typeof(DateTime);
                case "DATE TIME":
                    return typeof(DateTime);
                case "TIME":
                    return typeof(DateTime);
                case "INTEGER":
                    return typeof(Int32);
                case "FLOAT":
                    return typeof(float);
                case "SETNAME":
                    return typeof(SetName);
                default:
                    return null;
            }
        }
    }
}
