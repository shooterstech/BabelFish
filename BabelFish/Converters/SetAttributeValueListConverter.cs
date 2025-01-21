using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Scopos.BabelFish.DataModel.AttributeValue;

namespace Scopos.BabelFish.Converters {
    public class SetAttributeValueListConverter : JsonConverter<SetAttributeValueList> {
        public override SetAttributeValueList? Read( ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options ) {

            using (JsonDocument doc = JsonDocument.ParseValue( ref reader )) {
                JsonElement root = doc.RootElement;

                SetAttributeValueList setAttrValueList = new SetAttributeValueList();

                foreach ( var av in root.EnumerateObject()) {
                    SetAttributeValue buildAttribute = new SetAttributeValue();
                    buildAttribute.AttributeValue = av.Name;
                    var avObject = av.Value;

                    buildAttribute.StatusCode = avObject.GetProperty( "statusCode" ).GetInt32().ToString();
                    foreach (var message in avObject.GetProperty( "Message" ).EnumerateArray())
                        buildAttribute.Message.Add( message.GetString() );

                    setAttrValueList.SetAttributeValues.Add( buildAttribute );
                }

                return setAttrValueList;
            }
        }

        public override void Write( Utf8JsonWriter writer, SetAttributeValueList value, JsonSerializerOptions options ) {
            throw new NotImplementedException();
        }
    }
}
