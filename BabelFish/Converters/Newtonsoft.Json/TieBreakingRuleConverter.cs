using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using Scopos.BabelFish.DataModel.Definitions;

namespace Scopos.BabelFish.Converters.Newtonsoft {


    /// <summary>
    /// Custom converter class to deserialize the abstract class TieBreakingRule into one of its
    /// Concrete classes.
    ///
    /// Typeically can rely on the standard JsonConverter that looks at the $type variable to know
    /// what Concrete class to deserialize to. However, the value from $type is specific to a Media
    /// class, and not BabelFish, so the values don't match. Thus, we need to write our own converter.
    ///
    /// Recipe comes from https://stackoverflow.com/questions/20995865/deserializing-json-to-abstract-class
    /// </summary>
    public class TieBreakingRuleConverter : JsonConverter {

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
                            Source = TieBreakingRuleParticipantAttributeSource.DisplayName,
                            Comment = "Default TieBreakingRule because the value read in could not be deserialized."
                        };
                }
            } catch (Exception ex) {
                ;
                return new TieBreakingRuleParticipantAttribute() {
                    Source = TieBreakingRuleParticipantAttributeSource.DisplayName,
                    Comment = "Default TieBreakingRule because the value read in could not be deserialized."
                };
            }
        }

        public override bool CanWrite { get { return false; } }

        public override void WriteJson( JsonWriter writer, object value, JsonSerializer serializer ) {
            //When CanWrite is false, which it is, the standard converter is used and not this custom converter
        }
    }

    public class TieBreakingRuleBaseSpecifiedConcreteClassConverter : DefaultContractResolver {
        protected override JsonConverter ResolveContractConverter( Type objectType ) {
            if (typeof( TieBreakingRuleBase ).IsAssignableFrom( objectType ) && !objectType.IsAbstract)
                return null; // pretend TableSortRuleConvert is not specified (thus avoiding a stack overflow)
            return base.ResolveContractConverter( objectType );
        }
    }
}
