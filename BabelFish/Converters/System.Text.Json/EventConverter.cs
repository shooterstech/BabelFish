using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Scopos.BabelFish.DataModel.Definitions;

namespace Scopos.BabelFish.Converters.Microsoft {
    public class EventConverter : JsonConverter<Event> {
        public override Event? Read( ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options ) {

            using (JsonDocument doc = JsonDocument.ParseValue( ref reader )) {
                JsonElement root = doc.RootElement;
                string derivation = "";
                JsonElement derivationElement;
                if (root.TryGetProperty( "Derivation", out derivationElement )) {
                    derivation = derivationElement.GetString();
                }

                switch (derivation) {
                    case "EXPLICIT":
                        default:
                        return JsonSerializer.Deserialize<EventExplicit>( root.GetRawText(), options );

                    case "DERIVED":
                        return JsonSerializer.Deserialize<EventDerived>( root.GetRawText(), options );

                    case "EXPAND":
                        return JsonSerializer.Deserialize<EventExpand>( root.GetRawText(), options );
                }
            }
        }

        public override void Write( Utf8JsonWriter writer, Event value, JsonSerializerOptions options ) {
            JsonSerializer.Serialize( writer, value, value.GetType(), options );
        }
    }
}
