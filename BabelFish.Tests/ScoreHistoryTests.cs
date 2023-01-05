using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShootersTech.BabelFish.ScoreHistoryAPI;
using ShootersTech.BabelFish.Helpers;
using ShootersTech.BabelFish.DataModel.Definitions;
using ShootersTech.BabelFish.DataModel.Athena;
using ShootersTech.BabelFish.Requests.ScoreHistoryAPI;

namespace ShootersTech.BabelFish.Tests {
    [TestClass]
    public class ScoreHistoryTests
    {
        private static Dictionary<string, string> clientParams = new Dictionary<string, string>()
        {
            {"UserName", "test_dev_7@shooterstech.net"},
            {"PassWord", "abcd1234"},
        };
        private readonly ScoreHistoryAPIClient _client = new ScoreHistoryAPIClient( Constants.X_API_KEY );
        private readonly ScoreHistoryAPIClient _clientAuthenticated = new ScoreHistoryAPIClient( Constants.X_API_KEY, clientParams);

        // Test users and valid date ranges containing data
        private static string TestUser7 = "26f32227-d428-41f6-b224-beed7b6e8850";
        private static string TestUser9 = "28489692-0a61-470e-aed8-c71b9cfbfe6e";
        private static string TestUser10 = "6cd811f8-b6be-4adb-998b-acb8caa86035";

        private static DateTime StartDateTimeSpan = new DateTime(2022, 06, 01); //DateTime.Today.AddDays( -14 ),
        private static DateTime StartDate = new DateTime(2022, 06, 25); //DateTime.Today.AddDays( -14 ),
        private static DateTime EndDate = new DateTime(2022, 06, 30); //DateTime.Today,


        #region HISTORY
        [TestMethod]
        public void History_EventStyle_Single() {

            GetScoreHistoryRequest requestParameters = new GetScoreHistoryRequest() {
                UserIds = new List<string>() { TestUser9 },
                StartDate = StartDate,
                EndDate = EndDate,
                EventStyle = SetName.Parse( "v1.0:ntparc:Three-Position Precision Air Rifle" ),
            };

            var taskResult = _client.GetScoreHistoryAsync(requestParameters).Result;
            Assert.IsNotNull(taskResult);
            var objResponse = taskResult.ScoreHistory;
            Assert.IsNotNull(objResponse);
            Assert.IsTrue(objResponse.ScoreHistoryList.FirstOrDefault(x => x.UserId == requestParameters.UserIds[0]) != null);

            var msgResponse = taskResult.MessageResponse;
            Assert.IsNotNull(msgResponse);
            Assert.IsTrue(msgResponse.Message.Count == 0);
        }

        [TestMethod]
        public void History_EventStyle_Single_IncludeRelatedFalse() {

            GetScoreHistoryRequest requestParameters = new GetScoreHistoryRequest()
            {
                UserIds = new List<string>() { TestUser9 },
                StartDate = StartDate,
                EndDate = EndDate,
                EventStyle = SetName.Parse("v1.0:ntparc:Three-Position Precision Air Rifle"),
                IncludeRelated = false,
            };

            var taskResult = _client.GetScoreHistoryAsync(requestParameters).Result;
            Assert.IsNotNull(taskResult);
            var objResponse = taskResult.ScoreHistory;
            Assert.IsNotNull(objResponse);
            Assert.IsTrue(objResponse.ScoreHistoryList.FirstOrDefault(x => x.UserId == requestParameters.UserIds[0]) != null);

            var msgResponse = taskResult.MessageResponse;
            Assert.IsNotNull(msgResponse);
            Assert.IsTrue(msgResponse.Message.Count == 0);
        }

        [TestMethod]
        public void History_EventStyle_MultipleUserIds() {

            GetScoreHistoryRequest requestParameters = new GetScoreHistoryRequest() {
                UserIds = new List<string>() { TestUser7, TestUser9, TestUser10},
                StartDate = StartDate,
                EndDate = EndDate,
                EventStyle = SetName.Parse( "v1.0:ntparc:Three-Position Precision Air Rifle" ),
            };

            var taskResult = _client.GetScoreHistoryAsync(requestParameters).Result;
            Assert.IsNotNull(taskResult);
            var objResponse = taskResult.ScoreHistory;
            Assert.IsNotNull(objResponse);
            Assert.IsNotNull(objResponse.ScoreHistoryList.Count > 0);
            
            var msgResponse = taskResult.MessageResponse;
            Assert.IsNotNull(msgResponse);
            Assert.IsTrue(msgResponse.Message.Count == 0);
        }

        [TestMethod]
        public void History_EventStyle_TimeSpan() {

            GetScoreHistoryRequest requestParameters = new GetScoreHistoryRequest()
            {
                UserIds = new List<string>() { TestUser7 },
                StartDate = StartDateTimeSpan,
                EndDate = EndDate,
                EventStyle = SetName.Parse("v1.0:ntparc:Three-Position Precision Air Rifle"),
                Format = ScoreHistoryFormatOptions.MONTH,
            };

            var taskResult = _client.GetScoreHistoryAsync(requestParameters).Result;
            Assert.IsNotNull(taskResult);
            var objResponse = taskResult.ScoreHistory;
            Assert.IsNotNull(objResponse);
            Assert.IsTrue(objResponse.ScoreHistoryList.FirstOrDefault(x => x.UserId == requestParameters.UserIds[0]) != null);

            var msgResponse = taskResult.MessageResponse;
            Assert.IsNotNull(msgResponse);
            Assert.IsTrue(msgResponse.Message.Count == 0);
        }

        [TestMethod]
        public void History_EventStyle_TimeSpan_MultipleUserIds()
        {

            GetScoreHistoryRequest requestParameters = new GetScoreHistoryRequest()
            {
                UserIds = new List<string>() { TestUser7, TestUser9, TestUser10},
                StartDate = StartDate,
                EndDate = EndDate,
                EventStyle = SetName.Parse("v1.0:ntparc:Three-Position Precision Air Rifle"),
                Format = ScoreHistoryFormatOptions.MONTH,
            };

            var taskResult = _client.GetScoreHistoryAsync(requestParameters).Result;
            Assert.IsNotNull(taskResult);
            var objResponse = taskResult.ScoreHistory;
            Assert.IsNotNull(objResponse);
            Assert.IsNotNull(objResponse.ScoreHistoryList.Count > 0);

            var msgResponse = taskResult.MessageResponse;
            Assert.IsNotNull(msgResponse);
            Assert.IsTrue(msgResponse.Message.Count == 0);
        }

        [TestMethod]
        public void History_StageStyle_Single() {

            GetScoreHistoryRequest requestParameters = new GetScoreHistoryRequest() {
                UserIds = new List<string>() { TestUser9 },
                StartDate = StartDate,
                EndDate = EndDate,
                StageStyles = new List<SetName>() { SetName.Parse( "v1.0:ntparc:Precision Air Rifle Standing" ) },
            };

            var taskResult = _client.GetScoreHistoryAsync(requestParameters).Result;
            Assert.IsNotNull(taskResult);
            var objResponse = taskResult.ScoreHistory;
            Assert.IsNotNull(objResponse);
            Assert.IsTrue(objResponse.ScoreHistoryList.FirstOrDefault(x => x.UserId == requestParameters.UserIds[0]) != null);

            var msgResponse = taskResult.MessageResponse;
            Assert.IsNotNull(msgResponse);
            Assert.IsTrue(msgResponse.Message.Count == 0);
        }

        [TestMethod]
        public void History_StageStyle_MultipleStageStyles() {

            GetScoreHistoryRequest requestParameters = new GetScoreHistoryRequest() {
                UserIds = new List<string>() { TestUser9 },
                StartDate = StartDate,
                EndDate = EndDate,
                StageStyles = new List<SetName>() { SetName.Parse( "v1.0:ntparc:Precision Air Rifle Standing" ),
                                                    SetName.Parse( "v1.0:ntparc:Precision Air Rifle Prone" ),
                                                    SetName.Parse( "v1.0:ntparc:Precision Air Rifle Kneeling" )},
            };

            var taskResult = _client.GetScoreHistoryAsync(requestParameters).Result;
            Assert.IsNotNull(taskResult);
            var objResponse = taskResult.ScoreHistory;
            Assert.IsNotNull(objResponse);
            Assert.IsTrue(objResponse.ScoreHistoryList.FirstOrDefault(x => x.UserId == requestParameters.UserIds[0]) != null);

            var msgResponse = taskResult.MessageResponse;
            Assert.IsNotNull(msgResponse);
            Assert.IsTrue(msgResponse.Message.Count == 0);
        }

        [TestMethod]
        public void History_StageStyle_MultipleUserIds() {

            GetScoreHistoryRequest requestParameters = new GetScoreHistoryRequest()
            {
                UserIds = new List<string>() { TestUser7, TestUser9 },
                StartDate = StartDate,
                EndDate = EndDate,
                StageStyles = new List<SetName>() { SetName.Parse( "v1.0:ntparc:Precision Air Rifle Standing" ) },
            };

            var taskResult = _client.GetScoreHistoryAsync(requestParameters).Result;
            Assert.IsNotNull(taskResult);
            var objResponse = taskResult.ScoreHistory;
            Assert.IsNotNull(objResponse);
            Assert.IsNotNull(objResponse.ScoreHistoryList.Count == 2);

            var msgResponse = taskResult.MessageResponse;
            Assert.IsNotNull(msgResponse);
            Assert.IsTrue(msgResponse.Message.Count == 0);
        }

        [TestMethod]
        public void History_StageStyle_MultipleUserIdsAndStageStyles() {

            GetScoreHistoryRequest requestParameters = new GetScoreHistoryRequest()
            {
                UserIds = new List<string>() { TestUser7, TestUser9 },
                StartDate = StartDate,
                EndDate = EndDate,
                StageStyles = new List<SetName>() { SetName.Parse( "v1.0:ntparc:Precision Air Rifle Standing" ),
                                                    SetName.Parse( "v1.0:ntparc:Precision Air Rifle Prone" ),
                                                    SetName.Parse( "v1.0:ntparc:Precision Air Rifle Kneeling" )},
            };

            var taskResult = _client.GetScoreHistoryAsync(requestParameters).Result;
            Assert.IsNotNull(taskResult);
            var objResponse = taskResult.ScoreHistory;
            Assert.IsNotNull(objResponse);
            Assert.IsNotNull(objResponse.ScoreHistoryList.Count > 0);

            var msgResponse = taskResult.MessageResponse;
            Assert.IsNotNull(msgResponse);
            Assert.IsTrue(msgResponse.Message.Count == 0);
        }

        [TestMethod]
        public void History_StageStyle_TimeSpan()
        {

            GetScoreHistoryRequest requestParameters = new GetScoreHistoryRequest()
            {
                UserIds = new List<string>() { TestUser9 },
                StartDate = StartDate,
                EndDate = EndDate,
                StageStyles = new List<SetName>() { SetName.Parse("v1.0:ntparc:Precision Air Rifle Standing") },
                Format = ScoreHistoryFormatOptions.MONTH,
            };

            var taskResult = _client.GetScoreHistoryAsync(requestParameters).Result;
            Assert.IsNotNull(taskResult);
            var objResponse = taskResult.ScoreHistory;
            Assert.IsNotNull(objResponse);
            Assert.IsTrue(objResponse.ScoreHistoryList.FirstOrDefault(x => x.UserId == requestParameters.UserIds[0]) != null);

            var msgResponse = taskResult.MessageResponse;
            Assert.IsNotNull(msgResponse);
            Assert.IsTrue(msgResponse.Message.Count == 0);
        }

        [TestMethod]
        public void ContinuationToken() {

            GetScoreHistoryRequest requestParameters = new GetScoreHistoryRequest()
            {
                UserIds = new List<string>() { TestUser7 },
                StartDate = new DateTime(2022,06,22),
                EndDate = new DateTime(2022,07,06),
                EventStyle = SetName.Parse("v1.0:ntparc:Three-Position Precision Air Rifle"),
                Limit = 2,
                IncludeRelated = true,
            };

            var taskResult = _client.GetScoreHistoryAsync(requestParameters).Result;
            Assert.IsNotNull(taskResult);
            Assert.IsTrue(taskResult.NextToken != "");

            var objResponse = taskResult.ScoreHistory;
            Assert.IsNotNull(objResponse);
            Assert.IsTrue(objResponse.ScoreHistoryList.Count == 2);
            Assert.AreEqual("v1.0:ntparc:Three-Position Precision Air Rifle", 
                ((ShootersTech.BabelFish.DataModel.ScoreHistory.ScoreHistoryEventStyleEntry)objResponse.ScoreHistoryList[0]).EventStyle);

            // Assign Response.ContinuationToken for next set of data
            requestParameters.ContinuationToken = taskResult.NextToken;

            taskResult = _client.GetScoreHistoryAsync(requestParameters).Result;
            Assert.IsNotNull(taskResult);
            Assert.IsTrue(taskResult.NextToken == ""); // All data pulled

            objResponse = taskResult.ScoreHistory;
            Assert.IsNotNull(objResponse);
            Assert.IsTrue(objResponse.ScoreHistoryList.Count == 2);
            Assert.AreEqual("v1.0:ntparc:Precision Air Rifle Prone", 
                ((ShootersTech.BabelFish.DataModel.ScoreHistory.ScoreHistoryStageStyleEntry)objResponse.ScoreHistoryList[0]).StageStyle);
        }
        #endregion HISTORY

        #region AVERAGE
        [TestMethod]
        public void Average_EventStyle_Single() {

            GetScoreAverageRequest requestParameters = new GetScoreAverageRequest()
            {
                UserIds = new List<string>() { TestUser9 },
                StartDate = StartDate,
                EndDate = EndDate,
                EventStyle = SetName.Parse("v1.0:ntparc:Three-Position Precision Air Rifle"),
            };

            var taskResult = _client.GetScoreAverageAsync(requestParameters).Result;
            Assert.IsNotNull(taskResult);
            var objResponse = taskResult.ScoreAverage;
            Assert.IsNotNull(objResponse);
            Assert.IsTrue(objResponse.ScoreAverageList.FirstOrDefault(x => x.UserId == requestParameters.UserIds[0]) != null);

            var msgResponse = taskResult.MessageResponse;
            Assert.IsNotNull(msgResponse);
            Assert.IsTrue(msgResponse.Message.Count == 0);
        }

        [TestMethod]
        public void Average_EventStyle_Single_IncludeRelatedFalse() {

            GetScoreAverageRequest requestParameters = new GetScoreAverageRequest()
            {
                UserIds = new List<string>() { TestUser9 },
                StartDate = StartDate,
                EndDate = EndDate,
                EventStyle = SetName.Parse("v1.0:ntparc:Three-Position Precision Air Rifle"),
                IncludeRelated = false,
            };

            var taskResult = _client.GetScoreAverageAsync(requestParameters).Result;
            Assert.IsNotNull(taskResult);
            var objResponse = taskResult.ScoreAverage;
            Assert.IsNotNull(objResponse);
            Assert.IsTrue(objResponse.ScoreAverageList.FirstOrDefault(x => x.UserId == requestParameters.UserIds[0]) != null);

            var msgResponse = taskResult.MessageResponse;
            Assert.IsNotNull(msgResponse);
            Assert.IsTrue(msgResponse.Message.Count == 0);
        }

        [TestMethod]
        public void Average_EventStyle_MultipleUserIds() {

            GetScoreAverageRequest requestParameters = new GetScoreAverageRequest()
            {
                UserIds = new List<string>() { TestUser7, TestUser9, TestUser10 },
                StartDate = StartDate,
                EndDate = EndDate,
                EventStyle = SetName.Parse("v1.0:ntparc:Three-Position Precision Air Rifle"),
            };

            var taskResult = _client.GetScoreAverageAsync(requestParameters).Result;
            Assert.IsNotNull(taskResult);
            var objResponse = taskResult.ScoreAverage;
            Assert.IsNotNull(objResponse);
            Assert.IsNotNull(objResponse.ScoreAverageList.Count > 0);

            var msgResponse = taskResult.MessageResponse;
            Assert.IsNotNull(msgResponse);
            Assert.IsTrue(msgResponse.Message.Count == 0);
        }

        [TestMethod]
        public void Average_EventStyle_TimeSpan() {

            GetScoreAverageRequest requestParameters = new GetScoreAverageRequest()
            {
                UserIds = new List<string>() { TestUser7 },
                StartDate = StartDateTimeSpan,
                EndDate = EndDate,
                EventStyle = SetName.Parse("v1.0:ntparc:Three-Position Precision Air Rifle"),
                Format = ScoreHistoryFormatOptions.MONTH,
            };

            var taskResult = _client.GetScoreAverageAsync(requestParameters).Result;
            Assert.IsNotNull(taskResult);
            var objResponse = taskResult.ScoreAverage;
            Assert.IsNotNull(objResponse);
            Assert.IsTrue(objResponse.ScoreAverageList.FirstOrDefault(x => x.UserId == requestParameters.UserIds[0]) != null);

            var msgResponse = taskResult.MessageResponse;
            Assert.IsNotNull(msgResponse);
            Assert.IsTrue(msgResponse.Message.Count == 0);
        }

        [TestMethod]
        public void Average_EventStyle_TimeSpan_MultipleUserIds() {

            GetScoreAverageRequest requestParameters = new GetScoreAverageRequest()
            {
                UserIds = new List<string>() { TestUser7, TestUser9, TestUser10 },
                StartDate = StartDateTimeSpan,
                EndDate = EndDate,
                EventStyle = SetName.Parse("v1.0:ntparc:Three-Position Precision Air Rifle"),
                Format= ScoreHistoryFormatOptions.MONTH,
            };

            var taskResult = _client.GetScoreAverageAsync(requestParameters).Result;
            Assert.IsNotNull(taskResult);
            var objResponse = taskResult.ScoreAverage;
            Assert.IsNotNull(objResponse);
            Assert.IsNotNull(objResponse.ScoreAverageList.Count > 0);

            var msgResponse = taskResult.MessageResponse;
            Assert.IsNotNull(msgResponse);
            Assert.IsTrue(msgResponse.Message.Count == 0);
        }

        [TestMethod]
        public void Average_StageStyle_Single() {

            GetScoreAverageRequest requestParameters = new GetScoreAverageRequest()
            {
                UserIds = new List<string>() { TestUser9 },
                StartDate = StartDate,
                EndDate = EndDate,
                StageStyles = new List<SetName>() { SetName.Parse( "v1.0:ntparc:Precision Air Rifle Standing" ) },
            };

            var taskResult = _client.GetScoreAverageAsync(requestParameters).Result;
            Assert.IsNotNull(taskResult);
            var objResponse = taskResult.ScoreAverage;
            Assert.IsNotNull(objResponse);
            Assert.IsTrue(objResponse.ScoreAverageList.FirstOrDefault(x => x.UserId == requestParameters.UserIds[0]) != null);

            var msgResponse = taskResult.MessageResponse;
            Assert.IsNotNull(msgResponse);
            Assert.IsTrue(msgResponse.Message.Count == 0);
        }

        [TestMethod]
        public void Average_StageStyle_TimeSpan() {

            GetScoreAverageRequest requestParameters = new GetScoreAverageRequest()
            {
                UserIds = new List<string>() { TestUser7 },
                StartDate = StartDateTimeSpan,
                EndDate = EndDate,
                StageStyles = new List<SetName>() { SetName.Parse("v1.0:ntparc:Precision Air Rifle Standing") },
                Format = ScoreHistoryFormatOptions.MONTH,
            };

            var taskResult = _client.GetScoreAverageAsync(requestParameters).Result;
            Assert.IsNotNull(taskResult);
            var objResponse = taskResult.ScoreAverage;
            Assert.IsNotNull(objResponse);
            Assert.IsTrue(objResponse.ScoreAverageList.FirstOrDefault(x => x.UserId == requestParameters.UserIds[0]) != null);

            var msgResponse = taskResult.MessageResponse;
            Assert.IsNotNull(msgResponse);
            Assert.IsTrue(msgResponse.Message.Count == 0);
        }

        [TestMethod]
        public void Average_StageStyle_MultipleStageStyles() {

            GetScoreAverageRequest requestParameters = new GetScoreAverageRequest()
            {
                UserIds = new List<string>() { TestUser9 },
                StartDate = StartDate,
                EndDate = EndDate,
                StageStyles = new List<SetName>() { SetName.Parse( "v1.0:ntparc:Precision Air Rifle Standing" ),
                                                    SetName.Parse( "v1.0:ntparc:Precision Air Rifle Prone" ),
                                                    SetName.Parse( "v1.0:ntparc:Precision Air Rifle Kneeling" )
                },
            };

            var taskResult = _client.GetScoreAverageAsync(requestParameters).Result;
            Assert.IsNotNull(taskResult);
            var objResponse = taskResult.ScoreAverage;
            Assert.IsNotNull(objResponse);
            Assert.IsTrue(objResponse.ScoreAverageList.FirstOrDefault(x => x.UserId == requestParameters.UserIds[0]) != null);

            var msgResponse = taskResult.MessageResponse;
            Assert.IsNotNull(msgResponse);
            Assert.IsTrue(msgResponse.Message.Count == 0);
        }

        [TestMethod]
        public void Average_StageStyle_MultipleUserIds() {

            GetScoreAverageRequest requestParameters = new GetScoreAverageRequest()
            {
                UserIds = new List<string>() { TestUser7, TestUser9, TestUser10 },
                StartDate = StartDate,
                EndDate = EndDate,
                StageStyles = new List<SetName>() { SetName.Parse( "v1.0:ntparc:Precision Air Rifle Standing" ) },
            };

            var taskResult = _client.GetScoreAverageAsync(requestParameters).Result;
            Assert.IsNotNull(taskResult);
            var objResponse = taskResult.ScoreAverage;
            Assert.IsNotNull(objResponse);
            Assert.IsNotNull(objResponse.ScoreAverageList.Count > 0);

            var msgResponse = taskResult.MessageResponse;
            Assert.IsNotNull(msgResponse);
            Assert.IsTrue(msgResponse.Message.Count == 0);
        }

        [TestMethod]
        public void Average_StageStyle_MultipleUserIdsAndStageStyles() {

            GetScoreAverageRequest requestParameters = new GetScoreAverageRequest()
            {
                UserIds = new List<string>() { TestUser7, TestUser9, TestUser10 },
                StartDate = StartDate,
                EndDate = EndDate,
                StageStyles = new List<SetName>() { SetName.Parse( "v1.0:ntparc:Precision Air Rifle Standing" ),
                                                    SetName.Parse( "v1.0:ntparc:Precision Air Rifle Prone" ),
                                                    SetName.Parse( "v1.0:ntparc:Precision Air Rifle Kneeling" )
                },
            };

            var taskResult = _client.GetScoreAverageAsync(requestParameters).Result;
            Assert.IsNotNull(taskResult);
            var objResponse = taskResult.ScoreAverage;
            Assert.IsNotNull(objResponse);
            Assert.IsNotNull(objResponse.ScoreAverageList.Count > 0);

            var msgResponse = taskResult.MessageResponse;
            Assert.IsNotNull(msgResponse);
            Assert.IsTrue(msgResponse.Message.Count == 0);
        }
        #endregion AVERAGE

        #region ExpectedErrors
        [TestMethod]
        public void Error_HistoryRequest_AssignEventStyleThenStageStyle() {

            string expectedException = "Can not set both EventStyle and StageStyles.";

            try
            {
                //This should throw an error cannot assigning both EventStyle AND StageStyle in the same request
                GetScoreHistoryRequest requestParameters = new GetScoreHistoryRequest()
                {
                    EventStyle = SetName.Parse("v1.0:ntparc:Three-Position Precision Air Rifle"),
                    StageStyles = new List<SetName>() { SetName.Parse( "v1.0:ntparc:Precision Air Rifle Standing" ),
                                                    SetName.Parse( "v1.0:ntparc:Precision Air Rifle Prone" ),
                                                    SetName.Parse( "v1.0:ntparc:Precision Air Rifle Kneeling" )
                    },
                };
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex.Message.Contains(expectedException));
            }
        }

        [TestMethod]
        public void Error_HistoryRequest_AssignStageStyleThenEventStyle() {

            string expectedException = "Can not set both EventStyle and StageStyles.";

            try
            {
                //This should throw an error cannot assigning both EventStyle AND StageStyle in the same request
                GetScoreHistoryRequest requestParameters = new GetScoreHistoryRequest()
                {
                    StageStyles = new List<SetName>() { SetName.Parse( "v1.0:ntparc:Precision Air Rifle Standing" ),
                                                    SetName.Parse( "v1.0:ntparc:Precision Air Rifle Prone" ),
                                                    SetName.Parse( "v1.0:ntparc:Precision Air Rifle Kneeling" )
                    },
                    EventStyle = SetName.Parse("v1.0:ntparc:Three-Position Precision Air Rifle"),
                };
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex.Message.Contains(expectedException));
            }
        }

        [TestMethod]
        public void Error_AverageRequest_AssignEventStyleThenStageStyle() {

            string expectedException = "Can not set both EventStyle and StageStyles.";

            try
            {
                //This should throw an error cannot assigning both EventStyle AND StageStyle in the same request
                GetScoreAverageRequest requestParameters = new GetScoreAverageRequest()
                {
                    EventStyle = SetName.Parse("v1.0:ntparc:Three-Position Precision Air Rifle"),
                    StageStyles = new List<SetName>() { SetName.Parse( "v1.0:ntparc:Precision Air Rifle Standing" ),
                                                    SetName.Parse( "v1.0:ntparc:Precision Air Rifle Prone" ),
                                                    SetName.Parse( "v1.0:ntparc:Precision Air Rifle Kneeling" )
                    },
                };
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex.Message.Contains(expectedException));
            }
        }

        [TestMethod]
        public void Error_AverageRequest_AssignStageStyleThenEventStyle() {

            string expectedException = "Can not set both EventStyle and StageStyles.";

            try
            {
                //This should throw an error cannot assigning both EventStyle AND StageStyle in the same request
                GetScoreAverageRequest requestParameters = new GetScoreAverageRequest()
                {
                    StageStyles = new List<SetName>() { SetName.Parse( "v1.0:ntparc:Precision Air Rifle Standing" ),
                                                    SetName.Parse( "v1.0:ntparc:Precision Air Rifle Prone" ),
                                                    SetName.Parse( "v1.0:ntparc:Precision Air Rifle Kneeling" )
                    },
                    EventStyle = SetName.Parse("v1.0:ntparc:Three-Position Precision Air Rifle"),
                };
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex.Message.Contains(expectedException));
            }
        }

        [TestMethod]
        public void Error_HistoryRequest_NoAuthentication_NoUserId() {

            string expectedException = "UserIds required for Non-Authenticated request.";

            try
            {
                //This should throw an error calling Public API without UserIds specified
                GetScoreHistoryRequest requestParameters = new GetScoreHistoryRequest()
                {
                    StartDate = new DateTime(2022, 06, 25), //DateTime.Today.AddDays( -14 ),
                    EndDate = new DateTime(2022, 06, 30), //DateTime.Today,
                    EventStyle = SetName.Parse("v1.0:ntparc:Three-Position Precision Air Rifle"),
                    WithAuthentication = false,
                };

                // mock what request.QueryString accessing QueryParameters
                Dictionary<string, List<string>> processedQueryParameters = requestParameters.QueryParameters;

                // not expecting to get here
                Assert.Fail("Expected fault in QueryParameters.");
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex.Message.Contains(expectedException));
            }
        }
        #endregion ExpectedErrors

        #region Authenticated
        [TestMethod]
        public void History_EventStyle_Single_Authenticated() {

            GetScoreHistoryRequest requestParameters = new GetScoreHistoryRequest()
            {
                StartDate = StartDate,
                EndDate = EndDate,
                EventStyle = SetName.Parse("v1.0:ntparc:Three-Position Precision Air Rifle"),
                WithAuthentication = true,
            };

            // Instantiate with AuthenticationTokens then send WithAuthentication=true + no UserIds = returns logged in users data
            var taskResult = _clientAuthenticated.GetScoreHistoryAsync(requestParameters).Result;
            Assert.IsNotNull(taskResult);
            var objResponse = taskResult.ScoreHistory;
            Assert.IsNotNull(objResponse);
            Assert.IsTrue(objResponse.ScoreHistoryList.FirstOrDefault(x => x.UserId == "26f32227-d428-41f6-b224-beed7b6e8850") != null);

            var msgResponse = taskResult.MessageResponse;
            Assert.IsNotNull(msgResponse);
            Assert.IsTrue(msgResponse.Message.Count == 0);
        }
        [TestMethod]
        public void History_EventStyle_Single_Authenticated_WithUserId() {

            GetScoreHistoryRequest requestParameters = new GetScoreHistoryRequest()
            {
                StartDate = StartDate,
                EndDate = EndDate,
                EventStyle = SetName.Parse("v1.0:ntparc:Three-Position Precision Air Rifle"),
                WithAuthentication = true,
                UserIds = new List<string>() { TestUser9 },
            };

            // Instantiate with AuthenticationTokens then send WithAuthentication=true + UserIds = returns requested UserIds in data
            var taskResult = _clientAuthenticated.GetScoreHistoryAsync(requestParameters).Result;
            Assert.IsNotNull(taskResult);
            var objResponse = taskResult.ScoreHistory;
            Assert.IsNotNull(objResponse);
            Assert.IsTrue(objResponse.ScoreHistoryList.FirstOrDefault(x => x.UserId == "28489692-0a61-470e-aed8-c71b9cfbfe6e") != null);

            var msgResponse = taskResult.MessageResponse;
            Assert.IsNotNull(msgResponse);
            Assert.IsTrue(msgResponse.Message.Count == 0);
        }
        #endregion authenticated
    }
}
