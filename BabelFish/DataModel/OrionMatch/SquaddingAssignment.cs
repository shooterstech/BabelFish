using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace ShootersTech.BabelFish.DataModel.OrionMatch {
    /// <summary>
    /// Abstract class representing the complete squadding assignment for one participant (athlete or team).
    /// </summary>
    [Serializable]
    [JsonConverter( typeof( SquaddingAssignmentConverter ) )]
    public abstract class SquaddingAssignment: IDeserializableAbstractClass  {

        public SquaddingAssignment() { }

        public string Range { get; set; } = string.Empty;

        public int RangeSortOrder { get; set; } = 1;

        public int FiringOrder { get; set; } = 0;

        public Participant Participant { get; set; } = new Individual();

        /// <summary>
        /// Implementation of the IDeserializableAbstractClass interface.
        /// To have added control over the Deserialization of abstract classes, in to
        /// Concrete classes, the JSON should include a ConcreteClassId that specifies
        /// the Concrete class.
        /// </summary>
        public int ConcreteClassId { get; set; }

    }
    public class SquaddingAssignmentSpecifiedConcreteClassConverter : DefaultContractResolver {
        protected override JsonConverter ResolveContractConverter( Type objectType ) {
            if (typeof( SquaddingAssignment ).IsAssignableFrom( objectType ) && !objectType.IsAbstract)
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
    public class SquaddingAssignmentConverter : JsonConverter {
        static JsonSerializerSettings SpecifiedSubclassConversion = new JsonSerializerSettings() { ContractResolver = new SquaddingAssignmentSpecifiedConcreteClassConverter() };

        public override bool CanConvert( Type objectType ) {
            return (objectType == typeof( SquaddingAssignment ));
        }

        public override object ReadJson( JsonReader reader, Type objectType, object existingValue,
            JsonSerializer serializer ) {
            JObject jo = JObject.Load( reader );

            //first try using the ConcreteClassId, if it is a property of the json, as this will be a faster method.
            var id = jo["ConcreteClassId"]?.Value<int>();

            switch (id) {
                case SquaddingAssignmentFiringPoint.CONCRETE_CLASS_ID:
                    return JsonConvert.DeserializeObject<SquaddingAssignmentFiringPoint>( jo.ToString(), SpecifiedSubclassConversion );
                case SquaddingAssignmentBank.CONCRETE_CLASS_ID:
                    return JsonConvert.DeserializeObject<SquaddingAssignmentBank>( jo.ToString(), SpecifiedSubclassConversion );
                case SquaddingAssignmentSquad.CONCRETE_CLASS_ID:
                    return JsonConvert.DeserializeObject<SquaddingAssignmentSquad>( jo.ToString(), SpecifiedSubclassConversion );
                default:
                    break;
            }

            //If ConcreteClassId is not in the json, rely instead on the $type value.
            var type = jo["$type"]?.Value<string>();
            if (type != null) {
                if (type.Contains( "FiringPoint" ))
                    return JsonConvert.DeserializeObject<SquaddingAssignmentFiringPoint>( jo.ToString(), SpecifiedSubclassConversion );
                else if (type.Contains( "Bank" ))
                    return JsonConvert.DeserializeObject<SquaddingAssignmentBank>( jo.ToString(), SpecifiedSubclassConversion );
                else if (type.Contains( "Squad" ))
                    return JsonConvert.DeserializeObject<SquaddingAssignmentSquad>( jo.ToString(), SpecifiedSubclassConversion );
            }

            //If we get here, give up. 
            throw new NotImplementedException( $"Unable to convert type '{type}' to an Abstract class SquaddingAssignment." );
        }

        public override bool CanWrite {
            get { return false; }
        }

        public override void WriteJson( JsonWriter writer, object value, JsonSerializer serializer ) {
            throw new NotImplementedException(); // won't be called because CanWrite returns false
        }
    }
}
