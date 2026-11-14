using Newtonsoft.Json;
using Version = Scopos.BabelFish.DataModel.Common.Version;

namespace Scopos.BabelFish.Converters.Newtonsoft {
    public class VersionConverter : JsonConverter<Version> {

        /// <inheritdoc/>
        public override void WriteJson( JsonWriter writer, Version value, JsonSerializer serializer ) {
            writer.WriteValue( value.ToString() );
        }

        /// <inheritdoc/>
        public override Version ReadJson( JsonReader reader, Type objectType, Version existingValue, bool hasExistingValue, JsonSerializer serializer ) {
            string versionString = (string)reader.Value;

            return Version.Parse( versionString, false );
        }
    }
}
