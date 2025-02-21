using System.Threading.Tasks;
using Scopos.BabelFish.APIClients;
using Scopos.BabelFish.Requests.Athena;

namespace Scopos.BabelFish.Tests.Athena {
    [TestClass]
    public class OwnershipTests : BaseTestClass {

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
