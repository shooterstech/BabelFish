using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scopos.BabelFish.APIClients;
using Scopos.BabelFish.DataActors.PDF;
using Scopos.BabelFish.DataModel.OrionMatch;

namespace BabelFish.Tests.DataActors.PDF {
    [TestClass]
    public class ResultListPdfTests  : BaseTestClass {

        [TestMethod]
        public async Task BasicGenerationTest() {

            var client = new OrionMatchAPIClient( );

            //This match id has three relays of 20 athletes
            var matchId = new MatchID( "1.1.2025072113222488.0" );
            var resultListName = "Individual - All";

            var getResultListResponse = await client.GetResultListPublicAsync( matchId, resultListName );
            var resultList = getResultListResponse.ResultList;

            var pdf = new ResultListPdf(  resultList );
            await pdf.InitializeAsync();

            pdf.GeneratePdf();

        }
    }
}
