using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using Scopos.BabelFish.DataModel.Definitions;

namespace Scopos.BabelFish.Converters {

    /// <summary>
    /// Custom converter class to deserialize the abstract class ShowWhenBase into one of its
    /// Concrete classes.
    ///
    /// Typeically can rely on the standard JsonConverter that looks at the $type variable to know
    /// what Concrete class to deserialize to. However, the value from $type is specific to a Media
    /// class, and not BabelFish, so the values don't match. Thus, we need to write our own converter.
    ///
    /// Recipe comes from https://stackoverflow.com/questions/20995865/deserializing-json-to-abstract-class
    /// </summary>
    public class ShowWhenBaseConverter : JsonConverter {

        static JsonSerializerSettings SpecifiedSubclassConversion = new JsonSerializerSettings() { ContractResolver = new ShowWhenBaseSpecifiedConcreteClassConverter() };

        public override bool CanConvert( Type objectType ) {
            return (objectType == typeof( ShowWhenBase ));
        }

        public override object ReadJson( JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer ) {
            JObject jo = JObject.Load( reader );

            var id = jo["Operation"]?.Value<string>();

            switch (id) {
                case "EQUATION" :
                    return JsonConvert.DeserializeObject<ShowWhenEquation>( jo.ToString(), SpecifiedSubclassConversion );
                case "VARIABLE" :
                    return JsonConvert.DeserializeObject<ShowWhenVariable>( jo.ToString(), SpecifiedSubclassConversion );
                case "SEGMENT_GROUP":
                    return JsonConvert.DeserializeObject<ShowWhenSegmentGroup>( jo.ToString(), SpecifiedSubclassConversion );
                default:
                    break;
            }

            //If we get here, give up. 
            throw new NotImplementedException( $"Unable to convert type '{id}' to an Abstract class ShowWhenBase." );
        }

        public override bool CanWrite { get { return false; } }

        public override void WriteJson( JsonWriter writer, object value, JsonSerializer serializer ) {
            //When CanWrite is false, which it is, the standard converter is used and not this custom converter
        }
    }

    public class ShowWhenBaseSpecifiedConcreteClassConverter : DefaultContractResolver {
        protected override JsonConverter ResolveContractConverter( Type objectType ) {
            if (typeof( ShowWhenBase ).IsAssignableFrom( objectType ) && !objectType.IsAbstract)
                return null; // pretend TableSortRuleConvert is not specified (thus avoiding a stack overflow)
            return base.ResolveContractConverter( objectType );
        }
    }
}
