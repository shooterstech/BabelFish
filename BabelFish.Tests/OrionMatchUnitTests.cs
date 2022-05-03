using System.Net;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BabelFish;

namespace BabelFish.Tests
{
    [TestClass]
    public class OrionMatchUnitTests
    {
        private static string xApiKey = "wjM7eCb75aa3Okxj4FliXLY0VjHidoE2ei18pdg1";
        private readonly OrionMatchAPIClient _client = new OrionMatchAPIClient(xApiKey);

        [TestMethod]
        public void OrionMatchExpectedFailuresUnitTests() {

            var response = _client.GetMatchDetailAsync("1.2345.6789012345678901.0");

            var MessageResponse = response.Result.MessageResponse;
            Assert.AreEqual(response.Result.StatusCode, HttpStatusCode.NotFound);
            Assert.IsTrue(MessageResponse.Message.Count>0);
            Assert.IsTrue(MessageResponse.ResponseCodes.Contains("NotFound"));

            OrionMatchAPIClient _client2 = new OrionMatchAPIClient("abc123");
            response = _client2.GetMatchDetailAsync("1.2899.1040248529.0");
            Assert.AreEqual(response.Result.StatusCode, HttpStatusCode.Forbidden);
        }

        [TestMethod]
        public void OrionMatchAPI_Assigns_XApiKey()
        {
            string TestXApiKey = "mock_api_key_value";
            Assert.IsTrue(_client.XApiKey.Length > 0);
        }

        [TestMethod]
        public void OrionMatchAPI_GetAMatch() {

            var response = _client.GetMatchDetailAsync("1.2268.2022021516475240.0");
            Assert.IsNotNull(response);

            var match = response.Result.Match;
            var matchName = match.Name;

            Assert.IsNotNull(matchName);
            Assert.AreNotEqual(matchName, "");
        }

        [TestMethod]
        public void OrionMatchAPI_GetAResultList()
        {
            string MatchID = "1.2899.1040248529.0";
            string ResultListName = "Individual - All";
            var response = _client.GetResultListAsync(MatchID, ResultListName);
            Assert.IsNotNull(response);

            var result = response.Result.ResultList;
            var resultName = result.ResultName;

            Assert.IsNotNull(resultName);
            Assert.AreNotEqual(resultName, "");
        }

        [TestMethod]
        public void OrionMatchAPI_GetACourseOfFire()
        {
            string COFID = "03b0a667-f184-404b-8ba7-751599b7fd0b";
            var response = _client.GetResultCourseOfFireDetail(COFID);
            Assert.IsNotNull(response);

            var result = response.Result.ResultCOF;
            var resultName = result.MatchName;

            Assert.IsNotNull(resultName);
            Assert.AreNotEqual(resultName, "");
        }

        [TestMethod]
        public void OrionMatchAPI_GetAMatchSearch()
        {
            System.DateTime now = System.DateTime.Now;
            int DistanceSearch = 250;
            string StartingDate = new System.DateTime(now.Year, now.Month, 1).ToString("yyyy-MM-dd");
            string EndingDate = new System.DateTime(now.Year, now.Month, 1).AddMonths(1).AddDays(-1).ToString("yyyy-MM-dd");
            string ShootingStyle = "Air Rifle";
            int NumberOfMatchesToReturn = 100;
            double Longitude = -77.555569;
            double Latitude = 38.739453;

            var response = _client.GetMatchSearchAsync(
                DistanceSearch, StartingDate, EndingDate, ShootingStyle, NumberOfMatchesToReturn, Longitude, Latitude);
            Assert.IsNotNull(response);

            var result = response.Result.SearchList;
            var resultName = result[0].MatchContact;

            Assert.IsNotNull(resultName);
            Assert.AreNotEqual(resultName, "");
        }

        [TestMethod]
        public void OrionMatchAPI_GetASquaddingList()
        {
            string MatchID = "1.2899.1040248529.0";
            string SquaddingListName = "Individual";

            var response = _client.GetSquaddingListAsync(MatchID,SquaddingListName);
            Assert.IsNotNull(response);

            var result = response.Result.Squadding;
            var resultName = result.SquaddingList[0].Participant.DisplayName;

            Assert.IsNotNull(resultName);
            Assert.AreNotEqual(resultName, "");
        }

        [TestMethod]
        public void OrionMatchAPI_GetAMatchLocations()
        {

            var response = _client.GetMatchLocationsAsync();
            Assert.IsNotNull(response);

            var locations = response.Result.MatchLocations;
            Assert.IsTrue(locations.Count>0);
            var locationName = locations.FirstOrDefault().City;

            Assert.IsNotNull(locationName);
            Assert.AreNotEqual(locationName, "");
        }

        [TestMethod]
        public void OrionMatchAPI_AuthAWSSignaturev4()
        {
            Dictionary<string, string> clientParams = new Dictionary<string, string>()
            {
                {"UserName", "test_dev_7@shooterstech.net"},
                {"PassWord", "abcd1234"},
            };
            OrionMatchAPIClient _client3 = new OrionMatchAPIClient(xApiKey, clientParams);
            var response = _client3.GetMatchDetailAsync("1.2899.1040248529.0", true);
            Assert.IsNotNull(response);

            var match = response.Result.Match;
            var matchName = match.Name;
            // Need to find an Auth specific combination that returns extra values

            Assert.IsNotNull(matchName);
            Assert.AreNotEqual(matchName, "");
        }

        [TestMethod]
        public void OrionMatchAPI_GetMatchParticipantList()
        {
            string MatchID = "1.3197.2022042721544126.0";

            var response = _client.GetMatchParticipantListAsync(MatchID);
            Assert.IsNotNull(response);

            var result = response.Result.ParticipantList;
            var resultName = result[0].Participant.DisplayName;

            Assert.IsNotNull(resultName);
            Assert.AreNotEqual(resultName, "");
        }
    }
}