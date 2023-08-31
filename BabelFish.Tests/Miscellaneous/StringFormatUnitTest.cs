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
        /// </summary>
        [TestMethod]
        public async Task StringFormattingFormatScoreTest() {
            Stopwatch stopwatch = new Stopwatch();

            var client = new DefinitionAPIClient(Constants.X_API_KEY) { IgnoreLocalCache = true };
            var setName = SetName.Parse("v1.0:orion:Standard Score Formats");

            var result = await client.GetScoreFormatCollectionDefinitionAsync(setName);
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode, $"Expecting and OK status code, instead received {result.StatusCode}.");

            var formatDefinition = result.Definition;
            Assert.IsNotNull(formatDefinition);

            var scoreEventObj = new Scopos.BabelFish.DataModel.OrionMatch.Score();
            scoreEventObj.X = 4; // prolly keep something in this, or asserts with x will fail rn.
            scoreEventObj.I = 97;
            scoreEventObj.S = 103.4f;
            scoreEventObj.V = 101.8f;
            scoreEventObj.D = 105.2f;
            scoreEventObj.A = 10.3f;
            scoreEventObj.N = 11;

            var formattedInterger = StringFormatting.FormatScore(formatDefinition, "Integer", "Events", scoreEventObj);
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
    }
}
