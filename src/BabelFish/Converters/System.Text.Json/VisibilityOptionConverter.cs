using System.Text.Json;
using System.Text.Json.Serialization;
using Scopos.BabelFish.DataModel.Common;

namespace Scopos.BabelFish.Converters.Microsoft {
    public class VisibilityOptionJsonConverter : JsonConverter<VisibilityOption> {

        public override VisibilityOption Read( ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options ) {
            var value = reader.GetString();
            // League metadata uses uppercase values; other API payloads use title case.
            return value?.ToUpperInvariant() switch {
                "PUBLIC" => VisibilityOption.PUBLIC,
                "INTERNAL" => VisibilityOption.INTERNAL,
                "PROTECTED" => VisibilityOption.PROTECTED,
                "PRIVATE" => VisibilityOption.PRIVATE,
                _ => VisibilityOption.PRIVATE
            };
        }

        public override void Write( Utf8JsonWriter writer, VisibilityOption value, JsonSerializerOptions options ) {
            var str = value switch {
                VisibilityOption.PUBLIC => "Public",
                VisibilityOption.INTERNAL => "Internal",
                VisibilityOption.PROTECTED => "Protected",
                VisibilityOption.PRIVATE => "Private",
                _ => throw new JsonException( $"Unknown VisibilityOption value: {value}" )
            };
            writer.WriteStringValue( str );
        }
    }
}
