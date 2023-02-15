using System.Text;
using Scopos.BabelFish.DataModel;
using Scopos.BabelFish.DataModel.Definitions;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using NLog;
using Newtonsoft.Json.Linq;
using Scopos.BabelFish.Responses.AttributeValueAPI;

namespace Scopos.BabelFish.DataModel.AttributeValue {
    public class AttributeValue : IJToken {

        private Logger logger = LogManager.GetCurrentClassLogger();
        private static AttributeValueDefinitionFetcher FETCHER = AttributeValueDefinitionFetcher.FETCHER;

        private Dictionary<string, Dictionary<string, dynamic>> attributeValues = new Dictionary<string, Dictionary<string, dynamic>>();
        private SetName setName = null;
        private Scopos.BabelFish.DataModel.Definitions.Attribute definition;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="setName"></param>
        /// <exception cref="XApiKeyNotSetException">Thrown if the x-api-key has not yet been set on AttributeValueDefinitionFetcher.FETCHER.</exception>
        public AttributeValue( SetName setName ) {

            if (!FETCHER.IsXApiKeySet)
                throw new XApiKeyNotSetException();

            this.SetName= setName;
            definition = FETCHER.FetchAttributeDefinition( SetName );

            SetDefaultFieldValues();
        }

        /// <summary>
        /// Instantiate a new AttributeValue to modify
        /// </summary>
        /// <param name="setName">Assign a valid Attribute SetName</param>
        /// <exception cref="ArgumentException">Thrown if the passed in setName string could not be parsed into a validated SetName</exception>
        /// <exception cref="XApiKeyNotSetException">Thrown if the x-api-key has not yet been set on AttributeValueDefinitionFetcher.FETCHER.</exception>
        public AttributeValue( string setName ) {

            if (!FETCHER.IsXApiKeySet)
                throw new XApiKeyNotSetException();

            SetName = SetName.Parse( setName );
            definition = FETCHER.FetchAttributeDefinition( SetName );

            SetDefaultFieldValues();
        }

        public AttributeValue( string setName, JToken attributeValueAsJToken) {

            if (!FETCHER.IsXApiKeySet)
                throw new XApiKeyNotSetException();

            SetName = SetName.Parse( setName );
            definition = FETCHER.FetchAttributeDefinition( SetName );

            if (definition.MultipleValues) {
                foreach( var av in (JArray) attributeValueAsJToken) {
                    ParseJObject( (JObject) av );
                }
            } else {
                ParseJObject( (JObject) attributeValueAsJToken );
            }
        }

        private void ParseJObject( JObject attributeValueAsJObject ) {

            var keyFieldName = GetDefinitionKeyFieldName();
            var keyFieldValue = "";
            if (string.IsNullOrEmpty( keyFieldName ))
                keyFieldValue = (string) attributeValueAsJObject[keyFieldName];

            foreach( var field in definition.Fields) {
                var fieldName = field.FieldName;
                dynamic fieldValue = (dynamic) attributeValueAsJObject[fieldName];

                if (definition.MultipleValues) {
                    this.SetFieldValue( fieldName, fieldValue, keyFieldValue );
                } else {
                    this.SetFieldValue( fieldName, fieldValue );
                }
                
            }
        }

        /// <summary>
        /// View the SetName of the AttributeValue.
        /// Assignment is done at instantiation.
        /// </summary>
        public SetName SetName {
            get {
                return setName;
            }
            private set {
                if (setName == null) {
                    setName = value;
                } else {
                    var msg = "The value of SetName may only be set once, on instantiation of a AttributeValue object.";
                    throw new AttributeValueException( msg );
                }
            }
        }

        /// <summary>
        /// httpStatus (leave this as string in case we get an unexpected status not in an enum?)
        /// </summary>
        [JsonProperty( Order = 1 )]
        public string StatusCode { get; set; } = string.Empty;

        [JsonConverter( typeof( StringEnumConverter ) )]
        public VisibilityOption Visibility { get; set; } = VisibilityOption.PRIVATE;

        [JsonConverter( typeof( StringEnumConverter ) )]
        public Helpers.AttributeValueActionEnums Action { get; set; } = Helpers.AttributeValueActionEnums.EMPTY;

        #region Definition

        /// <summary>
        /// Helper function, returnss a list of AttributeFields that are defined in the Attribute's definition.
        /// </summary>
        /// <returns>List<AttributeField> from AttributeDefinition</returns>
        public List<AttributeField> GetDefintionFields() {
            return definition.Fields;
        }

        /// <summary>
        /// Helper function, returns the AttributeFeild with name fieldName, from the Attribute's definition
        /// </summary>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        /// <exception cref="AttributeValueException"></exception>
        public AttributeField GetAttributeField( string fieldName ) {
            foreach (var field in definition.Fields ) {
                if (field.FieldName== fieldName) 
                    return field;
            }

            throw new AttributeValueException( $"Field name {fieldName} is not part of the definition for {definition.SetName}." );
        }

        /// <summary>
        /// Only one AttributeField within an ATTRIBUTE may have Key set to true.
        /// https://support.orionscoringsystem.com/definition-attributefield.html
        /// </summary>
        /// <returns></returns>
        public string GetDefinitionKeyFieldName() {
            AttributeField findKey = definition.Fields.Where( x => x.Key ).FirstOrDefault();
            if (findKey != null)
                return findKey.FieldName;
            else
                return String.Empty;
        }

        /// <summary>
        /// Get Field list of default fields
        /// </summary>
        /// <returns>List<AttributeField> from AttributeDefintion</AttributeField></returns>
        public List<AttributeField> GetDefinitionFieldsDefaultValues() {
            return definition.Fields.Where( x => x.DefaultValue.ToString() != string.Empty ).ToList();
        }

        /// <summary>
        /// Get list of Required Fields
        /// </summary>
        /// <returns>List<AttributeField> from AttributeDefinition</returns>
        public List<AttributeField> GetDefinitionRequiredFields() {
            return definition.Fields.Where( x => x.Required == true ).ToList();
        }

        /// <summary>
        /// Returns a boolean indicating if the Attribute Value is allowed to have multiple values. 
        /// This is defined within the Attribute's definition.
        /// </summary>
        /// <returns>true or false</returns>
        public bool IsMultipleValue {
            get {
                return definition.MultipleValues;
            }
        }

        /// <summary>
        /// Checks Field's Required flag from AttributeDefinition
        /// </summary>
        /// <param name="fieldName">Field Name string from GetDefintionFields()</param>
        /// <returns>true or false</returns>
        public bool? IsFieldNameRequired( string fieldName ) {
            return definition.RequiredAttributes.Contains( fieldName );
        }
        #endregion Definition

        #region Attribute

        /// <summary>
        /// Get a string list of Field Keys for SetName
        /// </summary>
        /// <returns>List<string> of Field Keys</returns>
        /// <exception cref="AttributeValueException"></exception>
        public List<string> GetAttributeFieldKeys() {
            try {
                return attributeValues.Keys.ToList();
            } catch (Exception ex) {
                throw new AttributeValueException( "Unable to return a list of Attribute Field Keys", ex, logger );
            }
        }

        /// <summary>
        /// Retrieve single Value then cast object type based on GetFieldType()
        /// </summary>
        /// <param name="FieldName">Valid FieldName as defined in AttributeDefintion</param>
        /// <returns>object to be type cast</returns>
        /// <exception cref="AttributeValueException">Thrown when the passed in fieldName is a multi-value field.</exception>
        public dynamic GetFieldValue( string fieldName ) {
            var returnValue = new object();
            if (this.IsMultipleValue)
                throw new AttributeValueException( $"Querying a single value for a the multi-value '{fieldName}' in {SetName}", logger );
            else
                returnValue = (attributeValues["AttributeList"].ContainsKey( fieldName )) ? attributeValues["AttributeList"][fieldName] : new object();

            return returnValue;
        }

        /// <summary>
        /// Get mulitple Values then cast object type based on GetFieldType()
        /// </summary>
        /// <param name="FieldName">Valid FieldName from GetAttributeDefintionFields()</param>
        /// <param name="FieldKey">Valid FieldKey string from GetFieldKeys()</param>
        /// <returns>object to be Type cast; null object if not found</returns>
        /// <exception cref="AttributeValueException">Thrown when the passed in fieldName is a multi-value field.</exception>
        public dynamic GetFieldValue( string fieldName, string fieldKey ) {
            dynamic returnValue = null;
            if (!this.IsMultipleValue) {
                throw new AttributeValueException( $"Querying a single value for a the multi-value '{fieldName}' with key '{fieldKey}' in {SetName}", logger );
            } else {
                returnValue = (attributeValues.ContainsKey( fieldKey ) && attributeValues[fieldKey].ContainsKey( fieldName )) ? attributeValues[fieldKey][fieldName] : null;
                if (returnValue == null)
                    throw new AttributeValueException( $"The field name '{fieldName}' with key '{fieldKey}' does not seem to exist in {SetName}", logger );
            }

            return returnValue;
        }

        /// <summary>
        /// Set Attribute Value for Field Name
        /// </summary>
        /// <param name="fieldName">Field Name to set</param>
        /// <param name="fieldValue">Field Value to set</param>
        /// <exception cref="AttributeValueException">Thrown if the user tries to call this function on a Attribute that has MultipleValues set to true. User should instead call the overloaded SetFieldValue with three parameters (including the fieldKey).</exception>
        /// <exception cref="AttributeValueValidationException">Thrown if the fieldValue is either the wrong type or the value does not pass validation.</exception>
        public void SetFieldValue( string fieldName, dynamic fieldValue ) {
            if (this.IsMultipleValue)
                throw new AttributeValueException( $"Field being set is designated MultipleValue needing Key. Use overload SetFieldName(string fieldName, object fieldValue, string fieldKey)", logger );

            AttributeField attributeField = GetAttributeField( fieldName );

            if (!attributeField.ValidateFieldValue( fieldValue )) {
                throw new AttributeValueValidationException( $"Invalid Set Field Value {fieldValue} for {fieldName}", logger );
            } else {
                //If the ValueType is a DATE, TIME, or DATETIIME, convert it to a string before storing.
                dynamic valueToStore = attributeField.SerializeFieldValue( fieldValue );

                if (!attributeValues.ContainsKey( "AttributeList" ))
                    attributeValues.Add( "AttributeList", new Dictionary<string, dynamic> { { fieldName, valueToStore } } );
                else {
                    if (attributeValues["AttributeList"].ContainsKey( fieldName ))
                        attributeValues["AttributeList"][fieldName] = valueToStore;
                    else
                        attributeValues["AttributeList"].Add( fieldName, valueToStore );
                }
            }
        }

        /// <summary>
        /// Set Attribute Value for Field Name with Field Key
        /// </summary>
        /// <param name="fieldName">Field Name to set</param>
        /// <param name="fieldValue">Field value to set</param>
        /// <param name="fieldKey">Field Key to set</param>
        /// <exception cref="AttributeValueException">Thrown if the user tries to call this function on a Attribute that has MultipleValues set to false. User should instead call the overloaded SetFieldValue with two parameters (not including the fieldKey).</exception>
        /// <exception cref="AttributeValueValidationException">Thrown if the fieldValue is either the wrong type or the value does not pass validation.</exception>
        public void SetFieldValue( string fieldName, object fieldValue, string fieldKey ) {
            if (!this.IsMultipleValue)
                throw new AttributeValueException( $"Field being set is designated SingleValue not accepting a Key. Use overload SetFieldName(string fieldName, object fieldValue)", logger );

            AttributeField attributeField = GetAttributeField( fieldName );

            if (!attributeField.ValidateFieldValue( fieldValue ))
                throw new AttributeValueValidationException( $"Invalid Set Field Value {fieldValue} for {fieldName}", logger );
            else {
                //If the ValueType is a DATE, TIME, or DATETIIME, convert it to a string before storing.
                dynamic valueToStore = attributeField.SerializeFieldValue( fieldValue );

                if (!attributeValues.ContainsKey( fieldKey ))
                    attributeValues.Add( fieldKey, new Dictionary<string, dynamic> { { fieldName, valueToStore } } );
                else {
                    if (attributeValues[fieldKey].ContainsKey( fieldName ))
                        attributeValues[fieldKey][fieldName] = valueToStore;
                    else
                        attributeValues[fieldKey].Add( fieldName, valueToStore );
                }
            }
        }

        /// <summary>
        /// Add Field Key for multiple = true
        /// </summary>
        /// <param name="fieldKey">Field Key to add</param>
        /// <exception cref="AttributeValueException"></exception>
        public void AddFieldKey( string fieldKeyValue ) {
            if (!attributeValues.ContainsKey( fieldKeyValue )) {
                if (GetDefinitionKeyFieldName() != String.Empty) {
                    attributeValues[fieldKeyValue] = new Dictionary<string, dynamic>();
                    SetDefaultFieldValues( fieldKeyValue );
                } else {
                    throw new AttributeValueException( $"No Key expected with {fieldKeyValue}", logger );
                }
            }
        }

        /// <summary>
        /// Delete record for Field Key
        /// </summary>
        /// <param name="fieldKey">Field Key to delete</param>
        public void DeleteFieldKey( string fieldKey ) {
            throw new NotImplementedException();
        }

        private void SetDefaultFieldValues( string keyField = "" ) {
            try {
                foreach (AttributeField fieldDefaults in GetDefinitionFieldsDefaultValues()) {
                    if (fieldDefaults.Required == true) {
                        if (keyField == string.Empty)
                            SetFieldValue( fieldDefaults.FieldName, fieldDefaults.DefaultValue );
                        else if (keyField != string.Empty)
                            SetFieldValue( fieldDefaults.FieldName, fieldDefaults.DefaultValue, keyField );
                    }
                }
            } catch (Exception ex) {
                throw new AttributeValueException( $"Error setting Default Values: {ex.Message}", ex, logger );
            }
        }
        #endregion Attribute

        public override string ToString() {
            StringBuilder foo = new StringBuilder();
            foo.Append( $"{this.SetName} Attribute Value" );
            return foo.ToString();
        }

        public JToken ToJToken() {
            
            JArray multiPartValues = new JArray();
            foreach (var multiPartValue in attributeValues.Values) {
                JObject attributeValueJObject = new JObject();
                foreach (var av in multiPartValue) {
                    attributeValueJObject.Add( av.Key, av.Value );
                }
                multiPartValues.Add( attributeValueJObject );
            }

            if ( this.IsMultipleValue) {
                return multiPartValues;
            } else {
                return multiPartValues[0];
            }

        }
    }
}