using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using Scopos.BabelFish.DataModel.AttributeValue;
using Scopos.BabelFish.DataModel.Definitions;
using Scopos.BabelFish.DataModel.OrionMatch;
using Scopos.BabelFish.Responses.AttributeValueAPI;

namespace Scopos.BabelFish.Converters.Microsoft {

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

                AttributeValueType avType = AttributeValueType.MATCH;

                // First attempt to identify the type of AttributeValueDataPacket using the "Type" property, which is included in newer serializations.
                // If the "Type" property is not present, attempt to identify the type using the "ConcreteClassId" property, which is included in older serializations.
                int id = 0;
                try {
                    if (root.TryGetProperty( "Type", out temp )
                        && EnumHelper.TryParseEnumByDescription<AttributeValueType>( temp.GetString(), out var parsedType )) {
                        avType = parsedType;
                    } else {
                        id = root.GetProperty( "ConcreteClassId" ).GetInt32();
                        if (id == AttributeValueDataPacketMatch.CONCRETE_CLASS_ID)
                            avType = AttributeValueType.MATCH;
                        else if (id == AttributeValueDataPacketAPIResponse.CONCRETE_CLASS_ID)
                            avType = AttributeValueType.API_RESPONSE;
                        else if (id == AttributeConfiguration.CONCRETE_CLASS_ID)
                            avType = AttributeValueType.CONFIGURATION;
                    }
                } catch (KeyNotFoundException) {
                    //On some older serializations, the ConcreteClassId was not included. Infer the value based on what else is in the json
                    if (root.TryGetProperty( "StatusCode", out temp ))
                        avType = AttributeValueType.API_RESPONSE;
                    else
                        avType = AttributeValueType.MATCH;
                }

                AttributeValueDataPacket attributeValueDataPacket;
                bool okToDeserialize = true;

                switch (avType) {
                    case AttributeValueType.MATCH:
                        attributeValueDataPacket = new AttributeValueDataPacketMatch();

                        if (root.TryGetProperty( "CourseOfFireId", out temp ))
                            ((AttributeValueDataPacketMatch)attributeValueDataPacket).CourseOfFireId = temp.GetInt32();
                        break;

                    case AttributeValueType.CONFIGURATION:
                        attributeValueDataPacket = new AttributeConfiguration();

                        if (root.TryGetProperty( "CourseOfFireId", out temp ))
                            ((AttributeConfiguration)attributeValueDataPacket).CourseOfFireId = temp.GetInt32();
                        if (root.TryGetProperty( "Constant", out temp ))
                            ((AttributeConfiguration)attributeValueDataPacket).Constant = temp.GetBoolean();
                        break;

                    case AttributeValueType.API_RESPONSE:
                    default:
                        attributeValueDataPacket = new AttributeValueDataPacketAPIResponse();

                        if (root.TryGetProperty( "StatusCode", out temp ))
                            ((AttributeValueDataPacketAPIResponse)attributeValueDataPacket).StatusCode = (HttpStatusCode)Enum.Parse( typeof( HttpStatusCode ), temp.GetInt32().ToString() );

                        //EKA Note Jan 2025 the Message property is deprecated, and will soon be removed.
                        if (root.TryGetProperty( "Message", out temp ) && temp.ValueKind == JsonValueKind.Array && temp.GetArrayLength() > 0) {
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
                    attributeValueDataPacket.AttributeDef = SetName.Parse( root.GetProperty( "AttributeDef" ).GetString(), false );
                    //Have to creaet a copy of the AttributeValue JsonElement. If we dont', we run the risk that it gets disposed of before we have chance of interpreting it.
                    var attrValueAsJsonElement = CopyJsonElement( root.GetProperty( "AttributeValue" ) );
                    attributeValueDataPacket.AttributeValueTask = AttributeValue.CreateAsync( attributeValueDataPacket.AttributeDef, attrValueAsJsonElement );
                    if (root.TryGetProperty( "Visibility", out temp ))
                        attributeValueDataPacket.Visibility = EnumHelper.ParseVisibilityOption( temp.GetString() );
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
        public static JsonElement CopyJsonElement( JsonElement original ) {
            using (var memoryStream = new System.IO.MemoryStream()) {
                // Write the original JsonElement to the memory stream.
                using (var writer = new Utf8JsonWriter( memoryStream )) {
                    original.WriteTo( writer );
                }

                // Reset the memory stream position.
                memoryStream.Position = 0;

                // Parse the memory stream to create a new JsonDocument.
                using (var document = JsonDocument.Parse( memoryStream )) {
                    // Return the root element of the new JsonDocument.
                    return document.RootElement.Clone();
                }
            }
        }
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

            return (AttributeValueDataPacketAPIResponse)BaseConverter.Read( ref reader, typeToConvert, options );
        }

        public override void Write( Utf8JsonWriter writer, AttributeValueDataPacketAPIResponse value, JsonSerializerOptions options ) {

            BaseConverter.Write( writer, value, options );
        }

    }

    public class AttributeValueDataPacketMatchConverter : JsonConverter<AttributeValueDataPacketMatch> {

        private Logger logger = LogManager.GetCurrentClassLogger();

        private AttributeValueDataPacketConverter BaseConverter = new AttributeValueDataPacketConverter();

        public override AttributeValueDataPacketMatch? Read( ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options ) {

            return (AttributeValueDataPacketMatch)BaseConverter.Read( ref reader, typeToConvert, options );
        }

        public override void Write( Utf8JsonWriter writer, AttributeValueDataPacketMatch value, JsonSerializerOptions options ) {

            BaseConverter.Write( writer, value, options );
        }
    }

    public class AttributeConfigurationConverter : JsonConverter<AttributeConfiguration> {

        private Logger logger = LogManager.GetCurrentClassLogger();

        private AttributeValueDataPacketConverter BaseConverter = new AttributeValueDataPacketConverter();

        public override AttributeConfiguration? Read( ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options ) {

            return (AttributeConfiguration)BaseConverter.Read( ref reader, typeToConvert, options );
        }

        public override void Write( Utf8JsonWriter writer, AttributeConfiguration value, JsonSerializerOptions options ) {

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
                writer.WritePropertyName( av.AttributeDef.ToString() );
                BaseConverter.Write( writer, av, options );
            }
            writer.WriteEndObject();
            writer.WriteEndObject();
        }
    }
}
