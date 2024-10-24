using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scopos.BabelFish;
using Scopos.BabelFish.Helpers;
using Scopos.BabelFish.DataModel.Definitions;
using Scopos.BabelFish.APIClients;
using Scopos.BabelFish.DataModel.OrionMatch;
using Scopos.BabelFish.DataActors.OrionMatch;
using System.Diagnostics;

namespace Scopos.BabelFish.Tests.Definition {

    [TestClass]
    internal class ICopyTests {

        [TestInitialize]
        public void InitializeTest() {
            Scopos.BabelFish.Runtime.Settings.XApiKey = Constants.X_API_KEY;
        }

        /* Belongs in ICopyTests */
        [TestMethod]
        public void CopyASingular() {
            Singular orig = new Singular();
            orig.Values = "1..60";
            orig.ScoreFormat = "Shots";
            orig.Type = SingularType.SHOT;
            orig.ShotMappingMethod = ShotMappingMethodType.SEQUENTIAL;
            orig.EventName = "P{}";
            orig.StageLabel = "P";
            orig.Comment = "This is a test";

            //Perform the copy
            Stopwatch swCopy = Stopwatch.StartNew();
            Singular copy = orig.Copy();
            swCopy.Stop();

            Assert.AreEqual( orig.Values, copy.Values );
            Assert.AreEqual( orig.Values, copy.Values );
            Assert.AreEqual( orig.Type, copy.Type );
            Assert.AreEqual( orig.ShotMappingMethod, copy.ShotMappingMethod );
            Assert.AreEqual( orig.EventName, copy.EventName );
            Assert.AreEqual( orig.StageLabel, copy.StageLabel );
            Assert.AreEqual( orig.Comment, copy.Comment );

            orig.StageLabel = "S";
            Assert.AreNotEqual( orig.StageLabel, copy.StageLabel );

            Stopwatch swClone = Stopwatch.StartNew();
            var singularClone = orig.Clone();
            swClone.Stop();

            Assert.IsTrue( swClone.ElapsedTicks > swCopy.ElapsedTicks );
        }

        [TestMethod]
        public async Task CopyAEvent() {

            var client = new DefinitionAPIClient();
            var setName = SetName.Parse( "v2.0:orion:Informal Practice Air Rifle" );
            var cof = (await client.GetCourseOfFireDefinitionAsync( setName )).Value;

            foreach (var @event in cof.Events) {
                var copyOfEvent = @event.Copy();
                Assert.AreEqual( @event.EventName, copyOfEvent.EventName );
                Assert.AreEqual( @event.EventType, copyOfEvent.EventType );

                var copyEventNames = copyOfEvent.GetChildrenEventNames();
                var origEventNames = @event.GetChildrenEventNames();

                for (var i = 0; i < origEventNames.Count; i++) {
                    Assert.AreEqual( origEventNames[i], copyEventNames[i] );
                }
            }
        }

        [TestMethod]
        public async Task CopyATieBreakingRule() {


            var client = new DefinitionAPIClient();
            var setName = SetName.Parse( "v2.0:orion:Generic Qualification" );
            var rankingRule = (await client.GetRankingRuleDefinitionAsync( setName )).Value;

            foreach (var rr in rankingRule.RankingRules) {
                foreach (var tbr in rr.Rules) {
                    var copy = tbr.Copy();
                    Assert.AreEqual( tbr.EventName, copy.EventName );
                    Assert.AreEqual( tbr.Values, copy.Values );
                    Assert.AreEqual( tbr.Source, copy.Source );
                    Assert.AreEqual( tbr.ResultStatus, copy.ResultStatus );
                    Assert.AreEqual( tbr.Method, copy.Method );
                    Assert.AreEqual( tbr.SortOrder, copy.SortOrder );
                }
            }
        }
    }
}
