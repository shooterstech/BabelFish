using System.Text.Json;
using System.Text.Json.Serialization;
using Scopos.BabelFish.DataModel.OrionMatch;

namespace Scopos.BabelFish.Converters.Microsoft {
    /// <summary>
    /// Converts JSON data to and from instances of the CourseOfFireEntry class and its derived types.
    /// </summary>
    /// <remarks>This converter handles the deserialization of JSON objects based on the 'ParticipantType'
    /// property, allowing for the conversion of individual and team entries. It throws a NotImplementedException if the
    /// 'ParticipantType' does not match expected values.</remarks>
    public class CourseOfFireEntryConverter : JsonConverter<CourseOfFireEntry> {

        /// <inheritdoc />
        public override CourseOfFireEntry? Read( ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options ) {

            using (JsonDocument doc = JsonDocument.ParseValue( ref reader )) {
                JsonElement root = doc.RootElement;

                if (root.TryGetProperty( "ParticipantType", out JsonElement participantTypeElement )) {
                    string participantType = participantTypeElement.GetString();
                    switch (participantType) {
                        case "Individual":
                            return JsonSerializer.Deserialize<CourseOfFireEntryIndividual>( root.GetRawText(), options );
                        case "Team":
                            return JsonSerializer.Deserialize<CourseOfFireEntryTeam>( root.GetRawText(), options );
                        default:
                            break;
                    }
                }

                //If we get here, give up. 
                throw new NotImplementedException( $"Unable to convert to an Abstract class Participant." );
            }
        }

        /// <inheritdoc />
        public override void Write( Utf8JsonWriter writer, CourseOfFireEntry value, JsonSerializerOptions options ) {
            JsonSerializer.Serialize( writer, value, value.GetType(), options );
        }
    }
}
