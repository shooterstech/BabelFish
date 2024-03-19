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
using Scopos.BabelFish.Responses.ScoreHistoryAPI;
using Scopos.BabelFish.DataModel.ScoreHistory;

namespace Scopos.BabelFish.Tests.ScoreHistory {

    [TestClass]
    public class PublicScoreAverageTests {


        [TestMethod]
        public async Task GetScoreAverageUsingEventStyle() {

            var scoreHistoryClient = new ScoreHistoryAPIClient( Constants.X_API_KEY, APIStage.BETA );

            var scoreAverageRequest = new GetScoreAveragePublicRequest();
            scoreAverageRequest.StartDate = new DateTime( 2023, 04, 1 );
            scoreAverageRequest.EndDate = new DateTime( 2023, 04, 30 );
            scoreAverageRequest.UserIds = new List<string>() { Constants.TestDev7UserId };
            var eventStyleDef = "v1.0:ntparc:Three-Position Sporter Air Rifle";
            const string kneelingDef = "v1.0:ntparc:Sporter Air Rifle Kneeling";
            const string proneDef = "v1.0:ntparc:Sporter Air Rifle Prone";
            const string standingDef = "v1.0:ntparc:Sporter Air Rifle Standing";
            scoreAverageRequest.EventStyleDef = SetName.Parse( eventStyleDef );
            scoreAverageRequest.Format = ScoreHistoryFormatOptions.WEEK;

            var scoreAverageResponse = await scoreHistoryClient.GetScoreAveragePublicAsync( scoreAverageRequest );

            Assert.AreEqual( System.Net.HttpStatusCode.OK, scoreAverageResponse.StatusCode );

            bool hasAtLeastOneKneeling = false;
            bool hasAtLeastOneProne = false;
            bool hasAtLeastOneStanding = false;

            foreach (var scoreAverageBase in scoreAverageResponse.ScoreAverageList.Items) {
                if (scoreAverageBase is ScoreAverageStageStyleEntry) {
                    var scoreAverageStageStyle = (ScoreAverageStageStyleEntry)scoreAverageBase;
                    switch (scoreAverageStageStyle.StageStyleDef) {
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
        public async Task GetScoreAverageUsingStageStyle() {

            var scoreHistoryClient = new ScoreHistoryAPIClient( Constants.X_API_KEY, APIStage.BETA );

            var scoreAverageRequest = new GetScoreAveragePublicRequest();
            scoreAverageRequest.StartDate = new DateTime( 2023, 04, 1 );
            scoreAverageRequest.EndDate = new DateTime( 2023, 04, 30 );
            scoreAverageRequest.UserIds = new List<string>() { Constants.TestDev7UserId };
            const string kneelingDef = "v1.0:ntparc:Sporter Air Rifle Kneeling";
            const string proneDef = "v1.0:ntparc:Sporter Air Rifle Prone";
            const string standingDef = "v1.0:ntparc:Sporter Air Rifle Standing";
            scoreAverageRequest.StageStyleDefs = new List<SetName>() {
                SetName.Parse( kneelingDef ),
                SetName.Parse( proneDef ),
                SetName.Parse( standingDef )
            };
            scoreAverageRequest.Format = ScoreHistoryFormatOptions.WEEK;

            var scoreAverageResponse = await scoreHistoryClient.GetScoreAveragePublicAsync( scoreAverageRequest );

            Assert.AreEqual( System.Net.HttpStatusCode.OK, scoreAverageResponse.StatusCode );

            bool hasAtLeastOneKneeling = false;
            bool hasAtLeastOneProne = false;
            bool hasAtLeastOneStanding = false;

            foreach (var scoreAverageBase in scoreAverageResponse.ScoreAverageList.Items) {
                if (scoreAverageBase is ScoreAverageStageStyleEntry) {
                    var scoreAverageStageStyle = (ScoreAverageStageStyleEntry)scoreAverageBase;
                    switch (scoreAverageStageStyle.StageStyleDef) {
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
        public async Task GetScoreAverageUsingEventStyleWithTokens() {

            var scoreHistoryClient = new ScoreHistoryAPIClient( Constants.X_API_KEY, APIStage.PRODUCTION );

            GetScoreHistoryAbstractRequest scoreAverageRequest = new GetScoreAveragePublicRequest();
            scoreAverageRequest.StartDate = new DateTime( 2023, 05, 1 );
            scoreAverageRequest.EndDate = new DateTime( 2023, 05, 30 );
            scoreAverageRequest.UserIds = new List<string>() { Constants.TestDev7UserId
    };
            var eventStyleDef = "v1.0:ntparc:Three-Position Sporter Air Rifle";
            scoreAverageRequest.EventStyleDef = SetName.Parse( eventStyleDef );
            scoreAverageRequest.Format = ScoreHistoryFormatOptions.DAY;

            GetScoreAverageAbstractResponse scoreAverageResponse;
            List<ScoreAverageBase> myScoreAverages = new List<ScoreAverageBase>();
            bool moreData;
            string lastToken = "";

            do {
                scoreAverageResponse = await scoreHistoryClient.GetScoreAverageAsync( scoreAverageRequest );
                myScoreAverages.AddRange( scoreAverageResponse.ScoreAverageList.Items );
                
                Assert.AreNotEqual( lastToken, scoreAverageResponse.ScoreAverageList.NextToken );
                lastToken = scoreAverageResponse.ScoreAverageList.NextToken;
                
                moreData = !string.IsNullOrEmpty( scoreAverageResponse.ScoreAverageList.NextToken );
                scoreAverageRequest = scoreAverageResponse.GetNextRequest();
            } while (moreData);
        }

        [TestMethod]
        public async Task Getsomething() {
            var scoreHistoryClient = new ScoreHistoryAPIClient( Constants.X_API_KEY, APIStage.PRODUCTION );

            var scoreHistoryRequest = new GetScoreHistoryPublicRequest();
            scoreHistoryRequest.StartDate = new DateTime( 2023, 05, 25 );
            scoreHistoryRequest.EndDate = new DateTime( 2023, 05, 31 );
            scoreHistoryRequest.UserIds = new List<string>() { "26f32227-d428-41f6-b224-beed7b6e8850" };
            scoreHistoryRequest.Format = ScoreHistoryFormatOptions.DAY;

            var scoreHistoryResponse = await scoreHistoryClient.GetScoreHistoryPublicAsync( scoreHistoryRequest );

            Assert.AreEqual( System.Net.HttpStatusCode.OK, scoreHistoryResponse.StatusCode );
        }
    }
}
