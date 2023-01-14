using System.Net;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scopos.BabelFish.GetVersionAPI;
using Scopos.BabelFish.DataModel.ShootersTechData;
using Scopos.BabelFish.Requests.ShootersTechData;

namespace Scopos.BabelFish.Tests {
    [TestClass]
    public class ShootersTechDataTests {
        private readonly GetShootersTechDataClient _client = new GetShootersTechDataClient( Constants.X_API_KEY );

        [TestMethod]
        public void GetOrionServiceProductionLevel() {
            VersionService service = VersionService.ORION;
            VersionLevel level = VersionLevel.PRODUCTION;
            var response = _client.GetVersionAsync( service, level );

            var result = response.Result;
            Assert.IsNotNull( result );
            Assert.AreEqual( HttpStatusCode.OK, result.StatusCode, $"Expecting and OK status code, instead received {result.StatusCode}." );
            Assert.AreEqual( result.VersionList.Count, 1 );
            Assert.AreEqual( result.VersionList[0].Service, service );

            var MessageResponse = response.Result.MessageResponse;
            Assert.IsTrue( MessageResponse.Message.Count == 0 );
        }

        [TestMethod]
        public void GetAthenaServiceAlphaLevel() {
            VersionService service = VersionService.ATHENA;
            VersionLevel level = VersionLevel.ALPHA;
            var response = _client.GetVersionAsync( service, level );

            var result = response.Result;
            Assert.IsNotNull( result );
            Assert.AreEqual( HttpStatusCode.OK, result.StatusCode, $"Expecting and OK status code, instead received {result.StatusCode}." );
            Assert.AreEqual( result.VersionList.Count, 1 );
            Assert.AreEqual( result.VersionList[0].Service, service );

            var MessageResponse = response.Result.MessageResponse;
            Assert.IsTrue( MessageResponse.Message.Count == 0 );
        }

        [TestMethod]
        public void GetMultipleServicesProductionLevel() {
            GetVersionRequest request = new GetVersionRequest() {
                Services = new List<VersionService>() { VersionService.ORION, VersionService.ATHENA },
                Level = VersionLevel.PRODUCTION
            };

            var response = _client.GetVersionAsync( request );

            var result = response.Result;
            Assert.IsNotNull( result );
            Assert.AreEqual( HttpStatusCode.OK, result.StatusCode, $"Expecting and OK status code, instead received {result.StatusCode}." );
            Assert.AreEqual( result.VersionList.Count, 2 );
            Assert.IsTrue( result.VersionList.Any( x => x.Service == VersionService.ATHENA ) );

            var MessageResponse = response.Result.MessageResponse;
            Assert.IsTrue( MessageResponse.Message.Count == 0 );
        }

        [TestMethod]
        public async Task GetCupsOfCoffeeConsumedWithParams() {
            GetCupsOfCoffeeRequest request = new GetCupsOfCoffeeRequest();

            var response = _client.GetCuposOfCoffeeAsync( request );
            var result = response.Result;

            Assert.IsNotNull( result );
            Assert.AreEqual( HttpStatusCode.OK, result.StatusCode, $"Expecting and OK status code, instead received {result.StatusCode}." );
            Assert.IsTrue( result.CupsOfCoffeeConsumed > 1000, $"Expecting the number of coffee cups consumed to be greater than 1000, instead received {result.CupsOfCoffeeConsumed}." );
            Assert.AreEqual( result.CupsOfCoffeeConsumed, result.Value.CupsOfCoffeeConsumed );
        }

        [TestMethod]
        public async Task GetCupsOfCoffeeConsumedWithoutParams() {

            var response = _client.GetCuposOfCoffeeAsync( );
            var result = response.Result;

            Assert.IsNotNull( result );
            Assert.AreEqual( HttpStatusCode.OK, result.StatusCode, $"Expecting and OK status code, instead received {result.StatusCode}." );
            Assert.IsTrue( result.CupsOfCoffeeConsumed > 1000, $"Expecting the number of coffee cups consumed to be greater than 1000, instead received {result.CupsOfCoffeeConsumed}." );
            Assert.AreEqual( result.CupsOfCoffeeConsumed, result.Value.CupsOfCoffeeConsumed );
        }

    }
}
