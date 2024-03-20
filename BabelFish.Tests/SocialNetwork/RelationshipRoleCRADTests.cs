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
        public async Task CreateThenDeleteFollowRelationshipRole()
        {
            //create the role
            var userAuthentication = new UserAuthentication(
                Constants.TestDev7Credentials.Username,
                Constants.TestDev7Credentials.Password);
            await userAuthentication.InitializeAsync();

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

            //create the role
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

            //approve
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

            //read approval
            var readRequest = new ReadRelationshipRoleAuthenticatedRequest(user7Authentication);
            readRequest.RelationshipName = SocialRelationshipName.FOLLOW;
            readRequest.PassiveId = Constants.TestDev3UserId;

            var readResponse = await socialNetworkClient.ReadRelationshipRoleAuthenticatedAsync(readRequest);

            Assert.AreEqual(System.Net.HttpStatusCode.OK, readResponse.StatusCode);

            Assert.IsTrue(readResponse.SocialRelationship.Equals(expectedRelationship));

            //delete the role (request already created from initial delete)
            deleteResponse = await socialNetworkClient.DeleteRelationshipRoleAuthenticatedAsync(deleteRequest);
            Assert.AreEqual(System.Net.HttpStatusCode.OK, deleteResponse.StatusCode);

            deleteResponse = await socialNetworkClient.DeleteRelationshipRoleAuthenticatedAsync(deleteRequest); //make sure role was actually deleted
            Assert.AreEqual(System.Net.HttpStatusCode.NotFound, deleteResponse.StatusCode);

        }

    }
}
