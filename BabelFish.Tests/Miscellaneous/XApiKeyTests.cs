using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scopos.BabelFish.Runtime;
using Scopos.BabelFish.APIClients;
using Scopos.BabelFish.DataModel.Definitions;

namespace Scopos.BabelFish.Tests.Miscellaneous {

    [TestClass]
    public class XApiKeyTests {

        /*
         * Purposefully NOT setting the x api key in a initialize test method. as writing tests
         * to check what happens when it's not set
         */

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
