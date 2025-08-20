﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Scopos.BabelFish.DataModel.Definitions;
using Scopos.BabelFish.DataModel.OrionMatch;

namespace Scopos.BabelFish.Converters.Microsoft {


    /// <summary>
    /// Custom converter class to deserialize the abstract class TieBreakingRule into one of its
    /// Concrete classes.
    /// </summary>
    public class RelayConverter : JsonConverter<Relay> {

        public override Relay? Read( ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options ) {
            try {
                using (JsonDocument doc = JsonDocument.ParseValue( ref reader )) {
                    JsonElement root = doc.RootElement;
                    string method;
                    JsonElement jsonElementValue;
                    if (root.TryGetProperty( "RelayType", out jsonElementValue ))
                        method = jsonElementValue.ToString();
                    else
                        method = ""; //Which will force the default switch case

                    switch (method) {
                        case "FIRING_POINT":
                        case "":
                        default:
                            return JsonSerializer.Deserialize<RelayFiringPoint>( root.GetRawText(), options );
                        case "BANK":
                            return JsonSerializer.Deserialize<RelayBank>( root.GetRawText(), options );
                        case "SQUAD":
                            return JsonSerializer.Deserialize<RelaySquad>( root.GetRawText(), options );
                    }
                }
            } catch (Exception ex) {
                return new RelayFiringPoint();
            }
        }

        public override void Write( Utf8JsonWriter writer, Relay value, JsonSerializerOptions options ) {
            JsonSerializer.Serialize(writer, value, value.GetType(), options );
        }
    }
}
