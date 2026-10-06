using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Scopos.BabelFish.APIClients;
using Scopos.BabelFish.DataModel.Common;
using Scopos.BabelFish.DataModel.Definitions;
using Scopos.BabelFish.DataModel.OrionMatch;
using Scopos.BabelFish.Responses.OrionMatchAPI;
using Scopos.BabelFish.Tests.DataModel.DefinitionTests;

namespace Scopos.BabelFish.Tests.APIClients.OrionMatchAPIClientTests {
    [TestClass]
    public class OrionMatchPublicUnitTests : BaseTestClass {

        /// <summary>
        /// Unit test to confirm the Constructors set the api key and API stage as expected.
        /// </summary>
        [TestMethod]
        public void BasicConstructorTests() {

            var defaultConstructorClient = new OrionMatchAPIClient();
            var apiStageConstructorClient = new OrionMatchAPIClient( APIStage.BETA );

            Assert.AreEqual( APIStage.PRODUCTION, defaultConstructorClient.ApiStage );

            Assert.AreEqual( APIStage.BETA, apiStageConstructorClient.ApiStage );
        }

        /// <summary>
        /// Pass in a fake match id and check that a NotFound is returned. Then a match with PROTECTED visibility, to check Unauthroized is retrun.
        /// </summary>
        [TestMethod]
        public void OrionMatchExpectedFailuresUnitTests() {

            var client = new OrionMatchAPIClient( APIStage.BETA );
            //Pass in a fake match id
            var taskNotFound = client.GetMatchPublicAsync( new MatchID( "1.2345.6789012345678901.0" ) );

            var matchNotFoundResponse = taskNotFound.Result;
            Assert.AreEqual( HttpStatusCode.NotFound, matchNotFoundResponse.RestApiStatusCode );
            Assert.IsTrue( matchNotFoundResponse.MessageResponse.Message.Count > 0 );
            Assert.IsTrue( matchNotFoundResponse.MessageResponse.Message.Any( x => x.Contains( "could not be found" ) ) );

            //Match id with visibility set to PROTECTED, which can not be viewed from the public api call
            var taskUnauthorized = client.GetMatchPublicAsync( new MatchID( "1.1.2021031511174545.0" ) );

            var matchUnauthorizedResponse = taskUnauthorized.Result;
            Assert.AreEqual( HttpStatusCode.Unauthorized, matchUnauthorizedResponse.RestApiStatusCode );
            Assert.IsTrue( matchUnauthorizedResponse.MessageResponse.Message.Count > 0 );
            Assert.IsTrue( matchUnauthorizedResponse.MessageResponse.Message.Any( x => x.Contains( "does not have permission" ) ) );
        }

        [TestMethod]
        public async Task OrionMatchAPI_GetAMatch() {

            var client = new OrionMatchAPIClient( APIStage.BETA );
            var matchId = new MatchID( "1.1.2023011915575119.0" );
            var response = await client.GetMatchPublicAsync( matchId );

            Assert.AreEqual( HttpStatusCode.OK, response.RestApiStatusCode );

            var match = response.Match;

            //Perform some simple tests on the returned data.
            Assert.AreEqual( matchId.ToString(), match.MatchID.ToString() );
            Assert.AreEqual( "Unit Test Match", match.Name );
            Assert.AreEqual( VisibilityOption.PUBLIC, match.Visibility );
            Assert.AreEqual( new DateTime( 2023, 1, 19 ), match.StartDate );
            //The following test is added to check that the OnDeserialized method is being called, which sets the MatchStructure.Match property to the match itself. If this test fails, it may indicate an issue with the deserialization process.
            Assert.AreEqual( match, match.MatchStructure.Match );
        }

        /// <summary>
        /// Tests that the GetMatchPublicAsync method returns the expected metadata, and that the caching mechanism is working as intended. The test first
        /// retrieves a match and checks that the metadata is present and of the correct type. It then retrieves the same match again to confirm that the
        /// response is coming from the cache, and that the metadata in the cached response matches the original metadata. Finally, it checks that the
        /// time taken to retrieve the cached response is significantly less than the time taken to retrieve the original response, which would indicate
        /// that the caching mechanism is providing a performance benefit.
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task MatchMetaDataTests() {
            var client = new OrionMatchAPIClient( APIStage.PRODUCTION );
            var matchId = new MatchID( "1.1.2026040108475723.1" );
            var origResponse = await client.GetMatchPublicAsync( matchId );
            Assert.IsTrue( origResponse.HasOkStatusCode );

            var metaData = origResponse.MetaData;
            Assert.IsNotNull( metaData );
            Assert.IsInstanceOfType( metaData, typeof( MatchDetailMetaData ) );
            var matchDetailMetaData = (MatchDetailMetaData)metaData;

            Assert.IsTrue( matchDetailMetaData.Tournaments.Count > 0 );

            var cachedResponse = await client.GetMatchPublicAsync( matchId );
            Assert.IsTrue( cachedResponse.HasOkStatusCode );
            Assert.IsTrue( cachedResponse.InMemoryCachedResponse );

            var cachedMetaData = cachedResponse.MetaData;
            Assert.IsNotNull( cachedMetaData );
            Assert.IsInstanceOfType( cachedMetaData, typeof( MatchDetailMetaData ) );

            Assert.AreEqual( matchDetailMetaData.Tournaments.Count, ((MatchDetailMetaData)cachedMetaData).Tournaments.Count );
            Assert.AreEqual( matchDetailMetaData.HtmlReports.Count, ((MatchDetailMetaData)cachedMetaData).HtmlReports.Count );

            // The following test is added to confirm that the cached response is significantly faster than the original response, which would indicate that the caching mechanism is working as intended. 
            Assert.IsTrue( origResponse.TimeToRun.TotalMilliseconds > cachedResponse.TimeToRun.TotalMilliseconds * 10 );
        }

        [TestMethod]
        [Ignore]
        public async Task EriksPlayground() {
            var cof = CourseOfFireHelper.Get_60_Standing_Cof();

            var json = JsonSerializer.Serialize( cof, SerializerOptions.SystemTextJsonDeserializer );

            var cof2 = JsonSerializer.Deserialize<CourseOfFire>( json, SerializerOptions.SystemTextJsonDeserializer );

            var jsonDocument = JsonDocument.Parse( json );

            var cof3 = JsonSerializer.Deserialize<CourseOfFire>( jsonDocument, SerializerOptions.SystemTextJsonDeserializer );

            Console.WriteLine( json );
        }
    }
}
