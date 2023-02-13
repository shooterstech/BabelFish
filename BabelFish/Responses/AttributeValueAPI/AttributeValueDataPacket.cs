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

namespace Scopos.BabelFish.Responses.AttributeValueAPI {
    public class AttributeValueDataPacket : IJToken {

        public AttributeValueDataPacket() { }

        public System.Net.HttpStatusCode StatusCode { get; set; }

        public SetName SetName { get; set; }

        public AttributeValue AttributeValue { get; set; }

        public VisibilityOption Visibility { get; set; }

        public string Message { get; set; }

        /// <inheritdoc/>
        public JToken ToJToken() {
            
            JObject json = new JObject();
            json.Add( "attribute-def", SetName.ToString() );
            json.Add( "visibility", Visibility.ToString() );
            json.Add( "attribute-value", AttributeValue.ToJToken() );

            return json;
        }
    }
}
