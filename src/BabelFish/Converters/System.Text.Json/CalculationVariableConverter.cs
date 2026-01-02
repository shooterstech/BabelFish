
using System.Text.Json;
using System.Text.Json.Serialization;
using Scopos.BabelFish.DataModel.Definitions;

namespace Scopos.BabelFish.Converters.Microsoft
{
    public class CalculationVariableConverter : JsonConverter<CalculationVariable>
    {
        public override CalculationVariable Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            using (JsonDocument doc = JsonDocument.ParseValue(ref reader))
            {
                JsonElement root = doc.RootElement;
                string operation = string.Empty;
                JsonElement jsonElementValue;
                if (root.TryGetProperty( "VariableType", out jsonElementValue))
                    operation = jsonElementValue.GetString();
                else
                    operation = string.Empty;

                switch (operation)
                {
                    case "INTEGER":
                        return JsonSerializer.Deserialize<CalculationVariableInteger>(root.GetRawText(), options);

					case "FLOAT":
						return JsonSerializer.Deserialize<CalculationVariableFloat>( root.GetRawText(), options );

					case "STRING":
						return JsonSerializer.Deserialize<CalculationVariableString>( root.GetRawText(), options );

					case "SCORE":
						return JsonSerializer.Deserialize<CalculationVariableScoreComponent>( root.GetRawText(), options );

					default:
                        //If we get here, it is probable because of ill-formed json
                        return new CalculationVariableString() { 
                            Comment = $"Unable to deserialize. Unable to interpret VariableType {operation}" 
                        };
                }
            }
        }

        public override void Write(Utf8JsonWriter writer, CalculationVariable value, JsonSerializerOptions options)
        {
            JsonSerializer.Serialize(writer, value, value.GetType(), options);
        }
    }
}
