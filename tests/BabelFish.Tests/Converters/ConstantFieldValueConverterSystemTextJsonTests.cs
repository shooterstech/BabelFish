using System.Text.Json;
using Scopos.BabelFish.DataModel.AttributeValue;

namespace Scopos.BabelFish.Tests.Converters {
    [TestClass]
    public class ConstantFieldValueConverterSystemTextJsonTests : BaseTestClass {

        [TestMethod]
        public void Deserialize_ShouldCreateListFromDictionary() {
            // Arrange
            var json = "{\"FieldA\":\"AAA\",\"FieldB\":123}";

            // Act
            var result = JsonSerializer.Deserialize<ConstantFieldValueList>( json, SerializerOptions.SystemTextJsonDeserializer );

            // Assert
            Assert.IsNotNull( result );
            Assert.AreEqual( 2, result.Count );

            Assert.AreEqual( "FieldA", result[0].FieldName );
            Assert.AreEqual( "AAA", (string)result[0].Value );

            Assert.AreEqual( "FieldB", result[1].FieldName );
            Assert.AreEqual( 123, (int)result[1].Value );
        }

        [TestMethod]
        public void Serialize_ShouldWriteDictionaryJson() {
            // Arrange
            var list = new ConstantFieldValueList
            {
                new ConstantFieldValue { FieldName = "X", Value = 10 },
                new ConstantFieldValue { FieldName = "Y", Value = "Hello" }
            };

            // Act
            var json = JsonSerializer.Serialize( list, SerializerOptions.SystemTextJsonDeserializer ).Replace( "\r", "" ).Replace( "\n", "" ).Replace( " ", "" );

            // Assert
            Assert.AreEqual( "{\"X\":10,\"Y\":\"Hello\"}", json );
        }

        [TestMethod]
        public void RoundTrip_ShouldPreserveValues() {
            // Arrange
            var original = new ConstantFieldValueList
            {
                new ConstantFieldValue { FieldName = "A", Value = 1 },
                new ConstantFieldValue { FieldName = "B", Value = "Two" }
            };

            // Act
            var json = JsonSerializer.Serialize( original, SerializerOptions.SystemTextJsonDeserializer );
            var result = JsonSerializer.Deserialize<ConstantFieldValueList>( json, SerializerOptions.SystemTextJsonDeserializer );

            // Assert
            Assert.AreEqual( 2, result.Count );
            Assert.AreEqual( "A", result[0].FieldName );
            Assert.AreEqual( 1, (int)result[0].Value );
            Assert.AreEqual( "B", result[1].FieldName );
            Assert.AreEqual( "Two", (string)result[1].Value );
        }

        [TestMethod]
        public void Deserialize_EmptyObject_ShouldReturnEmptyList() {
            var result = JsonSerializer.Deserialize<ConstantFieldValueList>( "{}", SerializerOptions.SystemTextJsonDeserializer );
            Assert.AreEqual( 0, result.Count );
        }

        [TestMethod]
        public void Deserialize_NullValue_ShouldSetValueToNull() {
            var json = "{\"FieldA\":null}";
            var result = JsonSerializer.Deserialize<ConstantFieldValueList>( json, SerializerOptions.SystemTextJsonDeserializer );

            Assert.AreEqual( 1, result.Count );
            Assert.AreEqual( "FieldA", result[0].FieldName );
            Assert.IsNull( result[0].Value );
        }
    }


}
