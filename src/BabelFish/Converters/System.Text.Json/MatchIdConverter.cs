using System.Text.Json;
using System.Text.Json.Serialization;
using Scopos.BabelFish.DataModel.OrionMatch;

namespace Scopos.BabelFish.Converters.Microsoft {

    /// <summary>
    /// Custon System.Text.json converter for the MatchID class.
    /// </summary>
    public class MatchIdConverter : JsonConverter<MatchID> {

        private static Logger _logger = LogManager.GetCurrentClassLogger();

        /// <inheritdoc />
        public override MatchID? Read( ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options ) {

            string matchIdString = (string)reader.GetString();

            if (MatchID.TryParse( matchIdString, out MatchID mId )) {
                return mId;
            }

            _logger.Error( $"Failed to parse MatchID from string: {matchIdString}. Returning default MatchID." );
            return MatchID.DEFAULT;
        }

        /// <inheritdoc />
        public override void Write( Utf8JsonWriter writer, MatchID value, JsonSerializerOptions options ) {

            writer.WriteStringValue( value.ToString() );
        }

        /// <inheritdoc />
        public override MatchID ReadAsPropertyName( ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options ) {

            string matchIdString = (string)reader.GetString();

            if (MatchID.TryParse( matchIdString, out MatchID mId )) {
                return mId;
            }

            _logger.Error( $"Failed to parse MatchID from string: {matchIdString}. Returning default MatchID." );
            return MatchID.DEFAULT;
        }

        /// <inheritdoc />
        public override void WriteAsPropertyName( Utf8JsonWriter writer, MatchID value, JsonSerializerOptions options ) {

            writer.WriteStringValue( value.ToString() );
        }
    }
}
