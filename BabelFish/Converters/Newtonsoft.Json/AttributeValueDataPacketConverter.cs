using System;
using System.Collections.Generic;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using Scopos.BabelFish.DataModel.AttributeValue;
using Scopos.BabelFish.DataModel.OrionMatch;
using Scopos.BabelFish.Responses.AttributeValueAPI;
using Scopos.BabelFish.DataModel.Definitions;
using NLog;
using Scopos.BabelFish.DataModel.Common;

namespace Scopos.BabelFish.Converters.Newtonsoft {

    /// <summary>
    /// Custom converter for AttributeValues. 
    /// Which is needed because AttributeValues have a dynamic structure.
    /// </summary>
    public class AttributeValueDataPacketConverter : JsonConverter {

        private Logger logger = LogManager.GetCurrentClassLogger();

        static JsonSerializerSettings SpecifiedSubclassConversion = new JsonSerializerSettings() { ContractResolver = new AttributeValueDataPacketSpecifiedConcreteClassConverter() };

        /// <inheritdoc/>
        public override bool CanConvert( Type objectType ) {
            return (objectType == typeof( AttributeValue ));
        }

        public override bool CanWrite { get { return true; } }

        //Reading should be handled by System.Text.Json, not by Newtonsoft
        public override bool CanRead {  get { return false; } }

        /// <inheritdoc/>
        public override void WriteJson( JsonWriter writer, object? value, JsonSerializer serializer ) {
            /*
            serializer.Converters.Remove( this );
            JToken jToken = JToken.FromObject( value, serializer );
            serializer.Converters.Add( this );
            */

            var attrValueDataPacket = (AttributeValueDataPacket)value;
            JObject o = new JObject();

            o["AttributeDef"] = attrValueDataPacket.AttributeDef.ToString();
            o["Visibility"] = attrValueDataPacket.Visibility.ToString();
            o["ConcreteClassId"] = attrValueDataPacket.ConcreteClassId;
            o["AttributeValue"] = new JObject();

            if (attrValueDataPacket.AttributeValue != null) {
                foreach (var field in attrValueDataPacket.AttributeValue.GetDefintionFields()) {
                    o["AttributeValue"][field.FieldName] = attrValueDataPacket.AttributeValue.GetFieldValue( field.FieldName );
                }
            }
            
            o.WriteTo( writer );
        }

        /// <inheritdoc/>
        public override object? ReadJson( JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer ) {
            throw new NotImplementedException( "Reading an AttrtirbuteValueDataPacket Json should be done using System.Text.Json" );
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
