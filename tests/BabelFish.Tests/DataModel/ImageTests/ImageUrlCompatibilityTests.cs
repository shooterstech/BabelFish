using Scopos.BabelFish.DataModel.Image;

namespace Scopos.BabelFish.Tests.DataModel.ImageTests {
    [TestClass]
    public class ImageUrlCompatibilityTests {
        private const string ImageUrl = "https://example.test/image.png";

        [TestMethod]
        [DataRow("Url")]
        [DataRow("UrlPath")]
        public void FullImageReadsBothApiFieldNames(string fieldName) {
            var json = ImagePayload(fieldName);
            foreach (var image in new[] {
                G_STJ.JsonSerializer.Deserialize<ScoposImage>(json)!,
                G_NS.JsonConvert.DeserializeObject<ScoposImage>(json)!
            }) {
                Assert.AreEqual(ImageUrl, image.Url);
                Assert.AreEqual(ImageUrl, image.UrlPath);
                Assert.AreEqual(ImageUrl, image.ToScoposImageAbbr().Url);
            }
        }

        [TestMethod]
        [DataRow("Url")]
        [DataRow("UrlPath")]
        public void AbbreviatedImageReadsBothApiFieldNames(string fieldName) {
            var json = ImagePayload(fieldName);
            foreach (var image in new[] {
                G_STJ.JsonSerializer.Deserialize<ScoposImageAbbr>(json)!,
                G_NS.JsonConvert.DeserializeObject<ScoposImageAbbr>(json)!
            }) {
                Assert.AreEqual(ImageUrl, image.Url);
                Assert.AreEqual(ImageUrl, image.UrlPath);
            }
        }

        private static string ImagePayload(string fieldName) {
            // Matches the production team image response, which has no CreatedAt field.
            return $"{{\"{fieldName}\":\"{ImageUrl}\",\"Caption\":\"\",\"AltText\":\"\",\"ImageId\":\"image-id\"}}";
        }

        [TestMethod]
        public void FullImageUrlAliasPreservesWireContract() {
            var image = new ScoposImage { Url = ImageUrl };
            Assert.AreEqual(ImageUrl, image.UrlPath);
            image.UrlPath = ImageUrl + "?updated";
            Assert.AreEqual(image.UrlPath, image.Url);
            image.Url = ImageUrl;
            AssertWireContract(image);
            Assert.AreEqual(ImageUrl, image.ToScoposImageAbbr().Url);
        }

        [TestMethod]
        public void AbbreviatedImageUrlAliasPreservesWireContract() {
            var image = new ScoposImageAbbr { Url = ImageUrl };
            Assert.AreEqual(ImageUrl, image.UrlPath);
            image.UrlPath = ImageUrl + "?updated";
            Assert.AreEqual(image.UrlPath, image.Url);
            image.Url = ImageUrl;
            AssertWireContract(image);
        }

        private static void AssertWireContract<T>(T image) {
            foreach (var json in new[] {
                G_STJ.JsonSerializer.Serialize(image),
                G_NS.JsonConvert.SerializeObject(image)
            }) {
                using var document = G_STJ.JsonDocument.Parse(json);
                Assert.AreEqual(ImageUrl, document.RootElement.GetProperty("UrlPath").GetString());
                Assert.AreEqual(ImageUrl, document.RootElement.GetProperty("Url").GetString());
            }
        }
    }
}
