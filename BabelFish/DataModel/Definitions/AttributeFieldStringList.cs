using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Scopos.BabelFish.Converters;


namespace Scopos.BabelFish.DataModel.Definitions {
    public class AttributeFieldStringList : AttributeField, ICopy<AttributeFieldStringList> {

        /// <summary>
        /// Public default constructor
        /// </summary>
        public AttributeFieldStringList() {
            MultipleValues = true;
            ValueType = ValueType.STRING;
        }

        /// <inheritdoc />
        public AttributeFieldStringList Copy() {
            
            var copy = new AttributeFieldStringList();
            base.Copy(copy);

            copy.FieldType = this.FieldType;
            foreach( var option in this.Values ) 
                copy.Values.Add( option.Copy() );

            return copy;
        }

        /// <summary>
        /// The default value for this field, which is always a empty list.
        /// </summary>
        public List<string> DefaultValue { get; private set; } = new List<string>();


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
