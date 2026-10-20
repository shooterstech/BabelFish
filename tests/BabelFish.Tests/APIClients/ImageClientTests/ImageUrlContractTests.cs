using Scopos.BabelFish.DataModel.Image;
using Scopos.BabelFish.Responses.ScoposData;

namespace Scopos.BabelFish.Tests.APIClients.ImageClientTests {
    [TestClass]
    public class ImageUrlContractTests {
        [TestMethod]
        public void UploadResponseUsesUrlAndCopiesItToAbbreviatedImage() {
            SerializerOptions.InitSerializers();
            const string json = "{\"Image\":{\"ImageId\":\"00000000-0000-4000-8000-000000000002\",\"Url\":\"https://example.test/image.jpeg\"}}";
            var systemText = G_STJ.JsonSerializer.Deserialize<ImageWrapper>( json, SerializerOptions.SystemTextJsonDeserializer );
            var newtonsoft = G_NS.JsonConvert.DeserializeObject<ImageWrapper>( json, SerializerOptions.NewtonsoftJsonSerializer );
            Assert.IsNotNull( systemText );
            Assert.IsNotNull( newtonsoft );
            Assert.AreEqual( "https://example.test/image.jpeg", systemText.Image.Url );
            Assert.AreEqual( systemText.Image.Url, newtonsoft.Image.Url );
            Assert.AreEqual( systemText.Image.Url, systemText.Image.ToScoposImageAbbr().Url );
        }

        [TestMethod]
        [DataRow( false )]
        [DataRow( true )]
        public void ImageModelsSerializeUrlWithoutAlias( bool abbreviated ) {
            SerializerOptions.InitSerializers();
            const string url = "https://example.test/image.jpeg";
            object image = abbreviated ? new ScoposImageAbbr { Url = url } : new ScoposImage { Url = url };
            var systemText = G_STJ.JsonSerializer.Serialize( image, image.GetType(), SerializerOptions.SystemTextJsonDeserializer );
            var newtonsoft = G_NS.JsonConvert.SerializeObject( image, SerializerOptions.NewtonsoftJsonSerializer );
            foreach (var json in new[] { systemText, newtonsoft }) {
                using var body = G_STJ.JsonDocument.Parse( json );
                Assert.AreEqual( url, body.RootElement.GetProperty( "Url" ).GetString() );
                Assert.IsFalse( body.RootElement.TryGetProperty( "UrlPath", out _ ) );
            }
        }
    }
}
