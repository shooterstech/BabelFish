using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Net;

namespace Scopos.BabelFish.Converters {
    public class HttpStatusCodeConverter : JsonConverter<HttpStatusCode> {

        /*
         * Code stolen from CoPilot
         */

        public override HttpStatusCode Read( ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options ) {
            if (reader.TokenType == JsonTokenType.Number) {
                int statusCodeValue = reader.GetInt32(); 
                return (HttpStatusCode)statusCodeValue;
            } else if (reader.TokenType == JsonTokenType.String) {
                string statusCodeString = reader.GetString();
                return (HttpStatusCode) Enum.Parse( typeof( HttpStatusCode), statusCodeString );
            }

            throw new JsonException();
        }

        public override void Write( Utf8JsonWriter writer, HttpStatusCode value, JsonSerializerOptions options ) {
            writer.WriteNumberValue( (int)value );
        }

    }
}
