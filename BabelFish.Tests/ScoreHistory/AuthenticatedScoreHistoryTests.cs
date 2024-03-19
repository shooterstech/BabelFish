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
using Scopos.BabelFish.DataModel.AttributeValue;
using Newtonsoft.Json.Linq;
using NLog;

namespace Scopos.BabelFish.Tests.ScoreHistory {

    [TestClass]
    public class AuthenticatedScoreHistoryTests
    {

        [TestMethod]
        public async Task PostScoreHistory()
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
            body.Visibility = VisibilityOption.PUBLIC;


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

        [TestMethod]
        public async Task PatchScoreHistory()
        {
            var scoreHistoryClient = new ScoreHistoryAPIClient(Constants.X_API_KEY, APIStage.BETA);

            var userAuthentication = new UserAuthentication(
                Constants.TestDev7Credentials.Username,
                Constants.TestDev7Credentials.Password);
            await userAuthentication.InitializeAsync();

            var postRequest = new PostScoreHistoryRequest(userAuthentication);
            postRequest.ScoreHistoryPost.EventStyleDef = "evstyle";
            var scoreA = new Score();
            var entryA = new PostStageStyleScore("stageStyle_a", scoreA);

            var scoreB = new Score();
            var entryB = new PostStageStyleScore("stageStyle_b", scoreB);

            var scoreC = new Score();
            var entryC = new PostStageStyleScore("stageStyle_c", scoreB);

            postRequest.ScoreHistoryPost.StageScores = new List<PostStageStyleScore> { entryA, entryB, entryC };
            var postResponse = await scoreHistoryClient.PostScoreHistoryAsync(postRequest);
            Assert.AreEqual(System.Net.HttpStatusCode.OK, postResponse.StatusCode);


            var patchRequest = new PatchScoreHistoryRequest(userAuthentication);
            var body = postResponse.ScoreHistoryPost;
            patchRequest.ScoreHistoryPatch = body;
            body.LocalDate = new DateTime(2012, 04, 1);
            body.CourseOfFireDef = "newcofdef";
            body.MatchType = "newPRACTICE";
            body.MatchLocation = "newmosby";
            body.MatchName = "newmatchname";
            body.Visibility = VisibilityOption.PUBLIC;

            entryC.Score.I = 34;

            body.StageScores = new List<PostStageStyleScore> { entryC };

            var patchResponse = await scoreHistoryClient.PatchScoreHistoryAsync(patchRequest);

            Assert.AreEqual(System.Net.HttpStatusCode.OK, patchResponse.StatusCode);

        }

        [TestMethod]
        public async Task DeleteScoreHistory()
        {
            var scoreHistoryClient = new ScoreHistoryAPIClient(Constants.X_API_KEY, APIStage.BETA);

            var userAuthentication = new UserAuthentication(
                Constants.TestDev7Credentials.Username,
                Constants.TestDev7Credentials.Password);
            await userAuthentication.InitializeAsync();

            var postRequest = new PostScoreHistoryRequest(userAuthentication);
            postRequest.ScoreHistoryPost.EventStyleDef = "evstyle";
            var scoreA = new Score();
            var entryA = new PostStageStyleScore("stageStyle_a", scoreA);

            var scoreB = new Score();
            var entryB = new PostStageStyleScore("stageStyle_b", scoreB);

            var scoreC = new Score();
            var entryC = new PostStageStyleScore("stageStyle_c", scoreB);

            postRequest.ScoreHistoryPost.StageScores = new List<PostStageStyleScore> { entryA, entryB, entryC };
            var postResponse = await scoreHistoryClient.PostScoreHistoryAsync(postRequest);
            Assert.AreEqual(System.Net.HttpStatusCode.OK, postResponse.StatusCode);


            var deleteRequest = new DeleteScoreHistoryRequest(userAuthentication);
            deleteRequest.ResultCOFID = postResponse.ScoreHistoryPost.ResultCOFID;

            var deleteResponse = await scoreHistoryClient.DeleteScoreHistoryAsync(deleteRequest);

            Assert.AreEqual(System.Net.HttpStatusCode.OK, deleteResponse.StatusCode);

        }
  

		[TestMethod]
		public async Task GetEventStyleScoreHistory() {

			var scoreHistoryClient = new ScoreHistoryAPIClient( Constants.X_API_KEY, APIStage.BETA );

			var userAuthentication = new UserAuthentication(
				Constants.TestDev7Credentials.Username,
				Constants.TestDev7Credentials.Password );
			await userAuthentication.InitializeAsync();

			var scoreHistoryRequest = new GetScoreHistoryAuthenticatedRequest(userAuthentication);
			scoreHistoryRequest.StartDate = new DateTime( 2023, 04, 15 );
			scoreHistoryRequest.EndDate = new DateTime( 2023, 04, 22 );
			//scoreHistoryRequest.UserIds = new List<string>() { Constants.TestDev7UserId };
			
			//var eventStyleDef = "v1.0:ntparc:Three-Position Sporter Air Rifle";
			//scoreHistoryRequest.EventStyleDef = SetName.Parse( eventStyleDef );

			var scoreHistoryResponse = await scoreHistoryClient.GetScoreHistoryAuthenticatedAsync( scoreHistoryRequest );

			Assert.AreEqual( System.Net.HttpStatusCode.OK, scoreHistoryResponse.StatusCode );

			bool hasAtLeastOneEventStyleEntry = false;

			foreach (var scoreHistoryBase in scoreHistoryResponse.ScoreHistoryList.Items) {
				Assert.IsTrue( scoreHistoryBase.NumberOfShots > 0 );
				if (scoreHistoryBase is ScoreHistoryEventStyleEntry) {
					hasAtLeastOneEventStyleEntry |= true;
					var scoreHistoryEventStyle = (ScoreHistoryEventStyleEntry)scoreHistoryBase;
				}
			}

			Assert.IsTrue( hasAtLeastOneEventStyleEntry );
		}
	}
}
