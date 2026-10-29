using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Scopos.BabelFish.APIClients;
using Scopos.BabelFish.DataModel.ScoposData;
using Scopos.BabelFish.Requests.ScoposData;
using Version = Scopos.BabelFish.DataModel.Common.Version;

namespace Scopos.BabelFish.Tests.APIClients.ScoposDataClientTests {
    [TestClass]
    public class ScoposDataTests : BaseTestClass {

        /// <summary>
        /// Unit test to confirm the Constructors set the api key and API stage as expected.
        /// </summary>
        [TestMethod]
        public void BasicConstructorTests() {

            var defaultConstructorClient = new ScoposDataClient();
            var apiStageConstructorClient = new ScoposDataClient( APIStage.BETA );

            Assert.AreEqual( APIStage.PRODUCTION, defaultConstructorClient.ApiStage );

            Assert.AreEqual( APIStage.BETA, apiStageConstructorClient.ApiStage );
        }

        [TestMethod]
        public async Task GetReleaseTests() {
            var client = new ScoposDataClient( APIStage.BETA );

            ReleasePhase level = ReleasePhase.PRODUCTION;
            var appList = new List<string>() { "orion", "athena", "greengrassv2Deployment" };
            var appVersion = Version.Parse( "2.21.1" );
            var response = await client.GetReleasePublicAsync( level, appList, "000015-orion-001", appVersion, false, false, "00015" );

            Assert.AreEqual( Responses.RequestStatusCode.OK, response.OverallStatusCode, $"Expecting and OK status code, instead received Overall {response.OverallStatusCode} with REST API Status {response.RestApiStatusCode}, and message {response.ExceptionMessage}." );

            Assert.IsNotNull( response.ApplicationRelease );

            var releaseNotes = response.ApplicationRelease;
            foreach (var item in releaseNotes.Items) {
                Console.Write( item.Application + ": " );
                Console.Write( item.Version.ToString() + "\n" );
            }

            Assert.AreEqual( appList.Count, releaseNotes.Items.Count );

            //We should have one ReleaseNote for Orion, and one for Athena
            var orionExists = releaseNotes.Items.Any( releaseInfo => releaseInfo.Application == ApplicationName.ORION );
            var athenaExists = releaseNotes.Items.Any( releaseInfo => releaseInfo.Application == ApplicationName.ATHENA );
            Assert.IsTrue( orionExists );
            Assert.IsTrue( athenaExists );

            //Both should haver versions that are greater than 1.4
            var orionVersion = releaseNotes.Items.First( releaseInfo => releaseInfo.Application == ApplicationName.ORION ).Version;
            var athenaVersion = releaseNotes.Items.First( releaseInfo => releaseInfo.Application == ApplicationName.ATHENA ).Version;
            var referenceVersion = Version.Parse( "1.4.0.0" );
            Assert.IsTrue( orionVersion > referenceVersion );
            Assert.IsTrue( athenaVersion > referenceVersion );

        }

        [TestMethod]
        public async Task GetProductionVersionTests() {
            var client = new ScoposDataClient();

            var defaultVersionOnError = Version.Parse( "1.0.0" );
            // As of June 2026, we expect Orion to be at least version 2.25.0, and Athena to be at least 1.12.0. If we are getting the default version, or versions that are too old, then something is likely wrong with the API or our parsing of the response.
            var minimumOrionExpectedVersion = Version.Parse( "2.25.0" );
            var minimumAthenaExpectedVersion = Version.Parse( "1.12.0" );

            // Ask for the production version for Orion. This will also populate the cache, so subsequent calls should be faster and return the same value.
            var firstCallTimer = System.Diagnostics.Stopwatch.StartNew();
            var orionVersion = await client.GetProductionVersionAsync( ApplicationName.ORION );
            firstCallTimer.Stop();

            Assert.IsNotNull( orionVersion );
            Assert.IsTrue( orionVersion > defaultVersionOnError, $"Expecting the version to be greater than {defaultVersionOnError}, instead received {orionVersion}." );
            Assert.IsTrue( orionVersion >= minimumOrionExpectedVersion, $"Expecting the version to be at least {minimumOrionExpectedVersion}, instead received {orionVersion}." );

            // Ask for the production version for Athena. This should be pulled from cache if the API is working correctly, and should be much faster than the first call.
            var secondCallTimer = System.Diagnostics.Stopwatch.StartNew();
            var athenaVersion = await client.GetProductionVersionAsync( ApplicationName.ATHENA );
            secondCallTimer.Stop();

            Assert.IsNotNull( athenaVersion );
            Assert.IsTrue( athenaVersion > defaultVersionOnError, $"Expecting the version to be greater than {defaultVersionOnError}, instead received {athenaVersion}." );
            Assert.IsTrue( athenaVersion >= minimumAthenaExpectedVersion, $"Expecting the version to be at least {minimumAthenaExpectedVersion}, instead received {athenaVersion}." );

            //Since values are cached, the second call should be much faster.
            Assert.IsTrue( firstCallTimer.ElapsedMilliseconds > 100 * secondCallTimer.ElapsedMilliseconds, $"Expecting the second call to be faster than the first call, but the first call took {firstCallTimer.ElapsedMilliseconds} ms and the second call took {secondCallTimer.ElapsedMilliseconds} ms." );
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
