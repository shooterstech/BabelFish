using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scopos.BabelFish;
using Scopos.BabelFish.Helpers;
using Scopos.BabelFish.DataModel.Definitions;
using Scopos.BabelFish.APIClients;

namespace Scopos.BabelFish.Tests.Definition
{
    [TestClass]
    public class GetReconfigurableRulebookDefinitionTests {

        /// <summary>
        /// Unit test to confirm the Constructors set the api key and API stage as expected.
        /// </summary>
        [TestMethod]
        public void BasicConstructorTests() {

            var defaultConstructorClient = new DefinitionAPIClient( Constants.X_API_KEY );
            var apiStageConstructorClient = new DefinitionAPIClient( Constants.X_API_KEY, APIStage.BETA );

            Assert.AreEqual( Constants.X_API_KEY, defaultConstructorClient.XApiKey );
            Assert.AreEqual( APIStage.PRODUCTION, defaultConstructorClient.ApiStage );

            Assert.AreEqual( Constants.X_API_KEY, apiStageConstructorClient.XApiKey );
            Assert.AreEqual( APIStage.BETA, apiStageConstructorClient.ApiStage );
        }

        [TestMethod]
        public void GetAttributeAirRifleTest() {

            var client = new DefinitionAPIClient( Constants.X_API_KEY ) { IgnoreInMemoryCache = true };
            var setName = SetName.Parse("v1.0:ntparc:Three-Position Air Rifle Type");

            var taskResponse = client.GetAttributeDefinitionAsync(setName);
            var result = taskResponse.Result;
            Assert.AreEqual( HttpStatusCode.OK, result.StatusCode, $"Expecting and OK status code, instead received {result.StatusCode}." );

            var definition = result.Definition;
            var msgResponse = result.MessageResponse;

            Assert.IsNotNull( definition );
            Assert.IsNotNull(msgResponse);

            Assert.AreEqual( setName.ToString(), definition.SetName);
            Assert.AreEqual( result.DefinitionType, definition.Type );
            Assert.AreEqual(1, definition.Fields.Count );
            Assert.AreEqual( "Three-Position Air Rifle Type", definition.Fields[0].FieldName);
        }

        [TestMethod]
        public void GetCourseOfFireTest() {

            var client = new DefinitionAPIClient( Constants.X_API_KEY ) { IgnoreInMemoryCache = true };
            var setName = SetName.Parse( "v2.0:ntparc:Three-Position Air Rifle 3x10" );

            var taskResponse = client.GetCourseOfFireDefinition( setName );
            var result = taskResponse.Result;
            Assert.AreEqual( HttpStatusCode.OK, result.StatusCode, $"Expecting and OK status code, instead received {result.StatusCode}." );

            var definition = result.Definition;
            var msgResponse = result.MessageResponse;

            Assert.IsNotNull( definition );
            Assert.IsNotNull( msgResponse );

            Assert.AreEqual( setName.ToString(), definition.SetName );
            Assert.AreEqual( result.DefinitionType, definition.Type );
            Assert.IsTrue( definition.RangeScripts.Count > 0 );
            Assert.IsTrue( definition.Events.Count > 0 );
            Assert.IsTrue( definition.AbbreviatedFormats.Count > 0 );
            Assert.IsTrue( definition.Singulars.Count > 0 );
        }

        [TestMethod]
        public void GetEventStyleTest() {

            var client = new DefinitionAPIClient( Constants.X_API_KEY ) { IgnoreInMemoryCache = true };
            var setName = SetName.Parse( "v1.0:ntparc:Three-Position Precision Air Rifle" );

            var taskResponse = client.GetEventStyleDefinition( setName );
            var result = taskResponse.Result;
            Assert.AreEqual( HttpStatusCode.OK, result.StatusCode, $"Expecting and OK status code, instead received {result.StatusCode}." );

            var definition = result.Definition;
            var msgResponse = result.MessageResponse;

            Assert.IsNotNull( definition );
            Assert.IsNotNull( msgResponse );

            Assert.AreEqual( setName.ToString(), definition.SetName );
            Assert.AreEqual( result.DefinitionType, definition.Type );
            Assert.IsTrue( definition.StageStyles.Count > 0 );
            Assert.IsTrue( definition.RelatedEventStyles.Count > 0 );
        }

        [TestMethod]
        public void GetRankingRuleTest()
        {

            var client = new DefinitionAPIClient( Constants.X_API_KEY ) { IgnoreInMemoryCache = true };
            var setName = SetName.Parse("v1.0:nra:BB Gun Qualification");

            var taskResponse = client.GetRankingRuleDefinition( setName );
            var result = taskResponse.Result;
            Assert.AreEqual( HttpStatusCode.OK, result.StatusCode, $"Expecting and OK status code, instead received {result.StatusCode}." );

            var definition = result.Definition;
            var msgResponse = result.MessageResponse;

            Assert.IsNotNull( definition );
            Assert.IsNotNull( msgResponse );

            Assert.AreEqual( setName.ToString(), definition.SetName );
            Assert.AreEqual( result.DefinitionType, definition.Type );

            //Each Ranking Rule definition should have one or more Ranking Directives
            var rankingDirectives = definition.RankingRules;
            Assert.IsTrue( rankingDirectives.Count > 0 );

            //The first Ranking Directive's .AppliesTo must equal '*'
            Assert.AreEqual( "*", rankingDirectives[0].AppliesTo );

            //Must be one or more TieBreakingRules
            var tieBreakingRules = rankingDirectives[0].Rules;
            Assert.IsTrue( tieBreakingRules.Count > 0 );
        }

        [TestMethod]
        public void GetStageStyleTest()
        {

            var client = new DefinitionAPIClient( Constants.X_API_KEY ) { IgnoreInMemoryCache = true };
            var setName = SetName.Parse("v1.0:ntparc:Sporter Air Rifle Standing");

            var taskResponse = client.GetStageStyleDefinition( setName );
            var result = taskResponse.Result;
            Assert.AreEqual( HttpStatusCode.OK, result.StatusCode, $"Expecting and OK status code, instead received {result.StatusCode}." );

            var definition = result.Definition;
            var msgResponse = result.MessageResponse;

            Assert.IsNotNull( definition );
            Assert.IsNotNull( msgResponse );

            Assert.AreEqual( setName.ToString(), definition.SetName );
            Assert.AreEqual( result.DefinitionType, definition.Type );
            Assert.IsTrue(definition.DisplayScoreFormats.Count > 0);
            Assert.IsTrue( definition.RelatedStageStyles.Count > 0 );
        }

        [TestMethod]
        public void GetTargetCollectionTest()
        {

            var client = new DefinitionAPIClient( Constants.X_API_KEY ) { IgnoreInMemoryCache = true };
            var setName = SetName.Parse("v1.0:ntparc:Air Rifle");

            var taskResponse = client.GetTargetCollectionDefinition( setName );
            var result = taskResponse.Result;
            Assert.AreEqual( HttpStatusCode.OK, result.StatusCode, $"Expecting and OK status code, instead received {result.StatusCode}." );

            var definition = result.Definition;
            var msgResponse = result.MessageResponse;

            Assert.IsNotNull( definition );
            Assert.IsNotNull( msgResponse );

            Assert.AreEqual( setName.ToString(), definition.SetName );
            Assert.AreEqual( result.DefinitionType, definition.Type );
            Assert.IsTrue(definition.TargetCollections.Count >= 1);
        }

        [TestMethod]
        public void GetTargetTest()
        {

            var client = new DefinitionAPIClient( Constants.X_API_KEY ) { IgnoreInMemoryCache = true };
            var setName = SetName.Parse("v1.0:issf:10m Air Rifle");

            var taskResponse = client.GetTargetDefinition( setName );
            var result = taskResponse.Result;
            Assert.AreEqual( HttpStatusCode.OK, result.StatusCode, $"Expecting and OK status code, instead received {result.StatusCode}." );

            var definition = result.Definition;
            var msgResponse = result.MessageResponse;

            Assert.IsNotNull( definition );
            Assert.IsNotNull( msgResponse );

            Assert.AreEqual( setName.ToString(), definition.SetName );
            Assert.AreEqual( result.DefinitionType, definition.Type );
            Assert.IsTrue(definition.ScoringRings.Count > 0);
            Assert.IsTrue( definition.AimingMarks.Count > 0 );
        }

        [TestMethod]
        public void GetScoreFormatCollectionType()
        {

            var client = new DefinitionAPIClient( Constants.X_API_KEY ) { IgnoreInMemoryCache = true };
            var setName = SetName.Parse("v1.0:orion:Standard Score Formats");

            var taskResponse = client.GetScoreFormatCollectionDefinition( setName );
            var result = taskResponse.Result;
            Assert.AreEqual( HttpStatusCode.OK, result.StatusCode, $"Expecting and OK status code, instead received {result.StatusCode}." );

            var definition = result.Definition;
            var msgResponse = result.MessageResponse;

            Assert.IsNotNull( definition );
            Assert.IsNotNull( msgResponse );

            Assert.AreEqual( setName.ToString(), definition.SetName );
            Assert.AreEqual( result.DefinitionType, definition.Type );
            Assert.IsTrue(definition.ScoreFormats.Count > 0 );
            Assert.IsTrue( definition.ScoreConfigs.Count > 0 );
        }

        [TestMethod]
        public void GetResultListFormatTest() {

            var client = new DefinitionAPIClient( Constants.X_API_KEY ) { IgnoreInMemoryCache = true };
            var setName = SetName.Parse( "v1.0:orion:3P Individual" );

            var taskResponse = client.GetResultListFormatDefinitionAsync( setName );
            var result = taskResponse.Result;
            Assert.AreEqual( HttpStatusCode.OK, result.StatusCode, $"Expecting and OK status code, instead received {result.StatusCode}." );

            var definition = result.Definition;
            var msgResponse = result.MessageResponse;

            Assert.IsNotNull( definition );
            Assert.IsNotNull( msgResponse );

            Assert.AreEqual( setName.ToString(), definition.SetName );
            Assert.AreEqual( DefinitionType.RESULTLISTFORMAT, definition.Type );
            Assert.AreEqual( 4, definition.Fields.Count );
            Assert.IsTrue( definition.Format.Display.Body.RowLinkTo.Contains( LinkToOption.ResultCOF ) );
        }

        [TestMethod]
        public void GetAppellationTest() {

            var client = new DefinitionAPIClient(Constants.X_API_KEY) { IgnoreInMemoryCache = true };
            var setName = SetName.Parse("v3.0:ntparc:Three-Position Air Rifle 3x10");

            var taskResponse = client.GetCourseOfFireDefinitionAsync(setName);
            var result = taskResponse.Result;
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode, $"Expecting and OK status code, instead received {result.StatusCode}.");

            var definition = result.Definition;
            var msgResponse = result.MessageResponse;
            EventComposite eventTree = new EventComposite() { };
            eventTree = EventComposite.GrowEventTree(definition);
            
            foreach (var e in eventTree.GetEvents()) {
                if (e.EventType == EventtType.EVENT)
                    Assert.IsNotNull( e.EventStyleMapping );
                else
                    Assert.IsNull( e.EventStyleMapping );

                if (e.EventType == EventtType.STAGE)
					Assert.IsNotNull( e.StageStyleMapping );
				else
					Assert.IsNull( e.StageStyleMapping );
			}

            Assert.IsNotNull(definition);
            Assert.IsNotNull(msgResponse);
        }

        [TestMethod]
        public void GetEventAndStageMappingTest() {

            var client = new DefinitionAPIClient(Constants.X_API_KEY) { IgnoreInMemoryCache = true };
            var mappingSetName = SetName.Parse("v1.0:ntparc:Air Rifle");
            var cofSetName = SetName.Parse("v3.0:ntparc:Three-Position Air Rifle 3x10");

            var mappingResponse = client.GetEventAndStageStyhleMappingDefinitionAsync(mappingSetName);
            var mappingResult = mappingResponse.Result;
            var mapping = mappingResult.Definition;
            Assert.AreEqual(HttpStatusCode.OK, mappingResult.StatusCode, $"Expecting and OK status code, instead received {mappingResult.StatusCode}.");

            var cofResponse = client.GetCourseOfFireDefinitionAsync(cofSetName);
            var cofResult = cofResponse.Result;
            var cofDefinition = cofResult.Definition;
            Assert.AreEqual(HttpStatusCode.OK, cofResult.StatusCode, $"Expecting and OK status code, instead received {cofResult.StatusCode}.");

            EventAndStageStyleMappingCalculation mappingCalc = new EventAndStageStyleMappingCalculation(mapping);
            foreach (var thing in cofDefinition.Events) {
                if (thing.EventType == EventtType.EVENT) {
                    Console.WriteLine("EventAppell: " + thing.EventStyleMapping.EventAppellation.ToString());
                    Console.WriteLine("DefaultDef: " + thing.EventStyleMapping.DefaultDef.ToString());
                    Console.WriteLine("CalcReturn: " + mappingCalc.GetEventStyleDef("Precision", "10m Air Rifle", thing.EventStyleMapping));
                    //Assert.AreEqual("v1.0:nra:Conventional Position 50ft Metallic", mappingCalc.GetEventStyleDef("Conventional Metallic", "50ft Conventional Rifle", thing.EventStyleMapping));
                }
                // I am having issues with this right here, not sure what the heck is up with stage appellation not being passed forward, but it isn't but The DefaultDef is being passed. so IDFK
                if (thing.EventType == EventtType.STAGE) {
                    Console.WriteLine("StageAppell: " + thing.StageStyleMapping.StageAppellation.ToString());
                    Console.WriteLine("DefaultDef: " + thing.StageStyleMapping.DefaultDef.ToString());
                    Console.WriteLine("CalcReturn: " + mappingCalc.GetStageStyleDef("Precision", "10m Air Rifle", thing.StageStyleMapping));
                    //Assert.AreEqual("v1.0:nra:Conventional Position 50ft Metallic", mappingCalc.GetStageStyleDef("Conventional Metallic", "50ft Conventional Rifle", thing.StageStyleMapping));
                }
                Console.WriteLine("");
            }

        }

        [TestMethod]
        public void GetUSASAirRifleMappingTest() {

            var client = new DefinitionAPIClient( Constants.X_API_KEY ) { IgnoreInMemoryCache = true };
            var mappingSetName = SetName.Parse( "v1.0:usas:Air Rifle" );
            var cofSetName = SetName.Parse( "v2.0:usas:Air Rifle Qualification 60 Shots" );

            var mappingResponse = client.GetEventAndStageStyhleMappingDefinitionAsync( mappingSetName );
            var mappingResult = mappingResponse.Result;
            var mapping = mappingResult.Definition;
            Assert.AreEqual( HttpStatusCode.OK, mappingResult.StatusCode, $"Expecting and OK status code, instead received {mappingResult.StatusCode}." );

            var cofResponse = client.GetCourseOfFireDefinitionAsync( cofSetName );
            var cofResult = cofResponse.Result;
            var cofDefinition = cofResult.Definition;
            Assert.AreEqual( HttpStatusCode.OK, cofResult.StatusCode, $"Expecting and OK status code, instead received {cofResult.StatusCode}." );


            EventAndStageStyleMappingCalculation mappingCalc = new EventAndStageStyleMappingCalculation( mapping );
            foreach (var thing in cofDefinition.Events) {
                if (thing.EventType == EventtType.EVENT) {
                    Console.WriteLine( "EventAppell: " + thing.EventStyleMapping.EventAppellation.ToString() );
                    Console.WriteLine( "DefaultDef: " + thing.EventStyleMapping.DefaultDef.ToString() );
                    Console.WriteLine( "CalcReturn: " + mappingCalc.GetEventStyleDef( "SH1", "10m Air Rifle", thing.EventStyleMapping ) );
                    //Assert.AreEqual("v1.0:nra:Conventional Position 50ft Metallic", mappingCalc.GetEventStyleDef("Conventional Metallic", "50ft Conventional Rifle", thing.EventStyleMapping));
                }
                // I am having issues with this right here, not sure what the heck is up with stage appellation not being passed forward, but it isn't but The DefaultDef is being passed. so IDFK
                if (thing.EventType == EventtType.STAGE) {
                    Console.WriteLine( "StageAppell: " + thing.StageStyleMapping.StageAppellation.ToString() );
                    Console.WriteLine( "DefaultDef: " + thing.StageStyleMapping.DefaultDef.ToString() );
                    Console.WriteLine( "CalcReturn: " + mappingCalc.GetStageStyleDef( "SH1", "10m Air Rifle", thing.StageStyleMapping ) );
                    //Assert.AreEqual("v1.0:nra:Conventional Position 50ft Metallic", mappingCalc.GetStageStyleDef("Conventional Metallic", "50ft Conventional Rifle", thing.StageStyleMapping));
                }
                Console.WriteLine( "" );
            }

        }

        /// <summary>
        /// Tests if the GrewEventTree() and GetEvents methods returns a list of Events and Singulars in order as defined by the Course of Fire .Events.
        /// </summary>
        [TestMethod]
        public void DoEventsGrowInOrder() {

            var cof = CourseOfFireHelper.Get_3x20_KPS_Cof();
            var cof2 = CourseOfFireHelper.Get_60_Standing_Cof();
            EventComposite eventTree = EventComposite.GrowEventTree(cof);

            //Should be 1 Event, with name Qualification
            var events = eventTree.GetEvents( false, true, false, false, false, false );
            Assert.IsTrue( events.Count == 1 );
            Assert.AreEqual( "Qualification", events[0].EventName );

            //Should be 3 Stages
            var stages = eventTree.GetEvents( false, false, true, false, false, false );
            Assert.IsTrue( stages.Count == 3 );
            Assert.AreEqual( "Kneeling", stages[0].EventName );
            Assert.AreEqual( "Prone", stages[1].EventName );
            Assert.AreEqual( "Standing", stages[2].EventName );

            //Should be 6 Strings
            var strings = eventTree.GetEvents( false, false, false, false, true, false );
            Assert.IsTrue( strings.Count == 6 );
            Assert.AreEqual( "KN 1", strings[0].EventName );
            Assert.AreEqual( "KN 2", strings[1].EventName );
            Assert.AreEqual( "PR 1", strings[2].EventName );
            Assert.AreEqual( "PR 2", strings[3].EventName );
            Assert.AreEqual( "ST 1", strings[4].EventName );
            Assert.AreEqual( "ST 2", strings[5].EventName );

            //Should be 60 Singulars
            var singulars = eventTree.GetEvents( false, false, false, false, false, true );
            Assert.IsTrue( singulars.Count == 60 );
            for ( int i = 0; i < 20; i++) 
                Assert.AreEqual( $"K{i+1}", singulars[i].EventName );
            for (int i = 0; i < 20; i++)
                Assert.AreEqual( $"P{i + 1}", singulars[i+20].EventName );
            for (int i = 0; i < 20; i++)
                Assert.AreEqual( $"S{i + 1}", singulars[i+40].EventName );

        }

    }
}
