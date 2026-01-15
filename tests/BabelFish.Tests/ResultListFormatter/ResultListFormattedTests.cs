using System.Diagnostics;
using System.Threading.Tasks;
using Scopos.BabelFish.APIClients;
using Scopos.BabelFish.DataActors.ResultListFormatter;
using Scopos.BabelFish.DataActors.ResultListFormatter.UserProfile;
using Scopos.BabelFish.DataModel.Definitions;
using Scopos.BabelFish.DataModel.OrionMatch;

namespace Scopos.BabelFish.Tests.ResultListFormatter {

    [TestClass]
    public class ResultListFormattedTests : BaseTestClass {

        private OrionMatchAPIClient matchClient;
        private DefinitionAPIClient definitionClient;
        private IUserProfileLookup userProfileLookup;

        [TestInitialize]
        public override void InitializeTest() {
            base.InitializeTest();

            matchClient = new OrionMatchAPIClient();
            definitionClient = new DefinitionAPIClient();
            DefinitionAPIClient.LocalStoreDirectory = new System.IO.DirectoryInfo( @"C:\temp" );

            userProfileLookup = new BaseUserProfileLookup();
        }

        [TestMethod]
        public async Task TestIndividualResultList() {

            MatchID matchId = new MatchID( "1.3448.2022013116325242.0" );
            var matchDetailResponse = await matchClient.GetMatchPublicAsync( matchId );
            var match = matchDetailResponse.Match;
            var resultListName = "Individual - Sporter";

            //Get the Result List from the API Server
            var resultListResponse = await matchClient.GetResultListPublicAsync( matchId, resultListName );
            var resultList = resultListResponse.ResultList;
            var resultEventName = resultList.EventName;

            //Get the ResultListFormat to use for formatting
            var resultListFormatSetName = await ResultListFormatFactory.FACTORY.GetResultListFormatSetNameAsync( resultList );
            var resultListFormatResponse = await definitionClient.GetResultListFormatDefinitionAsync( resultListFormatSetName );
            var resultListFormat = resultListFormatResponse.Definition;
            Assert.IsNotNull( resultListFormat );

            //Test that the conversion was successful and has the same number of objects.
            ResultListIntermediateFormatted rlf = new ResultListIntermediateFormatted( resultList, resultListFormat, userProfileLookup );
            await rlf.InitializeAsync();
            Assert.IsNotNull( rlf );
            Assert.AreEqual( resultList.Items.Count, rlf.ShownRows.Count );

            //Use the first item in the resultEventIntermediateList for closer inspection
            var rei = rlf.ShownRows[0];

            List<string> fieldNameList = rlf.FieldNames;

            //Check that we can get a value back for each of the Fields
            //And that TryGetFieldValue returns the same value as GetFieldValue
            //And that the list of fileds in the resultEventIntermediate is the same as the list of fields in resultListFormat
            string tryFieldValue, fieldValue;
            foreach (var field in resultListFormat.Fields) {
                string fieldName = (string)field.FieldName;

                bool hasValue = rei.TryGetFieldValue( fieldName, out tryFieldValue );
                Assert.IsTrue( hasValue );

                if (hasValue) {
                    fieldValue = rei.GetFieldValue( fieldName );
                    Assert.AreEqual( fieldValue, tryFieldValue );
                }

                Assert.IsTrue( fieldNameList.Contains( fieldName ) );
            }

            CellValues tryCellValues, cellValues;
            for (int i = 0; i < rlf.GetColumnCount(); i++) {
                bool hasValue = rei.TryGetColumnBodyValue( i, out tryCellValues );
                Assert.IsTrue( hasValue );

                if (hasValue) {
                    cellValues = rei.GetColumnBodyCell( i );
                    Assert.AreEqual( tryCellValues.Text, cellValues.Text );
                }
            }
        }

        [TestMethod]
        public async Task Test3PIndividualResultListCells() {

            MatchID matchId = new MatchID( "1.3448.2022013116325242.0" );
            var matchDetailResponse = await matchClient.GetMatchPublicAsync( matchId );
            var match = matchDetailResponse.Match;
            var resultListName = "Individual - Sporter";

            //Get the Result List from the API Server
            var resultListResponse = await matchClient.GetResultListPublicAsync( matchId, resultListName );
            var resultList = resultListResponse.ResultList;
            var resultEventName = resultList.EventName;

            //Get the definition file that will tell us how to display the results.
            var resultListFormatSetName = await ResultListFormatFactory.FACTORY.GetResultListFormatSetNameAsync( resultList );
            var resultListFormatResponse = await definitionClient.GetResultListFormatDefinitionAsync( resultListFormatSetName );
            var resultListFormat = resultListFormatResponse.Definition;

            //Convert the result list into the result event intermediate list that we can use
            ResultListIntermediateFormatted rlf = new ResultListIntermediateFormatted( resultList, resultListFormat, userProfileLookup );
            await rlf.InitializeAsync();

            //Test the header row
            for (int i = 0; i < rlf.GetColumnCount(); i++) {
                var headerCell = rlf.GetColumnHeaderCell( i );
                Assert.IsNotNull( string.IsNullOrEmpty( headerCell.Text ) ); //A header cell can be an empty string, but can not be null
                Debug.Write( headerCell.Text );
                Debug.Write( " " );
            }

            Debug.WriteLine( "" );

            //For each participant
            foreach (var rei in rlf.ShownRows) {

                //For each column to display
                //NOTE: You can also get the column count via the resultListFormat, using: resultListFormat.Format.Columns.Count
                for (int i = 0; i < rlf.GetColumnCount(); i++) {
                    //CellValues returns both the value to display, and the list of classes
                    var cell = rei.GetColumnBodyCell( i );
                    Assert.IsNotNull( cell.Text );
                    Debug.Write( cell.Text );
                    Debug.Write( " " );

                    if (cell.LinkTo != LinkToOption.None)
                        Assert.AreNotEqual( "", cell.LinkToData );
                }

                Debug.WriteLine( "" );
            }

            //Test the footer row
            for (int i = 0; i < rlf.GetColumnCount(); i++) {
                var footerCell = rlf.GetColumnFooterCell( i );
                Assert.IsNotNull( footerCell.Text );
                Debug.Write( footerCell.Text );
                Debug.Write( " " );
            }
        }

        [TestMethod]
        public async Task TestAPIndividualResultListCells() {

            MatchID matchId = new MatchID( "1.2829.2023050507494348.0" );
            var matchDetailResponse = await matchClient.GetMatchPublicAsync( matchId );
            var match = matchDetailResponse.Match;
            var resultListName = "Team - All";

            //Get the Result List from the API Server
            var resultListResponse = await matchClient.GetResultListPublicAsync( matchId, resultListName );
            var resultList = resultListResponse.ResultList;
            var resultEventName = resultList.EventName;

            //Get the definition file that will tell us how to display the results.
            var resultListFormatSetName = await ResultListFormatFactory.FACTORY.GetResultListFormatSetNameAsync( resultList );
            definitionClient.IgnoreInMemoryCache = true;
            var resultListFormatResponse = await definitionClient.GetResultListFormatDefinitionAsync( resultListFormatSetName );
            var resultListFormat = resultListFormatResponse.Definition;

            //Convert the result list into the result event intermediate list that we can use
            ResultListIntermediateFormatted rlf = new ResultListIntermediateFormatted( resultList, resultListFormat, userProfileLookup );
            await rlf.InitializeAsync();

            //Test the header row
            for (int i = 0; i < rlf.GetColumnCount(); i++) {
                var headerCell = rlf.GetColumnHeaderCell( i );
                Assert.IsNotNull( headerCell.Text ); //A header cell can be an empty string, but can not be null
                Debug.Write( headerCell.Text );
                Debug.Write( " " );
            }

            Debug.WriteLine( "" );

            //For each participant
            foreach (var rei in rlf.ShownRows) {

                //For each column to display
                //NOTE: You can also get the column count via the resultListFormat, using: resultListFormat.Format.Columns.Count
                for (int i = 0; i < rlf.GetColumnCount(); i++) {
                    //CellValues returns both the value to display, and the list of classes
                    var cell = rei.GetColumnBodyCell( i );
                    Assert.IsNotNull( cell.Text );
                    Debug.Write( cell.Text );
                    Debug.Write( " " );

                    if (cell.LinkTo != LinkToOption.None)
                        Assert.AreNotEqual( "", cell.LinkToData );
                }

                Debug.WriteLine( "" );
            }

            //Test the footer row
            for (int i = 0; i < rlf.GetColumnCount(); i++) {
                var footerCell = rlf.GetColumnFooterCell( i );
                Assert.IsNotNull( footerCell.Text );
                Debug.Write( footerCell.Text );
                Debug.Write( " " );
            }
        }

        [TestMethod]
        public async Task TestBBGunIndividualResultListCells() {

            MatchID matchId = new MatchID( "1.15.2025030713204931.0" ); // "1.1.2023052011495618.0";

            var matchDetailResponse = await matchClient.GetMatchPublicAsync( matchId );
            var match = matchDetailResponse.Match;
            var resultListName = "Individual - All"; // "Individual - All";

            //Get the Result List from the API Server
            var resultListResponse = await matchClient.GetResultListPublicAsync( matchId, resultListName );
            var resultList = resultListResponse.ResultList;
            var resultEventName = resultList.EventName;

            //Get the definition file that will tell us how to display the results.
            var resultListFormatSetName = await ResultListFormatFactory.FACTORY.GetResultListFormatSetNameAsync( resultList );
            var resultListFormatResponse = await definitionClient.GetResultListFormatDefinitionAsync( resultListFormatSetName );
            var resultListFormat = resultListFormatResponse.Definition;

            //Convert the result list into the result event intermediate list that we can use
            ResultListIntermediateFormatted rlf = new ResultListIntermediateFormatted( resultList, resultListFormat, userProfileLookup );
            await rlf.InitializeAsync();

            //Test the header row
            for (int i = 0; i < rlf.GetColumnCount(); i++) {
                var headerCell = rlf.GetColumnHeaderCell( i );
                Assert.IsNotNull( headerCell.Text );
                Debug.Write( headerCell.Text );
                Debug.Write( " " );
            }

            Debug.WriteLine( "" );

            //For each participant
            foreach (var rei in rlf.ShownRows) {

                //For each column to display
                //NOTE: You can also get the column count via the resultListFormat, using: resultListFormat.Format.Columns.Count
                for (int i = 0; i < rlf.GetColumnCount(); i++) {
                    //CellValues returns both the value to display, and the list of classes
                    var cell = rei.GetColumnBodyCell( i );
                    Assert.IsNotNull( cell.Text );
                    Debug.Write( cell.Text );
                    Debug.Write( " " );

                    if (cell.LinkTo != LinkToOption.None)
                        Assert.AreNotEqual( "", cell.LinkToData );
                }

                Debug.WriteLine( "" );
            }

            //Test the footer row
            for (int i = 0; i < rlf.GetColumnCount(); i++) {
                var footerCell = rlf.GetColumnFooterCell( i );
                Assert.IsNotNull( footerCell.Text );
                Debug.Write( footerCell.Text );
                Debug.Write( " " );
            }
        }

        /// <summary>
        /// This is really just an example method to show how to use the ResultListFormatted class.
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task TestIndividualResultListAsHTML() {

            MatchID matchId = new MatchID( "1.15.2025030713204931.0" );
            var matchDetailResponse = await matchClient.GetMatchPublicAsync( matchId );
            var match = matchDetailResponse.Match;
            var resultListName = "Individual - All";

            //Get the Result List from the API Server
            var resultListResponse = await matchClient.GetResultListPublicAsync( matchId, resultListName );
            var resultList = resultListResponse.ResultList;
            var resultEventName = resultList.EventName;

            //Get the definition file that will tell us how to display the results.
            var resultListFormatSetName = await ResultListFormatFactory.FACTORY.GetResultListFormatSetNameAsync( resultList );
            var resultListFormatResponse = await definitionClient.GetResultListFormatDefinitionAsync( resultListFormatSetName );
            var resultListFormat = resultListFormatResponse.Definition;

            //Convert the result list into the result event intermediate list that we can use
            ResultListIntermediateFormatted rlf = new ResultListIntermediateFormatted( resultList, resultListFormat, userProfileLookup );
            await rlf.InitializeAsync();

            //Start the header row
            Debug.WriteLine( $"<div class=\"{string.Join( ',', rlf.GetHeaderRowClassList() )}\">" );

            //Write the header row
            for (int i = 0; i < rlf.GetColumnCount(); i++) {
                var headerCell = rlf.GetColumnHeaderCell( i );
                Debug.Write( $"<div class=\"{string.Join( ',', headerCell.ClassList )}\">" );
                Debug.Write( headerCell.Text );
                Debug.Write( $"</div>" );
            }

            //End the header row
            Debug.WriteLine( "" );
            Debug.WriteLine( $"</div>" );


            //For each row, each row represents on Competitor and their scores
            foreach (var rei in rlf.ShownRows) {

                //Start the Competitor row
                Debug.WriteLine( $"<div class=\"{string.Join( ',', rei.GetClassList() )}\">" );

                //Write the Competitor row
                for (int i = 0; i < rlf.GetColumnCount(); i++) {
                    Debug.Write( "  " );
                    var bodyCell = rei.GetColumnBodyCell( i );
                    Debug.Write( $"<div class=\"{string.Join( ',', bodyCell.ClassList )}\">" );
                    if (bodyCell.LinkTo != LinkToOption.None) {
                        Debug.Write( $"<a href=\"#{bodyCell.LinkTo.Description()}={bodyCell.LinkToData}\">" );
                    }
                    Debug.Write( bodyCell.Text );
                    if (bodyCell.LinkTo != LinkToOption.None) {
                        Debug.Write( $"</a>" );
                    }

                    Debug.Write( $"</div>" );
                }

                //End the Competitor row
                Debug.WriteLine( "" );
                Debug.WriteLine( $"</div>" );
            }

            //Start the footer row
            Debug.WriteLine( $"<div class=\"{string.Join( ',', rlf.GetFooterRowClassList() )}\">" );

            //Write the footer row
            for (int i = 0; i < rlf.GetColumnCount(); i++) {
                var headerCell = rlf.GetColumnFooterCell( i );
                Debug.Write( $"<div class=\"{string.Join( ',', headerCell.ClassList )}\">" );
                Debug.Write( headerCell.Text );
                Debug.Write( $"</div>" );
            }

            //End the footer row
            Debug.WriteLine( "" );
            Debug.WriteLine( $"</div>" );
        }

        [TestMethod]
        public async Task GetParticipantAttributeDelegateTest() {

            MatchID matchId = new MatchID( "1.2413.2025050117240235.0" );
            var matchDetailResponse = await matchClient.GetMatchPublicAsync( matchId );
            var match = matchDetailResponse.Match;
            var resultListName = "Individual - All";

            //Get the Result List from the API Server
            var resultListResponse = await matchClient.GetResultListPublicAsync( matchId, resultListName );
            var resultList = resultListResponse.ResultList;
            var resultEventName = resultList.EventName;

            //Get the definition file that will tell us how to display the results.
            var resultListFormatSetName = await ResultListFormatFactory.FACTORY.GetResultListFormatSetNameAsync( resultList );
            var resultListFormat = await DefinitionCache.GetResultListFormatDefinitionAsync( resultListFormatSetName );

            //Convert the result list into the result event intermediate list that we can use
            ResultListIntermediateFormatted rlf = new ResultListIntermediateFormatted( resultList, resultListFormat, userProfileLookup );
            await rlf.InitializeAsync();

            //First loop through, looking at rank which should be 1 .. 8
            for (int i = 0; i < 8; i++) {
                Console.WriteLine( rlf.Rows[i].GetFieldValue( "Rank" ) );
                Assert.AreEqual( (i + 1).ToString(), rlf.Rows[i].GetFieldValue( "Rank" ) );
            }

            //Point to a delegate that overrides getting rank. Then recalculate the value of the fields.
            rlf.GetParticipantAttributeRankPtr = GetParticipantAttributeRank;
            rlf.RefreshAllRowsParticipantAttributeFields();

            //First loop through, looking at rank which should be 1 .. 8
            for (int i = 0; i < 8; i++) {
                Console.WriteLine( rlf.Rows[i].GetFieldValue( "Rank" ) );
                Assert.AreEqual( (i + 1001).ToString(), rlf.Rows[i].GetFieldValue( "Rank" ) );
            }

        }

        public string GetParticipantAttributeRank( IRLIFItem item, ResultListIntermediateFormatted rlf ) {

            if (item is ResultEvent) {
                var resultEvent = (ResultEvent)item;
                var realRank = resultEvent.Rank;
                var modifiedRank = 1000 + realRank;

                return modifiedRank.ToString();
            }

            return "";
        }

        [TestMethod]
        public async Task TestShowRelay() {

            //MatchID matchId = new MatchID( "1.1.2025030313571346.1" );
            MatchID matchId = new MatchID( "1.1.2025081213222434.0" );
            var matchDetailResponse = await matchClient.GetMatchPublicAsync( matchId );
            var match = matchDetailResponse.Match;
            var squaddingName = match.SquaddingEvents[0].Name;

            //Get the Result List from the API Server
            var squaddingListResponse = await matchClient.GetSquaddingListPublicAsync( matchId, squaddingName );
            var squaddingList = squaddingListResponse.SquaddingList;

            //Get the ResultListFormat to use for formatting
            var resultListFormatSetName = await ResultListFormatFactory.FACTORY.GetResultListFormatSetNameAsync( squaddingList );
            var resultListFormatResponse = await definitionClient.GetResultListFormatDefinitionAsync( resultListFormatSetName );
            var resultListFormat = resultListFormatResponse.Definition;

            //Instantiate the RLIF
            ResultListIntermediateFormatted rlf = new ResultListIntermediateFormatted( squaddingList, resultListFormat, userProfileLookup );
            await rlf.InitializeAsync();

            //Seet .ShowRanks to 0, so it doesn't show the top three (like it would by default).
            rlf.ShowRanks = 0;

            //Before specifying a relay to show, make sure the default case (show everyone) works.
            Assert.AreEqual( 13, rlf.ShownRows.Count );

            rlf.ShowRelay = "1";
            Assert.AreEqual( 4, rlf.ShownRows.Count );

            rlf.ShowRelay = "2";
            Assert.AreEqual( 4, rlf.ShownRows.Count );

            rlf.ShowRelay = "3";
            Assert.AreEqual( 4, rlf.ShownRows.Count );

            rlf.ShowRelay = "4";
            Assert.AreEqual( 1, rlf.ShownRows.Count );
        }

        [TestMethod]
        public async Task EriksPlayground() {

            //MatchID matchId = new MatchID( "1.1.2025030313571346.1" );
            MatchID matchId = new MatchID( "1.1.2025122311175108.0" );
            var matchDetailResponse = await matchClient.GetMatchPublicAsync( matchId );
            var match = matchDetailResponse.Match;
            var resultListName = "Individual - All";

            //Get the Result List from the API Server
            var resultListResponse = await matchClient.GetResultListPublicAsync( matchId, resultListName );
            var resultList = resultListResponse.ResultList;
            var resultEventName = resultList.EventName;

            resultList.UserDefinedText[UserDefinedFieldNames.USER_DEFINED_FIELD_1] = "Bib: {CompetitorNumber}, Coach {Coach}";

            //Get the ResultListFormat to use for formatting
            var resultListFormatSetName = await ResultListFormatFactory.FACTORY.GetResultListFormatSetNameAsync( resultList );
            //var resultListFormatSetName = SetName.Parse( "v1.0:test:3P Qualification" );
            var resultListFormatResponse = await definitionClient.GetResultListFormatDefinitionAsync( resultListFormatSetName );
            var resultListFormat = resultListFormatResponse.Definition;
            Assert.IsNotNull( resultListFormat );

            //Test that the conversion was successful and has the same number of objects.
            ResultListIntermediateFormatted rlf = new ResultListIntermediateFormatted( resultList, resultListFormat, userProfileLookup );
            await rlf.InitializeAsync( false );
            Assert.IsNotNull( rlf );


            //await rlf.LoadSquaddingListAsync();

            rlf.Engagable = false;
            rlf.ResolutionWidth = 10000;
            rlf.ShowNumberOfChildRows = 5;
            rlf.ShowRanks = 3;
            rlf.ShowStatuses = null;
            rlf.ShowSupplementalInformation = false;
            rlf.ShowNumberOfBodyRows = int.MaxValue;
            rlf.ShowSpanningRows = true;
            rlf.RefreshAllRowsParticipantAttributeFields();

            //rlf.SetShowValuesToDefault();


            CellValues tryCellValues, cellValues;
            foreach (var cv in rlf.GetShownHeaderRow()) {
                Console.Write( $"{cv.Text}, " );
            }
            Console.WriteLine();

            foreach (var row in rlf.ShownRows) {
                foreach (var multiLineRow in row) {
                    foreach (var cv in multiLineRow.GetShownRow()) {
                        Console.Write( $"{cv.Text}, " );
                    }
                    //Console.Write( " : " );
                    //Console.Write( multiLineRow.GetParticipant().RemarkList.ToString() );
                    //Console.Write( " : " );
                    //Console.Write( string.Join( ", ", multiLineRow.GetClassList() ) );
                    Console.WriteLine();
                }
            }
        }
    }
}
