using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using Scopos.BabelFish.DataModel.OrionMatch;

namespace Scopos.BabelFish.Converters {

    /// <summary>
    /// Custom converter class to deserialize the abstract class Participant into one of its
    /// Concrete classes.
    ///
    /// Typeically can rely on the standard JsonConverter that looks at the $type variable to know
    /// what Concrete class to deserialize to. However, the value from $type is specific to a Media
    /// class, and not BabelFish, so the values don't match. Thus, we need to write our own converter.
    ///
    /// Recipe comes from https://stackoverflow.com/questions/20995865/deserializing-json-to-abstract-class
    /// </summary>
    public class ParticipantConverter : JsonConverter {
        static JsonSerializerSettings SpecifiedSubclassConversion = new JsonSerializerSettings() { ContractResolver = new ParticipantSpecifiedConcreteClassConverter() };

        public override bool CanConvert( Type objectType ) {
            return (objectType == typeof( Participant ));
        }

        public override object ReadJson( JsonReader reader, Type objectType, object existingValue,
            JsonSerializer serializer ) {
            JObject jo = JObject.Load( reader );

            //first try using the ConcreteClassId, if it is a property of the json, as this will be a faster method.
            var id = jo["ConcreteClassId"]?.Value<int>();

            switch (id) {
                case Individual.CONCRETE_CLASS_ID:
                    return JsonConvert.DeserializeObject<Individual>( jo.ToString(), SpecifiedSubclassConversion );
                case Team.CONCRETE_CLASS_ID:
                    return JsonConvert.DeserializeObject<Team>( jo.ToString(), SpecifiedSubclassConversion );
                default:
                    break;
            }

            //If ConcreteClassId is not in the json, rely instead on the $type value.
            var type = jo["$type"]?.Value<string>();
            if (type != null) {
                if (type.Contains( "Individual" ))
                    return JsonConvert.DeserializeObject<Individual>( jo.ToString(), SpecifiedSubclassConversion );
                else if (type.Contains( "Team" ))
                    return JsonConvert.DeserializeObject<Team>( jo.ToString(), SpecifiedSubclassConversion );
            }

            //If we get here, give up. 
            throw new NotImplementedException( $"Unable to convert type '{type}' to an Abstract class Participant." );
        }

        public override bool CanWrite {  get { return false; } }

        public override void WriteJson( JsonWriter writer, object value, JsonSerializer serializer ) {
            //When CanWrite is false, which it is, the standard converter is used and not this custom converter
        }
    }


    public class ParticipantSpecifiedConcreteClassConverter : DefaultContractResolver {
        protected override JsonConverter ResolveContractConverter( Type objectType ) {
            if (typeof( Participant ).IsAssignableFrom( objectType ) && !objectType.IsAbstract)
                return null; // pretend TableSortRuleConvert is not specified (thus avoiding a stack overflow)
            return base.ResolveContractConverter( objectType );
        }
    }
}
