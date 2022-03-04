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

        //[TestMethod]
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

    }
}