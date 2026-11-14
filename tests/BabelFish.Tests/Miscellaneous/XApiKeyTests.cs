using System.Threading.Tasks;
using Scopos.BabelFish.APIClients;
using Scopos.BabelFish.DataModel.Definitions;

namespace Scopos.BabelFish.Tests.Miscellaneous {

    [TestClass]
    public class XApiKeyTests : BaseTestClass {

        [TestInitialize]
        public override void InitializeTest() {

            /*
             * Purposefully NOT setting the x api key in a initialize test method. as writing tests
             * to check what happens when it's not set
             */

        }

        [TestMethod]
        public void XApiKeyNotSet() {

            Assert.Throws<XApiKeyNotSetException>( () => {
                //Without Settings.XApiKey set, this should throw an exceptino. 
                var client = new OrionMatchAPIClient();
            } );
        }

        [TestMethod]
        public async Task DefinitionFetcherThrowsException() {
            await Assert.ThrowsAsync<XApiKeyNotSetException>( async () => {

                //Without Settings.XApiKey set, this should throw an exceptino. 
                var fetcher = await DefinitionCache.GetTargetDefinitionAsync( SetName.Parse( "v1.0:issf:Air Rifle" ) );
            } );
        }
    }
}
