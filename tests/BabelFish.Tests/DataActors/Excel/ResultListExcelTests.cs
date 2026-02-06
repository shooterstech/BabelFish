using System.Threading.Tasks;
using Scopos.BabelFish.APIClients;
using Scopos.BabelFish.DataActors.Excel;
using Scopos.BabelFish.DataModel.OrionMatch;
using Scopos.BabelFish.Requests.OrionMatchAPI;

namespace BabelFish.Tests.DataActors.Excel {
    [TestClass]
    public class ResultListExcelTests : BaseTestClass {
        [TestMethod]
        public async Task ResultListExcelTest() {
            var client = new OrionMatchAPIClient();

            //12 athletes, one relay.
            var matchId = new MatchID( "1.1.2026020210021670.1" );
            var resultListName = "Team - All";

            var getResultListResponse = await client.GetResultListPublicAsync( matchId, resultListName );
            var resultList = getResultListResponse.ResultList;

            while (getResultListResponse.HasMoreItems) {
                var nextRequest = (GetResultListPublicRequest)getResultListResponse.GetNextRequest();
                getResultListResponse = await client.GetResultListPublicAsync( nextRequest );
                resultList.Items.AddRange( getResultListResponse.ResultList.Items );
            }

            var excel = await ResultListExcel.FactoryAsync( resultList );

            var package = excel.GenerateExcel( "c:\\temp\\hello.xlsx" );

            //await package.SaveAsAsync("c:\\temp\\hello.xlsx");
        }
    }
}
