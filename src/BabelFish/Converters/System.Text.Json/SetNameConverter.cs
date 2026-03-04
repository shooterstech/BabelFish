using System.Text.Json;
using System.Text.Json.Serialization;
using Scopos.BabelFish.DataModel.Definitions;

namespace Scopos.BabelFish.Converters.Microsoft {

    /// <summary>
    /// Custon System.Text.Json converter for the SetName class.
    /// </summary>
    public class SetNameConverter : JsonConverter<SetName> {

        private Logger _logger = NLog.LogManager.GetCurrentClassLogger();

        /// <inheritdoc />
        public override SetName? Read( ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options ) {

            string setNameString = (string)reader.GetString();

            if (SetName.TryParse( setNameString, out SetName sn )) {
                return sn;
            }

            _logger.Error( $"Couldn't parse the incoming SetName value '{setNameString}'." );

            return SetName.DEFAULT;
        }

        /// <inheritdoc />
        public override void Write( Utf8JsonWriter writer, SetName value, JsonSerializerOptions options ) {

            writer.WriteStringValue( value.ToString() );
        }

        /// <inheritdoc />
        public override SetName ReadAsPropertyName( ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options ) {

            string setNameString = reader.GetString();

            if (SetName.TryParse( setNameString, out SetName sn )) {
                return sn;
            }

            _logger.Error( $"Couldn't parse the incoming SetName value '{setNameString}'." );

            return SetName.DEFAULT;

        }

        /// <inheritdoc />
        public override void WriteAsPropertyName( Utf8JsonWriter writer, SetName value, JsonSerializerOptions options ) {
            writer.WritePropertyName( value.ToString() );
        }
    }
}
