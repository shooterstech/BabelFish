using System.ComponentModel;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
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

        private bool _throwErrorOnUnknownDescription = true;
        protected T DefaultValue = default;
        protected static Logger Logger = LogManager.GetCurrentClassLogger();

        public EnumConverterByDescription( bool throwErrorOnUnknownDescription, T defaultValue ) {
            _throwErrorOnUnknownDescription = throwErrorOnUnknownDescription;

            if (defaultValue == null)
                throw new ArgumentNullException( nameof( defaultValue ), "Default value cannot be null." );

            var values = Enum.GetValues( typeof( T ) );
            if (!values.Cast<T>().Contains( defaultValue ))
                throw new ArgumentOutOfRangeException( nameof( defaultValue ), defaultValue, $"The value '{defaultValue}' is not defined for enum type '{typeof( T ).Name}'." );

            DefaultValue = defaultValue;
        }

        public EnumConverterByDescription() {
            _throwErrorOnUnknownDescription = true;

            var values = Enum.GetValues( typeof( T ) );
            if (values.Length == 0)
                throw new InvalidOperationException( $"Enum type '{typeof( T ).FullName}' must define at least one value." );

            DefaultValue = (T)values.GetValue( 0 );
        }

        /// <inheritdoc />
        public override T Read( ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options ) {
            string description = string.Empty;

            if (reader.TokenType == JsonTokenType.String) {
                description = reader.GetString();
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
            } else {
                reader.Skip();
                description = "unable to parse";
                Logger.Error( $"Error reading enum value of type {typeToConvert.Name}" );

            }

            // If we get here, we didn't find a matching description or name, or the type was not a string.
            // Return a default value if that's what the user wants, otherwise throw an exception.
            if (!_throwErrorOnUnknownDescription) {
                return DefaultValue;
            }

            throw new ScoposAPIException( $"Unknown description '{description}' for enum of type {typeToConvert.Name}", Logger );
        }


        /// <inheritdoc />
        public override void Write( Utf8JsonWriter writer, T value, JsonSerializerOptions options ) {

            var field = value.GetType().GetField( value.ToString() );
            var description = field.GetCustomAttribute<DescriptionAttribute>()?.Description ?? value.ToString();
            writer.WriteStringValue( description );
        }

        /// <inheritdoc />
        public override T ReadAsPropertyName( ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options ) {
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

            //Open ended question, should I return a default value?
            return default( T );

        }

        /// <inheritdoc />
        public override void WriteAsPropertyName( Utf8JsonWriter writer, T value, JsonSerializerOptions options ) {

            var field = value.GetType().GetField( value.ToString() );
            var description = field.GetCustomAttribute<DescriptionAttribute>()?.Description ?? value.ToString();
            writer.WriteStringValue( description );

        }
    }
}
