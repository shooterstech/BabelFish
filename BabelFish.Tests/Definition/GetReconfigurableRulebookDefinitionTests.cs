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

namespace Scopos.BabelFish.Tests.Definition {
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

            var client = new DefinitionAPIClient( Constants.X_API_KEY ) { IgnoreLocalCache = true };
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

            var client = new DefinitionAPIClient( Constants.X_API_KEY ) { IgnoreLocalCache = true };
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

            var client = new DefinitionAPIClient( Constants.X_API_KEY ) { IgnoreLocalCache = true };
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

            var client = new DefinitionAPIClient( Constants.X_API_KEY ) { IgnoreLocalCache = true };
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
            Assert.IsTrue(definition.RankingRules.Count > 0 );
        }

        [TestMethod]
        public void GetStageStyleTest()
        {

            var client = new DefinitionAPIClient( Constants.X_API_KEY ) { IgnoreLocalCache = true };
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

            var client = new DefinitionAPIClient( Constants.X_API_KEY ) { IgnoreLocalCache = true };
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

            var client = new DefinitionAPIClient( Constants.X_API_KEY ) { IgnoreLocalCache = true };
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

            var client = new DefinitionAPIClient( Constants.X_API_KEY ) { IgnoreLocalCache = true };
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

    }
}
