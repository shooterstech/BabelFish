using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Scopos.BabelFish.DataModel.Definitions;

namespace Scopos.BabelFish.Converters {
    public class DefinitionConverter : JsonConverter<Definition> {

        /*

		static JsonSerializerSettings SpecifiedSubclassConversion = new JsonSerializerSettings() { ContractResolver = new DefinitionConcreteClassConverter() };

		public override bool CanConvert( Type objectType ) {
			return (objectType == typeof( Definition ));
		}

		public override object ReadJson( JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer ) {
			JObject jo = JObject.Load( reader );

			var type = jo["Type"]?.Value<string>();

			switch (type) {
				case "ATTRIBUTE":
					return JsonConvert.DeserializeObject<Scopos.BabelFish.DataModel.Definitions.Attribute>( jo.ToString(), SpecifiedSubclassConversion );
				case "COURSE OF FIRE":
					return JsonConvert.DeserializeObject<CourseOfFire>( jo.ToString(), SpecifiedSubclassConversion );
				case "EVENT STYLE":
					return JsonConvert.DeserializeObject<EventStyle>( jo.ToString(), SpecifiedSubclassConversion );
				case "EVENT AND STAGE STYLE MAPPING":
					return JsonConvert.DeserializeObject<EventAndStageStyleMapping>( jo.ToString(), SpecifiedSubclassConversion );
				case "RANKING RULES":
					return JsonConvert.DeserializeObject<RankingRule>( jo.ToString(), SpecifiedSubclassConversion );
				case "RESULT LIST FORMAT":
					return JsonConvert.DeserializeObject<ResultListFormat>( jo.ToString(), SpecifiedSubclassConversion );
				case "SCORE FORMAT COLLECTION":
					return JsonConvert.DeserializeObject<ScoreFormatCollection>( jo.ToString(), SpecifiedSubclassConversion );
				case "STAGE STYLE":
					return JsonConvert.DeserializeObject<StageStyle>( jo.ToString(), SpecifiedSubclassConversion );
				case "TARGET":
					return JsonConvert.DeserializeObject<Target>( jo.ToString(), SpecifiedSubclassConversion );
				case "TARGET COLLECTION":
					return JsonConvert.DeserializeObject<TargetCollection>( jo.ToString(), SpecifiedSubclassConversion );
				default:
					//If we get here, it is probable because of ill-formed json
					throw new NotImplementedException( $"Have no idea what definition type '{type}' is." );
			}
		}

		public override bool CanWrite { get { return false; } }

		public override void WriteJson( JsonWriter writer, object value, JsonSerializer serializer ) {
			//When CanWrite is false, which it is, the standard converter is used and not this custom converter
		}

		*/
        public override Definition? Read( ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options ) {

            using (JsonDocument doc = JsonDocument.ParseValue( ref reader )) {
                JsonElement root = doc.RootElement;
                string type = root.GetProperty( "Type" ).GetString();

                switch (type) {
                    case "ATTRIBUTE":
                        return JsonSerializer.Deserialize<Scopos.BabelFish.DataModel.Definitions.Attribute>( root.GetRawText(), options );
                    case "COURSE OF FIRE":
                        return JsonSerializer.Deserialize<CourseOfFire>( root.GetRawText(), options );
                    case "EVENT STYLE":
                        return JsonSerializer.Deserialize<EventStyle>( root.GetRawText(), options );
                    case "EVENT AND STAGE STYLE MAPPING":
                        return JsonSerializer.Deserialize<EventAndStageStyleMapping>( root.GetRawText(), options );
                    case "RANKING RULES":
                        return JsonSerializer.Deserialize<RankingRule>( root.GetRawText(), options );
                    case "RESULT LIST FORMAT":
                        return JsonSerializer.Deserialize<ResultListFormat>( root.GetRawText(), options );
                    case "SCORE FORMAT COLLECTION":
                        return JsonSerializer.Deserialize<ScoreFormatCollection>( root.GetRawText(), options );
                    case "STAGE STYLE":
                        return JsonSerializer.Deserialize<StageStyle>( root.GetRawText(), options );
                    case "TARGET":
                        return JsonSerializer.Deserialize<Target>( root.GetRawText(), options );
                    case "TARGET COLLECTION":
                        return JsonSerializer.Deserialize<TargetCollection>( root.GetRawText(), options );
                    default:
                        throw new NotImplementedException( $"Have no idea what definition type '{type}' is." );
                }
            }
        }

        public override void Write( Utf8JsonWriter writer, Definition value, JsonSerializerOptions options ) {
            JsonSerializer.Serialize( writer, value, value.GetType(), options );
        }
    }
}
