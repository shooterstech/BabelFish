using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuestPDF.Helpers;
using Scopos.BabelFish.APIClients;
using Scopos.BabelFish.DataActors.PDF;
using Scopos.BabelFish.DataModel.OrionMatch;
using Scopos.BabelFish.Requests.OrionMatchAPI;

namespace BabelFish.Tests.DataActors.PDF {
    [TestClass]
    public class ResultListPdfTests  : BaseTestClass {

        [TestMethod]
        public async Task GenerateResultListTest() {

            var client = new OrionMatchAPIClient( );

            //This match id has three relays of 20 athletes
            var matchId = new MatchID( "1.1.2025072316000865.0" );
            var resultListName = "Individual - All";

            var getResultListResponse = await client.GetResultListPublicAsync( matchId, resultListName );
            var resultList = getResultListResponse.ResultList;

            while (getResultListResponse.HasMoreItems) {
                var nextRequest = (GetResultListPublicRequest) getResultListResponse.GetNextRequest();
                getResultListResponse = await client.GetResultListPublicAsync( nextRequest );
                resultList.Items.AddRange( getResultListResponse.ResultList.Items );
            }

            var match = (await client.GetMatchAsync( matchId )).Match;

            var pdf = new ResultListPdf(  match, resultList );
            await pdf.InitializeAsync();

            pdf.GeneratePdf(PageSizes.Letter, "c:\\temp\\hello.pdf" );

        }

        [TestMethod]
        public async Task GenerateResultCOFTest() {

            var client = new OrionMatchAPIClient();

            var resultCofId = "0f814586-3513-411a-8229-914d4608db05";

            var getResultCofResponse = await client.GetResultCourseOfFireDetailPublicAsync( resultCofId );
            var resultCof = getResultCofResponse.ResultCOF;

            var pdf = new ResultCOFPdf( resultCof, Scopos.BabelFish.DataModel.Definitions.EventtType.STAGE );
            await pdf.InitializeAsync();

            pdf.GeneratePdf( PageSizes.Letter, "c:\\temp\\hello.pdf" );

        }

        [TestMethod]
        public async Task GenerateGroupingTest()
        {

            var client = new OrionMatchAPIClient();

            var resultCofId = "0f814586-3513-411a-8229-914d4608db05";

            var getResultCofResponse = await client.GetResultCourseOfFireDetailPublicAsync(resultCofId);
            var resultCof = getResultCofResponse.ResultCOF;

            var pdf = new AthleteCOFPdf(resultCof, Scopos.BabelFish.DataModel.Definitions.EventtType.STAGE);
            await pdf.InitializeAsync();

            pdf.GeneratePdf(PageSizes.Letter, "c:\\temp\\helloWHAT.pdf");

        }
    }
}
