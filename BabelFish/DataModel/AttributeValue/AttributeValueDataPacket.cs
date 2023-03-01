using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Scopos.BabelFish.Converters;

namespace Scopos.BabelFish.DataModel.AttributeValue {

    [Serializable]
    [JsonConverter( typeof( AttributeValueDataPacketConverter ) )]
    public abstract class AttributeValueDataPacket : IDeserializableAbstractClass {

        public const int CONCRETE_CLASS_ID = 1;

        public AttributeValueDataPacket() {
            this.ConcreteClassId = CONCRETE_CLASS_ID;
        }

        /// <summary>
        /// the SetName, formatted as a string, of the Attribute definition.
        /// </summary>
        public string AttributeDef { get; set; }

        
        public AttributeValue AttributeValue { get; set; }

        public VisibilityOption Visibility { get; set; }

        /// <inheritdoc/>
        [Obsolete("Use JSON Customer Converters instead")]
        public JToken ToJToken() {
            
            JObject json = new JObject();
            json.Add( "attribute-def", AttributeDef.ToString() );
            json.Add( "visibility", Visibility.ToString() );
            json.Add( "attribute-value", AttributeValue.ToJToken() );

            return json;
        }

        /// <summary>
        /// Implementation of the IDeserializableAbstractClass interface.
        /// To have added control over the Deserialization of abstract classes, in to
        /// Concrete classes, the JSON should include a ConcreteClassId that specifies
        /// the Concrete class.
        /// </summary>
        public int ConcreteClassId { get; set; }
    }
}
