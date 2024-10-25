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
    public class ICopyTests {

        [TestInitialize]
        public void InitializeTest() {
            Scopos.BabelFish.Runtime.Settings.XApiKey = Constants.X_API_KEY;
        }

        [TestMethod]
        public async Task CopyBarcodeLabel() {
            var orig = new BarcodeLabel();
            orig.StageLabel = "I";
            orig.Series = "1..8";
            orig.TargetName = "10m Air Rifle 12 Bull Target";
            orig.Comment = "This is a test";
            orig.LabelSize = BarcodeLabelSize.OL161;

            var copy = orig.Copy();

            Assert.AreEqual( orig.StageLabel, copy.StageLabel );
            Assert.AreEqual( orig.Series, copy.Series );
            Assert.AreEqual( orig.TargetName, copy.TargetName );
            Assert.AreEqual( orig.Comment, copy.Comment );
            Assert.AreEqual( orig.LabelSize, copy.LabelSize );
        }

        [TestMethod]
        public async Task CopyCourseOfFire() {

            var client = new DefinitionAPIClient();
            var setName = SetName.Parse( "v3.0:ntparc:Three-Position Air Rifle 3x20" );
            var orig = (await client.GetCourseOfFireDefinitionAsync( setName )).Value;
            var copy = orig.Copy();

            //Common fields for all top level Definition objects
            Assert.AreEqual( orig.HierarchicalName, copy.HierarchicalName );
            Assert.AreEqual( orig.Description, copy.Description );
            Assert.AreEqual( orig.Comment, copy.Comment );
            Assert.AreEqual( orig.Version, copy.Version );
            Assert.AreEqual( orig.Type, copy.Type );
            Assert.AreEqual( orig.SetName, copy.SetName );
            Assert.AreEqual( orig.Owner, copy.Owner );
            Assert.AreEqual( orig.Discipline, copy.Discipline );
            Assert.AreEqual( orig.Discontinued, copy.Discontinued );
            Assert.AreEqual( orig.Subdiscipline, copy.Subdiscipline );
            CollectionAssert.AreEqual( orig.Tags, copy.Tags );
            Assert.AreEqual( orig.JSONVersion, copy.JSONVersion );

            //Fields specific to Course of Fire
            Assert.AreEqual( orig.Description, copy.Description );
            Assert.AreEqual( orig.CommonName, copy.CommonName );
            Assert.AreEqual( orig.TargetCollectionDef, copy.TargetCollectionDef );
            Assert.AreEqual( orig.DefaultTargetCollectionName, copy.DefaultTargetCollectionName );
            Assert.AreEqual( orig.DefaultExpectedDiameter, copy.DefaultExpectedDiameter );
            Assert.AreEqual( orig.DefaultScoringDiameter, copy.DefaultScoringDiameter );
            Assert.AreEqual( orig.ScoreFormatCollectionDef, copy.ScoreFormatCollectionDef );
            Assert.AreEqual( orig.DefaultEventAndStageStyleMappingDef, copy.DefaultEventAndStageStyleMappingDef );
            Assert.AreEqual( orig.DefaultAttributeDef, copy.DefaultAttributeDef );
            Assert.AreEqual( orig.ScoreConfigDefault, copy.ScoreConfigDefault );
            Assert.AreEqual( orig.RangeScripts.Count, copy.RangeScripts.Count );
            Assert.AreEqual( orig.Events.Count, copy.Events.Count );
            Assert.AreEqual( orig.Singulars.Count, copy.Singulars.Count );
            Assert.AreEqual( orig.AbbreviatedFormats.Count, copy.AbbreviatedFormats.Count );

            //Test that all the events got copied correctly, and remain in same order
            var origEventComposite = EventComposite.GrowEventTree( orig );
            var copyEventComposite = EventComposite.GrowEventTree( copy );
            var origEvents = origEventComposite.GetEvents( true, true, true, true, true, true );
            var copyEvents = copyEventComposite.GetEvents( true, true, true, true, true, true );
            for( int i = 0; i < origEvents.Count; i++ ) {
                Assert.AreEqual( origEvents[i].EventName, copyEvents[i].EventName );
            }

            //Test that the range scripts got copied, and remain in the same order
            for (int i = 0; i < orig.RangeScripts.Count; i++) {
                Assert.AreEqual( orig.RangeScripts[i].RangeScriptName, copy.RangeScripts[i].RangeScriptName );
                for (int j = 0; j < orig.RangeScripts[i].SegmentGroups.Count; j++) {
                    Assert.AreEqual( orig.RangeScripts[i].SegmentGroups[j].SegmentGroupName, copy.RangeScripts[i].SegmentGroups[j].SegmentGroupName );

                    for (int k = 0; k < orig.RangeScripts[i].SegmentGroups[j].Commands.Count; k++) {
                        Assert.AreEqual( orig.RangeScripts[i].SegmentGroups[j].Commands[k].GetCommand(), copy.RangeScripts[i].SegmentGroups[j].Commands[k].GetCommand() );
                        Assert.AreEqual( orig.RangeScripts[i].SegmentGroups[j].Commands[k].GetDisplayEvent(), copy.RangeScripts[i].SegmentGroups[j].Commands[k].GetDisplayEvent() );
                        Assert.AreEqual( orig.RangeScripts[i].SegmentGroups[j].Commands[k].GetGreenLight(), copy.RangeScripts[i].SegmentGroups[j].Commands[k].GetGreenLight() );
                    }

                    for (int k = 0; k < orig.RangeScripts[i].SegmentGroups[j].Segments.Count; k++) {
                        Assert.AreEqual( orig.RangeScripts[i].SegmentGroups[j].Segments[k].GetAbbreviatedFormat(), copy.RangeScripts[i].SegmentGroups[j].Segments[k].GetAbbreviatedFormat() );
                        Assert.AreEqual( orig.RangeScripts[i].SegmentGroups[j].Segments[k].GetStageLabel(), copy.RangeScripts[i].SegmentGroups[j].Segments[k].GetStageLabel() );
                        Assert.AreEqual( orig.RangeScripts[i].SegmentGroups[j].Segments[k].GetTargetHeight(), copy.RangeScripts[i].SegmentGroups[j].Segments[k].GetTargetHeight() );
                    }
                }
            }
        }

        [TestMethod]
        public async Task CopyEvent() {

            var client = new DefinitionAPIClient();
            var setName = SetName.Parse( "v2.0:orion:Informal Practice Air Rifle" );
            var cof = (await client.GetCourseOfFireDefinitionAsync( setName )).Value;

            foreach (var @event in cof.Events) {
                var copyOfEvent = @event.Copy();
                Assert.AreEqual( @event.EventName, copyOfEvent.EventName );
                Assert.AreEqual( @event.EventType, copyOfEvent.EventType );
                Assert.AreEqual( @event.Calculation, copyOfEvent .Calculation );
                Assert.AreEqual( @event.ScoreFormat, copyOfEvent .ScoreFormat );
                Assert.AreEqual( @event.ResultListFormatDef, copyOfEvent.ResultListFormatDef );
                Assert.AreEqual( @event.Comment, copyOfEvent.Comment );
                Assert.AreEqual( @event.Values, copyOfEvent.Values );

                var copyEventNames = copyOfEvent.GetChildrenEventNames();
                var origEventNames = @event.GetChildrenEventNames();
                CollectionAssert.AreEqual( origEventNames, copyEventNames );
            }
        }

        [TestMethod]
        public async Task CopyEventAndStageStyleMapping()
        {

            var client = new DefinitionAPIClient();
            var setName = SetName.Parse("v1.0:orion:Air Rifle");
            var esm = (await client.GetEventAndStageStyleMappingDefinitionAsync(setName)).Value;

            var CopyOfesm = esm.Copy();

            Assert.AreEqual(esm.DefaultMapping.DefaultEventStyleDef, CopyOfesm.DefaultMapping.DefaultEventStyleDef);
            Assert.AreEqual(esm.DefaultMapping.DefaultStageStyleDef, CopyOfesm.DefaultMapping.DefaultStageStyleDef);
            Assert.AreEqual( esm.Mappings.Count, CopyOfesm.Mappings.Count );
            CollectionAssert.AreEqual(esm.DefaultMapping.AttributeValueAppellation, CopyOfesm.DefaultMapping.AttributeValueAppellation);
            CollectionAssert.AreEqual(esm.DefaultMapping.TargetCollectionName, CopyOfesm.DefaultMapping.TargetCollectionName);

            //Common fields for all top level Definition objects
            Assert.AreEqual( esm.HierarchicalName, CopyOfesm.HierarchicalName );
            Assert.AreEqual( esm.Description, CopyOfesm.Description );
            Assert.AreEqual( esm.Comment, CopyOfesm.Comment );
            Assert.AreEqual( esm.Version, CopyOfesm.Version );
            Assert.AreEqual( esm.Type, CopyOfesm.Type );
            Assert.AreEqual( esm.SetName, CopyOfesm.SetName );
            Assert.AreEqual( esm.Owner, CopyOfesm.Owner );
            Assert.AreEqual( esm.Discipline, CopyOfesm.Discipline );
            Assert.AreEqual( esm.Discontinued, CopyOfesm.Discontinued );
            Assert.AreEqual( esm.Subdiscipline, CopyOfesm.Subdiscipline );
            CollectionAssert.AreEqual( esm.Tags, CopyOfesm.Tags );
            Assert.AreEqual( esm.JSONVersion, CopyOfesm.JSONVersion );


            //This object also contains EventStyleSelection and StageStyleSelection, tests below
        }

        [TestMethod]
        public async Task CopyEventStyle()
        {
            var client = new DefinitionAPIClient();
            var setName = SetName.Parse("v1.0:ntparc:Precision Air Rifle Standing");
            var esd = (await client.GetEventStyleDefinitionAsync(setName)).Value;

            //copy of EventStyle
            EventStyle Copyesd = new EventStyle();
            Copyesd = esd.Copy();

            CollectionAssert.AreEquivalent(esd.EventStyles, Copyesd.EventStyles);
            CollectionAssert.AreEquivalent(esd.StageStyles, Copyesd.StageStyles);
            CollectionAssert.AreEquivalent(esd.RelatedEventStyles, Copyesd.RelatedEventStyles);
            //CopySimpleCOF should also be tested for functionality
        }

        [TestMethod]
        public async Task CopyEventStyleSelection()
        {
            //part of EventAndStageStyleMappingObj
            EventStyleSelection es = new EventStyleSelection();
            es.EventStyleDef = "v1.0:ntparc:Three-Position Sporter Air Rifle";
            es.EventAppellation = "Qualification3P";

            EventStyleSelection CopyOfes = new EventStyleSelection();
            CopyOfes = es.Copy();

            Assert.AreEqual(es.EventAppellation, CopyOfes.EventAppellation);
            Assert.AreEqual(es.EventStyleDef, CopyOfes.EventStyleDef);
        }

        [TestMethod]
        public async Task CopyRangeScripts() {
            var orig = new RangeScript();
            orig.RangeScriptName = "Bob";
            orig.DesignedForEST = true;
            orig.DesignedForPaper = true;
            orig.Comment = "This is a test";

            var copy = orig.Copy();

            Assert.AreEqual( orig.RangeScriptName, copy.RangeScriptName );
            Assert.AreEqual( orig.DesignedForEST, copy.DesignedForEST );
            Assert.AreEqual( orig.DesignedForPaper, copy.DesignedForPaper );
            Assert.AreEqual( orig.Comment, copy.Comment ); 
        }

        [TestMethod]
        public async Task CopySegmentGroup() {
            var orig = new SegmentGroup();
            orig.SegmentGroupName = "Bob";
            orig.Comment = "This is a test";

            var copy = orig.Copy();

            Assert.AreEqual( orig.SegmentGroupName, copy.SegmentGroupName );
            Assert.AreEqual( orig.Comment, copy.Comment );
        }

        [TestMethod]
        public async Task CopySegmentGroupCommand() {
            var orig = new SegmentGroupCommand();
            orig.Command = "This is a test";
            orig.Notes = "This too is a test";
            orig.Fade = 12;
            orig.Timer = "0:00:00.4";
            orig.TimerCommand = TimerCommandOptions.PAUSE;
            orig.OccursAt = "1:12:34.5";
            orig.GreenLight = LightIllumination.ON;
            orig.RedLight = LightIllumination.OFF;
            orig.TargetLight = LightIllumination.DIM;
            orig.ShotAttributes.Add( "FIRED BEFORE COMMAND START" );
            orig.ShotAttributes.Add( "FIRED AFTER COMMAND STOP" );
            orig.DisplayEvent = DisplayEventOptions.QualificationPostEvent;
            orig.Continue = 15;
            orig.NextCommandIndex = 17;
            orig.Comment = "Still, another test";

            var copy = orig.Copy();

            Assert.AreEqual ( orig.Command, copy.Command );
            Assert.AreEqual( orig.Notes, copy.Notes );
            Assert.AreEqual( orig.Fade, copy.Fade );
            Assert.AreEqual( orig.Timer, copy.Timer );
            Assert.AreEqual( orig.TimerCommand, copy.TimerCommand );
            Assert.AreEqual( orig.OccursAt, copy.OccursAt );
            Assert.AreEqual( orig.GreenLight, copy.GreenLight );
            Assert.AreEqual( orig.RedLight, copy.RedLight );
            Assert.AreEqual( orig.TargetLight, copy.TargetLight );
            Assert.AreEqual( orig.DisplayEvent, copy.DisplayEvent );
            Assert.AreEqual( orig.Continue, copy.Continue );
            Assert.AreEqual( orig.NextCommandIndex, copy.NextCommandIndex );
            Assert.AreEqual( orig.Comment, copy.Comment );
            CollectionAssert.AreEqual( orig.ShotAttributes, copy.ShotAttributes );
        }

        [TestMethod]
        public async Task CopySegmentGroupSegment() {
            var orig = new SegmentGroupSegment();
            orig.SegmentName = "Bob";
            orig.NumberOfShots = 12;
            orig.StageLabel = "Z";
            orig.TargetCollectionIndex = 15;
            orig.TargetHeight = 666;
            orig.AthleteHasControl.Add( "TargetLight" );
            orig.AthleteHasControl.Add( "TargetLift" );
            orig.ShotAttributes.Add( "SIGHTER" );
            orig.ShotAttributes.Add( "STOP" );
            orig.AbbreviatedFormat = "Susan";
            orig.Special.Add(SpecialOptions.GROUP_MODE );
            orig.Special.Add(SpecialOptions.SHOT_CALLING );
            orig.StringSize = 17;
            orig.TapeAdvance = 8;
            orig.Comment = "This is a test";

            var copy = orig.Copy();

            Assert.AreEqual( orig.SegmentName, copy.SegmentName );
            Assert.AreEqual( orig.NumberOfShots, copy.NumberOfShots );
            Assert.AreEqual( orig.StageLabel, copy.StageLabel );
            Assert.AreEqual( orig.TargetCollectionIndex, copy.TargetCollectionIndex );
            Assert.AreEqual( orig.TargetHeight, copy.TargetHeight );
            Assert.AreEqual( orig.StringSize, copy.StringSize );
            Assert.AreEqual( orig.TapeAdvance, copy.TapeAdvance );
            Assert.AreEqual( orig.Comment, copy.Comment );
            CollectionAssert.AreEqual( orig.AthleteHasControl, copy.AthleteHasControl );
            CollectionAssert.AreEqual( orig.ShotAttributes, copy.ShotAttributes );
            CollectionAssert.AreEqual( orig.Special, copy.Special );
        }

        [TestMethod]
        public async Task CopySegmentShow() {

            var orig = new ShowInSegment();
            orig.ShotPresentation = "PAST(5)";
            orig.Comment = "This is a test";
            orig.Competition = CompetitionType.SIGHTER;
            orig.StageLabel.Add( "P" );
            orig.StageLabel.Add( "S" );

            var copy = orig.Copy();

            Assert.IsNotNull( copy );
            Assert.AreEqual( orig.ShotPresentation, copy.ShotPresentation );
            Assert.AreEqual( orig.Comment, copy.Comment );
            Assert.AreEqual( orig.Competition, copy.Competition );
            CollectionAssert.AreEqual( orig.StageLabel, copy.StageLabel );

            //Change the orig valaues to make sure the copy remains unchanged
            orig.ShotPresentation = "ALL";
            orig.StageLabel.Add( "K" );
            orig.Comment = "Not a test";
            orig.Competition = CompetitionType.COMPETITION;
            Assert.AreNotEqual( orig.ShotPresentation, copy.ShotPresentation );
            Assert.AreNotEqual( orig.Comment, copy.Comment );
            Assert.AreNotEqual( orig.Competition, copy.Competition );
            CollectionAssert.AreNotEqual( orig.StageLabel, copy.StageLabel );
        }

        [TestMethod]
        public void CopySingular() {
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
        public async Task CopySimpleCOF()
        {
            // part of CopyEventStyle
            SimpleCOF sc = new SimpleCOF();
            sc.Name = "Something here";
            sc.CourseOfFireDef = "Somethingelse:here";

            //copy scof
            SimpleCOF Copysc = new SimpleCOF();
            Copysc = sc.Copy();

            Assert.AreEqual(sc.Name, Copysc.Name);
            Assert.AreEqual(sc.CourseOfFireDef, Copysc.CourseOfFireDef);
            //CopySimpleCOFComponent should also be tested for functionality
        }

        [TestMethod]
        public async Task CopySimpleCOFComponent()
        {
            // part of CopySimpleCOF
            SimpleCOFComponent scc = new SimpleCOFComponent();
            scc.StageStyle = "Something here";
            scc.Shots = 15;
            scc.ScoreFormat = "{d}";

            //copy scofc
            SimpleCOFComponent Copysc = new SimpleCOFComponent();
            Copysc = scc.Copy();

            Assert.AreEqual(scc.StageStyle, Copysc.StageStyle);
            Assert.AreEqual(scc.Shots, Copysc.Shots);
            Assert.AreEqual(scc.ScoreFormat, Copysc.ScoreFormat);
        }

        [TestMethod]
        public async Task CopyStageStyleSelection()
        {
            //part of EventAndStageStyleMappingObj
            StageStyleSelection es = new StageStyleSelection();
            es.StageStyleDef = "v1.0:ntparc:Sporter Air Rifle Final Kneeling 5-Shot Series";
            es.StageAppellation = "FiveShotFinalKneeling";

            StageStyleSelection CopyOfes = new StageStyleSelection();
            CopyOfes = es.Copy();

            Assert.AreEqual(es.StageAppellation, CopyOfes.StageAppellation);
            Assert.AreEqual(es.StageStyleDef, CopyOfes.StageStyleDef);
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

        [TestMethod] 
        public async Task EriksPlayground() {

            var client = new DefinitionAPIClient();
            var setName = SetName.Parse( "v3.0:ntparc:Three-Position Air Rifle 3x20" );
            var orig = (await client.GetCourseOfFireDefinitionAsync( setName )).Value;

            var swCopy = Stopwatch.StartNew();
            var copy = orig.Copy();
            swCopy.Stop();

            var swClone = Stopwatch.StartNew();
            var clone = orig.Clone();
            swClone.Stop();
        }
    }
}
