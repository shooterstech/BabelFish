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
        public async Task BasicGenerationTest() {

            var client = new OrionMatchAPIClient( );

            //This match id has three relays of 20 athletes
            var matchId = new MatchID( "1.5168.2025071316261327.0" );
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

            pdf.GeneratePdf(PageSizes.Letter.Landscape() );

        }
    }
}
