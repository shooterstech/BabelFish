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
    public class AbbreviatedFormatChildConverter : JsonConverter<AbbreviatedFormatChild> {

        public override AbbreviatedFormatChild? Read( ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options ) {

            using (JsonDocument doc = JsonDocument.ParseValue( ref reader )) {
                JsonElement root = doc.RootElement;
                string operation = string.Empty;
                JsonElement jsonElementValue;
                if (root.TryGetProperty( "Derivation", out jsonElementValue ))
                    operation = jsonElementValue.GetString();
                else 
                    operation = string.Empty;

                switch (operation) {
                    case "EXPLICIT":
                    default:
                        return JsonSerializer.Deserialize<AbbreviatedFormatChildExplicit>( root.GetRawText(), options );
                    case "DERIVED":
                        return JsonSerializer.Deserialize<AbbreviatedFormatChildDerived>( root.GetRawText(), options );
                    case "EXPAND":
                        return JsonSerializer.Deserialize<AbbreviatedFormatChildExpand>( root.GetRawText(), options );
                }
            }
        }

        public override void Write( Utf8JsonWriter writer, AbbreviatedFormatChild value, JsonSerializerOptions options ) {
            JsonSerializer.Serialize( writer, value, value.GetType(), options );
        }
    }
}
