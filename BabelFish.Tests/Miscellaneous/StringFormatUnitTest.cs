using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scopos.BabelFish.Runtime;
using Scopos.BabelFish.APIClients;
using Scopos.BabelFish.DataModel.Definitions;
using System.Net;
using Scopos.BabelFish.Helpers;
using System.Diagnostics;

namespace Scopos.BabelFish.Tests.Miscellaneous {

    [TestClass]
    public class StringFormatUnitTest {

        /// <summary>
        /// Tests Format Score of String Formatting works as expected.
        /// Using the happy path, with expected values
        /// </summary>
        [TestMethod]
        public async Task StringFormattingStandardScoreFormats() {

            var client = new DefinitionAPIClient( Constants.X_API_KEY ) { IgnoreInMemoryCache = true };
            var setName = SetName.Parse( "v1.0:orion:Standard Score Formats" );

            var result = await client.GetScoreFormatCollectionDefinitionAsync( setName );
            Assert.AreEqual( HttpStatusCode.OK, result.StatusCode, $"Expecting and OK status code, instead received {result.StatusCode}." );

            var formatDefinition = result.Definition;
            Assert.IsNotNull( formatDefinition );

            var scoreEventObj = new Scopos.BabelFish.DataModel.Athena.Score();
            scoreEventObj.X = 4; // prolly keep something in this, or asserts with x will fail rn.
            scoreEventObj.I = 97;
            scoreEventObj.S = 103.4f;
            scoreEventObj.D = 105.2f;

            var formattedInterger = StringFormatting.FormatScore( formatDefinition, "Integer", "Events", scoreEventObj );
            Assert.AreEqual( "97 - 4", formattedInterger );

            var formattedDecimal = StringFormatting.FormatScore( formatDefinition, "Decimal", "Events", scoreEventObj );
            Assert.AreEqual( "105.2", formattedDecimal );

            var formattedAccumulativeFinals = StringFormatting.FormatScore( formatDefinition, "Integer", "AccumulatedFinals", scoreEventObj );
            Assert.AreEqual( "103.4", formattedAccumulativeFinals );

            var scoreShotObj = new Scopos.BabelFish.DataModel.OrionMatch.Score() {
                X = 1,
                I = 10,
                D = 10.4f
            };

            var formattedShotInteger = StringFormatting.FormatScore( formatDefinition, "Integer", "Shots", scoreShotObj );
            Assert.AreEqual( "10.4*", formattedShotInteger );

            var formattedShotIntegerConv = StringFormatting.FormatScore( formatDefinition, "Conventional", "Shots", scoreShotObj );
            Assert.AreEqual( "10*", formattedShotIntegerConv );

            var formattedShotDecimal = StringFormatting.FormatScore( formatDefinition, "Decimal", "Shots", scoreShotObj );
            Assert.AreEqual( "10.4", formattedShotDecimal );
        }

        /// <summary>
        /// Tests Format Score of String Formatting works as expected.
        /// Using the sad path, with unexpected values
        /// </summary>
        [TestMethod]
        public async Task StringFormattingStandardScoreFormatsUnexpectedValues() {

            var client = new DefinitionAPIClient( Constants.X_API_KEY ) { IgnoreInMemoryCache = true };
            var setName = SetName.Parse( "v1.0:orion:Standard Score Formats" );

            var result = await client.GetScoreFormatCollectionDefinitionAsync( setName );
            Assert.AreEqual( HttpStatusCode.OK, result.StatusCode, $"Expecting and OK status code, instead received {result.StatusCode}." );

            var formatDefinition = result.Definition;
            Assert.IsNotNull( formatDefinition );

            var scoreEventObj = new Scopos.BabelFish.DataModel.Athena.Score();
            scoreEventObj.X = 4; // prolly keep something in this, or asserts with x will fail rn.
            scoreEventObj.I = 97;
            scoreEventObj.S = 103.4f;
            scoreEventObj.D = 105.2f;

            //Test using invallid values for ScoreConfigName. Should default to using the first ScoreConfig defined, which is Integer
            var formattedScore = StringFormatting.FormatScore( formatDefinition, "NotAScoreConfig", "Events", scoreEventObj );
            Assert.AreEqual( "97 - 4", formattedScore );

            formattedScore = StringFormatting.FormatScore( formatDefinition, "", "Events", scoreEventObj );
            Assert.AreEqual( "97 - 4", formattedScore );

            formattedScore = StringFormatting.FormatScore( formatDefinition, null, "Events", scoreEventObj );
            Assert.AreEqual( "97 - 4", formattedScore );

            //Test using invalid values for ScoreFormatName. Shold default to using the first ScoreFormat defined, which is Events
            formattedScore = StringFormatting.FormatScore( formatDefinition, "Decimal", "NotAScoreFormat", scoreEventObj );
            Assert.AreEqual( "105.2", formattedScore );

            formattedScore = StringFormatting.FormatScore( formatDefinition, "Decimal", "", scoreEventObj );
            Assert.AreEqual( "105.2", formattedScore );

            formattedScore = StringFormatting.FormatScore( formatDefinition, "Decimal", null, scoreEventObj );
            Assert.AreEqual( "105.2", formattedScore );

            //Test using invalid value for formatDefinition. Should default to using a format string that is just the decimal score
            formattedScore = StringFormatting.FormatScore( null, "Integer", "Conventional", scoreEventObj );
            Assert.AreEqual( "105.2", formattedScore );

            //Test using invalid value for score. Should return a simple stirng of "null"
            scoreEventObj = null;
            formattedScore = StringFormatting.FormatScore( formatDefinition, "Integer", "Conventional", scoreEventObj );
            Assert.AreEqual( "null", formattedScore );
        }

        [TestMethod]
        public async Task StringFormattingWithFormatString() {


            var scoreEventObj = new Scopos.BabelFish.DataModel.Athena.Score();
            scoreEventObj.X = 4; // prolly keep something in this, or asserts with x will fail rn.
            scoreEventObj.I = 97;
            scoreEventObj.S = 103.4f;
            scoreEventObj.D = 105.2f;
            scoreEventObj.J = 12.34f;
            scoreEventObj.K = 23.45f;
            scoreEventObj.L = 34.56f;

            //Test all of the different correct formats
            var formattedScore = StringFormatting.FormatScore( "{i}", scoreEventObj );
            Assert.AreEqual( "97", formattedScore );

            formattedScore = StringFormatting.FormatScore( "{d}", scoreEventObj );
            Assert.AreEqual( "105.2", formattedScore );

            formattedScore = StringFormatting.FormatScore( "{x}", scoreEventObj );
            Assert.AreEqual( "4", formattedScore );

            formattedScore = StringFormatting.FormatScore( "{X}", scoreEventObj );
            Assert.AreEqual( "*", formattedScore );

            formattedScore = StringFormatting.FormatScore( "{s}", scoreEventObj );
            Assert.AreEqual( "103.4", formattedScore );

            formattedScore = StringFormatting.FormatScore( "{m}", scoreEventObj );
            Assert.AreEqual( "", formattedScore );

            formattedScore = StringFormatting.FormatScore( "{j}", scoreEventObj );
            Assert.AreEqual( "12.3", formattedScore );

            formattedScore = StringFormatting.FormatScore( "{J}", scoreEventObj );
            Assert.AreEqual( "12", formattedScore );

            formattedScore = StringFormatting.FormatScore( "{k}", scoreEventObj );
            Assert.AreEqual( "23.5", formattedScore );

            formattedScore = StringFormatting.FormatScore( "{K}", scoreEventObj );
            Assert.AreEqual( "23", formattedScore );

            formattedScore = StringFormatting.FormatScore( "{l}", scoreEventObj );
            Assert.AreEqual( "34.6", formattedScore );

            formattedScore = StringFormatting.FormatScore( "{L}", scoreEventObj );
            Assert.AreEqual( "34", formattedScore );

            //Test a format that is not known

            formattedScore = StringFormatting.FormatScore( "{Z}", scoreEventObj );
            Assert.AreEqual( "{Z}", formattedScore );

            //Test, everything else is carried forward
            formattedScore = StringFormatting.FormatScore( "ABCDEFGHIJKLMNOP", scoreEventObj );
            Assert.AreEqual( "ABCDEFGHIJKLMNOP", formattedScore );
        }
    }
}
