using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using Scopos.BabelFish.Helpers;
using NLog;
using Scopos.BabelFish.DataModel.AttributeValue;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace Scopos.BabelFish.DataModel.Definitions {

    /// <summary>
    /// An AttributeField describes one field of an Attribute
    /// </summary>
    [Serializable]
    public abstract class AttributeField: IReconfigurableRulebookObject {

        protected Logger Logger= LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Public constructor.
        /// </summary>
        public AttributeField() {
            Required = false;
            Key = false;
        }

        /// <summary>
        /// Name given to this field. It is unique within the parent ATTRIBUTE.
        /// </summary>
        public string FieldName { get; set; } = string.Empty;

        private string displayName = string.Empty;

        /// <summary>
        /// Human readable name for the field. This is the value that is displayed to users in a form 
        /// entering ATTRIBUTE VALUES. In a Simple Attribute, it is customarily the same value as 
        /// the parent's (ATTRIBUTE's) DisplayName.
        /// </summary>
        public string DisplayName {
            get {
                if (string.IsNullOrEmpty( displayName )) {
                    return this.FieldName;
                } else {
                    return displayName;
                }
            }

            set {
                displayName = value;
            }
        }

        /// <summary>
        /// True if the user may enter multiple values in this field (in other words, its a list). 
        /// False if the user may only enter one value.
        /// </summary>
        [DefaultValue( false )]
        [JsonInclude]
        public bool MultipleValues { get; protected set; } = false;

        /// <summary>
        /// True if the user is required to enter a value. False if the user desn't have to. If the user doesn't have to, then the DefaultValue is applied.
        /// </summary>
        public bool Required { get; set; } = false;

        /// <summary>
        /// In an ATTRIBUTE that has MultipleValues set to true, Key determines the unique identifier in the list. 
        /// Exactly one AttributeField within an ATTRIBUTE must have Key set to true.
        /// </summary>
        public bool Key { get; set; } = false;

        /// <summary>
        /// Human readable description of what this feild represents. 
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Validation rule that all must be true in order for the value to pass validation.
        /// </summary>
        public abstract AttributeValidation Validation { get; set; }

        /// <summary>
        /// Returns a boolean indicating if the passed in value passes all validation tests for thei field.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual bool ValidateFieldValue( dynamic value ) {
            //TODO: Actually validate the field value
            return true;
        }

        /// <summary>
        /// This is a helper function to return the value of .DefaultValue as a dynamic.
        /// </summary>
        /// <returns></returns>
        public abstract dynamic GetDefaultValue();

        /// <summary>
        /// The type of data that this field will hold.
        /// </summary>
        [JsonInclude]
        public ValueType ValueType { get; protected set; } = ValueType.STRING;

        /// <inheritdoc/>
        [JsonIgnore( Condition = JsonIgnoreCondition.WhenWritingDefault )]
        [DefaultValue( "" )]
        public string Comment { get; set; } = string.Empty;

        /// <inheritdoc/>
        public override string ToString() {
            if (MultipleValues)
                return $"'{FieldName}' of list of type {ValueType}";
            else if (Key)
                return $"'{FieldName}' of type {ValueType} KEY VALUE";
            else
                return $"'{FieldName}' of type {ValueType}";
        }

        internal abstract dynamic DeserializeFromJsonElement( JsonElement value ); 

    }
}
