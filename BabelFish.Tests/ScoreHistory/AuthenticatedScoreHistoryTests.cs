using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scopos.BabelFish.APIClients;
using Scopos.BabelFish.DataModel.Athena;
using Scopos.BabelFish.DataModel.AttributeValue;
using Scopos.BabelFish.DataModel.Definitions;
using Scopos.BabelFish.DataModel.ScoreHistory;
using Scopos.BabelFish.DataModel.SocialNetwork;
using Scopos.BabelFish.Requests.ClubsAPI;
using Scopos.BabelFish.Requests.ScoreHistoryAPI;
using Scopos.BabelFish.Requests.SocialNetworkAPI;
using Scopos.BabelFish.Runtime.Authentication;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Scopos.BabelFish.Tests.ScoreHistory
{

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
			
			var eventStyleDef = "v1.0:ntparc:Three-Position Sporter Air Rifle";
			scoreHistoryRequest.EventStyleDef = SetName.Parse( eventStyleDef );

			var scoreHistoryResponse = await scoreHistoryClient.GetScoreHistoryAuthenticatedAsync( scoreHistoryRequest );

			Assert.AreEqual( System.Net.HttpStatusCode.OK, scoreHistoryResponse.StatusCode );

			bool hasAtLeastOneEventStyleEntry = false;

			foreach (var scoreHistoryBase in scoreHistoryResponse.ScoreHistory.Items) {
				Assert.IsTrue( scoreHistoryBase.NumberOfShots > 0 );
				if (scoreHistoryBase is ScoreHistoryEventStyleEntry) {
					hasAtLeastOneEventStyleEntry |= true;
					var scoreHistoryEventStyle = (ScoreHistoryEventStyleEntry)scoreHistoryBase;
				}
			}

			Assert.IsTrue( hasAtLeastOneEventStyleEntry );
		}

        [TestMethod]
        public async Task CoachAccessAthleteProtectedData()
        {
            var scoreHistoryClient = new ScoreHistoryAPIClient(Constants.X_API_KEY, APIStage.PRODUCTION);
            var clubsClient = new ClubsAPIClient(Constants.X_API_KEY, APIStage.PRODUCTION);
            var socialNetworkClient = new SocialNetworkAPIClient(Constants.X_API_KEY, APIStage.PRODUCTION);

            //TestDev1 is a POC for licence 7
            var userAuthentication = new UserAuthentication(
                Constants.TestDev1Credentials.Username,
                Constants.TestDev1Credentials.Password);
            await userAuthentication.InitializeAsync();
            int licenseNumber = 7;

            var coachAuthentication = new UserAuthentication( //TestDev3 will act as coach 
                Constants.TestDev3Credentials.Username,
                Constants.TestDev3Credentials.Password);
            await coachAuthentication.InitializeAsync();
            var coachUserId = Constants.TestDev3UserId;

            var athleteAuthentication = new UserAuthentication( //TestDev7 will act as athelete
                Constants.TestDev7Credentials.Username,
                Constants.TestDev7Credentials.Password);
            await athleteAuthentication.InitializeAsync();
            var athleteUserId = Constants.TestDev7UserId;

            //Reset Relationships for test
            //delete coach assignment
            var deleteCoachRequest = new DeleteCoachAssignmentAuthenticatedRequest(userAuthentication);
            deleteCoachRequest.LicenseNumber = licenseNumber;
            deleteCoachRequest.UserId.Add(coachUserId);
            var deleteResponse = await clubsClient.DeleteCoachAssignmentAuthenticatedAsync(deleteCoachRequest);
            Assert.AreEqual(System.Net.HttpStatusCode.OK, deleteResponse.StatusCode);

            //delete coach relationship (deleted by coach)
            var deleteRelationshipRequest = new DeleteRelationshipRoleAuthenticatedRequest(coachAuthentication);
            deleteRelationshipRequest.RelationshipName = SocialRelationshipName.COACH;
            deleteRelationshipRequest.PassiveId = athleteUserId;
            var deleteRelationshipResponse = await socialNetworkClient.DeleteRelationshipRoleAuthenticatedAsync(deleteRelationshipRequest);
            Assert.IsTrue(System.Net.HttpStatusCode.OK == deleteRelationshipResponse.StatusCode || System.Net.HttpStatusCode.NotFound == deleteRelationshipResponse.StatusCode);
            
            //END RESET RELATIONSHIPS

            //TestDev1 assigns TestDev3 as coach
            var postRequest = new CreateCoachAssignmentAuthenticatedRequest(userAuthentication);
            postRequest.LicenseNumber = licenseNumber;
            postRequest.UserId.Add(coachUserId);
            var postResponse = await clubsClient.CreateCoachAssignmentAuthenticatedAsync(postRequest);
            Assert.AreEqual(System.Net.HttpStatusCode.OK, postResponse.StatusCode);

            //coach requests to coach athlete
            var createRequestFromCoach = new CreateRelationshipRoleAuthenticatedRequest(coachAuthentication);
            createRequestFromCoach.RelationshipName = SocialRelationshipName.COACH;
            createRequestFromCoach.ActiveId = coachUserId;
            createRequestFromCoach.PassiveId = athleteUserId;
            var createResponse = await socialNetworkClient.CreateRelationshipRoleAuthenticatedAsync(createRequestFromCoach);
            Assert.AreEqual(System.Net.HttpStatusCode.OK, createResponse.StatusCode);

            //athlete hasn't approved, coach can't access athlete protected scores from history or avgs
            var scoreHistoryRequest = new GetScoreHistoryAuthenticatedRequest(coachAuthentication);
            scoreHistoryRequest.StartDate = new DateTime(2023, 04, 15);
            scoreHistoryRequest.EndDate = new DateTime(2023, 04, 22);
            scoreHistoryRequest.UserIds = new List<string>() { coachUserId, athleteUserId };

            var eventStyleDef = "v1.0:ntparc:Three-Position Sporter Air Rifle";
            scoreHistoryRequest.EventStyleDef = SetName.Parse(eventStyleDef);

            var scoreHistoryResponse = await scoreHistoryClient.GetScoreHistoryAuthenticatedAsync(scoreHistoryRequest);
            Assert.AreEqual(System.Net.HttpStatusCode.Unauthorized, scoreHistoryResponse.StatusCode);

            var scoreAverageRequest = new GetScoreAverageAuthenticatedRequest(coachAuthentication);
            scoreAverageRequest.StartDate = new DateTime(2023, 04, 1);
            scoreAverageRequest.EndDate = new DateTime(2023, 04, 30);
            scoreAverageRequest.UserIds = new List<string>() { athleteUserId };
            const string kneelingDef = "v1.0:ntparc:Sporter Air Rifle Kneeling";
            const string proneDef = "v1.0:ntparc:Sporter Air Rifle Prone";
            const string standingDef = "v1.0:ntparc:Sporter Air Rifle Standing";
            scoreAverageRequest.StageStyleDefs = new List<SetName>() {
                SetName.Parse( kneelingDef ),
                SetName.Parse( proneDef ),
                SetName.Parse( standingDef )
            };
            scoreAverageRequest.Format = ScoreHistoryFormatOptions.DAY;
            var scoreAverageResponse = await scoreHistoryClient.GetScoreAverageAuthenticatedAsync(scoreAverageRequest);
            Assert.AreEqual(System.Net.HttpStatusCode.Unauthorized, scoreAverageResponse.StatusCode);


            //athlete accepts coach request
            var approveRequest = new ApproveRelationshipRoleAuthenticatedRequest(athleteAuthentication);
            approveRequest.RelationshipName = SocialRelationshipName.COACH;
            approveRequest.ActiveId = coachUserId;
            var approveResponse = await socialNetworkClient.ApproveRelationshipRoleAuthenticatedAsync(approveRequest);
            Assert.AreEqual(System.Net.HttpStatusCode.OK, approveResponse.StatusCode);

            //coach can now access athlete's protected scores in addition to their own
            scoreHistoryResponse = await scoreHistoryClient.GetScoreHistoryAuthenticatedAsync(scoreHistoryRequest);
            Assert.AreEqual(System.Net.HttpStatusCode.OK, scoreHistoryResponse.StatusCode);

            //coach can now access protected score averages of athlete
            scoreAverageResponse = await scoreHistoryClient.GetScoreAverageAuthenticatedAsync(scoreAverageRequest);
            Assert.AreEqual(System.Net.HttpStatusCode.OK, scoreAverageResponse.StatusCode);

            //coach is removed as a coach from license
            deleteResponse = await clubsClient.DeleteCoachAssignmentAuthenticatedAsync(deleteCoachRequest);
            Assert.AreEqual(System.Net.HttpStatusCode.OK, deleteResponse.StatusCode);

            //coach can no longer access scores
            scoreHistoryResponse = await scoreHistoryClient.GetScoreHistoryAuthenticatedAsync(scoreHistoryRequest);
            Assert.AreEqual(System.Net.HttpStatusCode.Unauthorized, scoreHistoryResponse.StatusCode);
            scoreAverageResponse = await scoreHistoryClient.GetScoreAverageAuthenticatedAsync(scoreAverageRequest);
            Assert.AreEqual(System.Net.HttpStatusCode.Unauthorized, scoreAverageResponse.StatusCode);

            //re-add coach to license
            postResponse = await clubsClient.CreateCoachAssignmentAuthenticatedAsync(postRequest);
            Assert.AreEqual(System.Net.HttpStatusCode.OK, postResponse.StatusCode);

            //coach can access scores without having to re-request coach relationship
            scoreHistoryResponse = await scoreHistoryClient.GetScoreHistoryAuthenticatedAsync(scoreHistoryRequest);
            Assert.AreEqual(System.Net.HttpStatusCode.OK, scoreHistoryResponse.StatusCode);
            scoreAverageResponse = await scoreHistoryClient.GetScoreAverageAuthenticatedAsync(scoreAverageRequest);
            Assert.AreEqual(System.Net.HttpStatusCode.OK, scoreAverageResponse.StatusCode);

        }


	}
}
