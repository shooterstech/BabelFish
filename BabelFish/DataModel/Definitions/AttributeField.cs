using System.ComponentModel;

namespace Scopos.BabelFish.DataModel.Definitions {

    /// <summary>
    /// An AttributeField describes one field of an Attribute
    /// </summary>
    [Serializable]
    public abstract class AttributeField<T>: AttributeFieldBase {

        /// <summary>
        /// Public constructor.
        /// </summary>
        public AttributeField() : base() {

        }

        /// <summary>
        /// Returns a boolean indicating if the passed in value passes all validation tests for thei field.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public abstract bool ValidateFieldValue( T value );

        /// <summary>
        /// This is a helper function to return the value of .DefaultValue as a dynamic.
        /// </summary>
        /// <returns></returns>
        public abstract T GetDefaultValue();

        //internal abstract dynamic DeserializeFromJsonElement( G_STJ.JsonElement value );

        public override dynamic BaseGetDefaultValue() {
            return this.GetDefaultValue();
        }

        public override bool BaseValidateFieldValue( dynamic value ) {
            if (value is not T)
                return false;

            return this.ValidateFieldValue( (T)value );
        }

    }

    /// <summary>
    /// Common abstract class for all AttributeField generic classes. Primarly only exists to allow for Deserialization
    /// </summary>
    public abstract class AttributeFieldBase: IReconfigurableRulebookObject {

        protected Logger Logger = LogManager.GetCurrentClassLogger();

        public AttributeFieldBase() {
            Required = false;
            Key = false;
        }

        /// <summary>
        /// Name given to this field. It is unique within the parent ATTRIBUTE.
        /// </summary>
        [G_NS.JsonProperty( Order = 1 )]
        public string FieldName { get; set; } = string.Empty;

        private string displayName = string.Empty;

        /// <summary>
        /// Human readable name for the field. This is the value that is displayed to users in a form 
        /// entering ATTRIBUTE VALUES. In a Simple Attribute, it is customarily the same value as 
        /// the parent's (ATTRIBUTE's) DisplayName.
        /// </summary>
        [G_NS.JsonProperty( Order = 2 )]
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
        /// The type of data that this field will hold.
        /// </summary>
        [G_NS.JsonProperty( Order = 3, DefaultValueHandling = G_NS.DefaultValueHandling.Include )]
        public ValueType ValueType { get; protected set; } = ValueType.STRING;

        /// <summary>
        /// Human readable description of what this feild represents. 
        /// </summary>
        [G_NS.JsonProperty( Order = 4 )]
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// True if the user may enter multiple values in this field (in other words, its a list). 
        /// False if the user may only enter one value.
        /// </summary>
        [G_NS.JsonProperty( Order = 5, DefaultValueHandling = G_NS.DefaultValueHandling.Include )]
        [DefaultValue( false )]
        public bool MultipleValues { get; protected set; } = false;

        /// <summary>
        /// In an ATTRIBUTE that has MultipleValues set to true, Key determines the unique identifier in the list. 
        /// Exactly one AttributeField within an ATTRIBUTE must have Key set to true.
        /// </summary>
        [G_NS.JsonProperty( Order = 21 )]
        public bool Key { get; set; } = false;

		/// <summary>
		/// True if the user is required to enter a value. False if the user desn't have to. If the user doesn't have to, then the DefaultValue is applied.
		/// </summary>
		[G_NS.JsonProperty( Order = 22 )]
		public bool Required { get; set; } = false;

		/// <inheritdoc/>
		[G_NS.JsonProperty( Order = 99, DefaultValueHandling = G_NS.DefaultValueHandling.Ignore )]
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

        internal abstract dynamic DeserializeFromJsonElement( G_STJ.JsonElement value );

        public abstract dynamic BaseGetDefaultValue();

        public abstract bool BaseValidateFieldValue( dynamic value );
    }
}
