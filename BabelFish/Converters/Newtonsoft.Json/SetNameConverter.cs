using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Scopos.BabelFish.DataModel.Definitions;
using Scopos.BabelFish.DataModel.OrionMatch;
using NLog;

namespace Scopos.BabelFish.Converters.Newtonsoft {
    public class SetNameConverter : JsonConverter<SetName> {

        private Logger _logger = NLog.LogManager.GetCurrentClassLogger();

        public override void WriteJson( JsonWriter writer, SetName value, JsonSerializer serializer ) {
            writer.WriteValue( value.ToString() );
        }

        public override SetName ReadJson( JsonReader reader, Type objectType, SetName existingValue, bool hasExistingValue, JsonSerializer serializer ) {
            string setNameString = (string)reader.Value;

            if (SetName.TryParse( setNameString, out SetName sn ) ) {
                return sn;
            }

            _logger.Error( $"Couldn't parse the incoming SetName value '{setNameString}'." );

            return SetName.DEFAULT;
        }
    }
}
