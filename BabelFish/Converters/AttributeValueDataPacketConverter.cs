using System;
using System.Collections.Generic;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using Scopos.BabelFish.DataModel.AttributeValue;
using Scopos.BabelFish.DataModel.OrionMatch;
using Scopos.BabelFish.Responses.AttributeValueAPI;

namespace Scopos.BabelFish.Converters {

    /// <summary>
    /// Custom converter for AttributeValues. 
    /// Which is needed because AttributeValues have a dynamic structure.
    /// </summary>
    public class AttributeValueDataPacketConverter : JsonConverter {

        static JsonSerializerSettings SpecifiedSubclassConversion = new JsonSerializerSettings() { ContractResolver = new AttributeValueDataPacketSpecifiedConcreteClassConverter() };

        /// <inheritdoc/>
        public override bool CanConvert( Type objectType ) {
            return ( objectType == typeof( AttributeValue ) );
        }

        /// <inheritdoc/>
        public override void WriteJson( JsonWriter writer, object? value, JsonSerializer serializer ) {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override object? ReadJson( JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer ) {

            JObject jo = JObject.Load( reader );

            //first try using the ConcreteClassId, if it is a property of the json, as this will be a faster method.
            var id = jo["ConcreteClassId"]?.Value<int>();

            AttributeValueDataPacket attributeValueDataPacket;

            switch (id) {
                case AttributeValueDataPacketMatch.CONCRETE_CLASS_ID:
                    attributeValueDataPacket = new AttributeValueDataPacketMatch();

                    if (jo.ContainsKey("ReentryTag"))
                        ((AttributeValueDataPacketMatch)attributeValueDataPacket).ReentryTag = jo["ReentryTag"]?.Value<string>();
                    break;

                case AttributeValueDataPacketAPIResponse.CONCRETE_CLASS_ID:
                default:
                    attributeValueDataPacket = new AttributeValueDataPacketAPIResponse();

                    ((AttributeValueDataPacketAPIResponse)attributeValueDataPacket).StatusCode = (HttpStatusCode)Enum.Parse( typeof( HttpStatusCode ), (string)jo["StatusCode"] );
                    ((AttributeValueDataPacketAPIResponse)attributeValueDataPacket).Message = ( string)jo.GetValue( "Message" )[0];
                    break;
            }

            attributeValueDataPacket.AttributeDef = (string)jo.GetValue( "AttributeDef" );
            attributeValueDataPacket.AttributeValue = new AttributeValue( attributeValueDataPacket.AttributeDef, jo.GetValue( "AttributeValue" ) );
            attributeValueDataPacket.Visibility = (VisibilityOption)Enum.Parse( typeof( VisibilityOption ), (string)jo["Visibility"] );

            return attributeValueDataPacket;
        }
    }

    public class AttributeValueDataPacketSpecifiedConcreteClassConverter : DefaultContractResolver {
        protected override JsonConverter ResolveContractConverter( Type objectType ) {
            if (typeof( AttributeValue ).IsAssignableFrom( objectType ) && !objectType.IsAbstract)
                return null; // pretend TableSortRuleConvert is not specified (thus avoiding a stack overflow)
            return base.ResolveContractConverter( objectType );
        }
    }
}
