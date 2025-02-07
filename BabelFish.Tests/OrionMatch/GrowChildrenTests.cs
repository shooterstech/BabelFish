﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scopos.BabelFish.DataModel.Definitions;
using Scopos.BabelFish.Tests.Definition;

namespace Scopos.BabelFish.Tests.OrionMatch {

    [TestClass]
    public class GrowChildrenTests : BaseTestClass {

        [TestMethod]
        public void GrowChildrenFromEvents() {

            //The Information COF has all three type of Event concrete classes. Explicit, Expand, and Derived
            var cof = CourseOfFireHelper.Get_Informal_Air_Rifle();

            var topLevelEvent = EventComposite.GrowEventTree(cof);

            Assert.AreEqual( "Top Level", topLevelEvent.EventName );
            Assert.AreEqual( EventtType.EVENT, topLevelEvent.EventType );
            Assert.AreEqual( "Qualification3P", topLevelEvent.EventStyleMapping.EventAppellation );

            var shouldBeKneeling = topLevelEvent.Children[0];
            Assert.AreEqual( "Kneeling", shouldBeKneeling.EventName );
            Assert.AreEqual( EventtType.STAGE, shouldBeKneeling.EventType );
            Assert.AreEqual( "Kneeling", shouldBeKneeling.StageStyleMapping.StageAppellation );

            var shouldBeProne = topLevelEvent.Children[1];
            Assert.AreEqual( "Prone", shouldBeProne.EventName );
            Assert.AreEqual( EventtType.STAGE, shouldBeProne.EventType );
            Assert.AreEqual( "Prone", shouldBeProne.StageStyleMapping.StageAppellation );

            var shouldBeStanding = topLevelEvent.Children[2];
            Assert.AreEqual( "Standing", shouldBeStanding.EventName );
            Assert.AreEqual( EventtType.STAGE, shouldBeStanding.EventType );
            Assert.AreEqual( "Standing", shouldBeStanding.StageStyleMapping.StageAppellation );

            //Test Random paths
            Assert.AreEqual( "PR 9", shouldBeProne.Children[8].EventName );
            Assert.AreEqual( EventtType.STRING, shouldBeProne.Children[8].EventType );
            Assert.AreEqual( "P12", shouldBeProne.Children[1].Children[1].EventName );
            Assert.AreEqual( EventtType.SINGULAR, shouldBeProne.Children[1].Children[1].EventType );

            Assert.AreEqual( "ST 19", shouldBeStanding.Children[18].EventName );
            Assert.AreEqual( EventtType.STRING, shouldBeStanding.Children[18].EventType );
            Assert.AreEqual( "S105", shouldBeStanding.Children[10].Children[4].EventName );
            Assert.AreEqual( EventtType.SINGULAR, shouldBeStanding.Children[10].Children[4].EventType );

            Assert.AreEqual( "KN 28", shouldBeKneeling.Children[27].EventName );
            Assert.AreEqual( EventtType.STRING, shouldBeKneeling.Children[27].EventType );
            Assert.AreEqual( "K245", shouldBeKneeling.Children[24].Children[4].EventName );
            Assert.AreEqual( EventtType.SINGULAR, shouldBeKneeling.Children[24].Children[4].EventType );

            var allStages = topLevelEvent.GetEvents( false, false, true, false, false, false );
            Assert.AreEqual( 3, allStages.Count );

            var allStrings = topLevelEvent.GetEvents( false, false, false, false, true, false );
            Assert.AreEqual( 150, allStrings.Count );

            var allSingulars = topLevelEvent.GetEvents( false, false, false, false, false, true );
            Assert.AreEqual( 1500, allSingulars.Count );

            //PR AVG, ST AVG, and KN AVG are external to the event tree, and should not be found.
            var prAvgComp = topLevelEvent.FindEventComposite( "PR AVG" );
            Assert.IsNull( prAvgComp );
            var stAvgComp = topLevelEvent.FindEventComposite( "ST AVG" );
            Assert.IsNull( stAvgComp );
            var knAvgComp = topLevelEvent.FindEventComposite( "KN AVG" );
            Assert.IsNull( knAvgComp );

            //Compile all of the events
            List<Event> allCompiledEvents = new List<Event>();
            foreach( var e in cof.Events ) {
                allCompiledEvents.AddRange( e.GetCompiledEvents() );
            }

            var prAvgEvent = allCompiledEvents.FirstOrDefault( obj => obj.EventName == "PR AVG" );
            Assert.IsTrue( prAvgEvent.ExternalToEventTree );
            var stAvgEvent = allCompiledEvents.FirstOrDefault( obj => obj.EventName == "ST AVG" );
            Assert.IsTrue( stAvgEvent.ExternalToEventTree );
            var knAvgEvent = allCompiledEvents.FirstOrDefault( obj => obj.EventName == "KN AVG" );
            Assert.IsTrue( knAvgEvent.ExternalToEventTree );

            var json = G_NS.JsonConvert.SerializeObject( allCompiledEvents, SerializerOptions.NewtonsoftJsonSerializer );
            Console.WriteLine( json );
        }
    }
}
