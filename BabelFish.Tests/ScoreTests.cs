using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BabelFish;
using BabelFish.Helpers;
using BabelFish.DataModel.Definitions;
using ShootersTech.DataModel.Athena;

namespace BabelFish.Tests {
    [TestClass]
    public class ScoreTests
    {
        private static string xApiKey = "wjM7eCb75aa3Okxj4FliXLY0VjHidoE2ei18pdg1";
        private static Dictionary<string, string> clientParams = new Dictionary<string, string>()
        {
            {"UserName", "test_dev_7@shooterstech.net"},
            {"PassWord", "abcd1234"},
        };
        private readonly ScoreAPIClient _client = new ScoreAPIClient(xApiKey);
        //private readonly ScoreAPIClient _client = new ScoreAPIClient(xApiKey, clientParams);

        [TestMethod]
        public void EventStyleHistorySingle() {

            Dictionary<string, List<string>> requestParameters = new Dictionary<string, List<string>>();
            requestParameters.Add("user-id", new List<string>() { { "28489692-0a61-470e-aed8-c71b9cfbfe6e" } });
            requestParameters.Add("start-date", new List<string>() { { "2022-06-19" } });
            requestParameters.Add("end-date", new List<string>() { { "2022-06-25" } });
            requestParameters.Add("event-style-def", new List<string>() { { "v1.0:ntparc:Three-Position Precision Air Rifle" } });
            requestParameters.Add("include-related", new List<string>() { { "true" } });

            var response = _client.GetEventStyleScoreHistoryAsync(requestParameters);
            Assert.IsNotNull(response);

            var taskResult = response.Result;
            var objResponse = taskResult.Score;
            Assert.IsNotNull(objResponse);
            var msgResponse = taskResult.MessageResponse;
            Assert.IsNotNull(msgResponse);
            Assert.IsTrue(msgResponse.Message.Count == 0);
        }

        [TestMethod]
        public void EventStyleHistoryMultiple()
        {

            Dictionary<string, List<string>> requestParameters = new Dictionary<string, List<string>>();
            requestParameters.Add("user-id", new List<string>() { { "26f32227-d428-41f6-b224-beed7b6e8850" }, { "28489692-0a61-470e-aed8-c71b9cfbfe6e" }, { "6cd811f8-b6be-4adb-998b-acb8caa86035" } });
            requestParameters.Add("start-date", new List<string>() { { "2022-06-19" } });
            requestParameters.Add("end-date", new List<string>() { { "2022-06-25" } });
            requestParameters.Add("event-style-def", new List<string>() { { "v1.0:ntparc:Three-Position Precision Air Rifle" } });
            requestParameters.Add("include-related", new List<string>() { { "true" } });

            var response = _client.GetEventStyleScoreHistoryAsync(requestParameters);
            Assert.IsNotNull(response);

            var taskResult = response.Result;
            var objResponse = taskResult.Score;
            Assert.IsNotNull(objResponse);
            var msgResponse = taskResult.MessageResponse;
            Assert.IsNotNull(msgResponse);
            Assert.IsTrue(msgResponse.Message.Count == 0);
        }

        [TestMethod]
        public void StageStyleHistorySingle()
        {

            Dictionary<string, List<string>> requestParameters = new Dictionary<string, List<string>>();
            requestParameters.Add("user-id", new List<string>() { { "28489692-0a61-470e-aed8-c71b9cfbfe6e" } });
            requestParameters.Add("start-date", new List<string>() { { "2022-06-19" } });
            requestParameters.Add("end-date", new List<string>() { { "2022-06-25" } });
            requestParameters.Add("stage-style-def", new List<string>() { { "v1.0:ntparc:Precision Air Rifle Standing" } });
            requestParameters.Add("include-related", new List<string>() { { "true" } });

            var response = _client.GetStageStyleScoreHistoryAsync(requestParameters);
            Assert.IsNotNull(response);

            var taskResult = response.Result;
            var objResponse = taskResult.Score;
            Assert.IsNotNull(objResponse);
            var msgResponse = taskResult.MessageResponse;
            Assert.IsNotNull(msgResponse);
            Assert.IsTrue(msgResponse.Message.Count == 0);
        }

        [TestMethod]
        public void StageStyleHistoryMultiple()
        {

            Dictionary<string, List<string>> requestParameters = new Dictionary<string, List<string>>();
            requestParameters.Add("user-id", new List<string>() { { "28489692-0a61-470e-aed8-c71b9cfbfe6e" } });
            requestParameters.Add("start-date", new List<string>() { { "2022-06-19" } });
            requestParameters.Add("end-date", new List<string>() { { "2022-06-25" } });
            requestParameters.Add("stage-style-def", new List<string>() { { "v1.0:ntparc:Precision Air Rifle Standing" }, { "v1.0:ntparc:Precision Air Rifle Prone" } });
            requestParameters.Add("include-related", new List<string>() { { "true" } });

            var response = _client.GetStageStyleScoreHistoryAsync(requestParameters);
            Assert.IsNotNull(response);

            var taskResult = response.Result;
            var objResponse = taskResult.Score;
            Assert.IsNotNull(objResponse);
            var msgResponse = taskResult.MessageResponse;
            Assert.IsNotNull(msgResponse);
            Assert.IsTrue(msgResponse.Message.Count == 0);
        }

        [TestMethod]
        public void EventStyleAverageSingle()
        {
            Dictionary<string, List<string>> requestParameters = new Dictionary<string, List<string>>();
            requestParameters.Add("user-id", new List<string>() { { "28489692-0a61-470e-aed8-c71b9cfbfe6e" } });
            requestParameters.Add("start-date", new List<string>() { { "2022-06-19" } });
            requestParameters.Add("end-date", new List<string>() { { "2022-06-25" } });
            requestParameters.Add("event-style-def", new List<string>() { { "v1.0:ntparc:Three-Position Precision Air Rifle" } });

            var response = _client.GetEventStyleScoreAverageAsync(requestParameters);
            Assert.IsNotNull(response);

            var taskResult = response.Result;
            var objResponse = taskResult.Score;
            Assert.IsNotNull(objResponse);
            var msgResponse = taskResult.MessageResponse;
            Assert.IsNotNull(msgResponse);
            Assert.IsTrue(msgResponse.Message.Count == 0);
        }
    }
}
