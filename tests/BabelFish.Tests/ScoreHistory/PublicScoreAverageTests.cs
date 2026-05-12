using System.Threading.Tasks;
using Scopos.BabelFish.APIClients;
using Scopos.BabelFish.DataModel.Definitions;
using Scopos.BabelFish.DataModel.ScoreHistory;
using Scopos.BabelFish.Requests.ScoreHistoryAPI;
using Scopos.BabelFish.Responses.ScoreHistoryAPI;

namespace Scopos.BabelFish.Tests.ScoreHistory {

    [TestClass]
    public class PublicScoreAverageTests : BaseTestClass {

        [TestMethod]
        public async Task GetScoreAverageUsingEventStyle() {

            var scoreHistoryClient = new ScoreHistoryAPIClient( APIStage.BETA );

            var scoreAverageRequest = new GetScoreAveragePublicRequest();
            scoreAverageRequest.StartDate = new DateTime( 2023, 04, 1 );
            scoreAverageRequest.EndDate = new DateTime( 2023, 04, 30 );
            scoreAverageRequest.UserIds = new List<string>() { Constants.TestDev7UserId };
            var eventStyleDef = SetName.Parse( "v1.0:ntparc:Three-Position Sporter Air Rifle" );
            var kneelingDef = SetName.Parse( "v1.0:ntparc:Sporter Air Rifle Kneeling" );
            var proneDef = SetName.Parse( "v1.0:ntparc:Sporter Air Rifle Prone" );
            var standingDef = SetName.Parse( "v1.0:ntparc:Sporter Air Rifle Standing" );
            scoreAverageRequest.EventStyleDef = eventStyleDef;
            scoreAverageRequest.Format = ScoreHistoryFormatOptions.WEEK;

            var scoreAverageResponse = await scoreHistoryClient.GetScoreAveragePublicAsync( scoreAverageRequest );

            Assert.AreEqual( System.Net.HttpStatusCode.OK, scoreAverageResponse.RestApiStatusCode );

            bool hasAtLeastOneKneeling = false;
            bool hasAtLeastOneProne = false;
            bool hasAtLeastOneStanding = false;

            foreach (var scoreAverageBase in scoreAverageResponse.ScoreAverageList.Items) {
                if (scoreAverageBase is ScoreAverageStageStyleEntry) {
                    var scoreAverageStageStyle = (ScoreAverageStageStyleEntry)scoreAverageBase;
                    if (scoreAverageStageStyle.StageStyleDef.Equals( kneelingDef ))
                        hasAtLeastOneKneeling = true;
                    if (scoreAverageStageStyle.StageStyleDef.Equals( proneDef ))
                        hasAtLeastOneProne = true;
                    if (scoreAverageStageStyle.StageStyleDef.Equals( standingDef ))
                        hasAtLeastOneStanding = true;
                }
            }

            Assert.IsTrue( hasAtLeastOneKneeling );

            Assert.IsTrue( hasAtLeastOneProne );

            Assert.IsTrue( hasAtLeastOneStanding );
        }

        [TestMethod]
        public async Task GetScoreAverageUsingStageStyle() {

            var scoreHistoryClient = new ScoreHistoryAPIClient( APIStage.BETA );

            var scoreAverageRequest = new GetScoreAveragePublicRequest();
            scoreAverageRequest.StartDate = new DateTime( 2023, 04, 1 );
            scoreAverageRequest.EndDate = new DateTime( 2023, 04, 30 );
            scoreAverageRequest.UserIds = new List<string>() { Constants.TestDev7UserId };
            var kneelingDef = SetName.Parse( "v1.0:ntparc:Sporter Air Rifle Kneeling" );
            var proneDef = SetName.Parse( "v1.0:ntparc:Sporter Air Rifle Prone" );
            var standingDef = SetName.Parse( "v1.0:ntparc:Sporter Air Rifle Standing" );
            scoreAverageRequest.StageStyleDefs = new List<SetName>() {
                kneelingDef,
                proneDef,
                standingDef
            };
            scoreAverageRequest.Format = ScoreHistoryFormatOptions.WEEK;

            var scoreAverageResponse = await scoreHistoryClient.GetScoreAveragePublicAsync( scoreAverageRequest );

            Assert.AreEqual( System.Net.HttpStatusCode.OK, scoreAverageResponse.RestApiStatusCode );

            bool hasAtLeastOneKneeling = false;
            bool hasAtLeastOneProne = false;
            bool hasAtLeastOneStanding = false;

            foreach (var scoreAverageBase in scoreAverageResponse.ScoreAverageList.Items) {
                if (scoreAverageBase is ScoreAverageStageStyleEntry) {
                    var scoreAverageStageStyle = (ScoreAverageStageStyleEntry)scoreAverageBase;
                    if (scoreAverageStageStyle.StageStyleDef.Equals( kneelingDef ))
                        hasAtLeastOneKneeling = true;

                    if (scoreAverageStageStyle.StageStyleDef.Equals( proneDef ))
                        hasAtLeastOneProne = true;

                    if (scoreAverageStageStyle.StageStyleDef.Equals( standingDef ))
                        hasAtLeastOneStanding = true;
                }
            }

            Assert.IsTrue( hasAtLeastOneKneeling );

            Assert.IsTrue( hasAtLeastOneProne );

            Assert.IsTrue( hasAtLeastOneStanding );
        }

        [TestMethod]
        public async Task GetScoreAverageUsingEventStyleWithTokens() {

            var scoreHistoryClient = new ScoreHistoryAPIClient( APIStage.PRODUCTION );

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

                if (scoreAverageResponse.HasMoreItems)
                    scoreAverageRequest = scoreAverageResponse.GetNextRequest();
            } while (scoreAverageResponse.HasMoreItems);
        }

        [TestMethod]
        public async Task Getsomething() {
            var scoreHistoryClient = new ScoreHistoryAPIClient( APIStage.PRODUCTION );

            var scoreHistoryRequest = new GetScoreHistoryPublicRequest();
            scoreHistoryRequest.StartDate = new DateTime( 2023, 05, 25 );
            scoreHistoryRequest.EndDate = new DateTime( 2023, 05, 31 );
            scoreHistoryRequest.UserIds = new List<string>() { "26f32227-d428-41f6-b224-beed7b6e8850" };
            scoreHistoryRequest.Format = ScoreHistoryFormatOptions.DAY;

            var scoreHistoryResponse = await scoreHistoryClient.GetScoreHistoryPublicAsync( scoreHistoryRequest );

            Assert.AreEqual( System.Net.HttpStatusCode.OK, scoreHistoryResponse.RestApiStatusCode );
        }
    }
}
