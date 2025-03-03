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

        public override SquaddingAssignment? Read( ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options ) {

            using (JsonDocument doc = JsonDocument.ParseValue( ref reader )) {
                JsonElement root = doc.RootElement;
                int id;
                JsonElement jsonElementValue;
                if ( root.TryGetProperty( "ConcreteClassId", out jsonElementValue ) )
                    id = jsonElementValue.GetInt32();
                else
                    id = SquaddingAssignmentFiringPoint.CONCRETE_CLASS_ID;

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
