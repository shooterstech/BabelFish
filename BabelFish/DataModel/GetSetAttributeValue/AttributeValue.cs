using System.Text;
using BabelFish.DataModel.Definitions;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace BabelFish.DataModel.GetSetAttributeValue
{
    public class AttributeValue
    {

        internal Dictionary<string, Dictionary<string, dynamic>> attributeValues = new Dictionary<string, Dictionary<string, dynamic>>();

        internal AttributeValueDefinition attributeDef = new AttributeValueDefinition();

        AttributeValueValidation AttributeValueValidation = new AttributeValueValidation();

        /// <summary>
        /// Instantiate a new AttributeValue to modify
        /// </summary>
        /// <param name="setName">Assign a valid Attribute SetName</param>
        /// <exception cref="Exception"></exception>
        public AttributeValue(string setName)
        {
            SetName = setName;
            attributeDef.LoadDefinition(SetName);
            SetDefaultFieldValues();
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
        public Helpers.VisibilityOption Visibility { get; set; } = Helpers.VisibilityOption.PRIVATE;

        [JsonConverter(typeof(StringEnumConverter))]
        public Helpers.AttributeValueActionEnums Action { get; set; } = Helpers.AttributeValueActionEnums.ADD;

        #region Definition

        /// <summary>
        /// Get list of Attribute Definition Field objects
        /// </summary>
        /// <returns>List<AttributeField> from AttributeDefinition</returns>
        public List<AttributeField> GetDefintionFields()
        {
            return attributeDef.Fields;
        }

        /// <summary>
        /// Only one AttributeField within an ATTRIBUTE may have Key set to true.
        /// https://support.orionscoringsystem.com/definition-attributefield.html
        /// </summary>
        /// <returns></returns>
        public string GetDefinitionKeyFieldName()
        {
            AttributeField findKey = attributeDef.Fields.Where(x => x.Key).FirstOrDefault();
            if (findKey != null)
                return findKey.FieldName;
            else
                return String.Empty;
        }

        /// <summary>
        /// Get Field list of default fields
        /// </summary>
        /// <returns>List<AttributeField> from AttributeDefintion</AttributeField></returns>
        public List<AttributeField> GetDefinitionFieldsDefaultValues()
        {
            return attributeDef.Fields.Where(x => x.DefaultValue.ToString() != string.Empty).ToList();
        }

        /// <summary>
        /// Get list of Required Fields
        /// </summary>
        /// <returns>List<AttributeField> from AttributeDefinition</returns>
        public List<AttributeField> GetDefinitionRequiredFields()
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
        /// <param name="fieldName">Field Name string from GetDefintionFields()</param>
        /// <returns>true or false</returns>
        public bool? IsFieldNameRequired(string fieldName)
        {
            return attributeDef.IsFieldNameRequired(fieldName);
        }
        #endregion Definition

        #region Attribute

        /// <summary>
        /// Get a string list of Field Keys for SetName
        /// </summary>
        /// <returns>List<string> of Field Keys</returns>
        /// <exception cref="Exception"></exception>
        public List<string> GetAttributeFieldKeys()
        {
            ClearLastException();
            //TODO: is this supposed to be Definition.Fields.FieldName.Where(Key=true) or actual field values?
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

            return FieldKeys;
        }

        /// <summary>
        /// Retrieve single Value then cast object type based on GetFieldType()
        /// </summary>
        /// <param name="FieldName">Valid FieldName as defined in AttributeDefintion</param>
        /// <returns>object to be type cast</returns>
        public object GetFieldValue(string fieldName)
        {
            var returnValue = new object();
            ClearLastException();
            if (this.IsMultipleValue)
                LastException = AttributeValueException.GetExceptionFieldValueError($"querying multiple value for non multi-value {fieldName} in {SetName}");
            else
                returnValue = (attributeValues["AttributeList"].ContainsKey(fieldName)) ? attributeValues["AttributeList"][fieldName] : new object();

            return returnValue;
        }

        /// <summary>
        /// Get mulitple Values then cast object type based on GetFieldType()
        /// </summary>
        /// <param name="FieldName">Valid FieldName from GetAttributeDefintionFields()</param>
        /// <param name="FieldKey">Valid FieldKey string from GetFieldKeys()</param>
        /// <returns>object to be Type cast; null object if not found</returns>
        public object? GetFieldValue(string fieldName, string fieldKey)
        {
            object? returnValue = null;
            ClearLastException();
            if (this.IsMultipleValue)
                LastException = AttributeValueException.GetExceptionFieldValueError($"querying {fieldKey} on non multi-value field {fieldName}");
            else
                returnValue = (attributeValues.ContainsKey(fieldKey) && attributeValues[fieldKey].ContainsKey(fieldName)) ? attributeValues[fieldKey][fieldName] : null;

            return returnValue;
        }

        /// <summary>
        /// Set Attribute Value for Field Name
        /// </summary>
        /// <param name="fieldName">Field Name to set</param>
        /// <param name="fieldValue">Field Value to set</param>
        public void SetFieldName(string fieldName, object fieldValue)
        {
            ClearLastException();
            if (GetDefintionFields().Where(x => x.FieldName == fieldName).FirstOrDefault().MultipleValues == true)
                LastException = AttributeValueException.GetExceptionFieldValueError($"Field being set is designated MultipleValue needing Key. Use SetFieldName(string fieldName, object fieldValue, string fieldKey)");

            if (!AttributeValueValidation.ValidateSetFieldData(GetDefintionFields().Where(x => x.FieldName == fieldName).FirstOrDefault(), fieldName, fieldValue))
                LastException = AttributeValueException.GetExceptionFieldValueError($"Invalid Set Field Value {fieldValue} for {fieldName}");
            else 
            { 
                if ( !attributeValues.ContainsKey("AttributeList") )
                    attributeValues.Add("AttributeList", new Dictionary<string, dynamic> { { fieldName, fieldValue } });
                else
                {
                    if ( attributeValues["AttributeList"].ContainsKey(fieldName) )
                        attributeValues["AttributeList"][fieldName] = fieldValue;
                    else
                        attributeValues["AttributeList"].Add(fieldName, fieldValue);
                }
            }
        }

        /// <summary>
        /// Test Setting Attribute Value for Field Name
        /// </summary>
        /// <param name="fieldName"></param>
        /// <param name="fieldValue"></param>
        /// <returns>true if no errors, false if fails</returns>
        public bool TrySetFieldName(string fieldName, object fieldValue)
        {
            return AttributeValueValidation.ValidateSetFieldData(GetDefintionFields().Where(x => x.FieldName == fieldName).FirstOrDefault(), fieldName, fieldValue);
        }

        /// <summary>
        /// Set Attribute Value for Field Name with Field Key
        /// </summary>
        /// <param name="fieldName">Field Name to set</param>
        /// <param name="fieldValue">Field value to set</param>
        /// <param name="fieldKey">Field Key to set</param>
        /// <exception cref="Exception"></exception>
        public void SetFieldName(string fieldName, object fieldValue, string fieldKey)
        {
            ClearLastException();
            if ( !this.IsMultipleValue )
                LastException = AttributeValueException.GetExceptionFieldValueError($"Field being set is designated Single Value. Use SetFieldName(string fieldName, object fieldValue)");

            if (!AttributeValueValidation.ValidateSetFieldData(GetDefintionFields().Where(x => x.FieldName == fieldName).FirstOrDefault(), fieldName, fieldValue))
                LastException = AttributeValueException.GetExceptionFieldValueError($"Invalid Set Field Value {fieldValue} for {fieldName}");
            else
            {
                if (!attributeValues.ContainsKey(fieldKey))
                    attributeValues.Add(fieldKey, new Dictionary<string, dynamic> { { fieldName, fieldValue } });
                else
                {
                    if (attributeValues[fieldKey].ContainsKey(fieldName))
                        attributeValues[fieldKey][fieldName] = fieldValue;
                    else
                        attributeValues[fieldKey].Add(fieldName, fieldValue);

                }
            }
        }

        /// <summary>
        /// Test Setting Attribute Value for Field with key
        /// </summary>
        /// <param name="fieldName"></param>
        /// <param name="fieldValue"></param>
        /// <returns>true if no errors, false if fails</returns>
        public bool TrySetFieldName(string fieldName, object fieldValue, string fieldKey)
        {
            return AttributeValueValidation.ValidateSetFieldData(GetDefintionFields().Where(x => x.FieldName == fieldName).FirstOrDefault(), fieldName, fieldValue, fieldKey);
        }

        /// <summary>
        /// Add Field Key for multiple = true
        /// </summary>
        /// <param name="fieldKey">Field Key to add</param>
        /// <exception cref="Exception"></exception>
        public void AddFieldKey(string fieldKeyValue)
        {
            ClearLastException();
            if (!attributeValues.ContainsKey(fieldKeyValue))
            {
                if (GetDefinitionKeyFieldName() != String.Empty)
                {
                    attributeValues[fieldKeyValue] = new Dictionary<string, dynamic>();

                    // TODO: loop in all default values
                }
                else
                    LastException = AttributeValueException.GetExceptionKeyFieldNameError($"no Key expected with {fieldKeyValue}");
            }
        }

        /// <summary>
        /// Delete record for Field Key
        /// </summary>
        /// <param name="fieldKey">Field Key to delete</param>
        public void DeleteFieldKey(string fieldKey)
        {
            throw new NotImplementedException();
        }

        private void SetDefaultFieldValues()
        {
            foreach (AttributeField fieldDefaults in GetDefinitionFieldsDefaultValues())
                if ( attributeDef.MultipleValues==false && 
                    fieldDefaults.MultipleValues == false &&
                    fieldDefaults.Required == true)
                    SetFieldName(fieldDefaults.FieldName, fieldDefaults.DefaultValue);
        }
        #endregion Attribute

        private void ClearLastException()
        {
            LastException = string.Empty;
        }

        public override string ToString()
        {
            StringBuilder foo = new StringBuilder();
            foo.Append($"{this.SetName} Attribute Value");
            return foo.ToString();
        }
    }
}