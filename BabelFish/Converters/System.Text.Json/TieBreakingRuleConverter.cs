using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Scopos.BabelFish.DataModel.Definitions;

namespace Scopos.BabelFish.Converters.Microsoft {


    /// <summary>
    /// Custom converter class to deserialize the abstract class TieBreakingRule into one of its
    /// Concrete classes.
    /// </summary>
    public class TieBreakingRuleConverter : JsonConverter<TieBreakingRuleBase> {

        public override TieBreakingRuleBase? Read( ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options ) {
            try {
                using (JsonDocument doc = JsonDocument.ParseValue( ref reader )) {
                    JsonElement root = doc.RootElement;
                    string method;
                    JsonElement jsonElementValue;
                    if (root.TryGetProperty( "Method", out jsonElementValue ))
                        method = jsonElementValue.ToString();
                    else
                        method = "couldNotReadMethod"; //Which will force the default switch case

                    switch (method) {
                        case "Score":
                        case "":
                            return JsonSerializer.Deserialize<TieBreakingRuleScore>( root.GetRawText(), options );
                        case "CountOf":
                            return JsonSerializer.Deserialize<TieBreakingRuleCountOf>( root.GetRawText(), options );
                        case "ParticipantAttribute":
                            return JsonSerializer.Deserialize<TieBreakingRuleParticipantAttribute>( root.GetRawText(), options );
                        case "Attribute":
                            return JsonSerializer.Deserialize<TieBreakingRuleAttribute>( root.GetRawText(), options );
                        default:
                            return new TieBreakingRuleParticipantAttribute() {
                                Source = TieBreakingRuleParticipantAttributeSource.DisplayName,
                                Comment = "Default TieBreakingRule because the value read in could not be deserialized."
                            };
                    }
                }
            } catch (Exception ex) {
                return new TieBreakingRuleParticipantAttribute() {
                    Source = TieBreakingRuleParticipantAttributeSource.DisplayName,
                    Comment = "Default TieBreakingRule because the value read in could not be deserialized."
                };
            }
        }

        public override void Write( Utf8JsonWriter writer, TieBreakingRuleBase value, JsonSerializerOptions options ) {
            JsonSerializer.Serialize(writer, value, value.GetType(), options );
        }
    }
}
