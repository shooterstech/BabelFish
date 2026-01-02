using Newtonsoft.Json;
using Scopos.BabelFish.DataModel.Definitions;

namespace Scopos.BabelFish.Converters.Newtonsoft {
    public class ValueSeriesConverter : JsonConverter<ValueSeries> {

        public override void WriteJson( JsonWriter writer, ValueSeries value, JsonSerializer serializer ) {
            writer.WriteValue( value.ToString() );
        }

        public override ValueSeries ReadJson( JsonReader reader, Type objectType, ValueSeries existingValue, bool hasExistingValue, JsonSerializer serializer ) {
            string valueSeriesString = (string)reader.Value;

            if ( string.IsNullOrEmpty( valueSeriesString ) )
                return new ValueSeries( ValueSeries.APPLY_TO_ALL_FORMAT );

            return new ValueSeries( valueSeriesString );
        }
    }
}
