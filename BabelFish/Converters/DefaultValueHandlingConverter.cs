using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Scopos.BabelFish.Converters {

    /// <summary>
    /// When the JSON to be serialized has null for a non-nullable tyhpe (say an int), this will intercept the 
    /// deserialization and populate the property with the C# default value for the type. To use, apply the following
    /// to the property: 
    /// [JsonConverter( typeof( DefaultValueHandlingConverter<int> ) )]
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DefaultValueHandlingConverter<T> : JsonConverter<T> {

        public override T Read( ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options ) {

            if (reader.TokenType == JsonTokenType.Null) {

                return default( T );
            }

            return JsonSerializer.Deserialize<T>( ref reader, options );
        }

        public override void Write( Utf8JsonWriter writer, T value, JsonSerializerOptions options ) {
            JsonSerializer.Serialize( writer, value, options );
        }
    }

}
