using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;


namespace Scopos.BabelFish.Converters.Microsoft {


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
    /*
    public class IgnoreDefaultConverter<T> : JsonConverter<T> {
        private readonly T _ignoreValue;

        public IgnoreDefaultConverter( T ignoreValue ) {
            _ignoreValue = ignoreValue;
        }

        public override T Read( ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options ) {

                return JsonSerializer.Deserialize<T>( ref reader, options );
        }

        public override void Write( Utf8JsonWriter writer, T value, JsonSerializerOptions options ) {
            if (!EqualityComparer<T>.Default.Equals( value, _ignoreValue )
                && value != null) {
                JsonSerializer.Serialize( writer, value, options );
            }
        }
    }


    /// <summary>
    /// When Serializing, does not write to JSON if the value of the Property is 0.
    /// <para>When Deserializing, returns 0 if the value is null or missing. Otherwise returns the JSON value.</para>
    /// <para>To use, decorate the property with [JsonConverter( typeof( ExcludeZeroIntConverter ) )]</para>
    /// </summary>
    public class ExcludeZeroIntConverter : JsonConverter<int> {
        public override int Read( ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options ) {

            if (reader.TokenType == JsonTokenType.Null) {

                return 0;
            }

            return reader.GetInt32();
        }

        public override void Write( Utf8JsonWriter writer, int value, JsonSerializerOptions options ) {

            if (value != 0) {
                writer.WriteNumberValue( value );
            }
        }
    }

    /// <summary>
    /// When Serializing, does not write to JSON if the value of the Property is 0.
    /// <para>When Deserializing, returns 0 if the value is null or missing. Otherwise returns the JSON value.</para>
    /// <para>To use, decorate the property with [JsonConverter( typeof( ExcludeZeroFloatConverter ) )]</para>
    /// </summary>
    public class ExcludeZeroFloatConverter : JsonConverter<float> {
        public override float Read( ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options ) {

            if (reader.TokenType == JsonTokenType.Null) {

                return 0;
            }

            return reader.GetSingle();
        }

        public override void Write( Utf8JsonWriter writer, float value, JsonSerializerOptions options ) {

            if (value != 0) {
                writer.WriteNumberValue( value );
            }
        }
    }

    /// <summary>
    /// When Serializing, does not write to JSON if the value of the Property is an empty string or null.
    /// <para>When Deserializing, returns an empty string if the value is null or missing. Otherwise returns the JSON value.</para>
    /// <para>To use, decorate the property with [JsonConverter( typeof( ExcludeEmptryStringConverter ) )]</para>
    /// </summary>
    public class ExcludeEmptyStringConverter : JsonConverter<string> {
        public override string Read( ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options ) {

            if (reader.TokenType == JsonTokenType.Null) {

                return string.Empty;
            }

            return JsonSerializer.Deserialize<string>( ref reader, options );
        }

        public override void Write( Utf8JsonWriter writer, string value, JsonSerializerOptions options ) {

            if (! string.IsNullOrEmpty(value) ) {
                JsonSerializer.Serialize( writer, value, options );
            }
        }
    }
*/
