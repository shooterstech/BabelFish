using Scopos.BabelFish.DataModel.Definitions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Scopos.BabelFish.Converters {
    public class AttributeValidationConverter : JsonConverter<AttributeValidation> {
        public override AttributeValidation? Read( ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options ) {

            using (JsonDocument doc = JsonDocument.ParseValue( ref reader )) {
                JsonElement root = doc.RootElement;
                string valueType = root.GetProperty( "ValueType" ).GetString();

                switch (valueType) {
                    case "DATE":
                        return JsonSerializer.Deserialize<AttributeValidationDate>( root.GetRawText(), options );

                    case "TIME SPAN":
                        return JsonSerializer.Deserialize<AttributeValidationTimeSpan>( root.GetRawText(), options );

                    case "STRING":
                        return JsonSerializer.Deserialize<AttributeValidationString>( root.GetRawText(), options );

                    case "BOOLEAN":
                        return JsonSerializer.Deserialize<AttributeValidationBoolean>( root.GetRawText(), options );

                    case "INTEGER":
                        return JsonSerializer.Deserialize<AttributeValidationInteger>( root.GetRawText(), options );

                    case "FLOAT":
                        return JsonSerializer.Deserialize<AttributeValidationFloat>( root.GetRawText(), options );

                    default:

                        //If we get here, give up. 
                        throw new NotImplementedException( $"Unable to convert type '{valueType}' to an Abstract class AttributeField." );
                }
            }
        }

        public override void Write( Utf8JsonWriter writer, AttributeValidation value, JsonSerializerOptions options ) {
            JsonSerializer.Serialize( writer, value, value.GetType(), options );
        }
    }
}
