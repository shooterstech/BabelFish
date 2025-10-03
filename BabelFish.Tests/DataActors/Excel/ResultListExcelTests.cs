using Scopos.BabelFish.APIClients;
using Scopos.BabelFish.DataActors.Excel;
using Scopos.BabelFish.DataModel.OrionMatch;
using Scopos.BabelFish.Requests.OrionMatchAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BabelFish.Tests.DataActors.Excel
{
    [TestClass]
    public class ResultListExcelTests : BaseTestClass
    {
        [TestMethod]
        public async Task ResultListExcelTest() 
        {
            var client = new OrionMatchAPIClient();

            //This match id has three relays of 20 athletes
            var matchId = new MatchID("1.1.2025072316000865.0");
            var resultListName = "Individual - All";

            var getResultListResponse = await client.GetResultListPublicAsync(matchId, resultListName);
            var resultList = getResultListResponse.ResultList;

            while (getResultListResponse.HasMoreItems)
            {
                var nextRequest = (GetResultListPublicRequest)getResultListResponse.GetNextRequest();
                getResultListResponse = await client.GetResultListPublicAsync(nextRequest);
                resultList.Items.AddRange(getResultListResponse.ResultList.Items);
            }

            var match = (await client.GetMatchAsync(matchId)).Match;

            var excel = new ResultListExcel(resultList);
            var package = excel.GenerateExcel( "c:\\temp\\hello.xlsx" );

            //await package.SaveAsAsync("c:\\temp\\hello.xlsx");
        }
    }
}
