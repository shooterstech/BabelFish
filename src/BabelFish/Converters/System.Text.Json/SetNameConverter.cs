using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Scopos.BabelFish.DataModel.Definitions;
using Scopos.BabelFish.DataModel.OrionMatch;

namespace Scopos.BabelFish.Converters.Microsoft {
    public class SetNameConverter : JsonConverter<SetName> {

        private Logger _logger = NLog.LogManager.GetCurrentClassLogger();

        public override SetName? Read( ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options ) {

            string setNameString = (string)reader.GetString();

            if (SetName.TryParse( setNameString, out SetName sn )) {
                return sn;
            }

            _logger.Error( $"Couldn't parse the incoming SetName value '{setNameString}'." );

            return SetName.DEFAULT;
        }

        public override void Write( Utf8JsonWriter writer, SetName value, JsonSerializerOptions options ) {

            writer.WriteStringValue( value.ToString() );
        }
    }
}
