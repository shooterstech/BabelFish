using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Scopos.BabelFish.DataModel.ScoreHistory;

namespace Scopos.BabelFish.Converters {

    /// <summary>
    /// Custom converter class to deserialize the abstract class SquaddingAssignment into one of its
    /// Concrete classes.
    /// </summary>
    public class ScoreHistoryBaseConverter : JsonConverter<ScoreHistoryBase> {

        /*
        static JsonSerializerSettings SpecifiedSubclassConversion = new JsonSerializerSettings() { ContractResolver = new ScoreHistoryBaseSpecifiedConcreteClassConverter() };

        public override bool CanConvert( Type objectType ) {
            return (objectType == typeof( ScoreHistoryBase ));
        }

        public override object ReadJson( JsonReader reader, Type objectType, object existingValue,
            JsonSerializer serializer ) {
            JObject jo = JObject.Load( reader );

            //first try using the ConcreteClassId, if it is a property of the json, as this will be a faster method.
            var id = jo["ConcreteClassId"]?.Value<int>();

            switch (id) {
                case ScoreHistoryEventStyleEntry.CONCRETE_CLASS_ID:
                    return JsonConvert.DeserializeObject<ScoreHistoryEventStyleEntry>( jo.ToString(), SpecifiedSubclassConversion );
                case ScoreHistoryStageStyleEntry.CONCRETE_CLASS_ID:
                    return JsonConvert.DeserializeObject<ScoreHistoryStageStyleEntry>( jo.ToString(), SpecifiedSubclassConversion );
                case ScoreHistoryEventStyleTimespan.CONCRETE_CLASS_ID:
                    return JsonConvert.DeserializeObject<ScoreHistoryEventStyleTimespan>( jo.ToString(), SpecifiedSubclassConversion );
                case ScoreHistoryStageStyleTimespan.CONCRETE_CLASS_ID:
                    return JsonConvert.DeserializeObject<ScoreHistoryStageStyleTimespan>( jo.ToString(), SpecifiedSubclassConversion );
                default:
                    break;
            }

            //If we get here, give up. 
            throw new NotImplementedException( $"Unable to convert abstract class ScoreHistoryBase with ConcreteClassId '{id}' to it's concrete class." );
        }

        public override bool CanWrite { get { return false; } }

        public override void WriteJson( JsonWriter writer, object value, JsonSerializer serializer ) {
            //When CanWrite is false, which it is, the standard converter is used and not this custom converter
        }
        */

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
