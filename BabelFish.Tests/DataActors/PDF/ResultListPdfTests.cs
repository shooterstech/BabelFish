using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using Scopos.BabelFish.APIClients;
using Scopos.BabelFish.DataActors.PDF;
using Scopos.BabelFish.DataModel.OrionMatch;
using Scopos.BabelFish.Requests.OrionMatchAPI;

namespace BabelFish.Tests.DataActors.PDF {
    [TestClass]
    public class ResultListPdfTests  : BaseTestClass {

        [TestMethod]
        public async Task GenerateResultListPDFTest() {

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
            await pdf.RLIF.LoadSquaddingListAsync();
            pdf.RLIF.ShowRelay = "2";
            pdf.RLIF.ShowRanks = 3;

            pdf.GeneratePdf(PageSizes.Letter, "c:\\temp\\hello.pdf" );

		}

		[TestMethod]
		public async Task GenerateSquaddingtListPDFTest() {

			var client = new OrionMatchAPIClient();

			//This match id has three relays of 20 athletes
			var matchId = new MatchID( "1.1.2025081213222434.0" );
			var squaddingListName = "Qualification";

			var getResultListResponse = await client.GetSquaddingListPublicAsync( matchId, squaddingListName );
			var resultList = getResultListResponse.SquaddingList;

			var match = (await client.GetMatchAsync( matchId )).Match;

			var pdf = new ResultListPdf( match, resultList );
			await pdf.InitializeAsync();
            pdf.RLIF.ShowRelay = "1";
            pdf.SubTitle = "Relay 1";

			pdf.GeneratePdf( PageSizes.Letter, "c:\\temp\\hello.pdf" );

		}

		[TestMethod]
        public async Task GenerateResultCOFPDFTest() {

            var client = new OrionMatchAPIClient();

            var resultCofId = "0f814586-3513-411a-8229-914d4608db05";

            var getResultCofResponse = await client.GetResultCourseOfFireDetailPublicAsync( resultCofId );
            var resultCof = getResultCofResponse.ResultCOF;

            var pdf = new ResultCOFPdf( resultCof, Scopos.BabelFish.DataModel.Definitions.EventtType.STAGE );
            await pdf.InitializeAsync();

            pdf.GeneratePdf( PageSizes.Letter, "c:\\temp\\hello.pdf" );

        }

        [TestMethod]
        public async Task GenerateMergedAthleteCOFPDFTest() {

            var client = new OrionMatchAPIClient();

            //This match id has three relays of 20 athletes
            var matchId = new MatchID( "1.1.2025072316000865.0" );
            var resultListName = "Individual - All";

            var getResultListResponse = await client.GetResultListPublicAsync( matchId, resultListName );
            var resultList = getResultListResponse.ResultList;

            List<ResultCOF> documentsToPrint = new List<ResultCOF>();
            foreach ( var resultEvent in resultList.Items ) {
                var resultCofId = resultEvent.ResultCOFID;
                var getResultCof = await client.GetResultCourseOfFireDetailPublicAsync( resultCofId );
                var resultCof = getResultCof.ResultCOF;
                documentsToPrint.Add( resultCof );
            }

            await AthleteCOFPdf.GeneratePdfs( documentsToPrint, Scopos.BabelFish.DataModel.Definitions.EventtType.SERIES, PageSizes.Letter, "c:\\temp\\hello.pdf" );
        }

        [TestMethod]
        public async Task GenerateAthleteCofPdfTest()
        {

            var client = new OrionMatchAPIClient();

            //var resultCofId = "0f814586-3513-411a-8229-914d4608db05"; // sim air rifle
            //var resultCofId = "a85a5ed4-daeb-4488-a535-513bd590dfa1"; // air pistol
            //var resultCofId = "57b49cc3-db5d-4384-90eb-5be05d617664"; // Test scores
            var resultCofId = "0c9a775a-6390-4cb4-91f3-9724b699b5a9"; // Hit Miss

            var getResultCofResponse = await client.GetResultCourseOfFireDetailPublicAsync(resultCofId);
            var resultCof = getResultCofResponse.ResultCOF;

            var pdf = new AthleteCOFPdf(resultCof, Scopos.BabelFish.DataModel.Definitions.EventtType.STAGE);
            await pdf.InitializeAsync();

            pdf.GeneratePdf(PageSizes.A4, "c:\\temp\\helloWHAT.pdf");

        }
    }
}
