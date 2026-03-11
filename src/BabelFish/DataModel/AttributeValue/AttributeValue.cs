using System.Text.Json;
using Scopos.BabelFish.APIClients;
using Scopos.BabelFish.DataModel.Definitions;

namespace Scopos.BabelFish.DataModel.AttributeValue {

    /// <summary>
    /// An ATTRIBUTE VALUE stores the values for a given ATTRIBUTE for one Individual, Participant, or Club.
    /// <para>Programmatically, an ATTRIBUTE VALUE are dictionaries that store name (key) value pairs, containing the data
    /// from an <see cref="Scopos.BabelFish.DataModel.Definitions.Attribute">ATTRIBUTE</see>.</para>
    /// <para></para>It is generally best to construct a new instance using the <see cref="CreateAsync(SetName)"/> method.</para>
    /// </summary>
    [Serializable]
    public class AttributeValue : IEquatable<AttributeValue> {

        private Logger _logger = LogManager.GetCurrentClassLogger();

        private Dictionary<string, Dictionary<string, dynamic>> _attributeValues = new Dictionary<string, Dictionary<string, dynamic>>();
        private SetName _setName = null;
        internal const string KEY_FOR_SINGLE_ATTRIBUTES = "Single-Value-Attribute-45861567"; //Intended to be random that no one would use it for a key value.

        private Scopos.BabelFish.DataModel.Definitions.Attribute definition = null;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="setName"></param>
        private AttributeValue( SetName setName ) {

            this.SetName = setName;
        }

        public static async Task<AttributeValue> CreateAsync( SetName setName ) {
            AttributeValue av = new AttributeValue( setName );
            await av.InitializeAsync();

            return av;
        }

        /// <exception cref="AttributeNotFoundException">Thrown if the attribute def, identified by the SetName, could not be found.</exception>
        public static async Task<AttributeValue> CreateAsync( SetName setName, JsonElement attributeValueAsJsonElement ) {
            AttributeValue av = new AttributeValue( setName );
            await av.InitializeAsync();

            // attributeValueAsJsonElement should either be 
            // Array of Objects
            // Object
            if (av.definition.MultipleValues) {
                foreach (var avAsJsonElement in attributeValueAsJsonElement.EnumerateArray()) {
                    av.ParseJsonElement( avAsJsonElement );
                }
            } else {
                av.ParseJsonElement( attributeValueAsJsonElement );
            }

            return av;
        }

        /// <summary>
        /// Should be called after the constructor, to complete the async portion of the constructor process. 
        /// </summary>
        /// <returns></returns>
        /// <exception cref="AttributeNotFoundException">Thrown if the attribute def, identified by the SetName, could not be found.</exception>
        /// <exception cref="XApiKeyNotSetException">Thrown if the Settings.XApiKey value has not been set.</exception>
        private async Task InitializeAsync() {

            //var getDefinitionResponse = await DefinitionCache.GetAttributeDefinitionAsync( SetName );
            definition = await DefinitionCache.GetAttributeDefinitionAsync( SetName );

            SetDefaultFieldValues();
        }

        private void ParseJsonElement( JsonElement attributeValueAsJsonElement ) {

            JsonElement temp;

            var keyFieldName = GetDefinitionKeyFieldName();
            var keyFieldValue = "";
            if (!string.IsNullOrEmpty( keyFieldName )) {
                keyFieldValue = attributeValueAsJsonElement.GetProperty( keyFieldName ).GetString();
            }

            foreach (var field in definition.Fields) {
                var fieldName = field.FieldName;
                if (attributeValueAsJsonElement.TryGetProperty( fieldName, out temp )) {
                    dynamic fieldValue = field.DeserializeFromJsonElement( temp );

                    if (definition.MultipleValues) {
                        this.SetFieldValue( fieldName, fieldValue, keyFieldValue );
                    } else {
                        this.SetFieldValue( fieldName, fieldValue );
                    }
                }
                //If the fieldName is not part of what we are deserializing, then the startegy is to set the value
                //to the default value. Don't need to explicitly do that here, since when the user calls GetValue()
                //if a value is not set, it returns the default then.

            }
        }

        /// <summary>
        /// View the SetName of the AttributeValue.
        /// Assignment is done at instantiation.
        /// </summary>
        public SetName SetName {
            get {
                return _setName;
            }
            private set {
                if (_setName == null) {
                    _setName = value;
                } else {
                    var msg = "The value of SetName may only be set once, on instantiation of a AttributeValue object.";
                    throw new AttributeValueException( msg );
                }
            }
        }

        #region Definition

        /// <summary>
        /// Returns a copy of the Attributre that defines this Attribute Value
        /// </summary>
        public Scopos.BabelFish.DataModel.Definitions.Attribute Attribute { get { return definition; } }

        /// <summary>
        /// Helper function, returns a list of AttributeFields that are defined in the Attribute's definition.
        /// </summary>
        /// <returns>List<AttributeField> from AttributeDefinition</returns>
        public List<AttributeFieldBase> GetDefintionFields() {
            return definition.Fields;
        }

        /// <summary>
        /// Helper function, returns the AttributeFeild with name fieldName, from the Attribute's definition
        /// </summary>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        /// <exception cref="AttributeValueException">Thrown if the fieldName is not defined by the Attribute.</exception>
        public AttributeFieldBase GetAttributeField( string fieldName ) {
            foreach (var field in definition.Fields) {
                if (field.FieldName == fieldName)
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
            AttributeFieldBase findKey = definition.Fields.Where( x => x.Key ).FirstOrDefault();
            if (findKey != null)
                return findKey.FieldName;
            else
                return String.Empty;
        }

        /// <summary>
        /// Get list of AttributeFields that are Required
        /// </summary>
        /// <returns>List<AttributeField> from AttributeDefinition</returns>
        public List<AttributeFieldBase> GetDefinitionRequiredFields() {
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
        /// Helper property to return the Attribute definition for this AttributeValue. 
        /// </summary>
        [G_NS.JsonIgnore]
        protected internal Definitions.Attribute Definition {
            get {
                return definition;
            }

        }
        #endregion Definition

        #region Attribute

        /// <summary>
        /// Returns this Attribute Value's current field keys. Each one of these field keys may be used 
        /// as part of the .GetFieldValue() or .SetFieldValue() calls.
        /// </summary>
        /// <returns>List<string> of Field Keys</returns>
        /// <exception cref="AttributeValueException"></exception>
        public List<string> GetAttributeFieldKeys() {
            try {
                return _attributeValues.Keys.ToList();
            } catch (Exception ex) {
                throw new AttributeValueException( "Unable to return a list of Attribute Field Keys", ex, _logger );
            }
        }

        /// <summary>
        /// Intended for Attributes with MultipleValues set to false (not a list).
        /// Retreives the field value based on the passed in fieldName. If fieldName has not yet been set, the default value for the feild is returned.
        /// Caller is responsible for casting to the approrpirate .NET type.
        /// </summary>
        /// <param name="fieldName">Valid FieldName as defined in AttributeDefintion</param>
        /// <returns>object to be type cast</returns>
        /// <exception cref="AttributeValueException">Thrown when the passed in fieldName is unknown, or the Attribute is a multi-value attribute.</exception>
        public dynamic GetFieldValue( string fieldName ) {
            AttributeFieldBase attributeField = GetAttributeField( fieldName );

            if (this.IsMultipleValue) {
                throw new AttributeValueException( $"Querying a single value for a the multi-value '{fieldName}' in {SetName}", _logger );
            } else {
                if (_attributeValues[KEY_FOR_SINGLE_ATTRIBUTES].ContainsKey( fieldName ))
                    return _attributeValues[KEY_FOR_SINGLE_ATTRIBUTES][fieldName];
            }

            return attributeField.BaseGetDefaultValue();
        }

        /// <summary>
        /// Intended for Attributes with MultipleValues set to true (is a list).
        /// Retreives the field value based on the passed in fieldName and fieldKey. If fieldName has not yet been set, or the fieldKey is not yet used, the default value for the feild is returned.
        /// Caller is responsible for casting to the approrpirate .NET type.
        /// </summary>
        /// <param name="fieldName">Valid FieldName from GetAttributeDefintionFields()</param>
        /// <param name="fieldKey">Valid FieldKey string from GetFieldKeys()</param>
        /// <returns>object to be Type cast; null object if not found</returns>
        /// <exception cref="AttributeValueException">Thrown when the passed in fieldName is unknown, or the Attribute is not a multi-value attribute.</exception>
        public dynamic GetFieldValue( string fieldName, string fieldKey ) {
            AttributeFieldBase attributeField = GetAttributeField( fieldName );

            if (!this.IsMultipleValue) {
                throw new AttributeValueException( $"Querying a single value for a the multi-value '{fieldName}' with key '{fieldKey}' in {SetName}", _logger );
            } else {
                if (_attributeValues.ContainsKey( fieldKey ) && _attributeValues[fieldKey].ContainsKey( fieldName ))
                    return _attributeValues[fieldKey][fieldName];
            }

            return attributeField.BaseGetDefaultValue();
        }

        /// <summary>
        /// Special case for returning a field value when the Attribute is a Simple Attribute.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="ArgumentException">Thrown when the Attribute is not a simple attribute.</exception>
        public dynamic GetFieldValue() {

            if (this.definition.SimpleAttribute) {
                var firstField = this.definition.Fields[0];
                return this.GetFieldValue( firstField.FieldName );
            }

            throw new ArgumentException( "Can not call .GetFieldValue() (without arguments) unless the Attribute is a Simple Attribute. " );
        }

        /// <summary>
        /// Special case for returning a field's .Name property when the Attribute is a Simple Attribute.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="ArgumentException">Thrown when the Attribute is not a simple attribute.</exception>
        public dynamic GetFieldName() {

            if (this.definition.SimpleAttribute) {
                var firstField = this.definition.Fields[0];
                var fieldValue = this.GetFieldValue( firstField.FieldName );
                if (firstField is AttributeFieldString afs) {
                    foreach (var opt in afs.Values) {
                        if (opt.Value == fieldValue) {
                            return opt.Name;
                        }
                    }
                }

                return fieldValue;
            }

            throw new ArgumentException( "Can not call .GetFieldValue() (without arguments) unless the Attribute is a Simple Attribute. " );
        }

        /// <summary>
        /// If applicable, returns the AttributeValueAppellation for this AttributeValue.
        /// Only applicable if the underlying definition is a simple attribute, and the field
        /// type is CLOSED, and the AttributeField is a string
        /// </summary>
        public string AttributeValueAppellation {
            get {
                try {
                    if (definition.SimpleAttribute) {
                        var @field = GetDefintionFields()[0];
                        if (@field is AttributeFieldString) {
                            AttributeFieldString attributeFieldString = (AttributeFieldString)@field;
                            var value = GetFieldValue( @field.FieldName );
                            foreach (var foo in attributeFieldString.Values) {
                                if (foo.Value == value) {
                                    return foo.AttributeValueAppellation;
                                }
                            }
                        }
                        return "";
                    } else {
                        return "";
                    }
                } catch (Exception e) {
                    _logger.Error( e );
                    return "";
                }
            }
        }

        /// <summary>
        /// Special case for setting the field value on a Simple Attribute. Throws an exception if the Attribute is not simple.
        /// </summary>
        /// <param name="fieldValue"></param>
        /// <exception cref="ArgumentException">Thrown when the Attribute is not a Simple Attribute.</exception>
        public void SetFieldValue( dynamic fieldValue ) {


            if (this.definition.SimpleAttribute) {
                var firstField = this.definition.Fields[0];
                this.SetFieldValue( firstField.FieldName, fieldValue );
                return;
            }

            throw new ArgumentException( "Can not call .SetFieldValue() (without arguments) unless the Attribute is a Simple Attribute. " );
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
                throw new AttributeValueException( $"Field being set is designated MultipleValue needing Key. Use overload SetFieldName(string fieldName, object fieldValue, string fieldKey)", _logger );

            AttributeFieldBase attributeField = GetAttributeField( fieldName );

            if (!attributeField.BaseValidateFieldValue( fieldValue )) {
                throw new AttributeValueValidationException( $"Invalid Set Field Value {fieldValue} for {fieldName}", _logger );
            } else {

                SetDefaultFieldValues( KEY_FOR_SINGLE_ATTRIBUTES );
                if (_attributeValues[KEY_FOR_SINGLE_ATTRIBUTES].ContainsKey( fieldName ))
                    _attributeValues[KEY_FOR_SINGLE_ATTRIBUTES][fieldName] = fieldValue;
                else
                    _attributeValues[KEY_FOR_SINGLE_ATTRIBUTES].Add( fieldName, fieldValue );
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
                throw new AttributeValueException( $"Field being set is designated SingleValue not accepting a Key. Use overload SetFieldName(string fieldName, object fieldValue)", _logger );

            AttributeFieldBase attributeField = GetAttributeField( fieldName );


            if (fieldKey == KEY_FOR_SINGLE_ATTRIBUTES) {
                _logger.Warn( $"Trying to set Attribute Value for '{definition.CommonName}'  for field '{fieldName}' with value '{fieldValue}' and key '{fieldKey}.' However, this is the special use field key for non-MultipleValue attribute value. Will be skipping setting it." );
            } else if (!attributeField.BaseValidateFieldValue( fieldValue ))
                throw new AttributeValueValidationException( $"Invalid Set Field Value {fieldValue} for {fieldName}", _logger );
            else {

                SetDefaultFieldValues( fieldKey );
                if (_attributeValues[fieldKey].ContainsKey( fieldName ))
                    _attributeValues[fieldKey][fieldName] = fieldValue;
                else
                    _attributeValues[fieldKey].Add( fieldName, fieldValue );
            }
        }

        /// <summary>
        /// Add Field Key for multiple = true
        /// </summary>
        /// <param name="fieldKey">Field Key to add</param>
        /// <exception cref="AttributeValueException"></exception>
        public void AddFieldKey( string fieldKeyValue ) {
            if (!_attributeValues.ContainsKey( fieldKeyValue )) {
                if (GetDefinitionKeyFieldName() != String.Empty) {
                    _attributeValues[fieldKeyValue] = new Dictionary<string, dynamic>();
                    SetDefaultFieldValues( fieldKeyValue );
                } else {
                    throw new AttributeValueException( $"No Key expected with {fieldKeyValue}", _logger );
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

        /// <summary>
        /// Calling SetDefaultFieldValue will add the passed in keyField to attributeValues
        /// and set each field to its default value.
        /// If keyField is already a key in attributeValues, no action is taken.
        /// </summary>
        /// <param name="keyField"></param>
        /// <exception cref="AttributeValueException"></exception>
        private void SetDefaultFieldValues( string keyField = KEY_FOR_SINGLE_ATTRIBUTES ) {
            try {
                //Check for the special case that this is for an Attribute with MultipleValues and the 
                //key field is a sepcial key for SingleValue. Meaning, default values shouldn't be set.
                if (definition.MultipleValues && keyField == KEY_FOR_SINGLE_ATTRIBUTES)
                    return;

                if (!_attributeValues.ContainsKey( keyField )) {
                    _attributeValues[keyField] = new Dictionary<string, dynamic>();

                    foreach (AttributeFieldBase field in definition.Fields) {

                        if (field.Key)
                            SetFieldValue( field.FieldName, keyField, keyField );
                        else if (definition.MultipleValues)
                            SetFieldValue( field.FieldName, field.BaseGetDefaultValue(), keyField );
                        else
                            SetFieldValue( field.FieldName, field.BaseGetDefaultValue() );
                    }
                }
            } catch (Exception ex) {
                throw new AttributeValueException( $"Error setting Default Values: {ex.Message}", ex, _logger );
            }
        }
        #endregion Attribute

        /// <inheritdoc />
        public override string ToString() {
            return $"{this.SetName} Attribute Value";
        }

        /// <summary>
        /// Two AttributeValue objects are considered equal if they have the same SetName and the same field values for each field defined in the Attribute definition.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals( object obj ) {
            if (obj is AttributeValue av) {
                return Equals( av );
            }
            return false;
        }

        /// <inheritdoc />
        public override int GetHashCode() {
            int hash = _setName.GetHashCode();
            foreach (var key in _attributeValues) {
                hash ^= key.GetHashCode();
                foreach (var field in key.Value) {
                    hash ^= field.Key.GetHashCode();
                    if (field.Value != null)
                        hash ^= field.Value.GetHashCode();
                }
            }

            return hash;
        }

        /// <summary>
        /// Two AttributeValue objects are considered equal if they have the same SetName and the same field values for each field defined in the Attribute definition.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public bool Equals( AttributeValue other ) {

            //Start with a simple check that the setnames are equal, and that the number of field keys are the same. If either of those are not true, then the AttributeValues can't be equal.
            if (!this._setName.Equals( other._setName )
                || this._attributeValues.Count != other._attributeValues.Count) {
                return false;
            }

            if (this.Attribute.MultipleValues) {
                throw new NotImplementedException( "Equals() method for MultipleValues attributes has not yet been implemented." );
            }

            foreach (var field in this.GetDefintionFields()) {
                if (field.MultipleValues) {
                    throw new NotImplementedException( "Equals() method for MultipleValues fields has not yet been implemented." );
                }

                var fieldName = field.FieldName;
                var thisValue = this.GetFieldValue( fieldName );
                var otherValue = other.GetFieldValue( fieldName );
                if (thisValue != otherValue) {
                    return false;
                }
            }

            return true;

        }
    }
}
