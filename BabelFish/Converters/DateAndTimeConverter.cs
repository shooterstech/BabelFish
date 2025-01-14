using System;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Scopos.BabelFish.Converters {

    /// <summary>
    /// Abstract JsonConverter to serialize / deserialize DateTime properties using custom date time formats.
    /// </summary>
    public abstract class JsonDateTimeConverter : JsonConverter<DateTime> {

        protected string DateTimeFormat = Helpers.DateTimeFormats.DATETIME_FORMAT;

        /// <inheritdoc />
        public override DateTime Read( ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options ) {
            
            string dateString = reader.GetString();
            return DateTime.ParseExact( dateString, DateTimeFormat, Helpers.DateTimeFormats.CULTURE );
        }

        /// <inheritdoc />
        public override void Write( Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options ) {
            writer.WriteStringValue(value.ToString( DateTimeFormat ) );
        }
    }

    /// <summary>
    /// JsonConverter to serialize / deserialize DateTime properties with the format "yyyy-MM-dd"
    /// </summary>
    public class ScoposDateOnlyConverter : JsonDateTimeConverter {

        /// <summary>
        /// Public constructor
        /// </summary>
        public ScoposDateOnlyConverter() {
            base.DateTimeFormat = Helpers.DateTimeFormats.DATE_FORMAT;
        }
    }

    /// <summary>
    /// JsonConverter to serialize / deserialize DateTime properties with the format "yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'ffffffK"
    /// </summary>
    public class ScoposDateTimeConverter : JsonDateTimeConverter {

        /// <summary>
        /// Public constructor
        /// </summary>
        public ScoposDateTimeConverter() {
            base.DateTimeFormat = Helpers.DateTimeFormats.DATETIME_FORMAT;
        }
    }

    /// <summary>
    /// JsonConverter to serialize / deserialize DateTime properties with the format "hh\\:mm\\:ss\\.ffffff"
    /// </summary>
    [Obsolete("Time should be serialized to a float, representing the number of seconds.")]
    public class ScoposTimeOnlyConverter : JsonDateTimeConverter {

        /// <summary>
        /// Public constructor
        /// </summary>
        public ScoposTimeOnlyConverter() {
            base.DateTimeFormat = Helpers.DateTimeFormats.TIME_FORMAT;
        }
    }
}
