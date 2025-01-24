using Scopos.BabelFish.DataModel.Definitions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Scopos.BabelFish.Converters {
    public class AttributeFieldConverter : JsonConverter<AttributeField> {
        public override AttributeField? Read( ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options ) {

            using (JsonDocument doc = JsonDocument.ParseValue( ref reader )) {
                JsonElement root = doc.RootElement;
                bool multipleValues = root.GetProperty( "MultipleValues" ).GetBoolean();
                string valueType = root.GetProperty( "ValueType" ).GetString();

                switch (valueType) {
                    case "DATE":
                        if (multipleValues)
                            return JsonSerializer.Deserialize<AttributeFieldDateList>( root.GetRawText(), options );
                        else
                            return JsonSerializer.Deserialize<AttributeFieldDate>( root.GetRawText(), options );

                    case "DATE TIME":
                        if (multipleValues)
                            return JsonSerializer.Deserialize<AttributeFieldDateTimeList>( root.GetRawText(), options );
                        else
                            return JsonSerializer.Deserialize<AttributeFieldDateTime>( root.GetRawText(), options );

                    case "TIME SPAN":
                        if (multipleValues)
                            return JsonSerializer.Deserialize<AttributeFieldTimeSpanList>( root.GetRawText(), options );
                        else 
                            return JsonSerializer.Deserialize<AttributeFieldTimeSpan>( root.GetRawText(), options );

                    case "STRING":
                        if (multipleValues)
                            return JsonSerializer.Deserialize<AttributeFieldStringList>( root.GetRawText(), options );
                        else
                            return JsonSerializer.Deserialize<AttributeFieldString>( root.GetRawText(), options );

                    case "BOOLEAN":
                        //NOTE: We do not allow lists of booleans
                        return JsonSerializer.Deserialize<AttributeFieldBoolean>( root.GetRawText(), options );

                    case "INTEGER":
                        if (multipleValues)
                            return JsonSerializer.Deserialize<AttributeFieldIntegerList>( root.GetRawText(), options );
                        else
                            return JsonSerializer.Deserialize<AttributeFieldInteger>( root.GetRawText(), options );

                    case "FLOAT":
                        if (multipleValues)
                            return JsonSerializer.Deserialize<AttributeFieldFloatList>( root.GetRawText(), options );
                        else
                            return JsonSerializer.Deserialize<AttributeFieldFloat>( root.GetRawText(), options );

                    default:

                        //If we get here, give up. 
                        throw new NotImplementedException( $"Unable to convert type '{valueType}' to an Abstract class AttributeField." );
                }
            }
        }

        public override void Write( Utf8JsonWriter writer, AttributeField value, JsonSerializerOptions options ) {
            JsonSerializer.Serialize( writer, value, value.GetType(), options );
        }
    }
}
