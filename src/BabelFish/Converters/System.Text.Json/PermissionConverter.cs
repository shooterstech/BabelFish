using System.Text.Json;
using System.Text.Json.Serialization;
using Scopos.BabelFish.DataModel.Common;

namespace Scopos.BabelFish.Converters.Microsoft {
    public class PermissionConverter : JsonConverter<Permission> {

        private Logger _logger = NLog.LogManager.GetCurrentClassLogger();

        /// <inheritdoc />
        public override Permission? Read( ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options ) {

            string permissionString = (string)reader.GetString();

            if (Permission.TryParse( permissionString, out Permission sn )) {
                return sn;
            }

            _logger.Error( $"Couldn't parse the incoming Permission value '{permissionString}'." );

            return Permission.DEFAULT;
        }

        /// <inheritdoc />
        public override void Write( Utf8JsonWriter writer, Permission value, JsonSerializerOptions options ) {

            writer.WriteStringValue( value.ToString() );
        }

        /// <inheritdoc />
        public override Permission ReadAsPropertyName( ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options ) {

            string permissionString = reader.GetString();

            if (Permission.TryParse( permissionString, out Permission sn )) {
                return sn;
            }

            _logger.Error( $"Couldn't parse the incoming Permission value '{permissionString}'." );
            return Permission.DEFAULT;

        }

        /// <inheritdoc />
        public override void WriteAsPropertyName( Utf8JsonWriter writer, Permission value, JsonSerializerOptions options ) {
            writer.WritePropertyName( value.ToString() );
        }
    }
}
