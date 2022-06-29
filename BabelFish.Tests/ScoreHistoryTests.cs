using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShootersTech;
using ShootersTech.Helpers;
using ShootersTech.DataModel.Definitions;
using ShootersTech.DataModel.Athena;
using ShootersTech.Requests.ScoreHistoryAPI;

namespace ShootersTech.Tests {
    [TestClass]
    public class ScoreHistoryTests
    {
        private static string xApiKey = "wjM7eCb75aa3Okxj4FliXLY0VjHidoE2ei18pdg1";
        private static Dictionary<string, string> clientParams = new Dictionary<string, string>()
        {
            {"UserName", "test_dev_7@shooterstech.net"},
            {"PassWord", "abcd1234"},
        };
        private readonly ScoreHistoryAPIClient _client = new ScoreHistoryAPIClient(xApiKey);
        //private readonly ScoreAPIClient _client = new ScoreAPIClient(xApiKey, clientParams);

        [TestMethod]
        public void EventStyleHistorySingle() {

            GetScoreHistoryRequest requestParameters = new GetScoreHistoryRequest() {
                UserIds = new List<string>() { "28489692-0a61-470e-aed8-c71b9cfbfe6e" },
                StartDate = DateTime.Today.AddDays( -7 ),
                EndDate = DateTime.Today,
                EventStyle = SetName.Parse( "v1.0:ntparc:Three-Position Precision Air Rifle" ),
                IncludeRelated = true
            };

            var response = _client.GetScoreHistoryAsync(requestParameters);
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

            GetScoreHistoryRequest requestParameters = new GetScoreHistoryRequest() {
                UserIds = new List<string>() { "28489692-0a61-470e-aed8-c71b9cfbfe6e",
                    "26f32227-d428-41f6-b224-beed7b6e8850",
                    "6cd811f8-b6be-4adb-998b-acb8caa86035"},
                StartDate = DateTime.Today.AddDays( -7 ),
                EndDate = DateTime.Today,
                EventStyle = SetName.Parse( "v1.0:ntparc:Three-Position Precision Air Rifle" ),
                IncludeRelated = true
            };

            var response = _client.GetScoreHistoryAsync( requestParameters );
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

            GetScoreHistoryRequest requestParameters = new GetScoreHistoryRequest() {
                UserIds = new List<string>() { "28489692-0a61-470e-aed8-c71b9cfbfe6e" },
                StartDate = DateTime.Today.AddDays( -7 ),
                EndDate = DateTime.Today,
                StageStyles = new List<SetName>() { SetName.Parse( "v1.0:ntparc:Precision Air Rifle Standing" ) },
                IncludeRelated = true
            };

            var response = _client.GetScoreHistoryAsync( requestParameters );
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

            GetScoreHistoryRequest requestParameters = new GetScoreHistoryRequest() {
                UserIds = new List<string>() { "28489692-0a61-470e-aed8-c71b9cfbfe6e" },
                StartDate = DateTime.Today.AddDays( -7 ),
                EndDate = DateTime.Today,
                StageStyles = new List<SetName>() { SetName.Parse( "v1.0:ntparc:Precision Air Rifle Standing" ),
                    SetName.Parse( "v1.0:ntparc:Precision Air Rifle Prone" ),
                    SetName.Parse( "v1.0:ntparc:Precision Air Rifle Kneeling" )},
                IncludeRelated = true
            };

            var response = _client.GetScoreHistoryAsync( requestParameters );
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
