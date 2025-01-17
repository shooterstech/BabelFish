using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scopos.BabelFish.APIClients;
using Scopos.BabelFish.Requests.OrionMatchAPI;
using Scopos.BabelFish.DataModel.OrionMatch;
using Scopos.BabelFish.DataModel.AttributeValue;
using System.Runtime.CompilerServices;
using Scopos.BabelFish.Tests.Definition;
using System.Text.Json;
using Scopos.BabelFish.Converters;
using Scopos.BabelFish.DataModel.Definitions;

namespace Scopos.BabelFish.Tests.OrionMatch {
    [TestClass]
    public class OrionMatchPublicUnitTests {

        [TestInitialize]
        public void InitializeTest() {
            Scopos.BabelFish.Runtime.Settings.XApiKey = Constants.X_API_KEY;
        }

        /// <summary>
        /// Unit test to confirm the Constructors set the api key and API stage as expected.
        /// </summary>
        [TestMethod]
        public void BasicConstructorTests() {

            var defaultConstructorClient = new OrionMatchAPIClient( );
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
            var taskNotFound = client.GetMatchPublicAsync( new MatchID("1.2345.6789012345678901.0") );

            var matchNotFoundResponse = taskNotFound.Result;
            Assert.AreEqual( HttpStatusCode.NotFound, matchNotFoundResponse.StatusCode );
            Assert.IsTrue( matchNotFoundResponse.MessageResponse.Message.Count > 0 );
            Assert.IsTrue( matchNotFoundResponse.MessageResponse.Message.Any( x => x.Contains( "could not be found" ) ) );

            //Match id with visibility set to PROTECTED, which can not be viewed from the public api call
            var taskUnauthorized = client.GetMatchPublicAsync( new MatchID( "1.1.2021031511174545.0" ) );

            var matchUnauthorizedResponse = taskUnauthorized.Result;
            Assert.AreEqual( HttpStatusCode.Unauthorized, matchUnauthorizedResponse.StatusCode );
            Assert.IsTrue( matchUnauthorizedResponse.MessageResponse.Message.Count > 0 );
            Assert.IsTrue( matchUnauthorizedResponse.MessageResponse.Message.Any( x => x.Contains( "does not have permission" ) ) );
        }

        [TestMethod]
        public async Task OrionMatchAPI_GetAMatch() {

            var client = new OrionMatchAPIClient( APIStage.BETA );
            var matchId = new MatchID( "1.1.2023011915575119.0" );
            var response = await client.GetMatchPublicAsync( matchId );

            Assert.AreEqual( HttpStatusCode.OK, response.StatusCode );

            var match = response.Match;

            //Perform some simple tests on the returned data.
            Assert.AreEqual( matchId.ToString(), match.MatchID );
            Assert.AreEqual( "Unit Test Match", match.Name );
            Assert.AreEqual( VisibilityOption.PUBLIC, match.Visibility );
            Assert.AreEqual( new DateTime(2023, 1, 19), match.StartDate );
        }

        [TestMethod]
        public async Task EriksPlayground() {
            var cof = CourseOfFireHelper.Get_60_Standing_Cof();

            var json = JsonSerializer.Serialize( cof, SerializerOptions.APIClientSerializer );

            var cof2 = JsonSerializer.Deserialize<CourseOfFire>( json, SerializerOptions.APIClientSerializer );

            var jsonDocument = JsonDocument.Parse( json );

            var cof3 = JsonSerializer.Deserialize<CourseOfFire>( jsonDocument, SerializerOptions.APIClientSerializer );

            Console.WriteLine( json );
        }
    }
}