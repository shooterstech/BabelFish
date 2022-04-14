using System.Text.RegularExpressions;
using BabelFish.DataModel.Definitions;

namespace BabelFish.DataModel.GetSetAttributeValue
{
    internal class AttributeValueValidation
    {

        /// <summary>
        /// Helper function to validate setting Field Data
        /// </summary>
        /// <returns>true or false</returns>
        public bool ValidateSetFieldData(AttributeField DefinitionToValidate, string fieldName, object fieldValue, string fieldKey = "")
        {
            // TODO: Make sure fieldKey things
            //validate fieldName
            //validate fieldValue match definition type
            //validate value based on attribute validation 
            //  https://support.orionscoringsystem.com/index.html?definition-attribute.html
            // make sure things like if "CLOSED" type then must be from listsetting

            //all required fields - before update???

            if (DefinitionToValidate != null)
            {
                // FieldName exists in Definition
                if (DefinitionToValidate.FieldName != fieldName)
                    return false;

                // Match Field Name Value format to regex match
                if (DefinitionToValidate.Validation != null)
                {
                    Definitions.AttributeValidation FieldNameValidation = DefinitionToValidate.Validation;
                    if (!string.IsNullOrEmpty(FieldNameValidation.Regex))
                    {
                        Regex regex = new Regex(FieldNameValidation.Regex);
                        Match match = regex.Match(fieldValue.ToString());
                        if (!match.Success)
                            return false;
                    }
                }
            }
            else
                return false;

            // FieldValue is valid Type
            // TODO: figure this out and finish
            Type validType = GetDefinitionFieldValueType(DefinitionToValidate, fieldName);
            if (!this.ValidFieldValueType(validType, fieldValue))
                return false;

            // Validate fieldValue based on attribute validation
            //  https://support.orionscoringsystem.com/definition-attributefield.html
            if (validType.GetType() == typeof(bool))
            {
                //FieldType should not exist in definition
            }

            // FieldType required when string
            if (validType.GetType() == typeof(string))
            {
                // FieldType in valid list
                if (Enum.TryParse<Helpers.DefinitionFieldTypeEnums>(
                    DefinitionToValidate.FieldType
                    , out Helpers.DefinitionFieldTypeEnums enumMatch))
                {
                    if (enumMatch == Helpers.DefinitionFieldTypeEnums.CLOSED)
                    {
                        //TODO: maybe add a func GetSuggestionList() for user to get values from SUGGEST and CLOSED list?
                        List<AttributeValueOption> checkOptions = DefinitionToValidate.Values;
                        if (!checkOptions.Select(x => x.Value == fieldValue).FirstOrDefault())
                            return false;
                    }
                }
                else
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
            // TODO: build this out
            return true;
            //            switch (castType.ToString())
            //            {
            //                case typeof(string):
            //                    break;
            //            }
        }

        /// <summary>
        /// typeof(Type) for Field from AttributeDefinition
        /// </summary>
        /// <param name="fieldName">Field Name string from GetDefintionFields()</param>
        /// <returns>???Type or List<Type>??? from AttributeDefinition</returns>
        public Type GetDefinitionFieldValueType(AttributeField fieldDefinition, string fieldName)
        {
            string typeToCheck = fieldDefinition.ValueType;

            return ConvertValueTypeToType(typeToCheck);

            //TODO: if field marked multi, return is List<>  ??????
            //throw new NotImplementedException();
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
                case "DATE":
                    //Is there a JUST DATE or need to parse and validate later?
                    // -OR-
                    //Is this the string that needs regex "yyyy-mm-dd"
                    return typeof(DateTime);
                case "BOOLEAN":
                    return typeof(Boolean);
                case "DATE TIME":
                    //need sample to confirm with/without space???
                    return typeof(DateTime);
                case "TIME":
                    //Is there just a Time type???
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
