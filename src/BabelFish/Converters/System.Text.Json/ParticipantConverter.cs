using System.Text.Json;
using System.Text.Json.Serialization;
using Scopos.BabelFish.DataModel.OrionMatch;

namespace Scopos.BabelFish.Converters.Microsoft {

    /// <summary>
    /// Custom converter class to deserialize the abstract class Participant into one of its
    /// Concrete classes.
    /// </summary>
    public class ParticipantConverter : JsonConverter<Participant> {

        public override Participant? Read( ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options ) {

            using (JsonDocument doc = JsonDocument.ParseValue( ref reader )) {
                JsonElement root = doc.RootElement;

                if (root.TryGetProperty( "ParticipantType", out JsonElement participantTypeElement )) {
                    string participantType = participantTypeElement.GetString();
                    switch (participantType) {
                        case "Individual":
                            return JsonSerializer.Deserialize<Individual>( root.GetRawText(), options );
                        case "Team":
                            return JsonSerializer.Deserialize<Team>( root.GetRawText(), options );
                        default:
                            break;
                    }
                }

                if (root.TryGetProperty( "ConcreteClassId", out JsonElement concreteClassIdElement )) {
                    int id = concreteClassIdElement.GetInt32();

                    switch (id) {
                        case Individual.CONCRETE_CLASS_ID:
                            return JsonSerializer.Deserialize<Individual>( root.GetRawText(), options );
                        case Team.CONCRETE_CLASS_ID:
                            return JsonSerializer.Deserialize<Team>( root.GetRawText(), options );
                        default:
                            break;
                    }
                }

                //If we get here, give up. 
                throw new NotImplementedException( $"Unable to convert to an Abstract class Participant." );
            }
        }

        public override void Write( Utf8JsonWriter writer, Participant value, JsonSerializerOptions options ) {
            JsonSerializer.Serialize( writer, value, value.GetType(), options );
        }
    }
}
