using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Scopos.BabelFish.Converters;


namespace Scopos.BabelFish.DataModel.Definitions {
    public class AttributeFieldString : AttributeField, ICopy<AttributeFieldString> {

        /// <summary>
        /// Public default constructor
        /// </summary>
        public AttributeFieldString() {
            MultipleValues = false;
            ValueType = ValueType.STRING;
        }

        /// <inheritdoc />
        public AttributeFieldString Copy() {
            
            var copy = new AttributeFieldString();
            base.Copy(copy);

            copy.DefaultValue = this.DefaultValue;
            copy.FieldType = this.FieldType;
            foreach( var option in this.Values ) 
                copy.Values.Add( option.Copy() );

            return copy;
        }

        /// <summary>
        /// The default value for this field. It is the value assigned to the field if the user does not enter one.
        /// </summary>
        public string DefaultValue { get; set; } = string.Empty;


        /// <summary>
        /// Indicates if the value of the attribute must be chosen from a list, 
        /// may be any value, of the there is a suggested list of values.
        /// </summary>
        [DefaultValue( FieldType.OPEN )]
        public FieldType FieldType { get; protected set; } = FieldType.OPEN;

        /// <summary>
        /// List of possible values, when FieldType is CLOSED or SUGGEST
        /// </summary>
        public List<AttributeValueOption> Values { get; set; } = new List<AttributeValueOption>();
    }
}
