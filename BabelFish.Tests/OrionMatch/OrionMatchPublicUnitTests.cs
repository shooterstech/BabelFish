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

namespace Scopos.BabelFish.Tests.OrionMatch {
    [TestClass]
    public class OrionMatchPublicUnitTests {

        /// <summary>
        /// Unit test to confirm the Constructors set the api key and API stage as expected.
        /// </summary>
        [TestMethod]
        public void BasicConstructorTests() {

            var defaultConstructorClient = new OrionMatchAPIClient( Constants.X_API_KEY );
            var apiStageConstructorClient = new OrionMatchAPIClient( Constants.X_API_KEY, APIStage.BETA );

            Assert.AreEqual( Constants.X_API_KEY, defaultConstructorClient.XApiKey );
            Assert.AreEqual( APIStage.PRODUCTION, defaultConstructorClient.ApiStage );

            Assert.AreEqual( Constants.X_API_KEY, apiStageConstructorClient.XApiKey );
            Assert.AreEqual( APIStage.BETA, apiStageConstructorClient.ApiStage );
        }

        /// <summary>
        /// Pass in a fake match id and check that a NotFound is returned. Then a match with PROTECTED visibility, to check Unauthroized is retrun.
        /// </summary>
        [TestMethod]
        public void OrionMatchExpectedFailuresUnitTests() {

            var client = new OrionMatchAPIClient( Constants.X_API_KEY, APIStage.BETA );
            //Pass in a fake match id
            var taskNotFound = client.GetMatchDetailPublicAsync( new MatchID("1.2345.6789012345678901.0") );

            var matchNotFoundResponse = taskNotFound.Result;
            Assert.AreEqual( HttpStatusCode.NotFound, matchNotFoundResponse.StatusCode );
            Assert.IsTrue( matchNotFoundResponse.MessageResponse.Message.Count > 0 );
            Assert.IsTrue( matchNotFoundResponse.MessageResponse.Message.Any( x => x.Contains( "could not be found" ) ) );

            //Match id with visibility set to PROTECTED, which can not be viewed from the public api call
            var taskUnauthorized = client.GetMatchDetailPublicAsync( new MatchID( "1.1.2021031511174545.0" ) );

            var matchUnauthorizedResponse = taskUnauthorized.Result;
            Assert.AreEqual( HttpStatusCode.Unauthorized, matchUnauthorizedResponse.StatusCode );
            Assert.IsTrue( matchUnauthorizedResponse.MessageResponse.Message.Count > 0 );
            Assert.IsTrue( matchUnauthorizedResponse.MessageResponse.Message.Any( x => x.Contains( "does not have permission" ) ) );
        }

        [TestMethod]
        public async Task OrionMatchAPI_GetAMatch() {

            var client = new OrionMatchAPIClient( Constants.X_API_KEY, APIStage.BETA );
            var matchId = new MatchID( "1.1.2023011915575119.0" );
            var response = await client.GetMatchDetailPublicAsync( matchId );

            Assert.AreEqual( HttpStatusCode.OK, response.StatusCode );

            var match = response.Match;

            //Perform some simple tests on the returned data.
            Assert.AreEqual( matchId.ToString(), match.MatchID );
            Assert.AreEqual( "Unit Test Match", match.Name );
            Assert.AreEqual( VisibilityOption.PUBLIC, match.Visibility );
            Assert.AreEqual( "2023-01-19", match.StartDate );
        }
    }
}