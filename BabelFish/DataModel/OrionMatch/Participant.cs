using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace ShootersTech.DataModel.OrionMatch {
    /// <summary>
    /// A Participant is anyone who has a role in a Match. This includes athletes, teams, match officials, and coaches.
    /// </summary>
    [Serializable]
    [JsonConverter(typeof(ParticipantConverter))]
    public abstract class Participant: IDeserializableAbstractClass {

        public Participant() { }

        /// <summary>
        /// A unique, human readable, value assigned to all Participants in a match.
        /// 
        /// In most cases the CompetitorNumber will be numeric, but it can also be alphabetical.
        /// </summary>
        public string CompetitorNumber { get; set; } = string.Empty;

        public List<AttributeValue> AttributeValues { get; set; } = new List<AttributeValue>();

        /// <summary>
        /// When a competitor's name is displayed, this is the default display value.
        /// </summary>
        public string DisplayName { get; set; } = string.Empty;

        /// <summary>
        /// The three letter country code the participant is from.
        /// </summary>
        public string Country { get; set; } = string.Empty;

        /// <summary>
        /// The Hometown the participant is from.
        /// </summary>
        public string HomeTown { get; set; } = string.Empty;

        /// <summary>
        /// $type coming from API data
        /// </summary>
        [JsonProperty(PropertyName = "$type")]
        public string type{ get; set; } = string.Empty;

        /// <summary>
        /// When a competitor's name is displayed, and there is limited number of characters, use this value. 
        /// 
        /// There is no rule as to how long the Short value could be, but by convention 12 characters or less.
        /// </summary>
        public string DisplayNameShort { get; set; } = string.Empty;

        //TODO: Club, ReentryTag not in API return data
        /// <summary>
        /// The Hometown Club the Participant represents. Note, this is NOT the same as any team the Participant is shooting with. 
        /// </summary>
        public string Club { get; set; } = string.Empty;

        public string ReentryTag { get; set; } = string.Empty;

        public override string ToString() {
            return this.DisplayName;
        }

    }


    public class ParticipantSpecifiedConcreteClassConverter : DefaultContractResolver {
        protected override JsonConverter ResolveContractConverter( Type objectType ) {
            if (typeof( Participant ).IsAssignableFrom( objectType ) && !objectType.IsAbstract)
                return null; // pretend TableSortRuleConvert is not specified (thus avoiding a stack overflow)
            return base.ResolveContractConverter( objectType );
        }
    }

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

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
            JsonSerializer serializer) {
            JObject jo = JObject.Load(reader);

            //first try using the ConcreteClassId, if it is a property of the json, as this will be a faster method.
            var id = jo["ConcreteClassId"]?.Value<int>();

            switch (id) {
                case Individual.CONCRETE_CLASS_ID:
                    return JsonConvert.DeserializeObject<Individual>(jo.ToString(), SpecifiedSubclassConversion);
                case Team.CONCRETE_CLASS_ID:
                    return JsonConvert.DeserializeObject<Team>(jo.ToString(), SpecifiedSubclassConversion);
                default:
                    break;
            }

            //If ConcreteClassId is not in the json, rely instead on the $type value.
            var type = jo["$type"]?.Value<string>();
            if (type != null) {
                if (type.Contains("Individual"))
                    return JsonConvert.DeserializeObject<Individual>(jo.ToString(), SpecifiedSubclassConversion);
                else if (type.Contains("Team"))
                    return JsonConvert.DeserializeObject<Team>(jo.ToString(), SpecifiedSubclassConversion);
            }

            //If we get here, give up. 
            throw new NotImplementedException($"Unable to convert type '{type}' to an Abstract class Participant.");
        }

        public override bool CanWrite {
            get { return false; }
        }

        public override void WriteJson( JsonWriter writer, object value, JsonSerializer serializer ) {
            throw new NotImplementedException(); // won't be called because CanWrite returns false
        }
    }
}
