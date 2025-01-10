using System;
using System.ComponentModel; 
using System.Linq; 
using System.Reflection; 
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Scopos.BabelFish.Converters {

    /// <summary>
    /// Json converter for Enums. Uses an enum's Description first, if defined, else the enum string value.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class EnumConverterByDescription<T> : JsonConverter<T> where T : Enum {

        /// <inheritdoc />
        public override T Read( ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options ) { 
            string description = reader.GetString(); 
            foreach (var field in typeToConvert.GetFields()) { 
                //Try using the description first
                if (field.GetCustomAttribute<DescriptionAttribute>()?.Description == description) { 
                    return (T)field.GetValue( null ); 
                }
                //Second attempt using the name
                if (field.ToString() == description) {
                    return (T)field.GetValue( null );
                }
            } 
            throw new JsonException( $"Unknown description '{description}'" ); 
        }


        /// <inheritdoc />
        public override void Write( Utf8JsonWriter writer, T value, JsonSerializerOptions options ) {
            
            var field = value.GetType().GetField( value.ToString() );
            var description = field.GetCustomAttribute<DescriptionAttribute>()?.Description ?? value.ToString();
            writer.WriteStringValue(description );
    }
}
