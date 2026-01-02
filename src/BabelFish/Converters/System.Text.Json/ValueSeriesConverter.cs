using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Scopos.BabelFish.DataModel.Definitions;

namespace Scopos.BabelFish.Converters.Microsoft {
    internal class ValueSeriesConverter : JsonConverter<ValueSeries> {

        private Logger _logger = NLog.LogManager.GetCurrentClassLogger();

        public override ValueSeries Read( ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options ) {
            string valueSeriesString = reader.GetString();

            if (string.IsNullOrEmpty( valueSeriesString )) {
                return new ValueSeries( string.Empty );
            }

            try {
                var vs = new ValueSeries( valueSeriesString );
                return vs;
            } catch (Exception ex) {
                _logger.Error( $"Unable to parse the Value Series {valueSeriesString}. Returning a default VAlueSeries.", ex );
            }

            return new ValueSeries( string.Empty );
        }

        public override void Write( Utf8JsonWriter writer, ValueSeries value, JsonSerializerOptions options ) {
            writer.WriteStringValue( value.ToString() );
        }
    }
}
