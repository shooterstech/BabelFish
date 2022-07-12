using System.Net;
using System.Linq;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShootersTech.BabelFish.GetVersionAPI;
using ShootersTech.BabelFish.Helpers;
using ShootersTech.BabelFish.Requests.Misc;

namespace ShootersTech.BabelFish.Tests {
    [TestClass]
    public class MiscTests {
        private static string xApiKey = "wjM7eCb75aa3Okxj4FliXLY0VjHidoE2ei18pdg1";
        private readonly GetVersionAPIClient _client = new GetVersionAPIClient(xApiKey);

        [TestMethod]
        public void GetOrionServiceProductionLevel()
        {
            VersionService service = VersionService.orion;
            VersionLevel level = VersionLevel.production;
            var response = _client.GetVersionAsync(service, level);

            var result = response.Result;
            Assert.IsNotNull(result);
            Assert.AreEqual(result.StatusCode, HttpStatusCode.OK);
            Assert.AreEqual(result.VersionList.Count, 1);
            Assert.AreEqual(result.VersionList[0].Service, service);

            var MessageResponse = response.Result.MessageResponse;
            Assert.IsTrue(MessageResponse.Message.Count == 0);
        }

        [TestMethod]
        public void GetAthenaServiceAlphaLevel()
        {
            VersionService service = VersionService.athena;
            VersionLevel level = VersionLevel.alpha;
            var response = _client.GetVersionAsync(service, level);

            var result = response.Result;
            Assert.IsNotNull(result);
            Assert.AreEqual(result.StatusCode, HttpStatusCode.OK);
            Assert.AreEqual(result.VersionList.Count, 1);
            Assert.AreEqual(result.VersionList[0].Service, service);

            var MessageResponse = response.Result.MessageResponse;
            Assert.IsTrue(MessageResponse.Message.Count == 0);
        }

        [TestMethod]
        public void GetMultipleServicesProductionLevel()
        {
            GetVersionRequest request = new GetVersionRequest()
            {
                services = new List<VersionService>() { VersionService.orion, VersionService.athena },
                level = VersionLevel.production
            };
    
            var response = _client.GetVersionAsync(request);

            var result = response.Result;
            Assert.IsNotNull(result);
            Assert.AreEqual(result.StatusCode, HttpStatusCode.OK);
            Assert.AreEqual(result.VersionList.Count, 2);
            Assert.IsTrue(result.VersionList.Any(x => x.Service == VersionService.athena));

            var MessageResponse = response.Result.MessageResponse;
            Assert.IsTrue(MessageResponse.Message.Count == 0);
        }

    }
}
