using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scopos.BabelFish.APIClients;
using Scopos.BabelFish.Requests.OrionMatchAPI;
using Scopos.BabelFish.DataModel.OrionMatch;
using Scopos.BabelFish.DataModel.AttributeValue;

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
            var taskNotFound = client.GetMatchDetailPublicAsync( "1.2345.6789012345678901.0" );

            var matchNotFoundResponse = taskNotFound.Result;
            Assert.AreEqual( HttpStatusCode.NotFound, matchNotFoundResponse.StatusCode );
            Assert.IsTrue( matchNotFoundResponse.MessageResponse.Message.Count > 0 );
            Assert.IsTrue( matchNotFoundResponse.MessageResponse.Message.Any( x => x.Contains( "could not be found" ) ) );

            //Match id with visibility set to PROTECTED, which can not be viewed from the public api call
            var taskUnauthorized = client.GetMatchDetailPublicAsync( "1.1.2021031511174545.0" );

            var matchUnauthorizedResponse = taskUnauthorized.Result;
            Assert.AreEqual( HttpStatusCode.Unauthorized, matchUnauthorizedResponse.StatusCode );
            Assert.IsTrue( matchUnauthorizedResponse.MessageResponse.Message.Count > 0 );
            Assert.IsTrue( matchUnauthorizedResponse.MessageResponse.Message.Any( x => x.Contains( "does not have permission" ) ) );
        }

        [TestMethod]
        public void OrionMatchAPI_GetAMatch() {

            var client = new OrionMatchAPIClient( Constants.X_API_KEY, APIStage.BETA );
            var matchId = "1.1.2023011915575119.0";
            var taskResponse = client.GetMatchDetailPublicAsync( matchId );

            var response = taskResponse.Result;
            Assert.AreEqual( HttpStatusCode.OK, response.StatusCode );

            var match = response.Match;

            //Perform some simple tests on the returned data.
            Assert.AreEqual( matchId, match.MatchID );
            Assert.AreEqual( "Unit Test Match", match.Name );
            Assert.AreEqual( VisibilityOption.PUBLIC, match.Visibility );
            Assert.AreEqual( "2023-01-19", match.StartDate );
        }

        [TestMethod]
        public void OrionMatchAPI_GetAResultList() {

            var client = new OrionMatchAPIClient( Constants.X_API_KEY, APIStage.BETA );

            var matchId = "1.1.2023011915575119.0";
            var resultListName = "Individual - All";
            var taskResponse = client.GetResultListPublicAsync( matchId, resultListName );

            var response = taskResponse.Result;
            Assert.AreEqual( HttpStatusCode.OK, response.StatusCode );

            var resultList = response.ResultList;

            Assert.AreEqual( resultListName, resultList.ResultName );
            Assert.AreEqual( "Qualification", resultList.EventName );
            Assert.IsTrue( resultList.Results.Count > 0 );
        }

        [TestMethod]
        public void OrionMatchAPI_GetACourseOfFire() {
            //The first part of this tess retreives the Result List. Which we use to get the first ResultCOFID
            var client = new OrionMatchAPIClient( Constants.X_API_KEY, APIStage.BETA );

            var matchId = "1.1.2023011915575119.0";
            var resultListName = "Individual - All";
            var taskResultListResponse = client.GetResultListPublicAsync( matchId, resultListName );

            var resultListResponse = taskResultListResponse.Result;
            Assert.AreEqual( HttpStatusCode.OK, resultListResponse.StatusCode );
            var resultList = resultListResponse.ResultList;
            Assert.IsTrue( resultList.Results.Count > 0 );

            //Now read the resultCofId and some other values from the ResultList
            var resultCofId = resultList.Results[0].ResultCOFID;
            var userId = resultList.Results[0].UserID;
            var displayName = resultList.Results[0].DisplayName;
            var score = resultList.Results[0].Score;

            //Now we can retreive the ResultCOF Detail
            var taskResultCofIdResponse = client.GetResultCourseOfFireDetailPublicAsync( resultCofId );

            var resultCofResponse = taskResultCofIdResponse.Result;
            Assert.AreEqual( HttpStatusCode.OK, resultCofResponse.StatusCode );
            var resultCof = resultCofResponse.ResultCOF;

            //Test basic return data
            Assert.AreEqual( matchId, resultCof.MatchID );
            Assert.AreEqual( resultCofId, resultCof.ResultCOFID );
            Assert.AreEqual( displayName, resultCof.Participant.DisplayName );

            //This next few tests makes sure the abstract conversion of .Participant worked as expected.
            Assert.AreEqual( Individual.CONCRETE_CLASS_ID, resultCof.Participant.ConcreteClassId );
            Assert.IsTrue( resultCof.Participant is Individual );
            var individual = (Individual)resultCof.Participant;
            Assert.AreEqual( userId, individual.UserID );

            Assert.IsTrue( resultCof.EventScores.Count > 0 );
            Assert.IsTrue( resultCof.Shots.Count > 0 );
        }

        [TestMethod]
        public void OrionMatchAPI_GetAMatchSearch() {

            var client = new OrionMatchAPIClient( Constants.X_API_KEY, APIStage.BETA );

            GetMatchSearchPublicRequest requestParameters = new GetMatchSearchPublicRequest() {
                DistanceSearch = 5,
                StartDate = new DateTime( 2023, 1, 1 ),
                EndDate = new DateTime( 2023, 1, 31 ),
                ShootingStyle = new List<string>() { "Air Rifle", "Smallbore Rifle" },
                NumberOfMatchesToReturn = 100,
                Longitude = -77.555569,
                Latitude = 38.739453,
            };
            Assert.AreEqual( "MatchSearch", requestParameters.OperationId );

            var taskMatchSearchResponse = client.GetMatchSearchPublicAsync( requestParameters );
            var matchSearchResponse = taskMatchSearchResponse.Result;
            Assert.AreEqual( HttpStatusCode.OK, matchSearchResponse.StatusCode );

            var listOfMatches = matchSearchResponse.SearchList;
            Assert.IsTrue( listOfMatches.Count > 0 );
        }

        [TestMethod]
        public void OrionMatchAPI_GetAMatchLocations() {

            //NOTE: This is one of the few occasions where it is appropriate to run this test in PRODUCTION, as the GetMatchLocations call relies on fresh (not stale) data.
            var client = new OrionMatchAPIClient( Constants.X_API_KEY, APIStage.PRODUCTION );

            var response = client.GetMatchLocationsPublicAsync();
            Assert.IsNotNull( response );

            var locations = response.Result.MatchLocations;
            Assert.IsTrue( locations.Count > 0 );
            var locationName = locations.FirstOrDefault().City;

            Assert.IsNotNull( locationName );
            Assert.AreNotEqual( locationName, "" );
        }
    }
}