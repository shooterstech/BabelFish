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
            var matchId = new MatchID( "1.1.2026012816222333.0" );
            var resultListName = "Individual - All";

            var getResultListResponse = await client.GetResultListPublicAsync( matchId, resultListName );
            var resultList = getResultListResponse.ResultList;

            while (getResultListResponse.HasMoreItems) {
                var nextRequest = (GetResultListPublicRequest)getResultListResponse.GetNextRequest();
                getResultListResponse = await client.GetResultListPublicAsync( nextRequest );
                resultList.Items.AddRange( getResultListResponse.ResultList.Items );
            }

            var getSquaddingListResponse = await client.GetSquaddingListPublicAsync( matchId, "Qualification" );
            var squaddingList = getSquaddingListResponse.SquaddingList;

            var excel = await ResultListExcel.FactoryAsync( resultList, squaddingList );

            var package = excel.GenerateExcel( "c:\\temp\\hello.xlsx" );

            //await package.SaveAsAsync("c:\\temp\\hello.xlsx");
        }
    }
}
