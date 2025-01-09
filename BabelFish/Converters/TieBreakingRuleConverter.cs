using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Scopos.BabelFish.DataModel.Definitions;

namespace Scopos.BabelFish.Converters {


    /// <summary>
    /// Custom converter class to deserialize the abstract class TieBreakingRule into one of its
    /// Concrete classes.
    /// </summary>
    public class TieBreakingRuleConverter : JsonConverter<TieBreakingRuleBase> {

        /*
        static JsonSerializerSettings SpecifiedSubclassConversion = new JsonSerializerSettings() { ContractResolver = new TieBreakingRuleBaseSpecifiedConcreteClassConverter() };

        public override bool CanConvert( Type objectType ) {
            return (objectType == typeof( ShowWhenBase ));
        }

        public override object ReadJson( JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer ) {
            try {
                JObject jo = JObject.Load( reader );

                var id = jo["Method"]?.Value<string>();

                switch (id) {
                    case "Score":
                    case "":
                    case null:
                        return JsonConvert.DeserializeObject<TieBreakingRuleScore>( jo.ToString(), SpecifiedSubclassConversion );
                    case "CountOf":
                        return JsonConvert.DeserializeObject<TieBreakingRuleCountOf>( jo.ToString(), SpecifiedSubclassConversion );
                    case "ParticipantAttribute":
                        return JsonConvert.DeserializeObject<TieBreakingRuleParticipantAttribute>( jo.ToString(), SpecifiedSubclassConversion );
                    case "Attribute":
                        return JsonConvert.DeserializeObject<TieBreakingRuleAttribute>( jo.ToString(), SpecifiedSubclassConversion );
                    default:
                        //If we get here, it is probable because of ill-formed json
                        return new TieBreakingRuleParticipantAttribute() {
                            Source = "DisplayName",
                            Comment = "Default TieBreakingRule because the value read in could not be deserialized."
                        };
                }
            } catch (Exception ex) {
                ;
                return new TieBreakingRuleParticipantAttribute() {
                    Source = "DisplayName",
                    Comment = "Default TieBreakingRule because the value read in could not be deserialized."
                };
            }
        }
        */

        public override TieBreakingRuleBase? Read( ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options ) {
            try {
                using (JsonDocument doc = JsonDocument.ParseValue( ref reader )) {
                    JsonElement root = doc.RootElement;
                    string method = root.GetProperty( "Method" ).GetString();

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
                                Source = "DisplayName",
                                Comment = "Default TieBreakingRule because the value read in could not be deserialized."
                            };
                    }
                }
            } catch (Exception ex) {
                return new TieBreakingRuleParticipantAttribute() {
                    Source = "DisplayName",
                    Comment = "Default TieBreakingRule because the value read in could not be deserialized."
                };
            }
        }

        public override void Write( Utf8JsonWriter writer, TieBreakingRuleBase value, JsonSerializerOptions options ) {
            JsonSerializer.Serialize(writer, value, value.GetType(), options );
        }
    }
}
