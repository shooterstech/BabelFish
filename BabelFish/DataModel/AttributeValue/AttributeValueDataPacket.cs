using Scopos.BabelFish.DataModel.AttributeValue;
using Scopos.BabelFish.DataModel.Definitions;
using System;
using System.Collections.Generic;
using System.Text;
using Scopos.BabelFish.DataModel;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.ComponentModel;
using System.Runtime.Serialization;
using Scopos.BabelFish.Helpers;

namespace Scopos.BabelFish.DataModel.AttributeValue {
    public abstract class AttributeValueDataPacket : IJToken {

        public AttributeValueDataPacket() { }

        /// <summary>
        /// the SetName, formatted as a string, of the Attribute definition.
        /// </summary>
        public string AttributeDef { get; set; }

        [JsonIgnore]
        public AttributeValue AttributeValue { get; set; }

        public VisibilityOption Visibility { get; set; }

        /// <inheritdoc/>
        public JToken ToJToken() {
            
            JObject json = new JObject();
            json.Add( "attribute-def", AttributeDef.ToString() );
            json.Add( "visibility", Visibility.ToString() );
            json.Add( "attribute-value", AttributeValue.ToJToken() );

            return json;
        }
    }
}
