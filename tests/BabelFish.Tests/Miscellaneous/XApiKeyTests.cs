using System.Threading.Tasks;
using Scopos.BabelFish.Runtime;
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
        [ExpectedException(typeof(XApiKeyNotSetException))]
        public void XApiKeyNotSet() {

            //Without Settings.XApiKey set, this should throw an exceptino. 
            var client = new OrionMatchAPIClient();
        }

        [TestMethod]
        [ExpectedException( typeof( XApiKeyNotSetException ) )]
        public async Task DefinitionFetcherThrowsException() {

            //Without Settings.XApiKey set, this should throw an exceptino. 
            var fetcher = await DefinitionCache.GetTargetDefinitionAsync( SetName.Parse( "v1.0:issf:Air Rifle" ) );
        }
    }
}
