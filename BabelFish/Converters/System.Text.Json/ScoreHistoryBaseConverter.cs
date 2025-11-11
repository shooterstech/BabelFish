using System.Text.Json;
using System.Text.Json.Serialization;
using Scopos.BabelFish.DataModel.ScoreHistory;

namespace Scopos.BabelFish.Converters.Microsoft {

    /// <summary>
    /// Custom converter class to deserialize the abstract class SquaddingAssignment into one of its
    /// Concrete classes.
    /// </summary>
    public class ScoreHistoryBaseConverter : JsonConverter<ScoreHistoryBase> {

        public override ScoreHistoryBase? Read( ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options ) {
            using (JsonDocument doc = JsonDocument.ParseValue( ref reader )) {
                JsonElement root = doc.RootElement;
                var id = root.GetProperty( "ConcreteClassId" ).GetInt32();

                switch (id) {
                    case ScoreHistoryEventStyleEntry.CONCRETE_CLASS_ID:
                        return JsonSerializer.Deserialize<ScoreHistoryEventStyleEntry>( root.GetRawText(), options );
                    case ScoreHistoryStageStyleEntry.CONCRETE_CLASS_ID:
                        return JsonSerializer.Deserialize<ScoreHistoryStageStyleEntry>( root.GetRawText(), options );
                    case ScoreHistoryEventStyleTimespan.CONCRETE_CLASS_ID:
                        return JsonSerializer.Deserialize<ScoreHistoryEventStyleTimespan>( root.GetRawText(), options );
                    case ScoreHistoryStageStyleTimespan.CONCRETE_CLASS_ID:
                        return JsonSerializer.Deserialize<ScoreHistoryStageStyleTimespan>( root.GetRawText(), options );
                    default:
                        break;
                }

                //If we get here, give up. 
                throw new NotImplementedException( $"Unable to convert abstract class ScoreHistoryBase with ConcreteClassId '{id}' to it's concrete class." );
            }
        }

        public override void Write( Utf8JsonWriter writer, ScoreHistoryBase value, JsonSerializerOptions options ) {
            JsonSerializer.Serialize( writer, value, value.GetType(), options );
        }
    }
}
