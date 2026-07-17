using System.Threading.Tasks;
using QuestPDF.Helpers;
using Scopos.BabelFish.APIClients;
using Scopos.BabelFish.DataActors.PDF;
using Scopos.BabelFish.DataModel.OrionMatch;
using Scopos.BabelFish.Requests.OrionMatchAPI;

namespace Scopos.BabelFish.Tests.DataActors.PDF {
    [TestClass]
    public class ResultListPdfTests : BaseTestClass {

        private readonly string _filePath = "c:\\temp\\hello.pdf";

        [TestMethod]
        public async Task GenerateResultListPDFTest() {

            // Delete the output file if it already exists, so the test will fail if the PDF generation doesn't create a new file.
            if (System.IO.File.Exists( _filePath ))
                System.IO.File.Delete( _filePath );

            var client = new OrionMatchAPIClient();

            //This match id has three relays of 20 athletes
            var matchId = new MatchID( "1.4990.2025122906283918.0" );
            var resultListName = "Team - Sporter";

            var getResultListResponse = await client.GetResultListPublicAsync( matchId, resultListName );
            var resultList = getResultListResponse.ResultList;

            while (getResultListResponse.HasMoreItems) {
                var nextRequest = (GetResultListPublicRequest)getResultListResponse.GetNextRequest();
                getResultListResponse = await client.GetResultListPublicAsync( nextRequest );
                resultList.Items.AddRange( getResultListResponse.ResultList.Items );
            }

            var match = (await client.GetMatchAsync( matchId )).Match;

            var pdf = new ResultListPdf( match, resultList );
            await pdf.InitializeAsync();
            await pdf.RLIF.LoadSquaddingListAsync();
            //pdf.RLIF.ShowRelay = "2";
            //pdf.RLIF.ShowRanks = 3;

            pdf.GeneratePdf( PageSizes.Letter, _filePath );

            Assert.IsTrue( System.IO.File.Exists( _filePath ) );

        }

        [TestMethod]
        public async Task GenerateSquaddingtListPDFTest() {

            // Delete the output file if it already exists, so the test will fail if the PDF generation doesn't create a new file.
            if (System.IO.File.Exists( _filePath ))
                System.IO.File.Delete( _filePath );

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

            pdf.GeneratePdf( PageSizes.Letter, _filePath );

            Assert.IsTrue( System.IO.File.Exists( _filePath ) );
        }

        [TestMethod]
        public async Task GenerateResultCOFPDFTest() {

            // Delete the output file if it already exists, so the test will fail if the PDF generation doesn't create a new file.
            if (System.IO.File.Exists( _filePath ))
                System.IO.File.Delete( _filePath );

            var client = new OrionMatchAPIClient();

            var resultCofId = "f5854dc0-8c3c-446d-b613-459b1e02992d";

            var getResultCofResponse = await client.GetResultCourseOfFireDetailPublicAsync( resultCofId );
            var resultCof = getResultCofResponse.ResultCOF;

            var pdf = new ResultCOFPdf( resultCof, Scopos.BabelFish.DataModel.Definitions.EventtType.STAGE );
            await pdf.InitializeAsync();

            pdf.GeneratePdf( PageSizes.Letter, _filePath );

            Assert.IsTrue( System.IO.File.Exists( _filePath ) );

        }

        [TestMethod]
        public async Task GenerateMergedAthleteCOFPDFTest() {

            // Delete the output file if it already exists, so the test will fail if the PDF generation doesn't create a new file.
            if (System.IO.File.Exists( _filePath ))
                System.IO.File.Delete( _filePath );

            var client = new OrionMatchAPIClient();

            //This match id has three relays of 20 athletes
            var matchId = new MatchID( "1.1.2025072316000865.0" );
            var resultListName = "Individual - All";

            var getResultListResponse = await client.GetResultListPublicAsync( matchId, resultListName );
            var resultList = getResultListResponse.ResultList;

            List<ResultCOF> documentsToPrint = new List<ResultCOF>();
            foreach (var resultEvent in resultList.Items) {
                var resultCofId = resultEvent.ResultCOFID;
                var getResultCof = await client.GetResultCourseOfFireDetailPublicAsync( resultCofId );
                var resultCof = getResultCof.ResultCOF;
                documentsToPrint.Add( resultCof );
            }

            await AthleteCOFPdf.GeneratePdfs( documentsToPrint, Scopos.BabelFish.DataModel.Definitions.EventtType.SERIES, PageSizes.Letter, _filePath );

            Assert.IsTrue( System.IO.File.Exists( _filePath ) );
        }

        [TestMethod]
        public async Task GenerateAthleteCofPdfTest() {

            // Delete the output file if it already exists, so the test will fail if the PDF generation doesn't create a new file.
            if (System.IO.File.Exists( _filePath ))
                System.IO.File.Delete( _filePath );

            var client = new OrionMatchAPIClient();

            var resultCofId = "e7864548-e8c8-492b-8eb6-d6a81d49bde2"; // sim air rifle
            //var resultCofId = "5486e765-73db-4973-b97e-5c423e9395dc"; // air pistol
            //var resultCofId = "57b49cc3-db5d-4384-90eb-5be05d617664"; // Test scores
            //var resultCofId = "0c9a775a-6390-4cb4-91f3-9724b699b5a9"; // Hit Miss
            //var resultCofId = "9c47822f-c668-4b08-b7a1-ede5f9aae6c7"; // 3x20
            //var resultCofId = "7e9ab808-025e-42b6-886e-bf3f2423ae13"; // 3x10
            //var resultCofId = "5c36dd5a-1ffe-4b0d-8469-cb5dd75c9dd6"; // 3x40
            //var resultCofId = "ccc4d957-7df6-4666-9e59-25381beb6767"; //3x10 but incomplete
            //var resultCofId = "5490e48f-633b-4526-89d8-71fdbad15fda"; // 

            var getResultCofResponse = await client.GetResultCourseOfFireDetailPublicAsync( resultCofId );

            var resultCof = getResultCofResponse.ResultCOF;
            /*
            var pdfEvent = new AthleteCOFPdf(resultCof, Scopos.BabelFish.DataModel.Definitions.EventtType.EVENT);
            await pdfEvent.InitializeAsync();
            pdfEvent.GeneratePdf(PageSizes.Letter, "c:\\temp\\helloEVENT.pdf");

            var pdfStage = new AthleteCOFPdf(resultCof, Scopos.BabelFish.DataModel.Definitions.EventtType.STAGE);
            await pdfStage.InitializeAsync();
            pdfStage.GeneratePdf(PageSizes.Letter, "c:\\temp\\helloSTAGE.pdf");
            */
            var pdfSeries = new AthleteCOFPdf( resultCof, Scopos.BabelFish.DataModel.Definitions.EventtType.STAGE );
            //pdfSeries.DemographicText = "{CompetitorNumber} {Club} {Country}";
            await pdfSeries.InitializeAsync();
            pdfSeries.GeneratePdf( PageSizes.Letter, _filePath );

            Assert.IsTrue( System.IO.File.Exists( _filePath ) );

        }

        [TestMethod]
        public async Task TestClubQRCodePDF() {

            // Delete the output file if it already exists, so the test will fail if the PDF generation doesn't create a new file.
            if (System.IO.File.Exists( _filePath ))
                System.IO.File.Delete( _filePath );

            var client = new ClubsAPIClient();
            var getClubResonse = await client.GetClubDetailPublicAsync( "OrionAcct002022" );

            var clubDetail = getClubResonse.ClubDetail;

            var pdf = new ClubQRCodePDF( clubDetail );
            await pdf.InitializeAsync();
            pdf.GeneratePdf( PageSizes.Letter, _filePath );

            Assert.IsTrue( System.IO.File.Exists( _filePath ) );
        }

        [TestMethod]
        public async Task TestMatchQRCodePDF() {

            // Delete the output file if it already exists, so the test will fail if the PDF generation doesn't create a new file.
            if (System.IO.File.Exists( _filePath ))
                System.IO.File.Delete( _filePath );

            var client = new OrionMatchAPIClient();
            var matchId = new MatchID( "1.1.2025081316001310.1" );
            var getMatchResponse = await client.GetMatchPublicAsync( matchId );

            var matchDetail = getMatchResponse.Match;

            var pdf = new MatchQRCodePDF( matchDetail );
            await pdf.InitializeAsync();
            pdf.GeneratePdf( PageSizes.Letter, _filePath );

            Assert.IsTrue( System.IO.File.Exists( _filePath ) );
        }
    }
}
