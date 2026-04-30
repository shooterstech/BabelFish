using System.Text.Json;
using System.Text.Json.Serialization;
using Scopos.BabelFish.Responses;
using Scopos.BabelFish.Responses.OrionMatchAPI;

namespace Scopos.BabelFish.Converters.Microsoft {
    /// <summary>
    /// System.Text.Json converter for the abstract class MetaDataResponse. This converter looks for the "Type" property in the JSON to determine which concrete subclass of MetaDataResponse to deserialize into.
    /// </summary>
    public class MetaDataResponseConverter : JsonConverter<MetaDataResponse> {

        /// <inheritdoc/>
        public override MetaDataResponse? Read( ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options ) {

            using (JsonDocument doc = JsonDocument.ParseValue( ref reader )) {
                JsonElement root = doc.RootElement;

                if (root.TryGetProperty( "Type", out JsonElement classIdType )) {
                    string classId = classIdType.GetString();
                    switch (classId) {
                        case "Match":
                            return JsonSerializer.Deserialize<MatchDetailMetaData>( root.GetRawText(), options );
                        case "Unknown":
                        default:
                            break;
                    }
                }

                // If we can't determine the class type, return an instance of MetaDataResponseUnknown
                return new MetaDataResponseUnknown();
            }
        }

        /// <inheritdoc/>
        public override void Write( Utf8JsonWriter writer, MetaDataResponse value, JsonSerializerOptions options ) {
            JsonSerializer.Serialize( writer, value, value.GetType(), options );
        }
    }
}
