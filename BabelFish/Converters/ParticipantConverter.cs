using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Scopos.BabelFish.DataModel.OrionMatch;
using NLog;

namespace Scopos.BabelFish.Converters {

    /// <summary>
    /// Custom converter class to deserialize the abstract class Participant into one of its
    /// Concrete classes.
    /// </summary>
    public class ParticipantConverter : JsonConverter<Participant> {

        /*
        static JsonSerializerSettings SpecifiedSubclassConversion = new JsonSerializerSettings() { ContractResolver = new ParticipantSpecifiedConcreteClassConverter() };

        public override bool CanConvert( Type objectType ) {
            return (objectType == typeof( Participant ));
        }

        public override object ReadJson( JsonReader reader, Type objectType, object existingValue,
            JsonSerializer serializer ) {
            JObject jo = JObject.Load( reader );

            //first try using the ConcreteClassId, if it is a property of the json, as this will be a faster method.
            var id = jo["ConcreteClassId"]?.Value<int>();

            switch (id) {
                case Individual.CONCRETE_CLASS_ID:
                    return JsonConvert.DeserializeObject<Individual>( jo.ToString(), SpecifiedSubclassConversion );
                case Team.CONCRETE_CLASS_ID:
                    return JsonConvert.DeserializeObject<Team>( jo.ToString(), SpecifiedSubclassConversion );
                default:
                    break;
            }

            //If ConcreteClassId is not in the json, rely instead on the $type value.
            var type = jo["$type"]?.Value<string>();
            if (type != null) {
                if (type.Contains( "Individual" ))
                    return JsonConvert.DeserializeObject<Individual>( jo.ToString(), SpecifiedSubclassConversion );
                else if (type.Contains( "Team" ))
                    return JsonConvert.DeserializeObject<Team>( jo.ToString(), SpecifiedSubclassConversion );
            }

            //If we get here, give up. 
            throw new NotImplementedException( $"Unable to convert type '{type}' to an Abstract class Participant." );
        }

        public override bool CanWrite {  get { return false; } }

        public override void WriteJson( JsonWriter writer, object value, JsonSerializer serializer ) {
            //When CanWrite is false, which it is, the standard converter is used and not this custom converter
        }
        */

        public override Participant? Read( ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options ) {

            using (JsonDocument doc = JsonDocument.ParseValue( ref reader )) {
                JsonElement root = doc.RootElement;
                int id = root.GetProperty( "ConcreteClassId" ).GetInt32();

                switch (id) {
                    case Individual.CONCRETE_CLASS_ID:
                        return JsonSerializer.Deserialize<Individual>( root.GetRawText(), options );
                    case Team.CONCRETE_CLASS_ID:
                        return JsonSerializer.Deserialize<Team>( root.GetRawText(), options );
                    default:
                        break;
                }

                //If we get here, give up. 
                throw new NotImplementedException( $"Unable to convert type '{id}' to an Abstract class Participant." );
            }
        }

        public override void Write( Utf8JsonWriter writer, Participant value, JsonSerializerOptions options ) {
            JsonSerializer.Serialize( writer, value, value.GetType(), options );
        }
    }
}
