using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scopos.BabelFish.APIClients;
using Scopos.BabelFish.DataModel.SocialNetwork;
using Scopos.BabelFish.Requests.SocialNetworkAPI;
using Scopos.BabelFish.Runtime.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scopos.BabelFish.Tests.SocialNetwork
{
    [TestClass]
    public class RelationshipRoleCRADTests
    {
        private SocialNetworkAPIClient socialNetworkClient;

        
        [TestInitialize] 
        public void InitClient() {
            socialNetworkClient = new SocialNetworkAPIClient(Constants.X_API_KEY, APIStage.BETA);
        }

        [TestMethod]
        public async Task BadFollowRequest()
        {
            var userAuthentication = new UserAuthentication(
                Constants.TestDev7Credentials.Username,
                Constants.TestDev7Credentials.Password);
            await userAuthentication.InitializeAsync();

            //user 7 trys to make user 3 follow them and fails
            var createRequestBad = new CreateRelationshipRoleAuthenticatedRequest(userAuthentication);
            createRequestBad.RelationshipName = SocialRelationshipName.FOLLOW;
            createRequestBad.ActiveId = Constants.TestDev3UserId;

            var createResponseBad = await socialNetworkClient.CreateRelationshipRoleAuthenticatedAsync(createRequestBad);

            Assert.AreEqual(System.Net.HttpStatusCode.Unauthorized, createResponseBad.StatusCode);


        }

        [TestMethod]
        public async Task CreateThenDeleteFollowRelationshipRole()
        {
            var userAuthentication = new UserAuthentication(
                Constants.TestDev7Credentials.Username,
                Constants.TestDev7Credentials.Password);
            await userAuthentication.InitializeAsync();

            //user 7 follows user 3
            var createRequest = new CreateRelationshipRoleAuthenticatedRequest(userAuthentication);
            createRequest.RelationshipName = SocialRelationshipName.FOLLOW; 
            createRequest.PassiveId = Constants.TestDev3UserId;

            var createResponse = await socialNetworkClient.CreateRelationshipRoleAuthenticatedAsync(createRequest);
            
            Assert.AreEqual(System.Net.HttpStatusCode.OK, createResponse.StatusCode);

            //delete the role
            var deleteRequest = new DeleteRelationshipRoleAuthenticatedRequest(userAuthentication);
            deleteRequest.RelationshipName = SocialRelationshipName.FOLLOW;
            deleteRequest.PassiveId = Constants.TestDev3UserId;

            var deleteResponse = await socialNetworkClient.DeleteRelationshipRoleAuthenticatedAsync(deleteRequest);

            Assert.AreEqual(System.Net.HttpStatusCode.OK, deleteResponse.StatusCode);
        }

        [TestMethod]
        public async Task TestBlocks()
        {
            var user7Authentication = new UserAuthentication(
                Constants.TestDev7Credentials.Username,
                Constants.TestDev7Credentials.Password);
            await user7Authentication.InitializeAsync();

            var user3Authentication = new UserAuthentication(
                Constants.TestDev3Credentials.Username,
                Constants.TestDev3Credentials.Password);
            await user3Authentication.InitializeAsync();

            //ensure blocks relationships doesnt exist already
            var deleteRequest = new DeleteRelationshipRoleAuthenticatedRequest(user7Authentication);
            deleteRequest.RelationshipName = SocialRelationshipName.BLOCK;
            deleteRequest.PassiveId = Constants.TestDev3UserId;

            var deleteResponse = await socialNetworkClient.DeleteRelationshipRoleAuthenticatedAsync(deleteRequest);

            Assert.IsTrue(System.Net.HttpStatusCode.OK == deleteResponse.StatusCode || System.Net.HttpStatusCode.NotFound == deleteResponse.StatusCode);

             deleteRequest = new DeleteRelationshipRoleAuthenticatedRequest(user3Authentication);
            deleteRequest.RelationshipName = SocialRelationshipName.BLOCK;
            deleteRequest.PassiveId = Constants.TestDev7UserId;

             deleteResponse = await socialNetworkClient.DeleteRelationshipRoleAuthenticatedAsync(deleteRequest);

            Assert.IsTrue(System.Net.HttpStatusCode.OK == deleteResponse.StatusCode || System.Net.HttpStatusCode.NotFound == deleteResponse.StatusCode);

            //user 7 can't make user 3 block them
            var blockRequest = new CreateRelationshipRoleAuthenticatedRequest(user7Authentication);
            blockRequest.RelationshipName = SocialRelationshipName.BLOCK;
            blockRequest.ActiveId = Constants.TestDev3UserId;

            var blockResponse = await socialNetworkClient.CreateRelationshipRoleAuthenticatedAsync(blockRequest);

            Assert.AreEqual(System.Net.HttpStatusCode.Unauthorized, blockResponse.StatusCode);

            //user 7 blocks user 3
             blockRequest = new CreateRelationshipRoleAuthenticatedRequest(user7Authentication);
            blockRequest.RelationshipName = SocialRelationshipName.BLOCK;
            blockRequest.PassiveId = Constants.TestDev3UserId;

             blockResponse = await socialNetworkClient.CreateRelationshipRoleAuthenticatedAsync(blockRequest);

            Assert.AreEqual(System.Net.HttpStatusCode.OK, blockResponse.StatusCode);

            //user 7 can read block
            var readRequest = new ReadRelationshipRoleAuthenticatedRequest(user7Authentication);
            readRequest.RelationshipName = SocialRelationshipName.BLOCK;
            readRequest.PassiveId = Constants.TestDev3UserId;

            var readResponse = await socialNetworkClient.ReadRelationshipRoleAuthenticatedAsync(readRequest);

            Assert.AreEqual(System.Net.HttpStatusCode.OK, readResponse.StatusCode);

            //user 3 cannot read block
            readRequest = new ReadRelationshipRoleAuthenticatedRequest(user3Authentication);
            readRequest.RelationshipName = SocialRelationshipName.BLOCK;
            readRequest.ActiveId = Constants.TestDev7UserId;

            readResponse = await socialNetworkClient.ReadRelationshipRoleAuthenticatedAsync(readRequest);

            Assert.AreEqual(System.Net.HttpStatusCode.Unauthorized, readResponse.StatusCode);


            //user 7 can't follow user 3
            var createRequest = new CreateRelationshipRoleAuthenticatedRequest(user7Authentication);
            createRequest.RelationshipName = SocialRelationshipName.FOLLOW;
            createRequest.PassiveId = Constants.TestDev3UserId;

            var createResponse = await socialNetworkClient.CreateRelationshipRoleAuthenticatedAsync(createRequest);

            Assert.AreEqual(System.Net.HttpStatusCode.BadRequest, createResponse.StatusCode);

            //user 7 can't read follows relationship
             readRequest = new ReadRelationshipRoleAuthenticatedRequest(user7Authentication);
            readRequest.RelationshipName = SocialRelationshipName.FOLLOW;
            readRequest.PassiveId = Constants.TestDev3UserId;

             readResponse = await socialNetworkClient.ReadRelationshipRoleAuthenticatedAsync(readRequest);

            Assert.AreEqual(System.Net.HttpStatusCode.BadRequest, readResponse.StatusCode);

            //user 7 can't approve follows relationship
            var approveRequest = new ApproveRelationshipRoleAuthenticatedRequest(user7Authentication);
            approveRequest.RelationshipName = SocialRelationshipName.FOLLOW;
            approveRequest.ActiveId = Constants.TestDev3UserId;
            var approveResponse = await socialNetworkClient.ApproveRelationshipRoleAuthenticatedAsync(approveRequest);

            Assert.AreEqual(System.Net.HttpStatusCode.BadRequest, approveResponse.StatusCode);

            //user 7 can't delete follows relationship
            deleteRequest = new DeleteRelationshipRoleAuthenticatedRequest(user7Authentication);
            deleteRequest.RelationshipName = SocialRelationshipName.FOLLOW;
            deleteRequest.PassiveId = Constants.TestDev3UserId;

            deleteResponse = await socialNetworkClient.DeleteRelationshipRoleAuthenticatedAsync(deleteRequest);

            Assert.AreEqual(System.Net.HttpStatusCode.BadRequest, deleteResponse.StatusCode);

            //now try all operations for user 3 to user 7
            //user 3 can't follow user 7
             createRequest = new CreateRelationshipRoleAuthenticatedRequest(user3Authentication);
            createRequest.RelationshipName = SocialRelationshipName.FOLLOW;
            createRequest.PassiveId = Constants.TestDev7UserId;

             createResponse = await socialNetworkClient.CreateRelationshipRoleAuthenticatedAsync(createRequest);

            Assert.AreEqual(System.Net.HttpStatusCode.BadRequest, createResponse.StatusCode);

            //user 3 can't read follows relationship
             readRequest = new ReadRelationshipRoleAuthenticatedRequest(user3Authentication);
            readRequest.RelationshipName = SocialRelationshipName.FOLLOW;
            readRequest.PassiveId = Constants.TestDev7UserId;

             readResponse = await socialNetworkClient.ReadRelationshipRoleAuthenticatedAsync(readRequest);

            Assert.AreEqual(System.Net.HttpStatusCode.BadRequest, readResponse.StatusCode);

            //user 3 can't approve follows relationship
             approveRequest = new ApproveRelationshipRoleAuthenticatedRequest(user3Authentication);
            approveRequest.RelationshipName = SocialRelationshipName.FOLLOW;
            approveRequest.ActiveId = Constants.TestDev7UserId;
             approveResponse = await socialNetworkClient.ApproveRelationshipRoleAuthenticatedAsync(approveRequest);

            Assert.AreEqual(System.Net.HttpStatusCode.BadRequest, approveResponse.StatusCode);

            //user 3 can't delete follows relationship
            deleteRequest = new DeleteRelationshipRoleAuthenticatedRequest(user3Authentication);
            deleteRequest.RelationshipName = SocialRelationshipName.FOLLOW;
            deleteRequest.PassiveId = Constants.TestDev7UserId;

            deleteResponse = await socialNetworkClient.DeleteRelationshipRoleAuthenticatedAsync(deleteRequest);

            Assert.AreEqual(System.Net.HttpStatusCode.BadRequest, deleteResponse.StatusCode);

            //user 3 can't delete user 7's block
            deleteRequest = new DeleteRelationshipRoleAuthenticatedRequest(user3Authentication);
            deleteRequest.RelationshipName = SocialRelationshipName.BLOCK;
            deleteRequest.ActiveId = Constants.TestDev7UserId;

            deleteResponse = await socialNetworkClient.DeleteRelationshipRoleAuthenticatedAsync(deleteRequest);

            Assert.IsTrue(System.Net.HttpStatusCode.Unauthorized == deleteResponse.StatusCode);

            //user 3 CAN still block user 7
            blockRequest = new CreateRelationshipRoleAuthenticatedRequest(user3Authentication);
            blockRequest.RelationshipName = SocialRelationshipName.BLOCK;
            blockRequest.PassiveId = Constants.TestDev7UserId;

             blockResponse = await socialNetworkClient.CreateRelationshipRoleAuthenticatedAsync(blockRequest);

            Assert.AreEqual(System.Net.HttpStatusCode.OK, blockResponse.StatusCode);

            //user 7 can still read block against user 3
             readRequest = new ReadRelationshipRoleAuthenticatedRequest(user7Authentication);
            readRequest.RelationshipName = SocialRelationshipName.BLOCK;
            readRequest.PassiveId = Constants.TestDev3UserId;

             readResponse = await socialNetworkClient.ReadRelationshipRoleAuthenticatedAsync(readRequest);

            Assert.AreEqual(System.Net.HttpStatusCode.OK, readResponse.StatusCode);

            

            //delete both blocks
            deleteRequest = new DeleteRelationshipRoleAuthenticatedRequest(user7Authentication);
            deleteRequest.RelationshipName = SocialRelationshipName.BLOCK;
            deleteRequest.PassiveId = Constants.TestDev3UserId;

             deleteResponse = await socialNetworkClient.DeleteRelationshipRoleAuthenticatedAsync(deleteRequest);

            Assert.IsTrue(System.Net.HttpStatusCode.OK == deleteResponse.StatusCode);

            deleteRequest = new DeleteRelationshipRoleAuthenticatedRequest(user3Authentication);
            deleteRequest.RelationshipName = SocialRelationshipName.BLOCK;
            deleteRequest.PassiveId = Constants.TestDev7UserId;

            deleteResponse = await socialNetworkClient.DeleteRelationshipRoleAuthenticatedAsync(deleteRequest);

            Assert.IsTrue(System.Net.HttpStatusCode.OK == deleteResponse.StatusCode);

            //check follow does not exist between either
            readRequest = new ReadRelationshipRoleAuthenticatedRequest(user3Authentication);
            readRequest.RelationshipName = SocialRelationshipName.FOLLOW;
            readRequest.PassiveId = Constants.TestDev7UserId;

            readResponse = await socialNetworkClient.ReadRelationshipRoleAuthenticatedAsync(readRequest);

            Assert.AreEqual(System.Net.HttpStatusCode.NotFound, readResponse.StatusCode);

            readRequest = new ReadRelationshipRoleAuthenticatedRequest(user7Authentication);
            readRequest.RelationshipName = SocialRelationshipName.FOLLOW;
            readRequest.PassiveId = Constants.TestDev3UserId;

            readResponse = await socialNetworkClient.ReadRelationshipRoleAuthenticatedAsync(readRequest);

            Assert.AreEqual(System.Net.HttpStatusCode.NotFound, readResponse.StatusCode);
        }

        [TestMethod]
        public async Task TestFollowThenBlock()
        {
            var user7Authentication = new UserAuthentication(
                Constants.TestDev7Credentials.Username,
                Constants.TestDev7Credentials.Password);
            await user7Authentication.InitializeAsync();

            var user3Authentication = new UserAuthentication(
                Constants.TestDev3Credentials.Username,
                Constants.TestDev3Credentials.Password);
            await user3Authentication.InitializeAsync();

            //ensure blocks relationships doesnt exist already
            var deleteRequest = new DeleteRelationshipRoleAuthenticatedRequest(user7Authentication);
            deleteRequest.RelationshipName = SocialRelationshipName.BLOCK;
            deleteRequest.PassiveId = Constants.TestDev3UserId;

            var deleteResponse = await socialNetworkClient.DeleteRelationshipRoleAuthenticatedAsync(deleteRequest);

            Assert.IsTrue(System.Net.HttpStatusCode.OK == deleteResponse.StatusCode || System.Net.HttpStatusCode.NotFound == deleteResponse.StatusCode);

            //ensure follow doesnt exist already
             deleteRequest = new DeleteRelationshipRoleAuthenticatedRequest(user7Authentication);
            deleteRequest.RelationshipName = SocialRelationshipName.FOLLOW;
            deleteRequest.PassiveId = Constants.TestDev3UserId;

             deleteResponse = await socialNetworkClient.DeleteRelationshipRoleAuthenticatedAsync(deleteRequest);

            Assert.IsTrue(System.Net.HttpStatusCode.OK == deleteResponse.StatusCode || System.Net.HttpStatusCode.NotFound == deleteResponse.StatusCode);

            //user 7 follows user 3
            var createRequest = new CreateRelationshipRoleAuthenticatedRequest(user7Authentication);
            createRequest.RelationshipName = SocialRelationshipName.FOLLOW;
            createRequest.PassiveId = Constants.TestDev3UserId;

            var createResponse = await socialNetworkClient.CreateRelationshipRoleAuthenticatedAsync(createRequest);
            SocialRelationship followSocialRelationship = createResponse.SocialRelationship; //save to make sure is unchanged
            Assert.AreEqual(System.Net.HttpStatusCode.OK, createResponse.StatusCode);


            //user 7 blocks user 3
            var blockRequest = new CreateRelationshipRoleAuthenticatedRequest(user7Authentication);
            blockRequest.RelationshipName = SocialRelationshipName.BLOCK;
            blockRequest.PassiveId = Constants.TestDev3UserId;

            var blockResponse = await socialNetworkClient.CreateRelationshipRoleAuthenticatedAsync(blockRequest);

            Assert.AreEqual(System.Net.HttpStatusCode.OK, blockResponse.StatusCode);

            //user 3 can't approve follows relationship
            var approveRequest = new ApproveRelationshipRoleAuthenticatedRequest(user3Authentication);
            approveRequest.RelationshipName = SocialRelationshipName.FOLLOW;
            approveRequest.ActiveId = Constants.TestDev7UserId;
            var approveResponse = await socialNetworkClient.ApproveRelationshipRoleAuthenticatedAsync(approveRequest);

            Assert.AreEqual(System.Net.HttpStatusCode.BadRequest, approveResponse.StatusCode);

            //user 7 can't delete follows relationship
            deleteRequest = new DeleteRelationshipRoleAuthenticatedRequest(user7Authentication);
            deleteRequest.RelationshipName = SocialRelationshipName.FOLLOW;
            deleteRequest.PassiveId = Constants.TestDev3UserId;

            deleteResponse = await socialNetworkClient.DeleteRelationshipRoleAuthenticatedAsync(deleteRequest);

            Assert.AreEqual(System.Net.HttpStatusCode.BadRequest, deleteResponse.StatusCode);

            //user 7 unblocks
            deleteRequest = new DeleteRelationshipRoleAuthenticatedRequest(user7Authentication);
            deleteRequest.RelationshipName = SocialRelationshipName.BLOCK;
            deleteRequest.PassiveId = Constants.TestDev3UserId;

            deleteResponse = await socialNetworkClient.DeleteRelationshipRoleAuthenticatedAsync(deleteRequest);

            Assert.IsTrue(System.Net.HttpStatusCode.OK == deleteResponse.StatusCode);

            //user 3 reads relationshipship unchanged
            var readRequest = new ReadRelationshipRoleAuthenticatedRequest(user7Authentication);
            readRequest.RelationshipName = SocialRelationshipName.FOLLOW;
            readRequest.PassiveId = Constants.TestDev3UserId;

            var readResponse = await socialNetworkClient.ReadRelationshipRoleAuthenticatedAsync(readRequest);


            Assert.AreEqual(System.Net.HttpStatusCode.OK, readResponse.StatusCode);

            Assert.IsTrue(readResponse.SocialRelationship.Equals(followSocialRelationship));

            //user 7 unfollows
            deleteRequest = new DeleteRelationshipRoleAuthenticatedRequest(user7Authentication);
            deleteRequest.RelationshipName = SocialRelationshipName.FOLLOW;
            deleteRequest.PassiveId = Constants.TestDev3UserId;

            deleteResponse = await socialNetworkClient.DeleteRelationshipRoleAuthenticatedAsync(deleteRequest);

            Assert.IsTrue(System.Net.HttpStatusCode.OK == deleteResponse.StatusCode);

        }

        [TestMethod]
        public async Task TestFollowsApprovalProcess()
        {
            
            var user7Authentication = new UserAuthentication(
                Constants.TestDev7Credentials.Username,
                Constants.TestDev7Credentials.Password);
            await user7Authentication.InitializeAsync();

            //ensure role doesnt exist already
            var deleteRequest = new DeleteRelationshipRoleAuthenticatedRequest(user7Authentication);
            deleteRequest.RelationshipName = SocialRelationshipName.FOLLOW;
            deleteRequest.PassiveId = Constants.TestDev3UserId;

            var deleteResponse = await socialNetworkClient.DeleteRelationshipRoleAuthenticatedAsync(deleteRequest);

            Assert.IsTrue(System.Net.HttpStatusCode.OK == deleteResponse.StatusCode || System.Net.HttpStatusCode.NotFound == deleteResponse.StatusCode);

            //user 7 requests to follow user 3
            var createRequest = new CreateRelationshipRoleAuthenticatedRequest(user7Authentication);
            createRequest.RelationshipName = SocialRelationshipName.FOLLOW;
            createRequest.PassiveId = Constants.TestDev3UserId;

            var createResponse = await socialNetworkClient.CreateRelationshipRoleAuthenticatedAsync(createRequest);

            Assert.AreEqual(System.Net.HttpStatusCode.OK, createResponse.StatusCode);

            var expectedRelationship = new SocialRelationship();
            expectedRelationship.RelationshipName = createRequest.RelationshipName;
            expectedRelationship.ActiveId = Constants.TestDev7UserId;
            expectedRelationship.PassiveId = createRequest.PassiveId;
            expectedRelationship.ActiveApproved = true;
            expectedRelationship.PassiveApproved = false;
            expectedRelationship.DateCreated = DateTime.Today.ToString("yyyy-MM-dd");

            Assert.IsTrue(createResponse.SocialRelationship.Equals(expectedRelationship));

            //user 3 approves the follow request 
            var user3Authentication = new UserAuthentication(
                Constants.TestDev3Credentials.Username,
                Constants.TestDev3Credentials.Password);
            await user3Authentication.InitializeAsync();

            var approveRequest = new ApproveRelationshipRoleAuthenticatedRequest(user3Authentication);
            approveRequest.RelationshipName = SocialRelationshipName.FOLLOW;
            approveRequest.ActiveId = Constants.TestDev7UserId;

            var approveResponse = await socialNetworkClient.ApproveRelationshipRoleAuthenticatedAsync(approveRequest);

            Assert.AreEqual(System.Net.HttpStatusCode.OK, approveResponse.StatusCode);
            
            expectedRelationship.PassiveApproved = true;
            Assert.IsTrue(approveResponse.SocialRelationship.Equals(expectedRelationship));

            //user 7 reads the approved relationship
            var readRequest = new ReadRelationshipRoleAuthenticatedRequest(user7Authentication);
            readRequest.RelationshipName = SocialRelationshipName.FOLLOW;
            readRequest.PassiveId = Constants.TestDev3UserId;

            var readResponse = await socialNetworkClient.ReadRelationshipRoleAuthenticatedAsync(readRequest);

            Assert.AreEqual(System.Net.HttpStatusCode.OK, readResponse.StatusCode);

            Assert.IsTrue(readResponse.SocialRelationship.Equals(expectedRelationship));

            //user 7 unfollows user 3
            deleteResponse = await socialNetworkClient.DeleteRelationshipRoleAuthenticatedAsync(deleteRequest);
            Assert.AreEqual(System.Net.HttpStatusCode.OK, deleteResponse.StatusCode);

            deleteResponse = await socialNetworkClient.DeleteRelationshipRoleAuthenticatedAsync(deleteRequest); //make sure role was actually deleted
            Assert.AreEqual(System.Net.HttpStatusCode.NotFound, deleteResponse.StatusCode);

        }

        [TestMethod]
        public async Task TestCoachApprovalProcess()
        {

            var user7Authentication = new UserAuthentication(
                Constants.TestDev7Credentials.Username,
                Constants.TestDev7Credentials.Password);
            await user7Authentication.InitializeAsync();

            var user3Authentication = new UserAuthentication(
                Constants.TestDev3Credentials.Username,
                Constants.TestDev3Credentials.Password);
            await user3Authentication.InitializeAsync();

            //ensure role doesnt exist already
            var deleteRequest = new DeleteRelationshipRoleAuthenticatedRequest(user7Authentication);
            deleteRequest.RelationshipName = SocialRelationshipName.COACH;
            deleteRequest.ActiveId = Constants.TestDev3UserId;

            var deleteResponse = await socialNetworkClient.DeleteRelationshipRoleAuthenticatedAsync(deleteRequest);

            Assert.IsTrue(System.Net.HttpStatusCode.OK == deleteResponse.StatusCode || System.Net.HttpStatusCode.NotFound == deleteResponse.StatusCode);

            //user7 requests that user3 coaches them
            var createRequest = new CreateRelationshipRoleAuthenticatedRequest(user7Authentication);
            createRequest.RelationshipName = SocialRelationshipName.COACH;
            createRequest.ActiveId = Constants.TestDev3UserId;

            var createResponse = await socialNetworkClient.CreateRelationshipRoleAuthenticatedAsync(createRequest);

            Assert.AreEqual(System.Net.HttpStatusCode.OK, createResponse.StatusCode);

            var expectedRelationship = new SocialRelationship();
            expectedRelationship.RelationshipName = createRequest.RelationshipName;
            expectedRelationship.PassiveId = Constants.TestDev7UserId;
            expectedRelationship.ActiveId = createRequest.ActiveId;
            expectedRelationship.ActiveApproved = false;
            expectedRelationship.PassiveApproved = true;
            expectedRelationship.DateCreated = DateTime.Today.ToString("yyyy-MM-dd");

            Assert.IsTrue(createResponse.SocialRelationship.Equals(expectedRelationship));

            //user3 attempts to approve the coach request but accidentally puts themself as the passive user
            var approveRequestBad = new ApproveRelationshipRoleAuthenticatedRequest(user3Authentication);
            approveRequestBad.RelationshipName = SocialRelationshipName.COACH;
            approveRequestBad.ActiveId = Constants.TestDev7UserId;
            
            var approveResponseBad = await socialNetworkClient.ApproveRelationshipRoleAuthenticatedAsync(approveRequestBad);

            Assert.AreEqual(System.Net.HttpStatusCode.NotFound, approveResponseBad.StatusCode);
            
            //user3 successfully approves the coach request
            var approveRequestGood = new ApproveRelationshipRoleAuthenticatedRequest(user3Authentication);
            approveRequestGood.RelationshipName = SocialRelationshipName.COACH;
            approveRequestGood.PassiveId = Constants.TestDev7UserId;

            var approveResponseGood = await socialNetworkClient.ApproveRelationshipRoleAuthenticatedAsync(approveRequestGood);

            Assert.AreEqual(System.Net.HttpStatusCode.OK, approveResponseGood.StatusCode);

            expectedRelationship.ActiveApproved = true;
            Assert.IsTrue(approveResponseGood.SocialRelationship.Equals(expectedRelationship));

            //user 7 reads approved coach relationship
            var readRequest = new ReadRelationshipRoleAuthenticatedRequest(user7Authentication);
            readRequest.RelationshipName = SocialRelationshipName.COACH;
            readRequest.ActiveId = Constants.TestDev3UserId;

            var readResponse = await socialNetworkClient.ReadRelationshipRoleAuthenticatedAsync(readRequest);

            Assert.AreEqual(System.Net.HttpStatusCode.OK, readResponse.StatusCode);

            Assert.IsTrue(readResponse.SocialRelationship.Equals(expectedRelationship));

            //user 3 deletes the coaching relationship
            deleteRequest = new DeleteRelationshipRoleAuthenticatedRequest(user3Authentication);
            deleteRequest.RelationshipName = SocialRelationshipName.COACH;
            deleteRequest.PassiveId = Constants.TestDev7UserId;
            deleteResponse = await socialNetworkClient.DeleteRelationshipRoleAuthenticatedAsync(deleteRequest);
            Assert.AreEqual(System.Net.HttpStatusCode.OK, deleteResponse.StatusCode);

            deleteResponse = await socialNetworkClient.DeleteRelationshipRoleAuthenticatedAsync(deleteRequest); //make sure role was actually deleted
            Assert.AreEqual(System.Net.HttpStatusCode.NotFound, deleteResponse.StatusCode);

        }

    }
}
