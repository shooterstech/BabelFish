using System.Net;
using System.Linq;
using Scopos.BabelFish.APIClients;
using Scopos.BabelFish.DataModel.ScoposData;
using Scopos.BabelFish.Requests.ScoposData;

namespace Scopos.BabelFish.Tests.ScoposData {
    [TestClass]
    public class ScoposDataTests  : BaseTestClass {

        /// <summary>
        /// Unit test to confirm the Constructors set the api key and API stage as expected.
        /// </summary>
        [TestMethod]
        public void BasicConstructorTests() {

            var defaultConstructorClient = new ScoposDataClient( );
            var apiStageConstructorClient = new ScoposDataClient( APIStage.BETA );

            Assert.AreEqual( APIStage.PRODUCTION, defaultConstructorClient.ApiStage );

            Assert.AreEqual( APIStage.BETA, apiStageConstructorClient.ApiStage );
        }

        [TestMethod]
        public void GetReleaseList()
        {
            var client = new ScoposDataClient(APIStage.BETA);

            ReleasePhase level = ReleasePhase.PRODUCTION;
            var response = client.GetReleasePublicAsync(level);

            var result = response.Result;
            Assert.IsNotNull(result);
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode, $"Expecting and OK status code, instead received {result.StatusCode}.");
            Assert.AreEqual(2, result.ApplicationRelease.Items.Count);
        }

        [TestMethod]
        public void GetOrionServiceProductionLevel() {
            var client = new ScoposDataClient( APIStage.BETA );

            ApplicationName service = ApplicationName.ORION;
            ReleasePhase level = ReleasePhase.PRODUCTION;
            var response = client.GetVersionPublicAsync( service, level );

            var result = response.Result;
            Assert.IsNotNull( result );
            Assert.AreEqual( HttpStatusCode.OK, result.StatusCode, $"Expecting and OK status code, instead received {result.StatusCode}." );
            Assert.AreEqual( 1, result.VersionList.Count );
            Assert.AreEqual( service, result.VersionList[0].Service );
        }

        [TestMethod]
        public void GetAthenaServiceAlphaLevel() {
            var client = new ScoposDataClient( APIStage.BETA );

            ApplicationName service = ApplicationName.ATHENA;
            ReleasePhase level = ReleasePhase.ALPHA;
            var response = client.GetVersionPublicAsync( service, level );

            var result = response.Result;
            Assert.IsNotNull( result );
            Assert.AreEqual( HttpStatusCode.OK, result.StatusCode, $"Expecting and OK status code, instead received {result.StatusCode}." );
            Assert.AreEqual(1, result.VersionList.Count );
            Assert.AreEqual(service, result.VersionList[0].Service );
        }

        [TestMethod]
        public void GetMultipleServicesProductionLevel() {
            var client = new ScoposDataClient( APIStage.BETA );

            GetVersionPublicRequest request = new GetVersionPublicRequest() {
                Services = new List<ApplicationName>() { ApplicationName.ORION, ApplicationName.ATHENA },
                Level = ReleasePhase.PRODUCTION
            };

            var response = client.GetVersionPublicAsync( request );

            var result = response.Result;
            Assert.IsNotNull( result );
            Assert.AreEqual( HttpStatusCode.OK, result.StatusCode, $"Expecting and OK status code, instead received {result.StatusCode}." );
            Assert.AreEqual( result.VersionList.Count, 2 );
            Assert.IsTrue( result.VersionList.Any( x => x.Service == ApplicationName.ATHENA ) );
        }

        [TestMethod]
        public void GetCupsOfCoffeeConsumedWithRequestObject() {
            var client = new ScoposDataClient( APIStage.BETA );

            GetCupsOfCoffeePublicRequest request = new GetCupsOfCoffeePublicRequest();

            Assert.AreEqual( request.OperationId, "GetCoffee" );

            var response = client.GetCuposOfCoffeePublicAsync( request );
            var result = response.Result;

            Assert.IsNotNull( result );
            Assert.AreEqual( HttpStatusCode.OK, result.StatusCode, $"Expecting and OK status code, instead received {result.StatusCode}." );
            Assert.IsTrue( result.CupsOfCoffeeConsumed > 1000, $"Expecting the number of coffee cups consumed to be greater than 1000, instead received {result.CupsOfCoffeeConsumed}." );
            Assert.AreEqual( result.CupsOfCoffeeConsumed, result.Value.CupsOfCoffeeConsumed );
        }

        [TestMethod]
        public void GetCupsOfCoffeeConsumedWithoutRequestObject() {
            var client = new ScoposDataClient( APIStage.BETA );

            var response = client.GetCuposOfCoffeePublicAsync();
            var result = response.Result;

            Assert.IsNotNull( result );
            Assert.AreEqual( HttpStatusCode.OK, result.StatusCode, $"Expecting and OK status code, instead received {result.StatusCode}." );
            Assert.IsTrue( result.CupsOfCoffeeConsumed > 1000, $"Expecting the number of coffee cups consumed to be greater than 1000, instead received {result.CupsOfCoffeeConsumed}." );
            Assert.AreEqual( result.CupsOfCoffeeConsumed, result.Value.CupsOfCoffeeConsumed );
        }

    }
}
