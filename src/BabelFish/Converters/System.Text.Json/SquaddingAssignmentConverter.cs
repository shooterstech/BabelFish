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
                string squaddingType = "FIRING_POINT";
                JsonElement jsonElementValue;
                if ( root.TryGetProperty( "SquaddingType", out jsonElementValue ) )
					squaddingType = jsonElementValue.ToString();

                switch (squaddingType) {
                    case "FIRING_POINT":
                    default:
                        return JsonSerializer.Deserialize<SquaddingAssignmentFiringPoint>( root.GetRawText(), options );
                    case "BANK":
                        return JsonSerializer.Deserialize<SquaddingAssignmentBank>( root.GetRawText(), options );
                    case "SQUAD":
                        return JsonSerializer.Deserialize<SquaddingAssignmentSquad>( root.GetRawText(), options );
                }
            }
        }

        public override void Write( Utf8JsonWriter writer, SquaddingAssignment value, JsonSerializerOptions options ) {
            JsonSerializer.Serialize( writer, value, value.GetType(), options );
        }
    }
}
