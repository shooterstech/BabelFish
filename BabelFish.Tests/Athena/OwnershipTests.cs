using Scopos.BabelFish.Requests.Athena;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scopos.BabelFish.APIClients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scopos.BabelFish.Tests.Athena
{
    [TestClass]
    public class OwnershipTests {
        [TestInitialize]
        public void InitializeTest() {
            Scopos.BabelFish.Runtime.Settings.XApiKey = Constants.X_API_KEY;
        }

        [TestMethod]
        public async Task TestRemoveOwnership()
        {
            var ownershipClient = new AthenaAPIClient(APIStage.PRODUCTION);
            RemoveThingOwnershipRequest request = new RemoveThingOwnershipRequest();
            request.OwnerId = "AtHomeAcct000046"; //test_dev_9
            request.SharedKey = "";
            request.CpuSerial = ""; //ESTMonitor-000002139

            var response  = await ownershipClient.RemoveThingOwnershipAsync(request);
            Assert.AreEqual(System.Net.HttpStatusCode.OK, response.StatusCode);
        }
    }
}
