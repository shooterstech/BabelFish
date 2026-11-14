using System.Threading.Tasks;
using Scopos.BabelFish.APIClients;
using Scopos.BabelFish.DataModel.ScoposData;
using Scopos.BabelFish.Requests.ImageAPI;

namespace Scopos.BabelFish.Tests.APIClients.ImageClientTests {
    [TestClass]
    public class GetImageListTests : BaseTestClass {

        [TestMethod]
        public async Task GetClubImageListAsync() {
            var imageClient = new ImageClient();
            var request = new GetImagesRequest() {
                ImageCategory = ImageCategory.CLUB,
                PrimaryKey = "8",
                Limit = 10
            };
            var response = await imageClient.GetImagesPublicAsync( request );

            Assert.IsNotNull( response );
            Assert.IsTrue( response.HasOkStatusCode );
            Assert.IsNotNull( response.ImageList );
            Assert.IsTrue( response.ImageList.Items.Count <= 10 );

            foreach (var image in response.ImageList.Items) {
                Assert.IsTrue( image.IsValid );
            }
        }
    }
}
