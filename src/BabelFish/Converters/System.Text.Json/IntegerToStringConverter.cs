using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using NLog;
using Scopos.BabelFish.APIClients;

namespace Scopos.BabelFish.Converters.Microsoft {
    /// <summary>
    /// Intended for use on string properties that may receive some of their data from the REST API as integers and not strings.
    /// This converter converts the integer value into a string.
    /// </summary>
    /// <remarks>To use, add the following to the Property
    /// <para>[JsonConverter(typeof(IntegerToStringConverter))]</para>
    /// </remarks>
    public class IntegerToStringConverter : JsonConverter<string> {

        protected static Logger Logger = LogManager.GetCurrentClassLogger();

        protected bool UseDefaultValue = true;
        protected string DefaultValue = string.Empty;

        public override string? Read( ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options ) {
            if (reader.TokenType == JsonTokenType.String) {
                return reader.GetString();
            } else if (reader.TokenType == JsonTokenType.Number) {
                int intValue;
                float floatValue;
                if (reader.TryGetInt32( out intValue ))
                    return intValue.ToString();
                if (reader.TryGetSingle( out floatValue ))
                    return floatValue.ToString(); //Should I format this?
            }

            //If we get here we got an unexpcted type. Should we throw an exception or should we return a default value? 
            if (UseDefaultValue) {
                return DefaultValue;
            } else {
                var msg = $"Unable to interpret, what should be a string, the value '{reader.GetString()}' which is of type '{reader.TokenType}'.";
                throw new ScoposAPIException( msg, Logger );
            }

        }

        public override void Write( Utf8JsonWriter writer, string value, JsonSerializerOptions options ) {
            writer.WriteStringValue( value );
        }
    }

    /// <summary>
    /// Intended for use on classes that implement the ITokenItems interface. This helps interpret the values
    /// returned for the NextToken property. 
    /// </summary>
    /// <remarks>To use, add the following to the Property
    /// <para>[JsonConverter(typeof(NextTokenConverter))]</para>
    /// </remarks>
    public class NextTokenConverter : IntegerToStringConverter {

        public NextTokenConverter() : base() {
            this.UseDefaultValue = true;
            this.DefaultValue = string.Empty;
        }
    }
}
