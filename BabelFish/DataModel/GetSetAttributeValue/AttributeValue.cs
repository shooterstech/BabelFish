using System.Text;
using BabelFish.DataModel.Definitions;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Text.RegularExpressions;

namespace BabelFish.DataModel.GetSetAttributeValue
{
    public class AttributeValue
    {

        internal Dictionary<string, Dictionary<string, dynamic>> attributeValues = new Dictionary<string, Dictionary<string, dynamic>>();

        DefinitionAPIClient? clientDefinition;
        internal Definitions.Attribute attributeDef;

        AttributeValueValidation AttributeValueValidation = new AttributeValueValidation();

        /// <summary>
        /// Instantiate a new AttributeValue to modify
        /// </summary>
        /// <param name="setName">Assign a valid Attribute SetName</param>
        /// <exception cref="Exception"></exception>
        public AttributeValue(string setName)
        {
            SetName = setName;
            LoadDefinition();
// This is captured in MessageResponse error text
// if (attributeDef == null)
//                ExceptionList.Add(AttributeValueException.ExceptionTextFor("DefinitionNotFound", $"for {SetName}"));
        }

        /// <summary>
        /// View the SetName of the AttributeValue.
        /// Assignment is done at instantiation.
        /// </summary>
        public string SetName { get; private set; } = string.Empty;

        /// <summary>
        /// Store any exceptions to alert user
        /// </summary>
        public string LastException { get; private set; } = String.Empty;

        /// <summary>
        /// httpStatus (leave this as string in case we get an unexpected status not in an enum?)
        /// </summary>
        [JsonProperty(Order = 1)] public string statusCode { get; set; } = string.Empty;
        [JsonConverter(typeof(StringEnumConverter))]
        public Helpers.VisibilityOption Visibility { get; set; }


        /// <summary>
        /// Load Definition file for reference
        /// </summary>
        private async void LoadDefinition()
        {
            try
            {
                clientDefinition = new DefinitionAPIClient(Helpers.SettingsHelper.UserSettings["XApiKey"], Helpers.SettingsHelper.RevertSettingsFormat());

                Responses.DefinitionAPI.GetDefinitionResponse<Definitions.Attribute> DefinitionResponse =
                    await clientDefinition.GetAttributeDefinitionAsync(Definitions.SetName.Parse(SetName)).ConfigureAwait(false);
                if (DefinitionResponse.Definition != null)
                    attributeDef = DefinitionResponse.Definition;
            }
            catch (Exception ex) {
                LastException = AttributeValueException.GetExceptionDefinitionError($"Not Found for {SetName}");
            }
        }

        /// <summary>
        /// Retrieve single Value then cast object type based on GetFieldType()
        /// </summary>
        /// <param name="FieldName">Valid FieldName as defined in AttributeDefintion</param>
        /// <returns>object to be type cast</returns>
        /// <exception cref="Exception"></exception>
        public object GetFieldValue(string FieldName)
        {
            //not multi value
            //if multi, throw exception
            if (true)//if multi, throw exception
                LastException = AttributeValueException.GetExceptionFieldValueError($"for {FieldName} in {SetName}");
            else
                throw new NotImplementedException();

            return new object();
        }

        /// <summary>
        /// Get mulitple Values then cast object type based on GetFieldType()
        /// </summary>
        /// <param name="FieldName">Valid FieldName from GetAttributeFields()</param>
        /// <param name="FieldKey">Valid FieldKey string from GetFieldKeys()</param>
        /// <returns>object to be Type cast</returns>
        /// <exception cref="Exception"></exception>
        public object GetFieldValue(string fieldName, string fieldKey)
        {
            //only if is multi value
            //Get field from AttributeDefinition then check MultipleValues then error or return
            if (false)//if !multi, throw exception
                LastException = AttributeValueException.GetExceptionFieldValueError($"querying {fieldKey} on non multi-value field {fieldName}");
            else
                throw new NotImplementedException();
        }

        /// <summary>
        /// Get a string list of Field Keys for SetName
        /// </summary>
        /// <returns>List<string> of Field Keys</returns>
        /// <exception cref="Exception"></exception>
        public List<string> GetFieldKeys()
        {
            List<string> FieldKeys = new List<string>(); //could be empty

            try
            {
                foreach (string loopKey in attributeValues.Keys)
                    FieldKeys.Add(loopKey);
            }
            catch (Exception ex)
            {
                LastException = AttributeValueException.GetExceptionKeyFieldNameError($": {ex.ToString()}");
            }

            return FieldKeys; //return list fieldnames where Key = true
        }

        /// <summary>
        /// typeof(Type) for Field from AttributeDefinition
        /// </summary>
        /// <param name="fieldName">Field Name string from GetAttributeFields()</param>
        /// <returns>???Type or List<Type>??? from AttributeDefinition</returns>
        public Type GetAttributeFieldValueType(string fieldName)
        {
            string typeToCheck = GetAttributeFields().Where(x => x.FieldName == fieldName).FirstOrDefault().ValueType;
            if (string.IsNullOrEmpty(typeToCheck))
                LastException = AttributeValueException.GetExceptionFieldTypeError($"not found for {fieldName} in {SetName}");

            return ConvertValueTypeToType(typeToCheck);

            //TODO: if field marked multi, return is List<>  ??????
            //throw new NotImplementedException();
        }

        /// <summary>
        /// Get list of Field objects
        /// </summary>
        /// <returns>List<AttributeField> from AttributeDefinition</returns>
        public List<AttributeField> GetAttributeFields()
        {
            return attributeDef.Fields;
        }

        /// <summary>
        /// Only one AttributeField within an ATTRIBUTE may have Key set to true.
        /// https://support.orionscoringsystem.com/definition-attributefield.html
        /// </summary>
        /// <returns></returns>
        public string GetAttributeFieldKey()
        {
            AttributeField findKey = GetAttributeFields().Where(x => x.Key).FirstOrDefault();
            if (findKey != null)
                return findKey.FieldName;
            else
                return String.Empty;
        }

        /// <summary>
        /// Get Field list of default fields
        /// </summary>
        /// <returns>List<AttributeField> from AttributeDefintion</AttributeField></returns>
        public List<AttributeField> GetAttributeDefaultFields()
        {
            return attributeDef.Fields.Where(x => x.DefaultValue).ToList();
        }

        /// <summary>
        /// Get list of Required Fields
        /// </summary>
        /// <returns>List<AttributeField> from AttributeDefinition</returns>
        public List<AttributeField> GetRequiredFields()
        {
            return attributeDef.Fields.Where(x => x.Required == true).ToList();
        }


        /// <summary>
        /// Checks MultipleValues flag from AttributeDefinition
        /// </summary>
        /// <returns>true or false</returns>
        public bool IsMultipleValue {  
            get 
            {
                return attributeDef.MultipleValues;
            }
        }

        /// <summary>
        /// Checks Field's Required flag from AttributeDefinition
        /// </summary>
        /// <param name="fieldName">Field Name string from GetAttributeFields()</param>
        /// <returns>true or false</returns>
        public bool? IsFieldNameRequired(string fieldName)
        {
            try
            {
                AttributeField attrField = attributeDef.Fields.Where(x => x.FieldName == fieldName).FirstOrDefault();
                if (string.IsNullOrEmpty(attrField.FieldName))
                    LastException = AttributeValueException.GetExceptionFieldNameError($"check Required for {fieldName} not found in {SetName}");
                else
                    return attrField.Required;
            }catch (Exception ex)
            {
                LastException = AttributeValueException.GetExceptionFieldNameError($"check Required for {fieldName} not found in {SetName}: {ex.ToString}");
            }
            return null;
        }

        /// <summary>
        /// Set Value for Field Name 
        /// </summary>
        /// <param name="fieldName">Field Name to set</param>
        /// <param name="fieldValue">Field Value to set</param>
        /// <exception cref="Exception"></exception>
        public void SetFieldName(string fieldName, object fieldValue)
        {
            if (this.IsMultipleValue)
                LastException = AttributeValueException.GetExceptionFieldValueError($"Field being set is designated MultipleValue needing Key. Use SetFieldName(string fieldName, object fieldValue, string fieldKey)");

            if (!ValidateSetFieldData(fieldName, fieldValue))
                LastException = AttributeValueException.GetExceptionFieldValueError($"Invalid Set Field Value {fieldValue} for {fieldName}");
            else 
            { 
                if ( !attributeValues.ContainsKey("AttributeList") )
                    attributeValues.Add("AttributeList", new Dictionary<string, dynamic> { { fieldName, fieldValue } });
                else
                    attributeValues["AttributeList"].Add(fieldName, fieldValue);
            }
        }

        /// <summary>
        /// Test Setting Field
        /// </summary>
        /// <param name="fieldName"></param>
        /// <param name="fieldValue"></param>
        /// <returns>true if no errors, false if fails</returns>
        public bool TrySetFieldName(string fieldName, object fieldValue)
        {
            return ValidateSetFieldData(fieldName, fieldValue);
        }

        /// <summary>
        /// Set Value for Field Name with Field Key
        /// </summary>
        /// <param name="fieldName">Field Name to set</param>
        /// <param name="fieldValue">Field value to set</param>
        /// <param name="fieldKey">Field Key to set</param>
        /// <exception cref="Exception"></exception>
        public void SetFieldName(string fieldName, object fieldValue, string fieldKey)
        {
            if ( !this.IsMultipleValue )
                LastException = AttributeValueException.GetExceptionFieldValueError($"Field being set is designated Single Value. Use SetFieldName(string fieldName, object fieldValue)");

            if (!ValidateSetFieldData(fieldName, fieldValue))
                LastException = AttributeValueException.GetExceptionFieldValueError($"Invalid Set Field Value {fieldValue} for {fieldName}");
            else
            {
                if (!attributeValues.ContainsKey(fieldKey))
                    attributeValues.Add(fieldKey, new Dictionary<string, dynamic> { { fieldName, fieldValue } });
                else
                    attributeValues[fieldKey].Add(fieldName, fieldValue);
            }
        }

        /// <summary>
        /// Test Setting Field with key
        /// </summary>
        /// <param name="fieldName"></param>
        /// <param name="fieldValue"></param>
        /// <returns>true if no errors, false if fails</returns>
        public bool TrySetFieldName(string fieldName, object fieldValue, string fieldKey)
        {
            return ValidateSetFieldData(fieldName, fieldValue, fieldKey);
        }

        /// <summary>
        /// Add Field Key for multiple = true
        /// </summary>
        /// <param name="fieldKey">Field Key to add</param>
        /// <exception cref="Exception"></exception>
        public void AddFieldKey(string fieldKeyValue)
        {
            if (!attributeValues.ContainsKey(fieldKeyValue))
            {
                if (GetAttributeFieldKey() != String.Empty)
                {
                    attributeValues[fieldKeyValue] = new Dictionary<string, dynamic>();

                    // TODO: loop in all default values
                }
                else
                    LastException = AttributeValueException.GetExceptionKeyFieldNameError($"no Key expected with {fieldKeyValue}");
            }
        }

        /// <summary>
        /// Helper function to validate setting Field Data
        /// </summary>
        /// <returns>true or false</returns>
        private bool ValidateSetFieldData(string fieldName, object fieldValue, string fieldKey = "")
        {
            // TODO: Make sure fieldKey things
            //validate fieldName
            //validate fieldValue match definition type
            //validate value based on attribute validation 
            //  https://support.orionscoringsystem.com/index.html?definition-attribute.html
            // make sure things like if "CLOSED" type then must be from listsetting

            //all required fields - before update???

            AttributeField FieldToValidate = GetAttributeFields().Where(x => x.FieldName == fieldName).FirstOrDefault();

            if (FieldToValidate != null)
            {
                // FieldName exists in Definition
                if (FieldToValidate.FieldName != fieldName)
                    return false;

                // Match Field Name Value format to regex match
                if (FieldToValidate.Validation != null)
                {
                    Definitions.AttributeValidation FieldNameValidation = FieldToValidate.Validation;
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
            Type validType = GetAttributeFieldValueType(fieldName);
            if ( !ValidFieldValueType(validType, fieldValue) )
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
                    GetAttributeFields().Where(x => x.FieldName == fieldName).FirstOrDefault().FieldType
                    , out Helpers.DefinitionFieldTypeEnums enumMatch))
                {
                    if ( enumMatch == Helpers.DefinitionFieldTypeEnums.CLOSED)
                    {
//TODO: maybe add a func GetSuggestionList() for user to get values from SUGGEST and CLOSED list?
                        List<AttributeValueOption> checkOptions = GetAttributeFields().Where(x => x.FieldName == fieldName).FirstOrDefault().Values;
                        if (!checkOptions.Select(x => x.Value == fieldValue).FirstOrDefault())
                            return false;
                    }
                } else
                    return false;
            }

            // Made it through the gauntlet unscathed!!
            return true;
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
        /// Delete record for Field Key
        /// </summary>
        /// <param name="fieldKey">Field Key to delete</param>
        public void DeleteFieldKey(string fieldKey)
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            StringBuilder foo = new StringBuilder();
            foo.Append($"{this.SetName} Attribute Value");
            return foo.ToString();
        }
    }
}