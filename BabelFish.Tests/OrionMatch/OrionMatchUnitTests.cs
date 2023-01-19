using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scopos.BabelFish.APIClients;
using Scopos.BabelFish.Runtime;

namespace Scopos.BabelFish.Tests.OrionMatch

{
    [TestClass]
    public class OrionMatchUnitTests {

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

        [TestMethod]
        public void OrionMatchExpectedFailuresUnitTests() {


            var client = new OrionMatchAPIClient( Constants.X_API_KEY, APIStage.BETA );
            var response = client.GetMatchDetailAsync("1.2345.6789012345678901.0");

            var MessageResponse = response.Result.MessageResponse;
            Assert.AreEqual(response.Result.StatusCode, HttpStatusCode.NotFound);
            Assert.IsTrue(MessageResponse.Message.Count>0);
            Assert.IsTrue(MessageResponse.Message.Any(x => x.Contains("could not be found")));

            OrionMatchAPIClient _client2 = new OrionMatchAPIClient("abc123");
            response = _client2.GetMatchDetailAsync("1.2899.1040248529.0");
            Assert.AreEqual(response.Result.StatusCode, HttpStatusCode.Forbidden);
        }

        [TestMethod]
        public void OrionMatchAPI_GetAMatch() {

            var client = new OrionMatchAPIClient( Constants.X_API_KEY, APIStage.BETA );
            var response = client.GetMatchDetailAsync("1.2268.2022021516475240.0");
            Assert.IsNotNull(response);

            var match = response.Result.Match;
            var matchName = match.Name;

            Assert.IsNotNull(matchName);
            Assert.AreNotEqual(matchName, "");
        }

        [TestMethod]
        public void OrionMatchAPI_GetAResultList()
        {

            var client = new OrionMatchAPIClient( Constants.X_API_KEY, APIStage.BETA );

            string MatchID = "1.2899.1040248529.0";
            string ResultListName = "Individual - All";
            var response = client.GetResultListAsync(MatchID, ResultListName);
            Assert.IsNotNull(response);

            var result = response.Result.ResultList;
            var resultName = result.ResultName;

            Assert.IsNotNull(resultName);
            Assert.AreNotEqual(resultName, "");
        }

        [TestMethod]
        public void OrionMatchAPI_GetACourseOfFire()
        {

            var client = new OrionMatchAPIClient( Constants.X_API_KEY, APIStage.BETA );
            string COFID = "29b3b450-e796-4329-9ebb-cd841c6eab3e";
            var response = client.GetResultCourseOfFireDetail(COFID);
            Assert.IsNotNull(response);

            var result = response.Result.ResultCOF;
            var resultName = result.MatchName;

            Assert.IsNotNull(resultName);
            Assert.AreNotEqual(resultName, "");
        }

        [TestMethod]
        public void OrionMatchAPI_GetACourseOfFireOldFormat500error() {

            var client = new OrionMatchAPIClient( Constants.X_API_KEY, APIStage.BETA );
            string COFID = "03b0a667-f184-404b-8ba7-751599b7fd0b";
            var response = client.GetResultCourseOfFireDetail(COFID);
            Assert.IsNotNull(response);


            var result = response.Result;
            Assert.IsTrue(result.MessageResponse.Message.Any(x => x.Contains("Unable to Convert Result COF to 2022-04-09 format.")));
            Assert.AreEqual(result.StatusCode, HttpStatusCode.InternalServerError);
        }

        [TestMethod]
        public void OrionMatchAPI_GetAMatchSearch()
        {

            var client = new OrionMatchAPIClient( Constants.X_API_KEY, APIStage.BETA );

            Scopos.BabelFish.Requests.OrionMatchAPI.GetMatchSearchRequest requestParameters = new Scopos.BabelFish.Requests.OrionMatchAPI.GetMatchSearchRequest()
            {
                DistanceSearch = 5,
                StartDate = new DateTime(DateTime.Now.Year, 6, 1),
                EndDate = DateTime.Today,
                ShootingStyle = new List<string>() { "Air Rifle", "Smallbore Rifle" },
                NumberOfMatchesToReturn = 100,
                Longitude = -77.555569,
                Latitude = 38.739453,
            };

            var response = client.GetMatchSearchAsync(requestParameters);
            Assert.IsNotNull(response);

            var listOfMatches = response.Result.SearchList;
            var matchName = listOfMatches[0].Name;

            Assert.IsNotNull( listOfMatches );
            Assert.AreNotEqual(matchName, "");
        }


        [TestMethod]
        public void OrionMatchAPI_GetASquaddingList()
        {

            var client = new OrionMatchAPIClient( Constants.X_API_KEY, APIStage.BETA );

            string MatchID = "1.2899.1040248529.0";
            string SquaddingListName = "Individual";

            var response = client.GetSquaddingListAsync(MatchID,SquaddingListName);
            Assert.IsNotNull(response);

            var result = response.Result.Squadding;
            var resultName = result.SquaddingList[0].Participant.DisplayName;

            Assert.IsNotNull(resultName);
            Assert.AreNotEqual(resultName, "");
        }

        [TestMethod]
        public void OrionMatchAPI_GetAMatchLocations()
        {

            var client = new OrionMatchAPIClient( Constants.X_API_KEY, APIStage.BETA );

            var response = client.GetMatchLocationsAsync();
            Assert.IsNotNull(response);

            var locations = response.Result.MatchLocations;
            Assert.IsTrue(locations.Count>0);
            var locationName = locations.FirstOrDefault().City;

            Assert.IsNotNull(locationName);
            Assert.AreNotEqual(locationName, "");
        }

        [TestMethod]
        public void OrionMatchAPI_GetMatchParticipantList()
        {

            var client = new OrionMatchAPIClient( Constants.X_API_KEY, APIStage.BETA );
            string MatchID = "1.3197.2022042721544126.0";

            var response = client.GetMatchParticipantListAsync(MatchID);
            Assert.IsNotNull(response);

            var result = response.Result.ParticipantList;
            var resultName = result[0].Participant.DisplayName;

            Assert.IsNotNull(resultName);
            Assert.AreNotEqual(resultName, "");
        }
    }
}