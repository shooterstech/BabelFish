using System.Net;
using System.Linq;
using Scopos.BabelFish.APIClients;
using Scopos.BabelFish.DataModel.ScoposData;
using Scopos.BabelFish.Requests.ScoposData;
using System.Threading.Tasks;
using Scopos.BabelFish.DataModel.Common;

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
        public async Task GetReleaseTests()
        {
            var client = new ScoposDataClient(APIStage.BETA);

            ReleasePhase level = ReleasePhase.PRODUCTION;
            var appList = new List<string>() { "orion", "athena", "greengrassv2Deployment" };
            var appVersion = new Scopos.BabelFish.DataModel.Common.Version("2.21.1");
            var response = await client.GetReleasePublicAsync(level,appList, "000015-orion-001", appVersion, false, false, "00015");

            Assert.AreEqual( Responses.RequestStatusCode.OK , response.OverallStatusCode, $"Expecting and OK status code, instead received Overall {response.OverallStatusCode} with REST API Status {response.RestApiStatusCode}, and message {response.ExceptionMessage}." );

            Assert.IsNotNull(response.ApplicationRelease);

            var releaseNotes = response.ApplicationRelease;
            foreach (var item in releaseNotes.Items) {
                Console.Write(item.Application+": ");
                Console.Write(item.Version.ToString()+"\n");
            }
            
            Assert.AreEqual(appList.Count, releaseNotes.Items.Count);

            //We should have one ReleaseNote for Orion, and one for Athena
            var orionExists = releaseNotes.Items.Any( releaseInfo => releaseInfo.Application == ApplicationName.ORION );
            var athenaExists = releaseNotes.Items.Any( releaseInfo => releaseInfo.Application == ApplicationName.ATHENA );
            Assert.IsTrue( orionExists );
            Assert.IsTrue( athenaExists );

            //Both should haver versions that are greater than 1.4
            var orionVersion = releaseNotes.Items.First( releaseInfo => releaseInfo.Application == ApplicationName.ORION ).Version;
            var athenaVersion = releaseNotes.Items.First( releaseInfo => releaseInfo.Application == ApplicationName.ATHENA ).Version;
            var referenceVersion = new Scopos.BabelFish.DataModel.Common.Version( "1.4.0.0" );
            Assert.IsTrue( orionVersion > referenceVersion );
            Assert.IsTrue( athenaVersion > referenceVersion );

        }

        [TestMethod]
        public void GetOrionServiceProductionLevel() {
            var client = new ScoposDataClient( APIStage.BETA );

            ApplicationName service = ApplicationName.ORION;
            ReleasePhase level = ReleasePhase.PRODUCTION;
            var response = client.GetVersionPublicAsync( service, level );

            var result = response.Result;
            Assert.IsNotNull( result );
            Assert.AreEqual( HttpStatusCode.OK, result.RestApiStatusCode, $"Expecting and OK status code, instead received {result.RestApiStatusCode}." );
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
            Assert.AreEqual( HttpStatusCode.OK, result.RestApiStatusCode, $"Expecting and OK status code, instead received {result.RestApiStatusCode}." );
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
            Assert.AreEqual( HttpStatusCode.OK, result.RestApiStatusCode, $"Expecting and OK status code, instead received {result.RestApiStatusCode}." );
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
            Assert.AreEqual( HttpStatusCode.OK, result.RestApiStatusCode, $"Expecting and OK status code, instead received {result.RestApiStatusCode}." );
            Assert.IsTrue( result.CupsOfCoffeeConsumed > 1000, $"Expecting the number of coffee cups consumed to be greater than 1000, instead received {result.CupsOfCoffeeConsumed}." );
            Assert.AreEqual( result.CupsOfCoffeeConsumed, result.Value.CupsOfCoffeeConsumed );
        }

        [TestMethod]
        public void GetCupsOfCoffeeConsumedWithoutRequestObject() {
            var client = new ScoposDataClient( APIStage.BETA );

            var response = client.GetCuposOfCoffeePublicAsync();
            var result = response.Result;

            Assert.IsNotNull( result );
            Assert.AreEqual( HttpStatusCode.OK, result.RestApiStatusCode, $"Expecting and OK status code, instead received {result.RestApiStatusCode}." );
            Assert.IsTrue( result.CupsOfCoffeeConsumed > 1000, $"Expecting the number of coffee cups consumed to be greater than 1000, instead received {result.CupsOfCoffeeConsumed}." );
            Assert.AreEqual( result.CupsOfCoffeeConsumed, result.Value.CupsOfCoffeeConsumed );
        }

    }
}
