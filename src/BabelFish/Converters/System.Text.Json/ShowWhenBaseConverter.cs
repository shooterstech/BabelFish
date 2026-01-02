using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Scopos.BabelFish.DataModel.Definitions;
using Scopos.BabelFish.Helpers;

namespace Scopos.BabelFish.Converters.Microsoft {

    /// <summary>
    /// Custom converter class to deserialize the abstract class ShowWhenBase into one of its
    /// Concrete classes.
    /// </summary>
    public class ShowWhenBaseConverter : JsonConverter<ShowWhenBase> {

        public override ShowWhenBase? Read( ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options ) {

            using (JsonDocument doc = JsonDocument.ParseValue( ref reader )) {
                JsonElement root = doc.RootElement;
                string operation = string.Empty;
                JsonElement jsonElementValue;
                if (root.TryGetProperty( "Operation", out jsonElementValue ))
                    operation = jsonElementValue.GetString();
                else 
                    operation = string.Empty;

                switch (operation) {
                    case "EQUATION":
                        return JsonSerializer.Deserialize<ShowWhenEquation>( root.GetRawText(), options );
                    case "VARIABLE":
                        return JsonSerializer.Deserialize<ShowWhenVariable>( root.GetRawText(), options );
                    case "SEGMENT_GROUP":
                        return JsonSerializer.Deserialize<ShowWhenSegmentGroup>( root.GetRawText(), options );
                    default:
                        //If we get here, it is probable because of ill-formed json
                        return ShowWhenVariable.ALWAYS_SHOW.Clone();
                }
            }
        }

        public override void Write( Utf8JsonWriter writer, ShowWhenBase value, JsonSerializerOptions options ) {
            JsonSerializer.Serialize( writer, value, value.GetType(), options );
        }
    }
}
