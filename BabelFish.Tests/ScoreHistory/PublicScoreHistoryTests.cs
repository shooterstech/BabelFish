using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scopos.BabelFish.APIClients;
using Scopos.BabelFish.DataModel.Definitions;
using Scopos.BabelFish.DataModel.Athena;
using Scopos.BabelFish.Requests.ScoreHistoryAPI;
using Scopos.BabelFish.DataModel.AttributeValue;
using Scopos.BabelFish.DataModel.ScoreHistory;

namespace Scopos.BabelFish.Tests.ScoreHistory {

    [TestClass]
    public class PublicScoreHistoryTests {

        [TestInitialize]
        public void InitializeTest() {
            Scopos.BabelFish.Runtime.Settings.XApiKey = Constants.X_API_KEY;
        }

        /// <summary>
        /// Unit test to confirm the Constructors set the api key and API stage as expected.
        /// </summary>
        [TestMethod]
        public void BasicConstructorTests() {

            var defaultConstructorClient = new ScoreHistoryAPIClient( );
            var apiStageConstructorClient = new ScoreHistoryAPIClient(  APIStage.BETA );

            Assert.AreEqual( APIStage.PRODUCTION, defaultConstructorClient.ApiStage );

            Assert.AreEqual( APIStage.BETA, apiStageConstructorClient.ApiStage );
        }

        [TestMethod]
        public async Task GetEventStyleScoreHistory() {

            var scoreHistoryClient = new ScoreHistoryAPIClient( APIStage.BETA );

            var scoreHistoryRequest = new GetScoreHistoryPublicRequest();
            scoreHistoryRequest.StartDate = new DateTime( 2023, 04, 15 );
            scoreHistoryRequest.EndDate = new DateTime( 2023, 04, 22 );
            scoreHistoryRequest.UserIds = new List<string>() { Constants.TestDev7UserId };
            var eventStyleDef = "v1.0:ntparc:Three-Position Sporter Air Rifle";
            scoreHistoryRequest.EventStyleDef = SetName.Parse( eventStyleDef );

            var scoreHistoryResponse = await scoreHistoryClient.GetScoreHistoryPublicAsync( scoreHistoryRequest );

            Assert.AreEqual( System.Net.HttpStatusCode.OK, scoreHistoryResponse.StatusCode );

            bool hasAtLeastOneEventStyleEntry = false;

            foreach( var scoreHistoryBase in scoreHistoryResponse.ScoreHistoryList.Items ) {
                Assert.IsTrue( scoreHistoryBase.NumberOfShots > 0 );
                if ( scoreHistoryBase is ScoreHistoryEventStyleEntry ) {
                    hasAtLeastOneEventStyleEntry |= true;
                    var scoreHistoryEventStyle = (ScoreHistoryEventStyleEntry) scoreHistoryBase;
                    Assert.AreEqual( eventStyleDef, scoreHistoryEventStyle.EventStyleDef );
                }
            }

            Assert.IsTrue( hasAtLeastOneEventStyleEntry );
        }

        [TestMethod]
        public async Task GetSingleStageStyleScoreHistory() {

            var scoreHistoryClient = new ScoreHistoryAPIClient( APIStage.BETA );

            var scoreHistoryRequest = new GetScoreHistoryPublicRequest();
            scoreHistoryRequest.StartDate = new DateTime( 2023, 04, 15 );
            scoreHistoryRequest.EndDate = new DateTime( 2023, 04, 22 );
            scoreHistoryRequest.UserIds = new List<string>() { Constants.TestDev7UserId };
            var stageStyleDef = "v1.0:ntparc:Sporter Air Rifle Kneeling";
            scoreHistoryRequest.StageStyleDefs = new List<SetName>() { SetName.Parse( stageStyleDef ) };

            var scoreHistoryResponse = await scoreHistoryClient.GetScoreHistoryPublicAsync( scoreHistoryRequest );

            Assert.AreEqual( System.Net.HttpStatusCode.OK, scoreHistoryResponse.StatusCode );

            bool hasAtLeastOneStageStyleEntry = false;

            foreach (var scoreHistoryBase in scoreHistoryResponse.ScoreHistoryList.Items) {
                Assert.IsTrue( scoreHistoryBase.NumberOfShots > 0 );
                if (scoreHistoryBase is ScoreHistoryStageStyleEntry) {
                    hasAtLeastOneStageStyleEntry |= true;
                    var scoreHistoryStageStyle = (ScoreHistoryStageStyleEntry)scoreHistoryBase;
                    Assert.AreEqual( stageStyleDef, scoreHistoryStageStyle.StageStyleDef );
                    Assert.AreEqual( VisibilityOption.PUBLIC, scoreHistoryStageStyle.Visibility );
                }
            }

            Assert.IsTrue( hasAtLeastOneStageStyleEntry );
        }

        [TestMethod]
        public async Task GetMultipleStageStyleScoreHistory() {

            var scoreHistoryClient = new ScoreHistoryAPIClient( APIStage.BETA );

            var scoreHistoryRequest = new GetScoreHistoryPublicRequest();
            scoreHistoryRequest.StartDate = new DateTime( 2023, 04, 15 );
            scoreHistoryRequest.EndDate = new DateTime( 2023, 04, 22 );
            scoreHistoryRequest.UserIds = new List<string>() { Constants.TestDev7UserId };
            const string kneelingDef = "v1.0:ntparc:Sporter Air Rifle Kneeling";
            const string proneDef = "v1.0:ntparc:Sporter Air Rifle Prone";
            const string standingDef = "v1.0:ntparc:Sporter Air Rifle Standing";
            scoreHistoryRequest.StageStyleDefs = new List<SetName>() { 
                SetName.Parse( kneelingDef ),
                SetName.Parse( proneDef ),
                SetName.Parse( standingDef )
            };

            var scoreHistoryResponse = await scoreHistoryClient.GetScoreHistoryPublicAsync( scoreHistoryRequest );

            Assert.AreEqual( System.Net.HttpStatusCode.OK, scoreHistoryResponse.StatusCode );

            bool hasAtLeastOneKneeling = false;
            bool hasAtLeastOneProne = false;
            bool hasAtLeastOneStanding = false;

            foreach (var scoreHistoryBase in scoreHistoryResponse.ScoreHistoryList.Items) {
                Assert.IsTrue( scoreHistoryBase.NumberOfShots > 0 );
                if (scoreHistoryBase is ScoreHistoryStageStyleEntry) {
                    var scoreHistoryStageStyle = (ScoreHistoryStageStyleEntry)scoreHistoryBase;
                    switch (scoreHistoryStageStyle.StageStyleDef) {
                        case kneelingDef:
                            hasAtLeastOneKneeling = true;
                            break;
                        case proneDef:
                            hasAtLeastOneProne = true;
                            break;
                        case standingDef:
                            hasAtLeastOneStanding = true;
                            break;
                        default:
                            break;
                    }
                }
            }

            Assert.IsTrue( hasAtLeastOneKneeling );

            Assert.IsTrue( hasAtLeastOneProne );

            Assert.IsTrue( hasAtLeastOneStanding );
        }

        [TestMethod]
        public async Task GetEventStyleTimespanScoreHistory() {

            var scoreHistoryClient = new ScoreHistoryAPIClient( APIStage.BETA );

            var scoreHistoryRequest = new GetScoreHistoryPublicRequest();
            scoreHistoryRequest.StartDate = new DateTime( 2023, 04, 1 );
            scoreHistoryRequest.EndDate = new DateTime( 2023, 04, 30 );
            scoreHistoryRequest.UserIds = new List<string>() { Constants.TestDev7UserId };
            var eventStyleDef = "v1.0:ntparc:Three-Position Sporter Air Rifle";
            scoreHistoryRequest.EventStyleDef = SetName.Parse( eventStyleDef );
            scoreHistoryRequest.Format = ScoreHistoryFormatOptions.WEEK;

            var scoreHistoryResponse = await scoreHistoryClient.GetScoreHistoryPublicAsync( scoreHistoryRequest );

            Assert.AreEqual( System.Net.HttpStatusCode.OK, scoreHistoryResponse.StatusCode );

            bool hasAtLeastOneEventStyleEntry = false;

            foreach (var scoreHistoryBase in scoreHistoryResponse.ScoreHistoryList.Items) {
                Assert.IsTrue( scoreHistoryBase.NumberOfShots > 0 );
                if (scoreHistoryBase is ScoreHistoryEventStyleTimespan) {
                    hasAtLeastOneEventStyleEntry |= true;
                    var scoreHistoryEventStyle = (ScoreHistoryEventStyleTimespan)scoreHistoryBase;
                    Assert.AreEqual( eventStyleDef, scoreHistoryEventStyle.EventStyleDef );
                }
            }

            Assert.IsTrue( hasAtLeastOneEventStyleEntry );
        }

        [TestMethod]
        public async Task GetMultipleStageStyleTimespanScoreHistory() {

            var scoreHistoryClient = new ScoreHistoryAPIClient( APIStage.BETA );

            var scoreHistoryRequest = new GetScoreHistoryPublicRequest();
            scoreHistoryRequest.StartDate = new DateTime( 2023, 04, 1 );
            scoreHistoryRequest.EndDate = new DateTime( 2023, 04, 30 );
            scoreHistoryRequest.UserIds = new List<string>() { Constants.TestDev7UserId };
            const string kneelingDef = "v1.0:ntparc:Sporter Air Rifle Kneeling";
            const string proneDef = "v1.0:ntparc:Sporter Air Rifle Prone";
            const string standingDef = "v1.0:ntparc:Sporter Air Rifle Standing";
            scoreHistoryRequest.StageStyleDefs = new List<SetName>() {
                SetName.Parse( kneelingDef ),
                SetName.Parse( proneDef ),
                SetName.Parse( standingDef )
            };
            scoreHistoryRequest.Format = ScoreHistoryFormatOptions.WEEK;

            var scoreHistoryResponse = await scoreHistoryClient.GetScoreHistoryPublicAsync( scoreHistoryRequest );

            Assert.AreEqual( System.Net.HttpStatusCode.OK, scoreHistoryResponse.StatusCode );

            bool hasAtLeastOneKneeling = false;
            bool hasAtLeastOneProne = false;
            bool hasAtLeastOneStanding = false;

            foreach (var scoreHistoryBase in scoreHistoryResponse.ScoreHistoryList.Items) {
                Assert.IsTrue( scoreHistoryBase.NumberOfShots > 0 );
                if (scoreHistoryBase is ScoreHistoryStageStyleTimespan) {
                    var scoreHistoryStageStyle = (ScoreHistoryStageStyleTimespan)scoreHistoryBase;
                    switch (scoreHistoryStageStyle.StageStyleDef) {
                        case kneelingDef:
                            hasAtLeastOneKneeling = true;
                            break;
                        case proneDef:
                            hasAtLeastOneProne = true;
                            break;
                        case standingDef:
                            hasAtLeastOneStanding = true;
                            break;
                        default:
                            break;
                    }
                }
            }

            Assert.IsTrue( hasAtLeastOneKneeling );

            Assert.IsTrue( hasAtLeastOneProne );

            Assert.IsTrue( hasAtLeastOneStanding );
        }

    }

}
