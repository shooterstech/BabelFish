using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Scopos.BabelFish.DataModel.ScoreHistory;

namespace Scopos.BabelFish.Converters {

    public class ScoreAverageBaseSpecifiedConcreteClassConverter : DefaultContractResolver {
        protected override JsonConverter ResolveContractConverter( Type objectType ) {
            if (typeof( ScoreAverageBase ).IsAssignableFrom( objectType ) && !objectType.IsAbstract)
                return null; // pretend TableSortRuleConvert is not specified (thus avoiding a stack overflow)
            return base.ResolveContractConverter( objectType );
        }
    }

    /// <summary>
    /// Custom converter class to deserialize the abstract class SquaddingAssignment into one of its
    /// Concrete classes.
    ///
    /// Typeically can rely on the standard JsonConverter that looks at the $type variable to know
    /// what Concrete class to deserialize to. However, the value from $type is specific to a Media
    /// class, and not BabelFish, so the values don't match. Thus, we need to write our own converter.
    ///
    /// Recipe comes from https://stackoverflow.com/questions/20995865/deserializing-json-to-abstract-class
    /// </summary>
    public class ScoreAverageBaseConverter : JsonConverter {
        static JsonSerializerSettings SpecifiedSubclassConversion = new JsonSerializerSettings() { ContractResolver = new ScoreAverageBaseSpecifiedConcreteClassConverter() };

        public override bool CanConvert( Type objectType ) {
            return (objectType == typeof( ScoreAverageBase ));
        }

        public override object ReadJson( JsonReader reader, Type objectType, object existingValue,
            JsonSerializer serializer ) {
            JObject jo = JObject.Load( reader );

            //first try using the ConcreteClassId, if it is a property of the json, as this will be a faster method.
            var id = jo["ConcreteClassId"]?.Value<int>();

            switch (id) {
                case ScoreAverageStageStyleEntry.CONCRETE_CLASS_ID:
                    return JsonConvert.DeserializeObject<ScoreAverageStageStyleEntry>( jo.ToString(), SpecifiedSubclassConversion );
                default:
                    break;
            }

            //If we get here, give up. 
            throw new NotImplementedException( $"Unable to convert abstract class ScoreHistoryBase with ConcreteClassId '{id}' to it's concrete class." );
        }

        public override bool CanWrite {
            get { return false; }
        }

        public override void WriteJson( JsonWriter writer, object value, JsonSerializer serializer ) {
            throw new NotImplementedException(); // won't be called because CanWrite returns false
        }
    }
}
