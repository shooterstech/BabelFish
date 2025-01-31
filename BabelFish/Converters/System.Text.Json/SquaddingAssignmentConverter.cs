using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Scopos.BabelFish.DataModel.OrionMatch;

namespace Scopos.BabelFish.Converters.Microsoft {

    /// <summary>
    /// Custom converter class to deserialize the abstract class SquaddingAssignment into one of its
    /// Concrete classes.
    /// </summary>
    public class SquaddingAssignmentConverter : JsonConverter<SquaddingAssignment> {

        /*
        static JsonSerializerSettings SpecifiedSubclassConversion = new JsonSerializerSettings() { ContractResolver = new SquaddingAssignmentSpecifiedConcreteClassConverter() };

        public override bool CanConvert( Type objectType ) {
            return (objectType == typeof( SquaddingAssignment ));
        }

        public override object ReadJson( JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer ) {
            JObject jo = JObject.Load( reader );

            //first try using the ConcreteClassId, if it is a property of the json, as this will be a faster method.
            var id = jo["ConcreteClassId"]?.Value<int>();

            switch (id) {
                case SquaddingAssignmentFiringPoint.CONCRETE_CLASS_ID:
                    return JsonConvert.DeserializeObject<SquaddingAssignmentFiringPoint>( jo.ToString(), SpecifiedSubclassConversion );
                case SquaddingAssignmentBank.CONCRETE_CLASS_ID:
                    return JsonConvert.DeserializeObject<SquaddingAssignmentBank>( jo.ToString(), SpecifiedSubclassConversion );
                case SquaddingAssignmentSquad.CONCRETE_CLASS_ID:
                    return JsonConvert.DeserializeObject<SquaddingAssignmentSquad>( jo.ToString(), SpecifiedSubclassConversion );
                default:
                    break;
            }

            //If ConcreteClassId is not in the json, rely instead on the $type value.
            var type = jo["$type"]?.Value<string>();
            if (type != null) {
                if (type.Contains( "FiringPoint" ))
                    return JsonConvert.DeserializeObject<SquaddingAssignmentFiringPoint>( jo.ToString(), SpecifiedSubclassConversion );
                else if (type.Contains( "Bank" ))
                    return JsonConvert.DeserializeObject<SquaddingAssignmentBank>( jo.ToString(), SpecifiedSubclassConversion );
                else if (type.Contains( "Squad" ))
                    return JsonConvert.DeserializeObject<SquaddingAssignmentSquad>( jo.ToString(), SpecifiedSubclassConversion );
            }

            //If we get here, give up. 
            throw new NotImplementedException( $"Unable to convert type '{type}' to an Abstract class SquaddingAssignment." );
        }

        public override bool CanWrite { get { return false; } }

        public override void WriteJson( JsonWriter writer, object value, JsonSerializer serializer ) {
            //When CanWrite is false, which it is, the standard converter is used and not this custom converter
        }
        */

        public override SquaddingAssignment? Read( ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options ) {

            using (JsonDocument doc = JsonDocument.ParseValue( ref reader )) {
                JsonElement root = doc.RootElement;
                var id = root.GetProperty( "ConcreteClassId" ).GetInt32();

                switch (id) {
                    case SquaddingAssignmentFiringPoint.CONCRETE_CLASS_ID:
                        return JsonSerializer.Deserialize<SquaddingAssignmentFiringPoint>( root.GetRawText(), options );
                    case SquaddingAssignmentBank.CONCRETE_CLASS_ID:
                        return JsonSerializer.Deserialize<SquaddingAssignmentBank>( root.GetRawText(), options );
                    case SquaddingAssignmentSquad.CONCRETE_CLASS_ID:
                        return JsonSerializer.Deserialize<SquaddingAssignmentSquad>( root.GetRawText(), options );
                    default:
                        break;
                }

                throw new NotImplementedException( $"Unable to convert type '{id}' to an Abstract class SquaddingAssignment." );
            }
        }

        public override void Write( Utf8JsonWriter writer, SquaddingAssignment value, JsonSerializerOptions options ) {
            JsonSerializer.Serialize( writer, value, value.GetType(), options );
        }
    }
}
