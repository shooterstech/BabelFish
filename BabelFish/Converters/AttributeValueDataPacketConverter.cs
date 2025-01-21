using System;
using System.Collections.Generic;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using Scopos.BabelFish.DataModel.AttributeValue;
using Scopos.BabelFish.DataModel.OrionMatch;
using Scopos.BabelFish.Responses.AttributeValueAPI;
using Scopos.BabelFish.DataModel.Definitions;
using NLog;
using Scopos.BabelFish.DataModel.Common;

namespace Scopos.BabelFish.Converters
{

    /// <summary>
    /// Custom converter for AttributeValues. 
    /// Which is needed because AttributeValues have a dynamic structure.
    /// </summary>
    public class AttributeValueDataPacketConverter : JsonConverter<AttributeValueDataPacket> {

        private Logger logger = LogManager.GetCurrentClassLogger();

        public override AttributeValueDataPacket? Read( ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options ) {
            if (reader.TokenType != JsonTokenType.StartObject)
                throw new JsonException(); 
            
            JsonElement temp;

            using (JsonDocument doc = JsonDocument.ParseValue( ref reader )) {
                JsonElement root = doc.RootElement;

                int id = 0;
                try {
                    id = root.GetProperty( "ConcreteClassId" ).GetInt32();
                } catch (KeyNotFoundException) {
                    //On some older serializations, the ConcreteClassId was not included. Infer the value based on what else is in the json
                    if (root.TryGetProperty( "StatusCode", out temp) )
                        id = AttributeValueDataPacketAPIResponse.CONCRETE_CLASS_ID;
                    else if (root.TryGetProperty( "StatusCode", out temp ))
                        id = AttributeValueDataPacketAPIResponse.CONCRETE_CLASS_ID;
                    else
                        id = AttributeValueDataPacketMatch.CONCRETE_CLASS_ID;
                }

                AttributeValueDataPacket attributeValueDataPacket;
                bool okToDeserialize = true;

                switch (id) {
                    case AttributeValueDataPacketMatch.CONCRETE_CLASS_ID:
                        attributeValueDataPacket = new AttributeValueDataPacketMatch();

                        if (root.TryGetProperty( "ReentryTag", out temp ))
                            ((AttributeValueDataPacketMatch)attributeValueDataPacket).ReentryTag = temp.GetString();
                        break;

                    case AttributeValueDataPacketAPIResponse.CONCRETE_CLASS_ID:
                    default:
                        attributeValueDataPacket = new AttributeValueDataPacketAPIResponse();

                        if (root.TryGetProperty( "StatusCode", out temp ))
                            ((AttributeValueDataPacketAPIResponse)attributeValueDataPacket).StatusCode = (HttpStatusCode)Enum.Parse( typeof( HttpStatusCode ), temp.GetInt32().ToString() );

                        //EKA Note Jan 2025 the Message property is deprecated, and will soon be removed.
                        if (root.TryGetProperty( "Message", out temp ) && temp.ValueKind == JsonValueKind.Array && temp.GetArrayLength() > 0 ) {
                            try {
                                ((AttributeValueDataPacketAPIResponse)attributeValueDataPacket).Message = temp[0].GetString();
                            } catch (Exception ex) {
                                logger.Error( ex, $"Unable to read the Message property." );
                            }
                        }

                        if (((AttributeValueDataPacketAPIResponse)attributeValueDataPacket).StatusCode != HttpStatusCode.OK) {
                            okToDeserialize = false;
                            logger.Info( $"Unable to deserialize, received message '{((AttributeValueDataPacketAPIResponse)attributeValueDataPacket).Message}'." );
                        }
                        break;
                }

                if (okToDeserialize) {
                    attributeValueDataPacket.AttributeDef = root.GetProperty( "AttributeDef" ).GetString();
                    attributeValueDataPacket.AttributeValueTask = AttributeValue.CreateAsync( SetName.Parse( attributeValueDataPacket.AttributeDef ), root.GetProperty( "AttributeValue" ) );
                    if (root.TryGetProperty( "Visibility", out temp ))
                        attributeValueDataPacket.Visibility = (VisibilityOption)Enum.Parse( typeof( VisibilityOption ), temp.GetString() );
                }

                return attributeValueDataPacket;
            }
        }

        public override void Write( Utf8JsonWriter writer, AttributeValueDataPacket value, JsonSerializerOptions options ) {

            writer.WriteStartObject();

            writer.WriteString( "AttributeDef", value.AttributeDef.ToString() );
            writer.WriteString( "Visibility", value.Visibility.ToString() );
            writer.WriteNumber( "ConcreteClassId", value.ConcreteClassId );
            writer.WritePropertyName( "AttributeValue" );
            if (value.AttributeValue.IsMultipleValue) {
                writer.WriteStartArray();
                foreach (var fieldKey in value.AttributeValue.GetAttributeFieldKeys()) {

                    if (fieldKey != AttributeValue.KEY_FOR_SINGLE_ATTRIBUTES) {
                        writer.WriteStartObject();
                        foreach (var field in value.AttributeValue.GetDefintionFields()) {
                            writer.WritePropertyName( field.FieldName );
                            JsonSerializer.Serialize( writer, value.AttributeValue.GetFieldValue( field.FieldName, fieldKey ), options );
                        }
                        writer.WriteEndObject();
                    }
                }
                writer.WriteEndArray();

            } else {
                writer.WriteStartObject();
                foreach (var field in value.AttributeValue.GetDefintionFields()) {
                    writer.WritePropertyName( field.FieldName );
                    JsonSerializer.Serialize( writer, value.AttributeValue.GetFieldValue( field.FieldName ), options );
                }
                writer.WriteEndObject();
            }

            writer.WriteEndObject();
        }


        /*
        /// <inheritdoc/>
        public override void WriteJson( JsonWriter writer, object? value, JsonSerializer serializer ) {

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

            JObject jo = JObject.Load( reader );

            //first try using the ConcreteClassId, if it is a property of the json, as this will be a faster method.
            var id = jo["ConcreteClassId"]?.Value<int>();

            //if id is null, then attempt to identify the type of AttributeValueDataPacket based on payload
            if (id == null) {
                if (jo.ContainsKey( "StatusCode" ))
                    id = AttributeValueDataPacketAPIResponse.CONCRETE_CLASS_ID;
                else if (jo.ContainsKey( "Message" ))
                    id = AttributeValueDataPacketAPIResponse.CONCRETE_CLASS_ID;
                else
                    id = AttributeValueDataPacketMatch.CONCRETE_CLASS_ID;
            }

            AttributeValueDataPacket attributeValueDataPacket;
            bool okToDeserialize = true;

            switch (id) {
                case AttributeValueDataPacketMatch.CONCRETE_CLASS_ID:
                    attributeValueDataPacket = new AttributeValueDataPacketMatch();

                    if (jo.ContainsKey( "ReentryTag" ))
                        ((AttributeValueDataPacketMatch)attributeValueDataPacket).ReentryTag = jo["ReentryTag"]?.Value<string>();
                    break;

                case AttributeValueDataPacketAPIResponse.CONCRETE_CLASS_ID:
                default:
                    attributeValueDataPacket = new AttributeValueDataPacketAPIResponse();

                    if (jo.ContainsKey( "StatusCode" ))
                        ((AttributeValueDataPacketAPIResponse)attributeValueDataPacket).StatusCode = (HttpStatusCode)Enum.Parse( typeof( HttpStatusCode ), (string)jo["StatusCode"] );

                    if (jo.ContainsKey( "Message" ) && jo.GetValue( "Message" ).HasValues ) {
                        try {
                            ((AttributeValueDataPacketAPIResponse)attributeValueDataPacket).Message = (string)jo.GetValue( "Message" )[0];
                        } catch (Exception ex) {
                            logger.Error( ex, $"Unable to read the Message property." );
                        }
                    }

                    if (((AttributeValueDataPacketAPIResponse)attributeValueDataPacket).StatusCode != HttpStatusCode.OK) {
                        okToDeserialize = false;
                        logger.Info( $"Unable to deserialize, received message '{((AttributeValueDataPacketAPIResponse)attributeValueDataPacket).Message}'." );
                    }
                    break;
            }

            if (okToDeserialize) {
                attributeValueDataPacket.AttributeDef = (string)jo.GetValue( "AttributeDef" );
                //logger.Debug( $"About to call AttributeValue.CreateAsync() for {attributeValueDataPacket.AttributeDef}." );
                attributeValueDataPacket.AttributeValueTask = AttributeValue.CreateAsync( SetName.Parse( attributeValueDataPacket.AttributeDef ), jo.GetValue( "AttributeValue" ) );
                //logger.Debug( $"Returned from calling AttributeValue.CreateAsync() for {attributeValueDataPacket.AttributeDef}." );
                if (jo.ContainsKey("Visibility"))
                    attributeValueDataPacket.Visibility = (VisibilityOption)Enum.Parse( typeof( VisibilityOption ), (string)jo["Visibility"] );
            }

            return attributeValueDataPacket;
        }
        */


    }

    /// <summary>
    /// EKA Note Jan 2025.
    /// For reasons I don't understand, system.text.json will not use the AttributeValueDataPacketConverter when deserializing properties of type
    /// Dictionary<string, AttributeValueDataPacketAPIResponse> even though AttributeValueDataPacketAPIResponse is a child class of type 
    /// AttributeValueDataPacket. My work around for this issue is to write a concrete class specific converter for AttributeValueDataPacketAPIResponse.
    /// Which, ironicaly, just calls AttributeValueDataPacketConverter to do the conversion.
    /// 
    /// This converter class is specifically needed for Scopos.BabelFish.Responses.AttributeValueAPI.AttributeValueWrapper.
    /// </summary>
    public class AttributeValueDataPacketAPIResponseConverter : JsonConverter<AttributeValueDataPacketAPIResponse> {

        private Logger logger = LogManager.GetCurrentClassLogger();

        private AttributeValueDataPacketConverter BaseConverter = new AttributeValueDataPacketConverter();

        public override AttributeValueDataPacketAPIResponse? Read( ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options ) {
            
            return (AttributeValueDataPacketAPIResponse) BaseConverter.Read( ref reader, typeToConvert, options );
        }

        public override void Write( Utf8JsonWriter writer, AttributeValueDataPacketAPIResponse value, JsonSerializerOptions options ) {

            BaseConverter.Write( writer, value, options );
        }

    }

    public class ListOfAttributeValueDataPackets : JsonConverter<List<AttributeValueDataPacket>> {

        private Logger logger = LogManager.GetCurrentClassLogger();

        private AttributeValueDataPacketConverter BaseConverter = new AttributeValueDataPacketConverter();


        public override List<AttributeValueDataPacket>? Read( ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options ) {
            throw new NotImplementedException();
        }

        public override void Write( Utf8JsonWriter writer, List<AttributeValueDataPacket> value, JsonSerializerOptions options ) {
            writer.WriteStartObject();
            writer.WritePropertyName( "attribute-values" );
            writer.WriteStartObject();
            foreach (var av in value) {
                writer.WritePropertyName( av.AttributeDef );
                BaseConverter.Write( writer, av, options );
            }
            writer.WriteEndObject();
            writer.WriteEndObject();
        }
    }
}
