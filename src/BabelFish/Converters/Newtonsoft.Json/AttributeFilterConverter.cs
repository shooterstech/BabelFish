using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using Scopos.BabelFish.DataModel.OrionMatch;

namespace Scopos.BabelFish.Converters.Newtonsoft {
    public class AttributeFilterConverter : JsonConverter {

        static JsonSerializerSettings SpecifiedSubclassConversion = new JsonSerializerSettings() { ContractResolver = new AttributeFilterSpecifiedConcreteClassConverter() };

        public override bool CanConvert( Type objectType ) {
            return (objectType == typeof( AttributeFilter ));
        }

        public override object ReadJson( JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer ) {
            JObject jo = JObject.Load( reader );

            var id = jo["Operation"]?.Value<string>();

            switch (id) {
                case "ATTRIBUTE_VALUE":
                    return JsonConvert.DeserializeObject<AttributeFilterAttributeValue>( jo.ToString(), SpecifiedSubclassConversion );
                case "EQUATION":
                    return JsonConvert.DeserializeObject<AttributeFilterEquation>( jo.ToString(), SpecifiedSubclassConversion );
                default:
                    //If we get here, it is probable because of ill-formed json
                    return new AttributeFilterAttributeValue();
            }
        }

        public override bool CanWrite { get { return false; } }

        public override void WriteJson( JsonWriter writer, object value, JsonSerializer serializer ) {
            //When CanWrite is false, which it is, the standard converter is used and not this custom converter
        }
    }

    public class AttributeFilterSpecifiedConcreteClassConverter : DefaultContractResolver {
        protected override JsonConverter ResolveContractConverter( Type objectType ) {
            if (typeof( AttributeFilter ).IsAssignableFrom( objectType ) && !objectType.IsAbstract)
                return null; // pretend TableSortRuleConvert is not specified (thus avoiding a stack overflow)
            return base.ResolveContractConverter( objectType );
        }
    }
}
