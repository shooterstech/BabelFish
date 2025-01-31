using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Scopos.BabelFish.DataModel.Definitions;
using Scopos.BabelFish.Helpers;

namespace Scopos.BabelFish.Converters.Microsoft {

    /// <summary>
    /// Custom converter class to deserialize the abstract class ShowWhenBase into one of its
    /// Concrete classes.
    /// </summary>
    public class ShowWhenBaseConverter : JsonConverter<ShowWhenBase> {

        /*
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
                    //If we get here, it is probable because of ill-formed json
                    return ShowWhenVariable.ALWAYS_SHOW.Copy();
            }
        }

        public override bool CanWrite { get { return false; } }

        public override void WriteJson( JsonWriter writer, object value, JsonSerializer serializer ) {
            //When CanWrite is false, which it is, the standard converter is used and not this custom converter
        }
        */

        public override ShowWhenBase? Read( ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options ) {

            using (JsonDocument doc = JsonDocument.ParseValue( ref reader )) {
                JsonElement root = doc.RootElement;
                string operation = root.GetProperty( "Operation" ).GetString();

                switch (operation) {
                    case "EQUATION":
                        return JsonSerializer.Deserialize<ShowWhenEquation>( root.GetRawText(), options );
                    case "VARIABLE":
                        return JsonSerializer.Deserialize<ShowWhenVariable>( root.GetRawText(), options );
                    case "SEGMENT_GROUP":
                        return JsonSerializer.Deserialize<ShowWhenSegmentGroup>( root.GetRawText(), options );
                    default:
                        //If we get here, it is probable because of ill-formed json
                        return ShowWhenVariable.ALWAYS_SHOW.Clone();
                }
            }
        }

        public override void Write( Utf8JsonWriter writer, ShowWhenBase value, JsonSerializerOptions options ) {
            JsonSerializer.Serialize( writer, value, value.GetType(), options );
        }
    }
}
