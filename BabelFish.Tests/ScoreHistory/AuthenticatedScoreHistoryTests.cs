using System;
using System.Collections.Generic;
using System.Configuration;
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
using Scopos.BabelFish.DataModel.Athena;
using Newtonsoft.Json;
using Scopos.BabelFish.Runtime.Authentication;
using Scopos.BabelFish.APIClients;
using Scopos.BabelFish.Responses.AttributeValueAPI;
using Newtonsoft.Json.Linq;
using NLog;
namespace Scopos.BabelFish.Tests.ScoreHistory {

    [TestClass]
    public class AuthenticatedScoreHistoryTests
    {

        [TestMethod]
        public async Task GetScoreAverageUsingEventStyle()
        {

            var scoreHistoryClient = new ScoreHistoryAPIClient(Constants.X_API_KEY, APIStage.BETA);

            var userAuthentication = new UserAuthentication(
                Constants.TestDev7Credentials.Username,
                Constants.TestDev7Credentials.Password);
            await userAuthentication.InitializeAsync();

            var eventStyleDef = "v1.0:ntparc:Three-Position Sporter Air Rifle";
            const string kneelingDef = "v1.0:ntparc:Sporter Air Rifle Kneeling";
            const string proneDef = "v1.0:ntparc:Sporter Air Rifle Prone";
            const string standingDef = "v1.0:ntparc:Sporter Air Rifle Standing";



            var postRequest = new PostScoreHistoryRequest(userAuthentication);
            var body = new ScoreHistoryPostEntry();
            postRequest.ScoreHistoryPost = body;

            body.LocalDate = new DateTime(2023, 04, 1);
            body.CourseOfFireDef = "cofdef";
            body.MatchType = "PRACTICE";
            body.MatchLocation = "mosby";
            body.MatchName = "matchname";
            body.EventStyleDef = "evstyledef";


            var scoreA = new Score();
            var entryA = new PostStageStyleScore("stageStyle_a", scoreA);

            var scoreB = new Score();
            var entryB = new PostStageStyleScore("stageStyle_b", scoreB);

            var scoreC = new Score();
            var entryC = new PostStageStyleScore("stageStyle_c", scoreB);

            body.StageScores = new List<PostStageStyleScore> { entryA, entryB, entryC };


            var postResponse = await scoreHistoryClient.PostScoreHistoryAsync(postRequest);

            Assert.AreEqual(System.Net.HttpStatusCode.OK, postResponse.StatusCode);

        }
    }
        
}
