using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Scopos.BabelFish.APIClients;
using Scopos.BabelFish.Requests.OrionMatchAPI;
using Scopos.BabelFish.DataModel.OrionMatch;
using System.Text;
using System.Threading.Tasks;

namespace Scopos.BabelFish.Tests.OrionMatch
{
    [TestClass]
    public class ResultListExtensionsTests
    {

        [TestInitialize]
        public void InitializeTest()
        {
            Scopos.BabelFish.Runtime.Settings.XApiKey = Constants.X_API_KEY;
        }

        [TestMethod]
        public async Task GetResultListBasicPublicTest()
        {

            var client = new OrionMatchAPIClient(APIStage.BETA);

            //This match id has three relays of 20 athletes
            var matchId = new MatchID("1.1.2023011915575119.0");
            var resultListName = "Individual - All";

            var getResultListResponse = await client.GetResultListPublicAsync(matchId, resultListName);

            Assert.AreEqual(HttpStatusCode.OK, getResultListResponse.StatusCode);
            var resultList = getResultListResponse.ResultList;
            ResultStatus status = ResultStatus.FUTURE;
            //resultList.CalculateResultListStatus(status);

            Assert.AreEqual(matchId.ToString(), resultList.MatchID);
            Assert.AreEqual(resultListName, resultList.ResultName);

            Assert.IsTrue(resultList.Items.Count > 0);
        }
    }
}
