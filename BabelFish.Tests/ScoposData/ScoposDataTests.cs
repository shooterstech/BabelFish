using System.Net;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scopos.BabelFish.APIClients;
using Scopos.BabelFish.DataModel.ScoposData;
using Scopos.BabelFish.Requests.ScoposData;

namespace Scopos.BabelFish.Tests.ScoposData {
    [TestClass]
    public class ScoposDataTests {

        /// <summary>
        /// Unit test to confirm the Constructors set the api key and API stage as expected.
        /// </summary>
        [TestMethod]
        public void BasicConstructorTests() {

            var defaultConstructorClient = new ScoposDataClient( Constants.X_API_KEY );
            var apiStageConstructorClient = new ScoposDataClient( Constants.X_API_KEY, APIStage.BETA );

            Assert.AreEqual( Constants.X_API_KEY, defaultConstructorClient.XApiKey );
            Assert.AreEqual( APIStage.PRODUCTION, defaultConstructorClient.ApiStage );

            Assert.AreEqual( Constants.X_API_KEY, apiStageConstructorClient.XApiKey );
            Assert.AreEqual( APIStage.BETA, apiStageConstructorClient.ApiStage );
        }

        [TestMethod]
        public void GetOrionServiceProductionLevel() {
            var client = new ScoposDataClient( Constants.X_API_KEY, APIStage.BETA );

            VersionService service = VersionService.ORION;
            VersionLevel level = VersionLevel.PRODUCTION;
            var response = client.GetVersionAsync( service, level );

            var result = response.Result;
            Assert.IsNotNull( result );
            Assert.AreEqual( HttpStatusCode.OK, result.StatusCode, $"Expecting and OK status code, instead received {result.StatusCode}." );
            Assert.AreEqual( 1, result.VersionList.Count );
            Assert.AreEqual( service, result.VersionList[0].Service );
        }

        [TestMethod]
        public void GetAthenaServiceAlphaLevel() {
            var client = new ScoposDataClient( Constants.X_API_KEY, APIStage.BETA );

            VersionService service = VersionService.ATHENA;
            VersionLevel level = VersionLevel.ALPHA;
            var response = client.GetVersionAsync( service, level );

            var result = response.Result;
            Assert.IsNotNull( result );
            Assert.AreEqual( HttpStatusCode.OK, result.StatusCode, $"Expecting and OK status code, instead received {result.StatusCode}." );
            Assert.AreEqual(1, result.VersionList.Count );
            Assert.AreEqual(service, result.VersionList[0].Service );
        }

        [TestMethod]
        public void GetMultipleServicesProductionLevel() {
            var client = new ScoposDataClient( Constants.X_API_KEY, APIStage.BETA );

            GetVersionRequest request = new GetVersionRequest() {
                Services = new List<VersionService>() { VersionService.ORION, VersionService.ATHENA },
                Level = VersionLevel.PRODUCTION
            };

            var response = client.GetVersionAsync( request );

            var result = response.Result;
            Assert.IsNotNull( result );
            Assert.AreEqual( HttpStatusCode.OK, result.StatusCode, $"Expecting and OK status code, instead received {result.StatusCode}." );
            Assert.AreEqual( result.VersionList.Count, 2 );
            Assert.IsTrue( result.VersionList.Any( x => x.Service == VersionService.ATHENA ) );
        }

        [TestMethod]
        public async Task GetCupsOfCoffeeConsumedWithRequestObject() {
            var client = new ScoposDataClient( Constants.X_API_KEY, APIStage.BETA );

            GetCupsOfCoffeeRequest request = new GetCupsOfCoffeeRequest();

            Assert.AreEqual( request.OperationId, "GetCoffee" );

            var response = client.GetCuposOfCoffeeAsync( request );
            var result = response.Result;

            Assert.IsNotNull( result );
            Assert.AreEqual( HttpStatusCode.OK, result.StatusCode, $"Expecting and OK status code, instead received {result.StatusCode}." );
            Assert.IsTrue( result.CupsOfCoffeeConsumed > 1000, $"Expecting the number of coffee cups consumed to be greater than 1000, instead received {result.CupsOfCoffeeConsumed}." );
            Assert.AreEqual( result.CupsOfCoffeeConsumed, result.Value.CupsOfCoffeeConsumed );
        }

        [TestMethod]
        public async Task GetCupsOfCoffeeConsumedWithoutRequestObject() {
            var client = new ScoposDataClient( Constants.X_API_KEY, APIStage.BETA );

            var response = client.GetCuposOfCoffeeAsync();
            var result = response.Result;

            Assert.IsNotNull( result );
            Assert.AreEqual( HttpStatusCode.OK, result.StatusCode, $"Expecting and OK status code, instead received {result.StatusCode}." );
            Assert.IsTrue( result.CupsOfCoffeeConsumed > 1000, $"Expecting the number of coffee cups consumed to be greater than 1000, instead received {result.CupsOfCoffeeConsumed}." );
            Assert.AreEqual( result.CupsOfCoffeeConsumed, result.Value.CupsOfCoffeeConsumed );
        }

    }
}
