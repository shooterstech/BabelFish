
using System.Text.Json;
using System.Text.Json.Serialization;
using Scopos.BabelFish.DataModel.Definitions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Scopos.BabelFish.Converters.Microsoft
{
    public class CommandAutomationConverter : JsonConverter<CommandAutomation>
    {
        public override CommandAutomation? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            using (JsonDocument doc = JsonDocument.ParseValue(ref reader))
            {
                JsonElement root = doc.RootElement;
                string operation = string.Empty;
                JsonElement jsonElementValue;
                if (root.TryGetProperty("Subject", out jsonElementValue))
                    operation = jsonElementValue.GetString();
                else
                    operation = string.Empty;

                switch (operation)
                {
                    case "REMARK":
                        return JsonSerializer.Deserialize<CommandAutomationRemark>(root.GetRawText(), options);
                    default:
                        //If we get here, it is probable because of ill-formed json
                        return JsonSerializer.Deserialize<CommandAutomationNone>(root.GetRawText(), options);
                }
            }
        }

        public override void Write(Utf8JsonWriter writer, CommandAutomation value, JsonSerializerOptions options)
        {
            JsonSerializer.Serialize(writer, value, value.GetType(), options);
        }
    }
}
