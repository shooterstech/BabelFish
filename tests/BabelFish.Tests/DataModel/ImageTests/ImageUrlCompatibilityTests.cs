using Scopos.BabelFish.DataModel.Image;

namespace Scopos.BabelFish.Tests.DataModel.ImageTests {
    [TestClass]
    public class ImageUrlCompatibilityTests {
        private const string ImageUrl = "https://example.test/image.png";

        [TestMethod]
        [DataRow(false)]
        [DataRow(true)]
        public void FullImageReadsUrlAndIgnoresLegacyAlias(bool includeLegacyAlias) {
            var json = ImagePayload(includeLegacyAlias);
            foreach (var image in new[] {
                G_STJ.JsonSerializer.Deserialize<ScoposImage>(json)!,
                G_NS.JsonConvert.DeserializeObject<ScoposImage>(json)!
            }) {
                Assert.AreEqual(ImageUrl, image.Url);
                Assert.AreEqual(ImageUrl, image.ToScoposImageAbbr().Url);
            }
        }

        [TestMethod]
        [DataRow(false)]
        [DataRow(true)]
        public void AbbreviatedImageReadsUrlAndIgnoresLegacyAlias(bool includeLegacyAlias) {
            var json = ImagePayload(includeLegacyAlias);
            foreach (var image in new[] {
                G_STJ.JsonSerializer.Deserialize<ScoposImageAbbr>(json)!,
                G_NS.JsonConvert.DeserializeObject<ScoposImageAbbr>(json)!
            }) {
                Assert.AreEqual(ImageUrl, image.Url);
            }
        }

        private static string ImagePayload(bool includeLegacyAlias) {
            // Team images omit CreatedAt. A trailing legacy field must not overwrite Url.
            var legacyAlias = includeLegacyAlias ? ",\"UrlPath\":\"https://example.test/legacy.png\"" : "";
            return $"{{\"Url\":\"{ImageUrl}\",\"Caption\":\"\",\"AltText\":\"\",\"ImageId\":\"image-id\"{legacyAlias}}}";
        }
    }
}
