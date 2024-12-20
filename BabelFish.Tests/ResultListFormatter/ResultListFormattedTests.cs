using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scopos.BabelFish.APIClients;
using Scopos.BabelFish.DataModel.Definitions;
using Scopos.BabelFish.DataModel.OrionMatch;
using Scopos.BabelFish.Helpers;
using System.Threading.Tasks;
using Scopos.BabelFish.DataActors.ResultListFormatter;
using Scopos.BabelFish.DataActors.ResultListFormatter.UserProfile;

namespace Scopos.BabelFish.Tests.ResultListFormatter {

    [TestClass]
    public class ResultListFormattedTests {

        private OrionMatchAPIClient matchClient;
        private DefinitionAPIClient definitionClient;
        private IUserProfileLookup userProfileLookup;

        [TestInitialize]
        public async Task InitializeTest() {
            Scopos.BabelFish.Runtime.Settings.XApiKey = "wjM7eCb75aa3Okxj4FliXLY0VjHidoE2ei18pdg1"; // Constants.X_API_KEY;

            matchClient = new OrionMatchAPIClient( );
            definitionClient = new DefinitionAPIClient( );

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
            await rlf.InitializeAsync(  );

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

            MatchID matchId = new MatchID( "1.1.2023062817085368.0" ); // "1.1.2023052011495618.0";

            var matchDetailResponse = await matchClient.GetMatchPublicAsync( matchId );
            var match = matchDetailResponse.Match;
            var resultListName = "Individual - All"; // "Individual - All";

            //Get the Result List from the API Server
            var resultListResponse = await matchClient.GetResultListPublicAsync( matchId, resultListName );
            var resultList = resultListResponse.ResultList;
            var resultEventName = resultList.EventName;

            //Get the definition file that will tell us how to display the results.
            var resultListFormatSetName = await ResultListFormatFactory.FACTORY.GetResultListFormatSetNameAsync(resultList );
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
            ResultListIntermediateFormatted rlf = new ResultListIntermediateFormatted(resultList, resultListFormat, userProfileLookup );
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
        public async Task EriksPlayground() {

            MatchID matchId = new MatchID( "1.2457.2024103114463283.0" );
            var matchDetailResponse = await matchClient.GetMatchPublicAsync( matchId );
            var match = matchDetailResponse.Match;
            var resultListName = "Individual - Sporter";

            //Get the Result List from the API Server
            var resultListResponse = await matchClient.GetResultListPublicAsync( matchId, resultListName );
            var resultList = resultListResponse.ResultList;
            var resultEventName = resultList.EventName;

            //Get the ResultListFormat to use for formatting
            //var resultListFormatSetName = await ResultListFormatFactory.FACTORY.GetResultListFormatSetNameAsync( resultList );
            var resultListFormatSetName = SetName.Parse( "v1.0:test:3P Qualification" );
            var resultListFormatResponse = await definitionClient.GetResultListFormatDefinitionAsync( resultListFormatSetName );
            var resultListFormat = resultListFormatResponse.Definition;
            Assert.IsNotNull( resultListFormat );

            //Test that the conversion was successful and has the same number of objects.
            ResultListIntermediateFormatted rlf = new ResultListIntermediateFormatted( resultList, resultListFormat, userProfileLookup );
            await rlf.InitializeAsync();
            Assert.IsNotNull( rlf );

            rlf.Engagable = true;
            rlf.ResolutionWidth = 1800;
            rlf.ChildrenToShow = 4000;

            CellValues tryCellValues, cellValues;
            foreach (int i in rlf.GetShownColumnIndexes()) {
                Console.Write( $"{i}, " );
            }
            Console.WriteLine();

            foreach (var row in rlf.ShownRows) {
                foreach (int i in rlf.GetShownColumnIndexes()) {
                    var cell = row.GetColumnBodyCell( i );

                    Console.Write( $"{cell.Text}, " );
                }
                Console.WriteLine();
            }
        }
    }
}