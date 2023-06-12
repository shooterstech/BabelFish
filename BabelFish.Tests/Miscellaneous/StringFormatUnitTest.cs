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

        [TestMethod]
        public void DumbTest() {
            Assert.AreEqual(1, 1);
        }

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

            var scoreObj = new Scopos.BabelFish.DataModel.OrionMatch.Score();
            scoreObj.X = 4; // prolly keep something in this, or asserts with x will fail rn.
            scoreObj.I = 97;
            scoreObj.S = 42;
            scoreObj.V = 101.8f;
            scoreObj.D = 105.2f;
            scoreObj.A = 10.3f;
            scoreObj.N = 11;

            var integerEvent = StringFormatting.FormatScore(formatDefinition, "Integer", "Events", scoreObj);
            stopwatch.Start();
            var stdStringStart = stopwatch.Elapsed;
            Console.WriteLine("StandardString START:\t" + stdStringStart);
            for (int i = 0; i < 10000; i++) {
                integerEvent = StringFormatting.FormatScore(formatDefinition, "Integer", "Events", scoreObj);
            }
            //Assert.AreEqual(scoreObj.I.ToString() + " - " + scoreObj.X.ToString(), integerEvent);
            var stdStringStop = stopwatch.Elapsed;
            Console.WriteLine("StandardString STOP:\t" + stdStringStop);
            Console.WriteLine("AVG TIME/call:\t\t\t" + ((stdStringStop-stdStringStart)/10000) );
            Console.WriteLine(integerEvent);

            var integerAccumulated = StringFormatting.FormatScore(formatDefinition, "Integer", "AccumulatedFinals", scoreObj);
            Assert.AreEqual(scoreObj.S.ToString(), integerAccumulated);
            var integerShots = StringFormatting.FormatScore(formatDefinition, "Integer", "Shots", scoreObj);
            var asterisk = scoreObj.X > 0 ? "*" : "";
            Assert.AreEqual( scoreObj.D.ToString() + asterisk, integerShots);
        }
    }
}
