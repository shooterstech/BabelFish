using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Scopos.BabelFish.DataModel.AttributeValue;

namespace Scopos.BabelFish.Converters.Newtonsoft {
    public class ConstantFieldValueListConverter : JsonConverter<ConstantFieldValueList> {

        private Logger _logger = LogManager.GetCurrentClassLogger();

        public override ConstantFieldValueList ReadJson(
        JsonReader reader,
        Type objectType,
        ConstantFieldValueList existingValue,
        bool hasExistingValue,
        JsonSerializer serializer ) {
            if (reader.TokenType != JsonToken.StartObject)
                throw new JsonSerializationException( "Expected start of object for ConstantFieldValueList" );

            var list = new ConstantFieldValueList();

            // Load the object into a JObject for easy iteration
            var obj = JObject.Load( reader );

            foreach (var prop in obj.Properties()) {
                list.Add( new ConstantFieldValue {
                    FieldName = prop.Name,
                    Value = prop.Value.ToObject<object>( serializer )
                } );
            }

            return list;
        }

        public override void WriteJson(
            JsonWriter writer,
            ConstantFieldValueList value,
            JsonSerializer serializer ) {
            writer.WriteStartObject();

            foreach (var item in value) {
                writer.WritePropertyName( item.FieldName );
                serializer.Serialize( writer, item.Value );
            }

            writer.WriteEndObject();
        }


    }
}
