using System;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;
using NLog;
using Scopos.BabelFish.APIClients;

namespace Scopos.BabelFish.Converters {

    /// <summary>
    /// Abstract JsonConverter to serialize / deserialize DateTime properties using custom date time formats.
    /// </summary>
    public abstract class JsonDateTimeConverter : JsonConverter<DateTime> {

        protected string DateTimeFormat = Helpers.DateTimeFormats.DATETIME_FORMAT;
        protected string DateTimeFormatSecondary = null;
        protected DateTime DefaultValue = DateTime.Now;
        protected bool UseDefaultAsLastResort = false;

        protected Logger Logger = LogManager.GetCurrentClassLogger();

        /// <inheritdoc />
        public override DateTime Read( ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options ) {
            
            string dateString = reader.GetString();

            if (!string.IsNullOrEmpty( dateString )) {
                //Try parsing with the expected format first
                try {
                    return DateTime.ParseExact( dateString, DateTimeFormat, Helpers.DateTimeFormats.CULTURE );
                } catch (FormatException fe) {
                    Logger.Warn( $"Could not parse DateTime string '{dateString}' using the format '{DateTimeFormat}'." );
                }

                //Next try parsing with the secondary format, if one was specified
                try {
                    if (!string.IsNullOrEmpty( DateTimeFormatSecondary ))
                        return DateTime.ParseExact( dateString, DateTimeFormatSecondary, Helpers.DateTimeFormats.CULTURE );
                } catch (FormatException fe) {
                    Logger.Warn( $"Could not parse DateTime string '{dateString}' using the secondary format '{DateTimeFormatSecondary}'." );
                }

                //Last effort, try parsing generically
                try {
                    return DateTime.Parse( dateString );
                } catch (FormatException fe) {
                    Logger.Warn( $"Could not parse DateTime string '{dateString}' using a generic parser." );
                }
            }

            if (UseDefaultAsLastResort) {
                Logger.Warn( $"Returning a Default DateTime value because the following could not be parsed '{dateString}'.");
                return DefaultValue;
            }

            var msg = "Could not parse the DateTime value {dateString}.";
            throw new ScoposAPIException( msg, Logger );
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
            base.UseDefaultAsLastResort = true;
            base.DefaultValue = DateTime.Today;
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
            base.DateTimeFormatSecondary = Helpers.DateTimeFormats.DATETIME_FORMAT_SECONDARY;
            base.UseDefaultAsLastResort = true;
            base.DefaultValue = DateTime.UtcNow;
        }
    }
}
