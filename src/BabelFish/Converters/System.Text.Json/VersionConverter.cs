using System.Text.Json;
using System.Text.Json.Serialization;
using Version = Scopos.BabelFish.DataModel.Common.Version;

namespace Scopos.BabelFish.Converters.Microsoft {
    public class VersionConverter : JsonConverter<Version> {

        private static Logger _logger = LogManager.GetCurrentClassLogger();

        public override Version Read( ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options ) {
            try {
                string versionString = reader.GetString();

                return Version.Parse( versionString );
            } catch (Exception ex) {
                _logger.Error( ex );
                return Version.DEFAULT;
            }
        }

        public override void Write( Utf8JsonWriter writer, Version value, JsonSerializerOptions options ) {
            writer.WriteStringValue( value.ToString() );
        }

    }
}
