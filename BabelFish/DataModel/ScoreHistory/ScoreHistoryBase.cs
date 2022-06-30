using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace ShootersTech.DataModel.ScoreHistory {

    /// <summary>
    /// Base class for a Score History Response object .... needed so the compiler doesn't scream at us.
    /// </summary>
    [Serializable]
    [JsonConverter( typeof( ScoreAverageBaseConverter ) )]
    public abstract class ScoreHistoryBase : IDeserializableAbstractClass {

        /// <summary>
        /// The number of shots fired within this ScoreHistoryEntry
        /// </summary>
        public int NumberOfShots { get; set; } = 0;

        /// <summary>
        /// GUID formatted User ID of the athlete who shot this score.
        /// </summary>
        public string UserId { get; set; } = string.Empty;

        /// <summary>
        /// The value of ConcreteClassId should be set in the Constructor of the
        /// Concrete class. Users
        /// should not change this value ... unless they want really bizarre and
        /// uncontrolled behavior. 
        /// </summary>
        public int ConcreteClassId { get; set; }
    }

    public class ScoreHistoryBaseSpecifiedConcreteClassConverter : DefaultContractResolver {
        protected override JsonConverter ResolveContractConverter( Type objectType ) {
            if (typeof( ScoreHistoryBase ).IsAssignableFrom( objectType ) && !objectType.IsAbstract)
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
    public class ScoreHistoryBaseConverter : JsonConverter {
        static JsonSerializerSettings SpecifiedSubclassConversion = new JsonSerializerSettings() { ContractResolver = new ScoreHistoryBaseSpecifiedConcreteClassConverter() };

        public override bool CanConvert( Type objectType ) {
            return (objectType == typeof( ScoreHistoryBase ));
        }

        public override object ReadJson( JsonReader reader, Type objectType, object existingValue,
            JsonSerializer serializer ) {
            JObject jo = JObject.Load( reader );

            //first try using the ConcreteClassId, if it is a property of the json, as this will be a faster method.
            var id = jo["ConcreteClassId"]?.Value<int>();

            switch (id) {
                case ScoreHistoryEventStyleEntry.CONCRETE_CLASS_ID:
                    return JsonConvert.DeserializeObject<ScoreHistoryEventStyleEntry>( jo.ToString(), SpecifiedSubclassConversion );
                case ScoreHistoryStageStyleEntry.CONCRETE_CLASS_ID:
                    return JsonConvert.DeserializeObject<ScoreHistoryStageStyleEntry>( jo.ToString(), SpecifiedSubclassConversion );
                case ScoreHistoryEventStyleTimespan.CONCRETE_CLASS_ID:
                    return JsonConvert.DeserializeObject<ScoreHistoryEventStyleTimespan>( jo.ToString(), SpecifiedSubclassConversion );
                case ScoreHistoryStageStyleTimespan.CONCRETE_CLASS_ID:
                    return JsonConvert.DeserializeObject<ScoreHistoryStageStyleTimespan>( jo.ToString(), SpecifiedSubclassConversion );
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
