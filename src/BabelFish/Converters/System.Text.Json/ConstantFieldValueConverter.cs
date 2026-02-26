using System.Text.Json;
using System.Text.Json.Serialization;
using Scopos.BabelFish.DataModel.AttributeValue;

namespace Scopos.BabelFish.Converters.Microsoft {
    public class ConstantFieldValueListConverter : JsonConverter<ConstantFieldValueList> {

        private Logger logger = LogManager.GetCurrentClassLogger();

        public override ConstantFieldValueList? Read( ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options ) {

            if (reader.TokenType != JsonTokenType.StartObject)
                throw new JsonException();

            ConstantFieldValueList list = new ConstantFieldValueList();

            while (reader.Read()) {
                if (reader.TokenType == JsonTokenType.EndObject)
                    return list;

                if (reader.TokenType != JsonTokenType.PropertyName)
                    throw new JsonException( "Expected property name" );

                string fieldName = reader.GetString()!;

                // Move to the value
                reader.Read();

                // Let System.Text.Json deserialize the value dynamically
                dynamic? value = JsonSerializer.Deserialize<dynamic>( ref reader, options );
                dynamic valueDeserialized = null;

                if (value.ValueKind == G_STJ.JsonValueKind.String) {
                    valueDeserialized = value.GetString();
                } else if (value.ValueKind == JsonValueKind.Number) {
                    valueDeserialized = value.GetInt32();
                } else if (value.ValueKind == JsonValueKind.True || value.ValueKind == JsonValueKind.False) {
                    valueDeserialized = value.GetBoolean();
                } else if (value.ValueKind == G_STJ.JsonValueKind.Number) {
                    valueDeserialized = value.GetSingle();
                }

                list.Add( new ConstantFieldValue {
                    FieldName = fieldName,
                    Value = valueDeserialized
                } );


            }
            throw new JsonException( "Invalid JSON for ConstantFieldValueList" );
        }

        public override void Write(
            Utf8JsonWriter writer,
            ConstantFieldValueList value,
            JsonSerializerOptions options ) {
            writer.WriteStartObject();

            foreach (var item in value) {
                writer.WritePropertyName( item.FieldName );
                JsonSerializer.Serialize( writer, item.Value, options );
            }

            writer.WriteEndObject();
        }
    }
}
