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
        [Ignore]
        public async Task CopyAimingMark() {
            //Choosing not to write a seperate unit test for AimingMark, as the unit test for Target is comprehensive and covers this class
        }

        [TestMethod]
        public async Task CopyAttribute() {

            var client = new DefinitionAPIClient();
            var setName = SetName.Parse( "v1.0:orion:Address" );
            var orig = (await client.GetAttributeDefinitionAsync( setName )).Value;

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

            //Attribute Specific fields
            Assert.AreEqual( orig.DisplayName, copy.DisplayName );
            CollectionAssert.AreEqual( orig.Designation, copy.Designation );
            Assert.AreEqual( orig.MultipleValues, copy.MultipleValues );
            Assert.AreEqual( orig.MaxVisibility, copy.MaxVisibility );
            Assert.AreEqual( orig.DefaultVisibility, copy.DefaultVisibility );

            for (int i = 0; i < orig.Fields.Count; i++) {
                Assert.AreEqual( orig.Fields[i].FieldName, copy.Fields[i].FieldName );
                Assert.AreEqual( orig.Fields[i].MultipleValues, copy.Fields[i].MultipleValues );
                Assert.AreEqual( orig.Fields[i].Required, copy.Fields[i].Required );
                Assert.AreEqual( orig.Fields[i].ValueType, copy.Fields[i].ValueType );
                Assert.AreEqual( orig.Fields[i].Key, copy.Fields[i].Key );
                Assert.AreEqual( orig.Fields[i].FieldType, copy.Fields[i].FieldType );
                Assert.AreEqual( orig.Fields[i].Comment, copy.Fields[i].Comment );

                //test AttributeValueOption
                for (int j = 0; j < orig.Fields[i].Values.Count; j++) {
                    Assert.AreEqual( orig.Fields[i].Values[j].Value, copy.Fields[i].Values[j].Value );
                    Assert.AreEqual( orig.Fields[i].Values[j].Name, copy.Fields[i].Values[j].Name );
                }

                //test AttributeValidation 
                if (orig.Fields[i].Validation != null) {
                    Assert.AreEqual( orig.Fields[i].Validation.Regex, copy.Fields[i].Validation.Regex );
                    Assert.AreEqual( orig.Fields[i].Validation.ErrorMessage, copy.Fields[i].Validation.ErrorMessage );
                    Assert.AreEqual( orig.Fields[i].Validation.Comment, copy.Fields[i].Validation.Comment );
                    if (orig.Fields[i].Validation.MinValue != null) {
                        Assert.AreEqual( orig.Fields[i].Validation.MinValue, copy.Fields[i].Validation.MinValue );
                    } else {
                        Assert.IsNull( copy.Fields[i].Validation.MinValue );
                    }
                    if (orig.Fields[i].Validation.MaxValue != null) {
                        Assert.AreEqual( orig.Fields[i].Validation.MaxValue, copy.Fields[i].Validation.MaxValue );
                    } else {
                        Assert.IsNull( copy.Fields[i].Validation.MaxValue );
                    }
                } else {
                    Assert.IsNull( copy.Fields[i].Validation );
                }
            }
        }

        [TestMethod]
        [Ignore]
        public async Task CopyAttributeValidation() {
            //Choosing not to write a seperate unit test for AttributeValidation, as the unit test for Attribute is comprehensive and covers this class
        }

        [TestMethod]
        [Ignore]
        public async Task CopyAttributeValueOption() {
            //Choosing not to write a seperate unit test for AttributeValueOption, as the unit test for Attribute is comprehensive and covers this class
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

            //Common fields for all top level Definition objects
            Assert.AreEqual(esd.HierarchicalName, Copyesd.HierarchicalName);
            Assert.AreEqual(esd.Description, Copyesd.Description);
            Assert.AreEqual(esd.Comment, Copyesd.Comment);
            Assert.AreEqual(esd.Version, Copyesd.Version);
            Assert.AreEqual(esd.Type, Copyesd.Type);
            Assert.AreEqual(esd.SetName, Copyesd.SetName);
            Assert.AreEqual(esd.Owner, Copyesd.Owner);
            Assert.AreEqual(esd.Discipline, Copyesd.Discipline);
            Assert.AreEqual(esd.Discontinued, Copyesd.Discontinued);
            Assert.AreEqual(esd.Subdiscipline, Copyesd.Subdiscipline);
            CollectionAssert.AreEqual(esd.Tags, Copyesd.Tags);
            Assert.AreEqual(esd.JSONVersion, Copyesd.JSONVersion);

            for( int i = 0; i < esd.SimpleCOFs.Count(); i++)
            {
                Assert.AreEqual(esd.SimpleCOFs[i].Name, Copyesd.SimpleCOFs[i].Name);
                Assert.AreEqual(esd.SimpleCOFs[i].CourseOfFireDef, Copyesd.SimpleCOFs[i].CourseOfFireDef);
                for(int j = 0; j < esd.SimpleCOFs[i].Components.Count(); j++)
                {
                    Assert.AreEqual(esd.SimpleCOFs[i].Components[j].StageStyle, Copyesd.SimpleCOFs[i].Components[j].StageStyle);
                }
            }
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
        public async Task CopyRankingRules()
        {
            var client = new DefinitionAPIClient();
            var setName = SetName.Parse("v1.0:orion:Generic Qualification");
            var rr = (await client.GetRankingRuleDefinitionAsync(setName)).Value;

            var copy = rr.Copy();

            //Common fields for all top level Definition objects
            Assert.AreEqual(rr.HierarchicalName, copy.HierarchicalName);
            Assert.AreEqual(rr.Description, copy.Description);
            Assert.AreEqual(rr.Comment, copy.Comment);
            Assert.AreEqual(rr.Version, copy.Version);
            Assert.AreEqual(rr.Type, copy.Type);
            Assert.AreEqual(rr.SetName, copy.SetName);
            Assert.AreEqual(rr.Owner, copy.Owner);
            Assert.AreEqual(rr.Discipline, copy.Discipline);
            Assert.AreEqual(rr.Discontinued, copy.Discontinued);
            Assert.AreEqual(rr.Subdiscipline, copy.Subdiscipline);
            CollectionAssert.AreEqual(rr.Tags, copy.Tags);
            Assert.AreEqual(rr.JSONVersion, copy.JSONVersion);

            for (int i = 0; i<rr.RankingRules.Count(); i++)
            {
                Assert.AreEqual(rr.RankingRules[i].AppliesTo, copy.RankingRules[i].AppliesTo);
                for(int j = 0; j < rr.RankingRules[i].Rules.Count(); j++)
                {
                    Assert.AreEqual(rr.RankingRules[i].Rules[j].EventName, copy.RankingRules[i].Rules[j].EventName);
                    Assert.AreEqual(rr.RankingRules[i].Rules[j].Values, copy.RankingRules[i].Rules[j].Values);
                    Assert.AreEqual(rr.RankingRules[i].Rules[j].Method, copy.RankingRules[i].Rules[j].Method);
                    Assert.AreEqual(rr.RankingRules[i].Rules[j].SortOrder, copy.RankingRules[i].Rules[j].SortOrder);
                    Assert.AreEqual(rr.RankingRules[i].Rules[j].ResultStatus, copy.RankingRules[i].Rules[j].ResultStatus);
                    Assert.AreEqual(rr.RankingRules[i].Rules[j].Source, copy.RankingRules[i].Rules[j].Source);
                }
                for(int j = 0; j < rr.RankingRules[i].ListOnly.Count(); j++)
                {
                    Assert.AreEqual(rr.RankingRules[i].ListOnly[j].EventName, copy.RankingRules[i].ListOnly[j].EventName);
                    Assert.AreEqual(rr.RankingRules[i].ListOnly[j].Values, copy.RankingRules[i].ListOnly[j].Values);
                    Assert.AreEqual(rr.RankingRules[i].ListOnly[j].Method, copy.RankingRules[i].ListOnly[j].Method);
                    Assert.AreEqual(rr.RankingRules[i].ListOnly[j].SortOrder, copy.RankingRules[i].ListOnly[j].SortOrder);
                    Assert.AreEqual(rr.RankingRules[i].ListOnly[j].ResultStatus, copy.RankingRules[i].ListOnly[j].ResultStatus);
                    Assert.AreEqual(rr.RankingRules[i].ListOnly[j].Source, copy.RankingRules[i].ListOnly[j].Source);
                }
            }
        }

        [TestMethod]
        public async Task CopyResultListFormat()
        {
            var client = new DefinitionAPIClient();
            var setName = SetName.Parse("v1.0:orion:3P Individual");
            var rlf = (await client.GetResultListFormatDefinitionAsync(setName)).Value;

            var copy = rlf.Copy();

            //Common fields for all top level Definition objects
            Assert.AreEqual(rlf.HierarchicalName, copy.HierarchicalName);
            Assert.AreEqual(rlf.Description, copy.Description);
            Assert.AreEqual(rlf.Comment, copy.Comment);
            Assert.AreEqual(rlf.Version, copy.Version);
            Assert.AreEqual(rlf.Type, copy.Type);
            Assert.AreEqual(rlf.SetName, copy.SetName);
            Assert.AreEqual(rlf.Owner, copy.Owner);
            Assert.AreEqual(rlf.Discipline, copy.Discipline);
            Assert.AreEqual(rlf.Discontinued, copy.Discontinued);
            Assert.AreEqual(rlf.Subdiscipline, copy.Subdiscipline);
            CollectionAssert.AreEqual(rlf.Tags, copy.Tags);
            Assert.AreEqual(rlf.JSONVersion, copy.JSONVersion);

            Assert.AreEqual(rlf.ScoreFormatCollectionDef, copy.ScoreFormatCollectionDef);
            Assert.AreEqual(rlf.ScoreConfigDefault, copy.ScoreConfigDefault);

            for (int i = 0; i < rlf.Fields.Count(); i++)
            {
                Assert.AreEqual(rlf.Fields[i].FieldName, copy.Fields[i].FieldName);
                Assert.AreEqual(rlf.Fields[i].Method, copy.Fields[i].Method);

                Assert.AreEqual(rlf.Fields[i].Source.Name, copy.Fields[i].Source.Name);
                Assert.AreEqual(rlf.Fields[i].Source.ScoreFormat, copy.Fields[i].Source.ScoreFormat);
                Assert.AreEqual(rlf.Fields[i].Source.Value, copy.Fields[i].Source.Value);
            }
            //choosing only to do Display and not DisplayForTeam (same thing)
            CollectionAssert.AreEqual(rlf.Format.Display.Header.ClassList, copy.Format.Display.Header.ClassList);
            CollectionAssert.AreEqual(rlf.Format.Display.Header.RowClass, copy.Format.Display.Header.RowClass);

            CollectionAssert.AreEqual(rlf.Format.Display.Body.ClassList, copy.Format.Display.Header.ClassList);
            CollectionAssert.AreEqual(rlf.Format.Display.Body.RowClass, copy.Format.Display.Body.RowClass);
            CollectionAssert.AreEqual(rlf.Format.Display.Body.RowLinkTo, copy.Format.Display.Body.RowLinkTo);

            for (int i = 0; i < rlf.Format.Columns.Count(); i++)
            {
                Assert.AreEqual(rlf.Format.Columns[i].Header, copy.Format.Columns[i].Header);
                CollectionAssert.AreEqual(rlf.Format.Columns[i].ClassList, copy.Format.Columns[i].ClassList);
                Assert.AreEqual(rlf.Format.Columns[i].Body, copy.Format.Columns[i].Body);
                Assert.AreEqual(rlf.Format.Columns[i].BodyLinkTo, copy.Format.Columns[i].BodyLinkTo);
                Assert.AreEqual(rlf.Format.Columns[i].Footer, copy.Format.Columns[i].Footer);
                CollectionAssert.AreEqual(rlf.Format.Columns[i].HeaderClassList, copy.Format.Columns[i].HeaderClassList);
                CollectionAssert.AreEqual(rlf.Format.Columns[i].BodyClassList, copy.Format.Columns[i].BodyClassList);
                CollectionAssert.AreEqual(rlf.Format.Columns[i].FooterClassList, copy.Format.Columns[i].FooterClassList);
            }
        }

        [TestMethod]
        public async Task CopyScoreFormatCollection()
        {
            var client = new DefinitionAPIClient();
            var setName = SetName.Parse("v1.0:orion:Standard Score Formats");
            var call = await client.GetScoreFormatCollectionDefinitionAsync(setName);
            var sfc = call.Value;

            var copy = sfc.Copy();

            //Common fields for all top level Definition objects
            Assert.AreEqual(sfc.HierarchicalName, copy.HierarchicalName);
            Assert.AreEqual(sfc.Description, copy.Description);
            Assert.AreEqual(sfc.Comment, copy.Comment);
            Assert.AreEqual(sfc.Version, copy.Version);
            Assert.AreEqual(sfc.Type, copy.Type);
            Assert.AreEqual(sfc.SetName, copy.SetName);
            Assert.AreEqual(sfc.Owner, copy.Owner);
            Assert.AreEqual(sfc.Discipline, copy.Discipline);
            Assert.AreEqual(sfc.Discontinued, copy.Discontinued);
            Assert.AreEqual(sfc.Subdiscipline, copy.Subdiscipline);
            CollectionAssert.AreEqual(sfc.Tags, copy.Tags);
            Assert.AreEqual(sfc.JSONVersion, copy.JSONVersion);

            CollectionAssert.AreEqual(sfc.ScoreFormats, copy.ScoreFormats);
            
            for (int i = 0; i < sfc.ScoreConfigs.Count(); i++)
            {
                Assert.AreEqual(sfc.ScoreConfigs[i].ScoreConfigName, copy.ScoreConfigs[i].ScoreConfigName);
                CollectionAssert.AreEqual(sfc.ScoreConfigs[i].ScoreFormats, copy.ScoreConfigs[i].ScoreFormats);
            }
        }

        [TestMethod]
        [Ignore]
        public async Task CopyScoringRing() {
            //Choosing not to write a seperate unit test for ScoringRing, as the unit test for Target is comprehensive and covers this class
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
        public async Task CopyStageStyle()
        {
            var client = new DefinitionAPIClient();
            var setName = SetName.Parse("v1.0:ntparc:Precision Air Rifle Standing");
            var call = await client.GetStageStyleDefinitionAsync(setName);
            var ss = call.Value;

            var copy = ss.Copy();

            //Common fields for all top level Definition objects
            Assert.AreEqual(ss.HierarchicalName, copy.HierarchicalName);
            Assert.AreEqual(ss.Description, copy.Description);
            Assert.AreEqual(ss.Comment, copy.Comment);
            Assert.AreEqual(ss.Version, copy.Version);
            Assert.AreEqual(ss.Type, copy.Type);
            Assert.AreEqual(ss.SetName, copy.SetName);
            Assert.AreEqual(ss.Owner, copy.Owner);
            Assert.AreEqual(ss.Discipline, copy.Discipline);
            Assert.AreEqual(ss.Discontinued, copy.Discontinued);
            Assert.AreEqual(ss.Subdiscipline, copy.Subdiscipline);
            CollectionAssert.AreEqual(ss.Tags, copy.Tags);
            Assert.AreEqual(ss.JSONVersion, copy.JSONVersion);

            CollectionAssert.AreEqual(ss.RelatedStageStyles, copy.RelatedStageStyles);
            Assert.AreEqual(ss.ShotsInSeries, copy.ShotsInSeries);
            for (int i = 0; i < ss.DisplayScoreFormats.Count(); i++)
            {
                Assert.AreEqual(ss.DisplayScoreFormats[i].Name, copy.DisplayScoreFormats[i].Name);
                Assert.AreEqual(ss.DisplayScoreFormats[i].ScoreFormat, copy.DisplayScoreFormats[i].ScoreFormat);
                Assert.AreEqual(ss.DisplayScoreFormats[i].MaxShotValue, copy.DisplayScoreFormats[i].MaxShotValue);
            }

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
        public async Task CopyTarget() {

            var client = new DefinitionAPIClient();
            var setName = SetName.Parse( "v1.0:issf:25m Precision Pistol" );
            var orig = (await client.GetTargetDefinitionAsync( setName )).Value;

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

            Assert.AreEqual( orig.BackgroundColor, copy.BackgroundColor );
            Assert.AreEqual( orig.Distance, copy.Distance );
            Assert.AreEqual( orig.InnerTen.Dimension, copy.InnerTen.Dimension );
            Assert.AreEqual( orig.InnerTen.Comment, copy.InnerTen.Comment );
            Assert.AreEqual( orig.InnerTen.Value, copy.InnerTen.Value );
            Assert.AreEqual( orig.InnerTen.Shape, copy.InnerTen.Shape );

            for (int i = 0; i < orig.ScoringRings.Count; i++) {
                Assert.AreEqual( orig.ScoringRings[i].Dimension, copy.ScoringRings[i].Dimension );
                Assert.AreEqual( orig.ScoringRings[i].Value, copy.ScoringRings[i].Value );
                Assert.AreEqual( orig.ScoringRings[i].Shape, copy.ScoringRings[i].Shape );
                Assert.AreEqual( orig.ScoringRings[i].Comment, copy.ScoringRings[i].Comment );
            }

            for (int i = 0; i < orig.AimingMarks.Count; i++) {
                Assert.AreEqual( orig.AimingMarks[i].Dimension, copy.AimingMarks[i].Dimension );
                Assert.AreEqual( orig.AimingMarks[i].Color, copy.AimingMarks[i].Color );
                Assert.AreEqual( orig.AimingMarks[i].Shape, copy.AimingMarks[i].Shape );
                Assert.AreEqual( orig.AimingMarks[i].Comment, copy.AimingMarks[i].Comment );
            }

        }

        [TestMethod]
        public async Task CopyTargetCollection() {

            var client = new DefinitionAPIClient();
            var setName = SetName.Parse( "v1.0:cmp:Smallbore Rifle Prone" );
            var orig = (await client.GetTargetCollectionDefinitionAsync( setName )).Value;

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

            for (int i = 0; i < orig.TargetCollections.Count; i++) {
                Assert.AreEqual( orig.TargetCollections[i].TargetCollectionName, copy.TargetCollections[i].TargetCollectionName );
                Assert.AreEqual( orig.TargetCollections[i].RangeDistance, copy.TargetCollections[i].RangeDistance );
                Assert.AreEqual( orig.TargetCollections[i].Comment, copy.TargetCollections[i].Comment );
                CollectionAssert.AreEqual( orig.TargetCollections[i].TargetDefs, copy.TargetCollections[i].TargetDefs );
            }
        }

        [TestMethod]
        [Ignore]
        public async Task CopyTargetCollectionModal() {
            //Choosing not to write a seperate unit test for TargetCollectionModal, as the unit test for TargetCollection is comprehensive and covers this class
        }

        [TestMethod]
        public async Task CopyTieBreakingRule() {


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

            var swCopyEventsOnly = Stopwatch.StartNew();
            var copyEventsOnly = orig.Copy();
            swCopyEventsOnly.Stop();

            var swClone = Stopwatch.StartNew();
            var clone = orig.Clone();
            swClone.Stop();

            Console.WriteLine( $"Copy took {swCopy.ElapsedTicks} ticks" );
            Console.WriteLine( $"Copy Events Only took {swCopyEventsOnly.ElapsedTicks} ticks" );
            Console.WriteLine( $"Clone took {swClone.ElapsedTicks} ticks" );
        }
    }
}
