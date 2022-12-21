using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShootersTech.BabelFish.DefinitionAPI;
using ShootersTech.BabelFish.Helpers;
using ShootersTech.BabelFish.DataModel.Definitions;

namespace ShootersTech.BabelFish.Tests {
    [TestClass]
    public class GetReconfigurableRulebookDefinitionTests
    {
        private SetName setName;
        private readonly DefinitionAPIClient _client = new DefinitionAPIClient( Constants.X_API_KEY );

        [TestMethod]
        public void GetAttributeAirRifleType() {

            var setName = SetName.Parse("v1.0:ntparc:Three-Position Air Rifle Type");

            var response = _client.GetAttributeDefinitionAsync(setName);
            Assert.IsNotNull(response);

            var taskResult = response.Result;
            var objResponse = taskResult.Definition;
            var msgResponse = taskResult.MessageResponse;

            Assert.IsNotNull(objResponse);
            Assert.IsNotNull(msgResponse);

            Assert.AreEqual(objResponse.SetName, setName.ToString());
            Assert.AreEqual(objResponse.Type, taskResult.DefinitionType);
            Assert.AreEqual(objResponse.Fields.Count, 1);
            Assert.AreEqual(objResponse.Fields[0].FieldName, "Three-Position Air Rifle Type");
        }

        [TestMethod]
        public void GetCourseOfFireType()
        {

            var setName = SetName.Parse("v2.0:ntparc:Three-Position Air Rifle 3x10");

            var response = _client.GetCourseOfFireDefinition(setName);
            Assert.IsNotNull(response);

            var taskResult = response.Result;
            var objResponse = taskResult.Definition;
            var msgResponse = taskResult.MessageResponse;

            Assert.IsNotNull(objResponse);
            Assert.IsNotNull(msgResponse);

            Assert.AreEqual(objResponse.SetName, setName.ToString());
            Assert.AreEqual(objResponse.Type, taskResult.DefinitionType);
            Assert.IsTrue(objResponse.RangeScripts.Count>=1);
            Assert.AreEqual(objResponse.Description, setName.ProperName);
        }

        [TestMethod]
        public void GetEventStyleType()
        {

            var setName = SetName.Parse("v1.0:ntparc:Three-Position Precision Air Rifle");

            var response = _client.GetEventStyleDefinition(setName);
            Assert.IsNotNull(response);

            var taskResult = response.Result;
            var objResponse = taskResult.Definition;
            var msgResponse = taskResult.MessageResponse;

            Assert.IsNotNull(objResponse);
            Assert.IsNotNull(msgResponse);

            Assert.AreEqual(objResponse.SetName, setName.ToString());
            Assert.AreEqual(objResponse.Type, taskResult.DefinitionType);
            Assert.IsTrue(objResponse.StageStyles.Count >= 1);
            Assert.AreEqual(objResponse.Description, setName.ProperName);
        }

        [TestMethod]
        public void GetRankingRuleType()
        {

            var setName = SetName.Parse("v1.0:nra:BB Gun Qualification");

            var response = _client.GetRankingRuleDefinition(setName);
            Assert.IsNotNull(response);

            var taskResult = response.Result;
            var objResponse = taskResult.Definition;
            var msgResponse = taskResult.MessageResponse;

            Assert.IsNotNull(objResponse);
            Assert.IsNotNull(msgResponse);

            Assert.AreEqual(objResponse.SetName, setName.ToString());
            Assert.AreEqual(objResponse.Type, taskResult.DefinitionType);
            Assert.IsTrue(objResponse.RankingRules.Count >= 1);
            Assert.AreEqual(objResponse.HierarchicalName, $"{setName.Namespace}:{setName.ProperName}");
        }

        [TestMethod]
        public void GetStageStyleType()
        {

            var setName = SetName.Parse("v1.0:ntparc:Sporter Air Rifle Standing");

            var response = _client.GetStageStyleDefinition(setName);
            Assert.IsNotNull(response);

            var taskResult = response.Result;
            var objResponse = taskResult.Definition;
            var msgResponse = taskResult.MessageResponse;

            Assert.IsNotNull(objResponse);
            Assert.IsNotNull(msgResponse);

            Assert.AreEqual(objResponse.SetName, setName.ToString());
            Assert.AreEqual(objResponse.Type, taskResult.DefinitionType);
            Assert.IsTrue(objResponse.DisplayScoreFormats.Count >= 1);
            Assert.AreEqual(objResponse.Description, setName.ProperName);
        }

        [TestMethod]
        public void GetTargetCollectionType()
        {

            var setName = SetName.Parse("v1.0:ntparc:Air Rifle");

            var response = _client.GetTargetCollectionDefinition(setName);
            Assert.IsNotNull(response);

            var taskResult = response.Result;
            var objResponse = taskResult.Definition;
            var msgResponse = taskResult.MessageResponse;

            Assert.IsNotNull(objResponse);
            Assert.IsNotNull(msgResponse);

            Assert.AreEqual(objResponse.SetName, setName.ToString());
            Assert.AreEqual(objResponse.Type, taskResult.DefinitionType);
            Assert.IsTrue(objResponse.TargetCollections.Count >= 1);
            Assert.AreEqual(objResponse.HierarchicalName, $"{setName.Namespace}:{setName.ProperName}");
        }

        [TestMethod]
        public void GetTargetType()
        {

            var setName = SetName.Parse("v1.0:issf:10m Air Rifle");

            var response = _client.GetTargetDefinition(setName);
            Assert.IsNotNull(response);

            var taskResult = response.Result;
            var objResponse = taskResult.Definition;
            var msgResponse = taskResult.MessageResponse;

            Assert.IsNotNull(objResponse);
            Assert.IsNotNull(msgResponse);

            Assert.AreEqual(objResponse.SetName, setName.ToString());
            Assert.AreEqual(objResponse.Type, taskResult.DefinitionType);
            Assert.IsTrue(objResponse.ScoringRings.Count >= 1);
            Assert.AreEqual(objResponse.HierarchicalName, $"{setName.Namespace}:{setName.ProperName}");
        }

        [TestMethod]
        public void GetScoreFormatCollectionType()
        {

            var setName = SetName.Parse("v1.0:orion:Standard Score Formats");

            var response = _client.GetScoreFormatCollectionDefinition(setName);
            Assert.IsNotNull(response);

            var taskResult = response.Result;
            var objResponse = taskResult.Definition;
            var msgResponse = taskResult.MessageResponse;

            Assert.IsNotNull(objResponse);
            Assert.IsNotNull(msgResponse);

            Assert.AreEqual(objResponse.SetName, setName.ToString());
            Assert.AreEqual(objResponse.Type, taskResult.DefinitionType);
            Assert.IsTrue(objResponse.ScoreFormats.Count >= 1);
            Assert.AreEqual(objResponse.HierarchicalName, $"{setName.Namespace}:{setName.ProperName}");
        }

        //[TestMethod]
        public void GetResultListFormatType()
        {
            //API fetch needs built out
            var setName = SetName.Parse("v1.0:orion:Standard Score Formats");

            var response = _client.GetResultListFormatCollection(setName);
            Assert.IsNotNull(response);

            var taskResult = response.Result;
            var objResponse = taskResult.Definition;
            var msgResponse = taskResult.MessageResponse;

            Assert.IsNotNull(objResponse);
            Assert.IsNotNull(msgResponse);

            //Assert.AreEqual(objResponse.SetName, setName.ToString());
            //Assert.AreEqual(objResponse.Type, taskResult.DefinitionType);
            //Assert.IsTrue(objResponse.ScoreFormats.Count >= 1);
            //Assert.AreEqual(objResponse.Description, setName.ProperName);
        }

    }
}
