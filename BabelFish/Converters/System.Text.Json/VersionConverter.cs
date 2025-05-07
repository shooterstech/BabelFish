using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Version = Scopos.BabelFish.DataModel.Common.Version;

namespace Scopos.BabelFish.Converters.Microsoft {
    public class VersionConverter : JsonConverter<Version> {

        public override Version Read( ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options ) {
            string versionString = reader.GetString();

            if (Version.TryParse( versionString, out Version version )) {
                return version;
            }

            return new Version();
        }

        public override void Write( Utf8JsonWriter writer, Version value, JsonSerializerOptions options ) {
            writer.WriteStringValue( value.ToString() );
        }

    }
}
