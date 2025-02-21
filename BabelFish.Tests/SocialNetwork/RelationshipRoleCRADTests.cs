using Scopos.BabelFish.APIClients;
using Scopos.BabelFish.DataModel.SocialNetwork;
using Scopos.BabelFish.Requests.ClubsAPI;
using Scopos.BabelFish.Requests.SocialNetworkAPI;
using Scopos.BabelFish.Runtime.Authentication;
using System.Threading.Tasks;

namespace Scopos.BabelFish.Tests.SocialNetwork {
    [TestClass]
    public class RelationshipRoleCRADTests : BaseTestClass {
        private SocialNetworkAPIClient socialNetworkClient;
        private ClubsAPIClient clubsClient; //required to test coach relationship/ designate coaches

        [TestInitialize]
        public override void InitializeTest() {
            base.InitializeTest();

            socialNetworkClient = new SocialNetworkAPIClient( APIStage.PRODTEST );
            clubsClient = new ClubsAPIClient( APIStage.PRODTEST );
        }

        [TestMethod]
        public async Task BadFollowRequest() {
            var userAuthentication = new UserAuthentication(
                Constants.TestDev7Credentials.Username,
                Constants.TestDev7Credentials.Password );
            await userAuthentication.InitializeAsync();

            //user 7 trys to make user 3 follow them and fails
            var createRequestBad = new CreateRelationshipRoleAuthenticatedRequest( userAuthentication );
            createRequestBad.RelationshipName = SocialRelationshipName.FOLLOW;
            createRequestBad.ActiveId = Constants.TestDev3UserId;

            var createResponseBad = await socialNetworkClient.CreateRelationshipRoleAuthenticatedAsync( createRequestBad );

            Assert.AreEqual( System.Net.HttpStatusCode.Unauthorized, createResponseBad.StatusCode );


        }

        [TestMethod]
        public async Task CreateThenDeleteFollowRelationshipRole() {
            var userAuthentication = new UserAuthentication(
                Constants.TestDev7Credentials.Username,
                Constants.TestDev7Credentials.Password );
            await userAuthentication.InitializeAsync();

            //user 7 follows user 3
            var createRequest = new CreateRelationshipRoleAuthenticatedRequest( userAuthentication );
            createRequest.RelationshipName = SocialRelationshipName.FOLLOW;
            createRequest.PassiveId = Constants.TestDev3UserId;

            var createResponse = await socialNetworkClient.CreateRelationshipRoleAuthenticatedAsync( createRequest );

            Assert.AreEqual( System.Net.HttpStatusCode.OK, createResponse.StatusCode );

            //delete the role
            var deleteRequest = new DeleteRelationshipRoleAuthenticatedRequest( userAuthentication );
            deleteRequest.RelationshipName = SocialRelationshipName.FOLLOW;
            deleteRequest.PassiveId = Constants.TestDev3UserId;

            var deleteResponse = await socialNetworkClient.DeleteRelationshipRoleAuthenticatedAsync( deleteRequest );

            Assert.AreEqual( System.Net.HttpStatusCode.OK, deleteResponse.StatusCode );
        }

        [TestMethod]
        public async Task TestBlocks() {
            var user7Authentication = new UserAuthentication(
                Constants.TestDev7Credentials.Username,
                Constants.TestDev7Credentials.Password );
            await user7Authentication.InitializeAsync();

            var user3Authentication = new UserAuthentication(
                Constants.TestDev3Credentials.Username,
                Constants.TestDev3Credentials.Password );
            await user3Authentication.InitializeAsync();

            //ensure blocks relationships doesnt exist already
            var deleteRequest = new DeleteRelationshipRoleAuthenticatedRequest( user7Authentication );
            deleteRequest.RelationshipName = SocialRelationshipName.BLOCK;
            deleteRequest.PassiveId = Constants.TestDev3UserId;

            var deleteResponse = await socialNetworkClient.DeleteRelationshipRoleAuthenticatedAsync( deleteRequest );

            Assert.IsTrue( System.Net.HttpStatusCode.OK == deleteResponse.StatusCode || System.Net.HttpStatusCode.NotFound == deleteResponse.StatusCode );

            deleteRequest = new DeleteRelationshipRoleAuthenticatedRequest( user3Authentication );
            deleteRequest.RelationshipName = SocialRelationshipName.BLOCK;
            deleteRequest.PassiveId = Constants.TestDev7UserId;

            deleteResponse = await socialNetworkClient.DeleteRelationshipRoleAuthenticatedAsync( deleteRequest );

            Assert.IsTrue( System.Net.HttpStatusCode.OK == deleteResponse.StatusCode || System.Net.HttpStatusCode.NotFound == deleteResponse.StatusCode );

            //user 7 can't make user 3 block them
            var blockRequest = new CreateRelationshipRoleAuthenticatedRequest( user7Authentication );
            blockRequest.RelationshipName = SocialRelationshipName.BLOCK;
            blockRequest.ActiveId = Constants.TestDev3UserId;

            var blockResponse = await socialNetworkClient.CreateRelationshipRoleAuthenticatedAsync( blockRequest );

            Assert.AreEqual( System.Net.HttpStatusCode.Unauthorized, blockResponse.StatusCode );

            //user 7 blocks user 3
            blockRequest = new CreateRelationshipRoleAuthenticatedRequest( user7Authentication );
            blockRequest.RelationshipName = SocialRelationshipName.BLOCK;
            blockRequest.PassiveId = Constants.TestDev3UserId;

            blockResponse = await socialNetworkClient.CreateRelationshipRoleAuthenticatedAsync( blockRequest );

            Assert.AreEqual( System.Net.HttpStatusCode.OK, blockResponse.StatusCode );

            //user 7 can read block
            var readRequest = new ReadRelationshipRoleAuthenticatedRequest( user7Authentication );
            readRequest.RelationshipName = SocialRelationshipName.BLOCK;
            readRequest.PassiveId = Constants.TestDev3UserId;

            var readResponse = await socialNetworkClient.ReadRelationshipRoleAuthenticatedAsync( readRequest );

            Assert.AreEqual( System.Net.HttpStatusCode.OK, readResponse.StatusCode );

            //user 3 cannot read block
            readRequest = new ReadRelationshipRoleAuthenticatedRequest( user3Authentication );
            readRequest.RelationshipName = SocialRelationshipName.BLOCK;
            readRequest.ActiveId = Constants.TestDev7UserId;

            readResponse = await socialNetworkClient.ReadRelationshipRoleAuthenticatedAsync( readRequest );

            Assert.AreEqual( System.Net.HttpStatusCode.Unauthorized, readResponse.StatusCode );


            //user 7 can't follow user 3
            var createRequest = new CreateRelationshipRoleAuthenticatedRequest( user7Authentication );
            createRequest.RelationshipName = SocialRelationshipName.FOLLOW;
            createRequest.PassiveId = Constants.TestDev3UserId;

            var createResponse = await socialNetworkClient.CreateRelationshipRoleAuthenticatedAsync( createRequest );

            Assert.AreEqual( System.Net.HttpStatusCode.BadRequest, createResponse.StatusCode );

            //user 7 can't read follows relationship
            readRequest = new ReadRelationshipRoleAuthenticatedRequest( user7Authentication );
            readRequest.RelationshipName = SocialRelationshipName.FOLLOW;
            readRequest.PassiveId = Constants.TestDev3UserId;

            readResponse = await socialNetworkClient.ReadRelationshipRoleAuthenticatedAsync( readRequest );

            Assert.AreEqual( System.Net.HttpStatusCode.BadRequest, readResponse.StatusCode );

            //user 7 can't approve follows relationship
            var approveRequest = new ApproveRelationshipRoleAuthenticatedRequest( user7Authentication );
            approveRequest.RelationshipName = SocialRelationshipName.FOLLOW;
            approveRequest.ActiveId = Constants.TestDev3UserId;
            var approveResponse = await socialNetworkClient.ApproveRelationshipRoleAuthenticatedAsync( approveRequest );

            Assert.AreEqual( System.Net.HttpStatusCode.BadRequest, approveResponse.StatusCode );

            //user 7 can't delete follows relationship
            deleteRequest = new DeleteRelationshipRoleAuthenticatedRequest( user7Authentication );
            deleteRequest.RelationshipName = SocialRelationshipName.FOLLOW;
            deleteRequest.PassiveId = Constants.TestDev3UserId;

            deleteResponse = await socialNetworkClient.DeleteRelationshipRoleAuthenticatedAsync( deleteRequest );

            Assert.AreEqual( System.Net.HttpStatusCode.BadRequest, deleteResponse.StatusCode );

            //now try all operations for user 3 to user 7
            //user 3 can't follow user 7
            createRequest = new CreateRelationshipRoleAuthenticatedRequest( user3Authentication );
            createRequest.RelationshipName = SocialRelationshipName.FOLLOW;
            createRequest.PassiveId = Constants.TestDev7UserId;

            createResponse = await socialNetworkClient.CreateRelationshipRoleAuthenticatedAsync( createRequest );

            Assert.AreEqual( System.Net.HttpStatusCode.BadRequest, createResponse.StatusCode );

            //user 3 can't read follows relationship
            readRequest = new ReadRelationshipRoleAuthenticatedRequest( user3Authentication );
            readRequest.RelationshipName = SocialRelationshipName.FOLLOW;
            readRequest.PassiveId = Constants.TestDev7UserId;

            readResponse = await socialNetworkClient.ReadRelationshipRoleAuthenticatedAsync( readRequest );

            Assert.AreEqual( System.Net.HttpStatusCode.BadRequest, readResponse.StatusCode );

            //user 3 can't approve follows relationship
            approveRequest = new ApproveRelationshipRoleAuthenticatedRequest( user3Authentication );
            approveRequest.RelationshipName = SocialRelationshipName.FOLLOW;
            approveRequest.ActiveId = Constants.TestDev7UserId;
            approveResponse = await socialNetworkClient.ApproveRelationshipRoleAuthenticatedAsync( approveRequest );

            Assert.AreEqual( System.Net.HttpStatusCode.BadRequest, approveResponse.StatusCode );

            //user 3 can't delete follows relationship
            deleteRequest = new DeleteRelationshipRoleAuthenticatedRequest( user3Authentication );
            deleteRequest.RelationshipName = SocialRelationshipName.FOLLOW;
            deleteRequest.PassiveId = Constants.TestDev7UserId;

            deleteResponse = await socialNetworkClient.DeleteRelationshipRoleAuthenticatedAsync( deleteRequest );

            Assert.AreEqual( System.Net.HttpStatusCode.BadRequest, deleteResponse.StatusCode );

            //user 3 can't delete user 7's block
            deleteRequest = new DeleteRelationshipRoleAuthenticatedRequest( user3Authentication );
            deleteRequest.RelationshipName = SocialRelationshipName.BLOCK;
            deleteRequest.ActiveId = Constants.TestDev7UserId;

            deleteResponse = await socialNetworkClient.DeleteRelationshipRoleAuthenticatedAsync( deleteRequest );

            Assert.IsTrue( System.Net.HttpStatusCode.Unauthorized == deleteResponse.StatusCode );

            //user 3 CAN still block user 7
            blockRequest = new CreateRelationshipRoleAuthenticatedRequest( user3Authentication );
            blockRequest.RelationshipName = SocialRelationshipName.BLOCK;
            blockRequest.PassiveId = Constants.TestDev7UserId;

            blockResponse = await socialNetworkClient.CreateRelationshipRoleAuthenticatedAsync( blockRequest );

            Assert.AreEqual( System.Net.HttpStatusCode.OK, blockResponse.StatusCode );

            //user 7 can still read block against user 3
            readRequest = new ReadRelationshipRoleAuthenticatedRequest( user7Authentication );
            readRequest.RelationshipName = SocialRelationshipName.BLOCK;
            readRequest.PassiveId = Constants.TestDev3UserId;

            readResponse = await socialNetworkClient.ReadRelationshipRoleAuthenticatedAsync( readRequest );

            Assert.AreEqual( System.Net.HttpStatusCode.OK, readResponse.StatusCode );



            //delete both blocks
            deleteRequest = new DeleteRelationshipRoleAuthenticatedRequest( user7Authentication );
            deleteRequest.RelationshipName = SocialRelationshipName.BLOCK;
            deleteRequest.PassiveId = Constants.TestDev3UserId;

            deleteResponse = await socialNetworkClient.DeleteRelationshipRoleAuthenticatedAsync( deleteRequest );

            Assert.IsTrue( System.Net.HttpStatusCode.OK == deleteResponse.StatusCode );

            deleteRequest = new DeleteRelationshipRoleAuthenticatedRequest( user3Authentication );
            deleteRequest.RelationshipName = SocialRelationshipName.BLOCK;
            deleteRequest.PassiveId = Constants.TestDev7UserId;

            deleteResponse = await socialNetworkClient.DeleteRelationshipRoleAuthenticatedAsync( deleteRequest );

            Assert.IsTrue( System.Net.HttpStatusCode.OK == deleteResponse.StatusCode );

            //check follow does not exist between either
            readRequest = new ReadRelationshipRoleAuthenticatedRequest( user3Authentication );
            readRequest.RelationshipName = SocialRelationshipName.FOLLOW;
            readRequest.PassiveId = Constants.TestDev7UserId;

            readResponse = await socialNetworkClient.ReadRelationshipRoleAuthenticatedAsync( readRequest );

            Assert.AreEqual( System.Net.HttpStatusCode.NotFound, readResponse.StatusCode );

            readRequest = new ReadRelationshipRoleAuthenticatedRequest( user7Authentication );
            readRequest.RelationshipName = SocialRelationshipName.FOLLOW;
            readRequest.PassiveId = Constants.TestDev3UserId;

            readResponse = await socialNetworkClient.ReadRelationshipRoleAuthenticatedAsync( readRequest );

            Assert.AreEqual( System.Net.HttpStatusCode.NotFound, readResponse.StatusCode );
        }

        [TestMethod]
        public async Task TestFollowThenBlock() {
            var user7Authentication = new UserAuthentication(
                Constants.TestDev7Credentials.Username,
                Constants.TestDev7Credentials.Password );
            await user7Authentication.InitializeAsync();

            var user3Authentication = new UserAuthentication(
                Constants.TestDev3Credentials.Username,
                Constants.TestDev3Credentials.Password );
            await user3Authentication.InitializeAsync();

            //ensure blocks relationships doesnt exist already
            var deleteRequest = new DeleteRelationshipRoleAuthenticatedRequest( user7Authentication );
            deleteRequest.RelationshipName = SocialRelationshipName.BLOCK;
            deleteRequest.PassiveId = Constants.TestDev3UserId;

            var deleteResponse = await socialNetworkClient.DeleteRelationshipRoleAuthenticatedAsync( deleteRequest );

            Assert.IsTrue( System.Net.HttpStatusCode.OK == deleteResponse.StatusCode || System.Net.HttpStatusCode.NotFound == deleteResponse.StatusCode );

            //ensure follow doesnt exist already
            deleteRequest = new DeleteRelationshipRoleAuthenticatedRequest( user7Authentication );
            deleteRequest.RelationshipName = SocialRelationshipName.FOLLOW;
            deleteRequest.PassiveId = Constants.TestDev3UserId;

            deleteResponse = await socialNetworkClient.DeleteRelationshipRoleAuthenticatedAsync( deleteRequest );

            Assert.IsTrue( System.Net.HttpStatusCode.OK == deleteResponse.StatusCode || System.Net.HttpStatusCode.NotFound == deleteResponse.StatusCode );

            //user 7 follows user 3
            var createRequest = new CreateRelationshipRoleAuthenticatedRequest( user7Authentication );
            createRequest.RelationshipName = SocialRelationshipName.FOLLOW;
            createRequest.PassiveId = Constants.TestDev3UserId;

            var createResponse = await socialNetworkClient.CreateRelationshipRoleAuthenticatedAsync( createRequest );
            SocialRelationship followSocialRelationship = createResponse.SocialRelationship; //save to make sure is unchanged
            Assert.AreEqual( System.Net.HttpStatusCode.OK, createResponse.StatusCode );


            //user 7 blocks user 3
            var blockRequest = new CreateRelationshipRoleAuthenticatedRequest( user7Authentication );
            blockRequest.RelationshipName = SocialRelationshipName.BLOCK;
            blockRequest.PassiveId = Constants.TestDev3UserId;

            var blockResponse = await socialNetworkClient.CreateRelationshipRoleAuthenticatedAsync( blockRequest );

            Assert.AreEqual( System.Net.HttpStatusCode.OK, blockResponse.StatusCode );

            //user 3 can't approve follows relationship
            var approveRequest = new ApproveRelationshipRoleAuthenticatedRequest( user3Authentication );
            approveRequest.RelationshipName = SocialRelationshipName.FOLLOW;
            approveRequest.ActiveId = Constants.TestDev7UserId;
            var approveResponse = await socialNetworkClient.ApproveRelationshipRoleAuthenticatedAsync( approveRequest );

            Assert.AreEqual( System.Net.HttpStatusCode.BadRequest, approveResponse.StatusCode );

            //user 7 can't delete follows relationship
            deleteRequest = new DeleteRelationshipRoleAuthenticatedRequest( user7Authentication );
            deleteRequest.RelationshipName = SocialRelationshipName.FOLLOW;
            deleteRequest.PassiveId = Constants.TestDev3UserId;

            deleteResponse = await socialNetworkClient.DeleteRelationshipRoleAuthenticatedAsync( deleteRequest );

            Assert.AreEqual( System.Net.HttpStatusCode.BadRequest, deleteResponse.StatusCode );

            //user 7 unblocks
            deleteRequest = new DeleteRelationshipRoleAuthenticatedRequest( user7Authentication );
            deleteRequest.RelationshipName = SocialRelationshipName.BLOCK;
            deleteRequest.PassiveId = Constants.TestDev3UserId;

            deleteResponse = await socialNetworkClient.DeleteRelationshipRoleAuthenticatedAsync( deleteRequest );

            Assert.IsTrue( System.Net.HttpStatusCode.OK == deleteResponse.StatusCode );

            //user 3 reads relationshipship unchanged
            var readRequest = new ReadRelationshipRoleAuthenticatedRequest( user7Authentication );
            readRequest.RelationshipName = SocialRelationshipName.FOLLOW;
            readRequest.PassiveId = Constants.TestDev3UserId;

            var readResponse = await socialNetworkClient.ReadRelationshipRoleAuthenticatedAsync( readRequest );


            Assert.AreEqual( System.Net.HttpStatusCode.OK, readResponse.StatusCode );

            Assert.IsTrue( readResponse.SocialRelationship.Equals( followSocialRelationship ) );

            //user 7 unfollows
            deleteRequest = new DeleteRelationshipRoleAuthenticatedRequest( user7Authentication );
            deleteRequest.RelationshipName = SocialRelationshipName.FOLLOW;
            deleteRequest.PassiveId = Constants.TestDev3UserId;

            deleteResponse = await socialNetworkClient.DeleteRelationshipRoleAuthenticatedAsync( deleteRequest );

            Assert.IsTrue( System.Net.HttpStatusCode.OK == deleteResponse.StatusCode );

        }


        [TestMethod]
        public async Task TestFollowsProcess() {
            var user7Authentication = new UserAuthentication(
                Constants.TestDev7Credentials.Username,
                Constants.TestDev7Credentials.Password );
            await user7Authentication.InitializeAsync();

            //ensure role doesnt exist already
            var deleteRequest = new DeleteRelationshipRoleAuthenticatedRequest( user7Authentication );
            deleteRequest.RelationshipName = SocialRelationshipName.FOLLOW;
            deleteRequest.PassiveId = Constants.TestDev3UserId;

            var deleteResponse = await socialNetworkClient.DeleteRelationshipRoleAuthenticatedAsync( deleteRequest );

            Assert.IsTrue( System.Net.HttpStatusCode.OK == deleteResponse.StatusCode || System.Net.HttpStatusCode.NotFound == deleteResponse.StatusCode );

            //user 7 requests to follow user 3
            var createRequest = new CreateRelationshipRoleAuthenticatedRequest( user7Authentication );
            createRequest.RelationshipName = SocialRelationshipName.FOLLOW;
            createRequest.PassiveId = Constants.TestDev3UserId;

            var createResponse = await socialNetworkClient.CreateRelationshipRoleAuthenticatedAsync( createRequest );

            Assert.AreEqual( System.Net.HttpStatusCode.OK, createResponse.StatusCode );

            var expectedRelationship = new SocialRelationship();
            expectedRelationship.RelationshipName = createRequest.RelationshipName;
            expectedRelationship.ActiveId = Constants.TestDev7UserId;
            expectedRelationship.PassiveId = createRequest.PassiveId;
            expectedRelationship.ActiveApproved = true;
            expectedRelationship.PassiveApproved = false;
            expectedRelationship.DateCreated = DateTime.Today.ToString( "yyyy-MM-dd" );

            Assert.IsTrue( createResponse.SocialRelationship.Equals( expectedRelationship ) );

            expectedRelationship.PassiveApproved = true;

            //user 7 reads the approved relationship
            var readRequest = new ReadRelationshipRoleAuthenticatedRequest( user7Authentication );
            readRequest.RelationshipName = SocialRelationshipName.FOLLOW;
            readRequest.PassiveId = Constants.TestDev3UserId;

            var readResponse = await socialNetworkClient.ReadRelationshipRoleAuthenticatedAsync( readRequest );

            Assert.AreEqual( System.Net.HttpStatusCode.OK, readResponse.StatusCode );

            Assert.IsTrue( readResponse.SocialRelationship.Equals( expectedRelationship ) );

            //user 7 unfollows user 3
            deleteResponse = await socialNetworkClient.DeleteRelationshipRoleAuthenticatedAsync( deleteRequest );
            Assert.AreEqual( System.Net.HttpStatusCode.OK, deleteResponse.StatusCode );

            deleteResponse = await socialNetworkClient.DeleteRelationshipRoleAuthenticatedAsync( deleteRequest ); //make sure role was actually deleted
            Assert.AreEqual( System.Net.HttpStatusCode.NotFound, deleteResponse.StatusCode );

        }

        [TestMethod]
        public async Task TestFollowsApprovalProcess() {
            return; //CURRENTLY APPROVAL IS NOT REQUIRED FOR FOLLOWING
            var user7Authentication = new UserAuthentication(
                Constants.TestDev7Credentials.Username,
                Constants.TestDev7Credentials.Password );
            await user7Authentication.InitializeAsync();

            //ensure role doesnt exist already
            var deleteRequest = new DeleteRelationshipRoleAuthenticatedRequest( user7Authentication );
            deleteRequest.RelationshipName = SocialRelationshipName.FOLLOW;
            deleteRequest.PassiveId = Constants.TestDev3UserId;

            var deleteResponse = await socialNetworkClient.DeleteRelationshipRoleAuthenticatedAsync( deleteRequest );

            Assert.IsTrue( System.Net.HttpStatusCode.OK == deleteResponse.StatusCode || System.Net.HttpStatusCode.NotFound == deleteResponse.StatusCode );

            //user 7 requests to follow user 3
            var createRequest = new CreateRelationshipRoleAuthenticatedRequest( user7Authentication );
            createRequest.RelationshipName = SocialRelationshipName.FOLLOW;
            createRequest.PassiveId = Constants.TestDev3UserId;

            var createResponse = await socialNetworkClient.CreateRelationshipRoleAuthenticatedAsync( createRequest );

            Assert.AreEqual( System.Net.HttpStatusCode.OK, createResponse.StatusCode );

            var expectedRelationship = new SocialRelationship();
            expectedRelationship.RelationshipName = createRequest.RelationshipName;
            expectedRelationship.ActiveId = Constants.TestDev7UserId;
            expectedRelationship.PassiveId = createRequest.PassiveId;
            expectedRelationship.ActiveApproved = true;
            expectedRelationship.PassiveApproved = false;
            expectedRelationship.DateCreated = DateTime.Today.ToString( "yyyy-MM-dd" );

            Assert.IsTrue( createResponse.SocialRelationship.Equals( expectedRelationship ) );

            //user 3 approves the follow request 
            var user3Authentication = new UserAuthentication(
                Constants.TestDev3Credentials.Username,
                Constants.TestDev3Credentials.Password );
            await user3Authentication.InitializeAsync();

            var approveRequest = new ApproveRelationshipRoleAuthenticatedRequest( user3Authentication );
            approveRequest.RelationshipName = SocialRelationshipName.FOLLOW;
            approveRequest.ActiveId = Constants.TestDev7UserId;

            var approveResponse = await socialNetworkClient.ApproveRelationshipRoleAuthenticatedAsync( approveRequest );

            Assert.AreEqual( System.Net.HttpStatusCode.OK, approveResponse.StatusCode );

            expectedRelationship.PassiveApproved = true;
            Assert.IsTrue( approveResponse.SocialRelationship.Equals( expectedRelationship ) );

            //user 7 reads the approved relationship
            var readRequest = new ReadRelationshipRoleAuthenticatedRequest( user7Authentication );
            readRequest.RelationshipName = SocialRelationshipName.FOLLOW;
            readRequest.PassiveId = Constants.TestDev3UserId;

            var readResponse = await socialNetworkClient.ReadRelationshipRoleAuthenticatedAsync( readRequest );

            Assert.AreEqual( System.Net.HttpStatusCode.OK, readResponse.StatusCode );

            Assert.IsTrue( readResponse.SocialRelationship.Equals( expectedRelationship ) );

            //user 7 unfollows user 3
            deleteResponse = await socialNetworkClient.DeleteRelationshipRoleAuthenticatedAsync( deleteRequest );
            Assert.AreEqual( System.Net.HttpStatusCode.OK, deleteResponse.StatusCode );

            deleteResponse = await socialNetworkClient.DeleteRelationshipRoleAuthenticatedAsync( deleteRequest ); //make sure role was actually deleted
            Assert.AreEqual( System.Net.HttpStatusCode.NotFound, deleteResponse.StatusCode );

        }

        [TestMethod]
        public async Task TestCoachApprovalErrors() {
            /* In this test case, User1 is a POC for Orion license 7 (valid) and 2 (expired)
             *  User 3 is acting as a coach
             *  User 7 is acting as an athlete
             *  
             *  TODO: How to unit test license expiring?
             *  - Cant assign a coach to expired license through CoachAssignmentCRUD
             */
            var pocAuthentication = new UserAuthentication( //license 7 POC
                Constants.TestDev1Credentials.Username,
                Constants.TestDev1Credentials.Password );
            await pocAuthentication.InitializeAsync();

            var athleteAuthentication = new UserAuthentication(
                Constants.TestDev7Credentials.Username,
                Constants.TestDev7Credentials.Password );
            await athleteAuthentication.InitializeAsync();
            var athleteUserId = Constants.TestDev7UserId;

            var coachAuthentication = new UserAuthentication(
                Constants.TestDev3Credentials.Username,
                Constants.TestDev3Credentials.Password );
            await coachAuthentication.InitializeAsync();
            var coachUserId = Constants.TestDev3UserId;

            int validLicense = 7;
            //int expiredLicense = 2; //cant assign a coach to expired license through CoachAssignmentCRUD

            //user 3 should not be a designated coach to start out
            var coachDeleteReq = new DeleteCoachAssignmentAuthenticatedRequest( pocAuthentication );
            coachDeleteReq.LicenseNumber = validLicense;
            coachDeleteReq.UserId.Add( coachUserId );
            var coachDeleteResp = await clubsClient.DeleteCoachAssignmentAuthenticatedAsync( coachDeleteReq );
            Assert.AreEqual( System.Net.HttpStatusCode.OK, coachDeleteResp.StatusCode );
            //coachDeleteReq.LicenseNumber = expiredLicense;
            //coachDeleteResp = await clubsClient.DeleteCoachAssignmentAuthenticatedAsync(coachDeleteReq);
            //Assert.AreEqual(System.Net.HttpStatusCode.OK, coachDeleteResp.StatusCode);

            //ensure role doesnt exist already
            var deleteRequest = new DeleteRelationshipRoleAuthenticatedRequest( athleteAuthentication );
            deleteRequest.RelationshipName = SocialRelationshipName.COACH;
            deleteRequest.ActiveId = coachUserId;
            var deleteResponse = await socialNetworkClient.DeleteRelationshipRoleAuthenticatedAsync( deleteRequest );
            Assert.IsTrue( System.Net.HttpStatusCode.OK == deleteResponse.StatusCode || System.Net.HttpStatusCode.NotFound == deleteResponse.StatusCode );

            //unauthorized coach requests to coach an athlete
            var createRequestFromCoach = new CreateRelationshipRoleAuthenticatedRequest( coachAuthentication );
            createRequestFromCoach.RelationshipName = SocialRelationshipName.COACH;
            createRequestFromCoach.ActiveId = coachUserId;
            createRequestFromCoach.PassiveId = athleteUserId;
            var createResponse = await socialNetworkClient.CreateRelationshipRoleAuthenticatedAsync( createRequestFromCoach );
            Assert.AreEqual( System.Net.HttpStatusCode.Unauthorized, createResponse.StatusCode );

            //athlete requests to be coached by unauthorized coach 
            var createRequestFromAthlete = new CreateRelationshipRoleAuthenticatedRequest( athleteAuthentication );
            createRequestFromAthlete.RelationshipName = SocialRelationshipName.COACH;
            createRequestFromAthlete.ActiveId = coachUserId;
            createRequestFromAthlete.PassiveId = athleteUserId;
            createResponse = await socialNetworkClient.CreateRelationshipRoleAuthenticatedAsync( createRequestFromAthlete );
            Assert.AreEqual( System.Net.HttpStatusCode.Unauthorized, createResponse.StatusCode );

            //add coach to expired license
            //var coachCreateReq = new CreateCoachAssignmentAuthenticatedRequest(pocAuthentication);
            //coachCreateReq.LicenseNumber = expiredLicense;
            //coachCreateReq.UserId.Add(coachUserId);
            //var coachCreateResp = await clubsClient.CreateCoachAssignmentAuthenticatedAsync(coachCreateReq);
            //Assert.AreEqual(System.Net.HttpStatusCode.OK, coachCreateResp.StatusCode);

            ////expired coach requests to coach an athlete
            //createResponse = await socialNetworkClient.CreateRelationshipRoleAuthenticatedAsync(createRequestFromCoach);
            //Assert.AreEqual(System.Net.HttpStatusCode.Unauthorized, createResponse.StatusCode);

            ////athlete requests to be coached by expired coach
            //createResponse = await socialNetworkClient.CreateRelationshipRoleAuthenticatedAsync(createRequestFromAthlete);
            //Assert.AreEqual(System.Net.HttpStatusCode.Unauthorized, createResponse.StatusCode);

            //add coach to valid license (still attached to expired license)
            var coachCreateReq = new CreateCoachAssignmentAuthenticatedRequest( pocAuthentication );
            coachCreateReq.UserId.Add( coachUserId );
            coachCreateReq.LicenseNumber = validLicense;
            var coachCreateResp = await clubsClient.CreateCoachAssignmentAuthenticatedAsync( coachCreateReq );
            Assert.AreEqual( System.Net.HttpStatusCode.OK, coachCreateResp.StatusCode );

            //BEGIN COACH REQUEST ATHLETE TESTS

            //valid coach requests athlete
            createResponse = await socialNetworkClient.CreateRelationshipRoleAuthenticatedAsync( createRequestFromCoach );
            Assert.AreEqual( System.Net.HttpStatusCode.OK, createResponse.StatusCode );

            //coach is removed from valid license (now unauthorized) //(all licenses now expired)
            coachDeleteReq.LicenseNumber = validLicense;
            coachDeleteResp = await clubsClient.DeleteCoachAssignmentAuthenticatedAsync( coachDeleteReq );
            Assert.AreEqual( System.Net.HttpStatusCode.OK, coachDeleteResp.StatusCode );

            //athlete attempts to accept coah request from expired coach
            //var approveRequestFromAthlete = new ApproveRelationshipRoleAuthenticatedRequest(athleteAuthentication);
            //approveRequestFromAthlete.RelationshipName = SocialRelationshipName.COACH;
            //approveRequestFromAthlete.ActiveId = coachUserId;
            //approveRequestFromAthlete.PassiveId = athleteUserId;
            //var approveResponse = await socialNetworkClient.ApproveRelationshipRoleAuthenticatedAsync(approveRequestFromAthlete);
            //Assert.AreEqual(System.Net.HttpStatusCode.Unauthorized, approveResponse.StatusCode);

            ////expired license removed from coach (coach is now not designated under any license)
            //coachDeleteReq.LicenseNumber = expiredLicense;
            //coachDeleteResp = await clubsClient.DeleteCoachAssignmentAuthenticatedAsync(coachDeleteReq);
            //Assert.AreEqual(System.Net.HttpStatusCode.OK, coachDeleteResp.StatusCode);

            //athlete attempts to accept coach request from unauthorized coach
            var approveRequestFromAthlete = new ApproveRelationshipRoleAuthenticatedRequest( athleteAuthentication );
            approveRequestFromAthlete.RelationshipName = SocialRelationshipName.COACH;
            approveRequestFromAthlete.ActiveId = coachUserId;
            approveRequestFromAthlete.PassiveId = athleteUserId;
            var approveResponse = await socialNetworkClient.ApproveRelationshipRoleAuthenticatedAsync( approveRequestFromAthlete );
            Assert.AreEqual( System.Net.HttpStatusCode.Unauthorized, approveResponse.StatusCode );

            //athlete deletes request
            deleteResponse = await socialNetworkClient.DeleteRelationshipRoleAuthenticatedAsync( deleteRequest );
            Assert.IsTrue( System.Net.HttpStatusCode.OK == deleteResponse.StatusCode );

            //BEGIN ATHLETE REQUEST COACH TESTS

            //Add coach to valid license
            coachCreateReq.LicenseNumber = validLicense;
            coachCreateResp = await clubsClient.CreateCoachAssignmentAuthenticatedAsync( coachCreateReq );
            Assert.IsTrue( System.Net.HttpStatusCode.OK == coachCreateResp.StatusCode );

            //athlete requests to coach a valid coach (athlete is not a valid coach)
            createRequestFromAthlete.ActiveId = athleteUserId;
            createRequestFromAthlete.PassiveId = coachUserId;
            createResponse = await socialNetworkClient.CreateRelationshipRoleAuthenticatedAsync( createRequestFromAthlete );
            Assert.AreEqual( System.Net.HttpStatusCode.Unauthorized, createResponse.StatusCode );

            //athlete requests valid coach
            createRequestFromAthlete.ActiveId = coachUserId;
            createRequestFromAthlete.PassiveId = athleteUserId;
            createResponse = await socialNetworkClient.CreateRelationshipRoleAuthenticatedAsync( createRequestFromAthlete );
            Assert.AreEqual( System.Net.HttpStatusCode.OK, createResponse.StatusCode );

            //add coach to expired license
            //coachCreateReq.LicenseNumber = expiredLicense;
            //coachCreateResp = await clubsClient.CreateCoachAssignmentAuthenticatedAsync(coachCreateReq);
            //Assert.IsTrue(System.Net.HttpStatusCode.OK == coachCreateResp.StatusCode);

            //coach is removed from valid license (now unauthorized) //(all licenses now expired)
            coachDeleteReq.LicenseNumber = validLicense;
            coachDeleteResp = await clubsClient.DeleteCoachAssignmentAuthenticatedAsync( coachDeleteReq );
            Assert.AreEqual( System.Net.HttpStatusCode.OK, coachDeleteResp.StatusCode );

            ////expired coach attempts to accept athlete request
            //var approveRequestFromCoach = new ApproveRelationshipRoleAuthenticatedRequest(coachAuthentication);
            //approveRequestFromCoach.RelationshipName = SocialRelationshipName.COACH;
            //approveRequestFromCoach.ActiveId = coachUserId;
            //approveRequestFromCoach.PassiveId = athleteUserId;
            //approveResponse = await socialNetworkClient.ApproveRelationshipRoleAuthenticatedAsync(approveRequestFromCoach);
            //Assert.AreEqual(System.Net.HttpStatusCode.Unauthorized, approveResponse.StatusCode);

            ////expired license removed from expired license (coach is now not designated under any license)
            //coachDeleteReq.LicenseNumber = expiredLicense;
            //coachDeleteResp = await clubsClient.DeleteCoachAssignmentAuthenticatedAsync(coachDeleteReq);
            //Assert.AreEqual(System.Net.HttpStatusCode.OK, coachDeleteResp.StatusCode);

            //unauthorized coach attempts to accept athlete request
            var approveRequestFromCoach = new ApproveRelationshipRoleAuthenticatedRequest( coachAuthentication );
            approveRequestFromCoach.RelationshipName = SocialRelationshipName.COACH;
            approveRequestFromCoach.ActiveId = coachUserId;
            approveRequestFromCoach.PassiveId = athleteUserId;
            approveResponse = await socialNetworkClient.ApproveRelationshipRoleAuthenticatedAsync( approveRequestFromCoach );
            Assert.AreEqual( System.Net.HttpStatusCode.Unauthorized, approveResponse.StatusCode );

            //athlete deletes request
            deleteResponse = await socialNetworkClient.DeleteRelationshipRoleAuthenticatedAsync( deleteRequest );
            Assert.IsTrue( System.Net.HttpStatusCode.OK == deleteResponse.StatusCode );



            //TODO(?): should coach relationship still be able to be read if coach is no longer valid?

        }

        [TestMethod]
        public async Task TestCoachApprovalProcess() {
            var user1Authentication = new UserAuthentication( //license 7 POC
                Constants.TestDev1Credentials.Username,
                Constants.TestDev1Credentials.Password );
            await user1Authentication.InitializeAsync();
            int licenseNumber = 7;

            var user7Authentication = new UserAuthentication(
                Constants.TestDev7Credentials.Username,
                Constants.TestDev7Credentials.Password );
            await user7Authentication.InitializeAsync();

            var user3Authentication = new UserAuthentication(
                Constants.TestDev3Credentials.Username,
                Constants.TestDev3Credentials.Password );
            await user3Authentication.InitializeAsync();


            //ensure user3 is designated coach
            var createCoachReq = new CreateCoachAssignmentAuthenticatedRequest( user1Authentication );
            createCoachReq.LicenseNumber = licenseNumber;
            createCoachReq.UserId.Add( Constants.TestDev3UserId );
            var createCoachResp = await clubsClient.CreateCoachAssignmentAuthenticatedAsync( createCoachReq );
            Assert.AreEqual( System.Net.HttpStatusCode.OK, createCoachResp.StatusCode );

            //ensure role doesnt exist already
            var deleteRequest = new DeleteRelationshipRoleAuthenticatedRequest( user7Authentication );
            deleteRequest.RelationshipName = SocialRelationshipName.COACH;
            deleteRequest.ActiveId = Constants.TestDev3UserId;

            var deleteResponse = await socialNetworkClient.DeleteRelationshipRoleAuthenticatedAsync( deleteRequest );

            Assert.IsTrue( System.Net.HttpStatusCode.OK == deleteResponse.StatusCode || System.Net.HttpStatusCode.NotFound == deleteResponse.StatusCode );


            //athlete (user 7) requests that user3 coaches them
            var createRequest = new CreateRelationshipRoleAuthenticatedRequest( user7Authentication );
            createRequest.RelationshipName = SocialRelationshipName.COACH;
            createRequest.ActiveId = Constants.TestDev3UserId;

            var createResponse = await socialNetworkClient.CreateRelationshipRoleAuthenticatedAsync( createRequest );

            Assert.AreEqual( System.Net.HttpStatusCode.OK, createResponse.StatusCode );

            var expectedRelationship = new SocialRelationship();
            expectedRelationship.RelationshipName = createRequest.RelationshipName;
            expectedRelationship.PassiveId = Constants.TestDev7UserId;
            expectedRelationship.ActiveId = createRequest.ActiveId;
            expectedRelationship.ActiveApproved = false;
            expectedRelationship.PassiveApproved = true;
            expectedRelationship.DateCreated = DateTime.Today.ToString( "yyyy-MM-dd" );

            Assert.IsTrue( createResponse.SocialRelationship.Equals( expectedRelationship ) );

            //user3 attempts to approve the coach request but accidentally puts themself as the passive user
            var approveRequestBad = new ApproveRelationshipRoleAuthenticatedRequest( user3Authentication );
            approveRequestBad.RelationshipName = SocialRelationshipName.COACH;
            approveRequestBad.ActiveId = Constants.TestDev7UserId;

            var approveResponseBad = await socialNetworkClient.ApproveRelationshipRoleAuthenticatedAsync( approveRequestBad );

            Assert.AreEqual( System.Net.HttpStatusCode.NotFound, approveResponseBad.StatusCode );

            //user3 successfully approves the coach request
            var approveRequestGood = new ApproveRelationshipRoleAuthenticatedRequest( user3Authentication );
            approveRequestGood.RelationshipName = SocialRelationshipName.COACH;
            approveRequestGood.PassiveId = Constants.TestDev7UserId;

            var approveResponseGood = await socialNetworkClient.ApproveRelationshipRoleAuthenticatedAsync( approveRequestGood );

            Assert.AreEqual( System.Net.HttpStatusCode.OK, approveResponseGood.StatusCode );

            expectedRelationship.ActiveApproved = true;
            Assert.IsTrue( approveResponseGood.SocialRelationship.Equals( expectedRelationship ) );

            //user 7 reads approved coach relationship
            var readRequest = new ReadRelationshipRoleAuthenticatedRequest( user7Authentication );
            readRequest.RelationshipName = SocialRelationshipName.COACH;
            readRequest.ActiveId = Constants.TestDev3UserId;

            var readResponse = await socialNetworkClient.ReadRelationshipRoleAuthenticatedAsync( readRequest );

            Assert.AreEqual( System.Net.HttpStatusCode.OK, readResponse.StatusCode );

            Assert.IsTrue( readResponse.SocialRelationship.Equals( expectedRelationship ) );

            //user 3 deletes the coaching relationship
            deleteRequest = new DeleteRelationshipRoleAuthenticatedRequest( user3Authentication );
            deleteRequest.RelationshipName = SocialRelationshipName.COACH;
            deleteRequest.PassiveId = Constants.TestDev7UserId;
            deleteResponse = await socialNetworkClient.DeleteRelationshipRoleAuthenticatedAsync( deleteRequest );
            Assert.AreEqual( System.Net.HttpStatusCode.OK, deleteResponse.StatusCode );

            deleteResponse = await socialNetworkClient.DeleteRelationshipRoleAuthenticatedAsync( deleteRequest ); //make sure role was actually deleted
            Assert.AreEqual( System.Net.HttpStatusCode.NotFound, deleteResponse.StatusCode );

        }

    }
}
