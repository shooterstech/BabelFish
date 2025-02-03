using System.Net;
using System.Threading.Tasks;
using Scopos.BabelFish.APIClients;
using Scopos.BabelFish.DataModel.OrionMatch;

namespace Scopos.BabelFish.Tests.OrionMatch {
    [TestClass]
    public class ResultListExtensionsTests : BaseTestClass {

        [TestMethod]
        public async Task GetResultListBasicPublicTest() {

            var client = new OrionMatchAPIClient( APIStage.BETA );

            //This match id has three relays of 20 athletes
            var matchId = new MatchID( "1.1.2023011915575119.0" );
            var resultListName = "Individual - All";

            var getResultListResponse = await client.GetResultListPublicAsync( matchId, resultListName );

            Assert.AreEqual( HttpStatusCode.OK, getResultListResponse.StatusCode );
            var resultList = getResultListResponse.ResultList;
            ResultStatus status = ResultStatus.FUTURE;
            //resultList.CalculateResultListStatus(status);

            Assert.AreEqual( matchId.ToString(), resultList.MatchID );
            Assert.AreEqual( resultListName, resultList.ResultName );

            Assert.IsTrue( resultList.Items.Count > 0 );
        }
    }
}
