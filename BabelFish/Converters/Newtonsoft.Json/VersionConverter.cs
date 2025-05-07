﻿using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Scopos.BabelFish.Converters.Newtonsoft {
    public class VersionConverter : JsonConverter<Version> {

        public override void WriteJson( JsonWriter writer, Version value, JsonSerializer serializer ) {
            writer.WriteValue( value.ToString() );
        }

        public override Version ReadJson( JsonReader reader, Type objectType, Version existingValue, bool hasExistingValue, JsonSerializer serializer ) {
            string versionString = (string)reader.Value;

            return Version.TryParse( versionString, out Version version ) ? version : new Version();
        }
    }
}
