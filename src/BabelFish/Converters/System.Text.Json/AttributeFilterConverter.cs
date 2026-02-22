using System.Text.Json;
using System.Text.Json.Serialization;
using BabelFish.DataModel.OrionMatch;
using Scopos.BabelFish.DataModel.OrionMatch;

namespace Scopos.BabelFish.Converters.Microsoft {
    internal class AttributeFilterConverter : JsonConverter<AttributeFilter> {

        public override AttributeFilter? Read( ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options ) {

            using (JsonDocument doc = JsonDocument.ParseValue( ref reader )) {
                JsonElement root = doc.RootElement;
                string operation = string.Empty;
                JsonElement jsonElementValue;
                if (root.TryGetProperty( "Operation", out jsonElementValue ))
                    operation = jsonElementValue.GetString();
                else
                    operation = string.Empty;

                switch (operation) {
                    case "EQUATION":
                        return JsonSerializer.Deserialize<AttributeFilterEquation>( root.GetRawText(), options );
                    case "ATTRIBUTE_VALUE":
                        return JsonSerializer.Deserialize<AttributeFilterAttributeValue>( root.GetRawText(), options );
                    default:
                        //If we get here, it is probable because of ill-formed json
                        return new AttributeFilterAttributeValue();
                }
            }
        }

        public override void Write( Utf8JsonWriter writer, AttributeFilter value, JsonSerializerOptions options ) {
            JsonSerializer.Serialize( writer, value, value.GetType(), options );
        }
    }
}
