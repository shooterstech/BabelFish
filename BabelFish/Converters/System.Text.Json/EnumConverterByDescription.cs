using System;
using System.ComponentModel; 
using System.Linq; 
using System.Reflection; 
using System.Text.Json;
using System.Text.Json.Serialization;
using NLog;
using Scopos.BabelFish.APIClients;

namespace Scopos.BabelFish.Converters.Microsoft {

	/// <summary>
	/// Json converter for Enums. Uses an enum's Description first, if defined, else the enum string value.
	/// </summary>
	/// <remarks>
	/// As a general rule, enums properties should be decorated with the following JsonProperty to always wrtie their value when JSON is serialized.
	/// [G_NS.JsonProperty( DefaultValueHandling = Newtonsoft.Json.DefaultValueHandling.Include )]
	/// </remarks>
	/// <typeparam name="T"></typeparam>
	public class EnumConverterByDescription<T> : JsonConverter<T> where T : Enum {


        protected static Logger Logger = LogManager.GetCurrentClassLogger();

        /// <inheritdoc />
        public override T Read( ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options ) { 
            string description = reader.GetString(); 
            foreach (var field in typeToConvert.GetFields()) {
                //Try using the description first
                if (field.GetCustomAttribute<DescriptionAttribute>()?.Description == description) { 
                    return (T)field.GetValue( null ); 
                }
                //Second attempt using the name
                if (field.Name == description) {
                    return (T)field.GetValue( null );
                }
            } 

            //Open ended question, should I return a default value.

            throw new ScoposAPIException( $"Unknown description '{description}'", Logger ); 
        }


        /// <inheritdoc />
        public override void Write( Utf8JsonWriter writer, T value, JsonSerializerOptions options ) {

            var field = value.GetType().GetField( value.ToString() );
            var description = field.GetCustomAttribute<DescriptionAttribute>()?.Description ?? value.ToString();
            writer.WriteStringValue( description );
        }
    }
}
