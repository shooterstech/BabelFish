using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Scopos.BabelFish.DataModel.Athena.AbstractEST;
using Scopos.BabelFish.DataModel.Definitions;
using Scopos.BabelFish.Helpers;

namespace Scopos.BabelFish.Converters.Microsoft {

    /// <summary>
    /// Custom converter class to deserialize the abstract class ShowWhenBase into one of its
    /// Concrete classes.
    /// </summary>
    public class DisplayEntityConfigurationConverter : JsonConverter<DisplayEntityConfiguration> {

        public override DisplayEntityConfiguration? Read( ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options ) {

            using (JsonDocument doc = JsonDocument.ParseValue( ref reader )) {
                JsonElement root = doc.RootElement;
                string operation = string.Empty;
                JsonElement jsonElementValue;
                if (root.TryGetProperty( "DisplayEntity", out jsonElementValue ))
                    operation = jsonElementValue.GetString();
                else 
                    operation = string.Empty;

                switch (operation) {
                    case "AthleteDisplay":
                    default:
                        return JsonSerializer.Deserialize<AthleteDisplayConfiguration>( root.GetRawText(), options );
                    case "ImageDisplay":
                        return JsonSerializer.Deserialize<ImageDisplayConfiguration>( root.GetRawText(), options );
                    case "ResultList":
                        return JsonSerializer.Deserialize<ResultListConfiguration>( root.GetRawText(), options );
                    case "SquaddingList":
                        return JsonSerializer.Deserialize<SquaddingListConfiguration>( root.GetRawText(), options );
                }
            }
        }

        public override void Write( Utf8JsonWriter writer, DisplayEntityConfiguration value, JsonSerializerOptions options ) {
            JsonSerializer.Serialize( writer, value, value.GetType(), options );
        }
    }
}
