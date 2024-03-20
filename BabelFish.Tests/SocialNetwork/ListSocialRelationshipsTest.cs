using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scopos.BabelFish.APIClients;
using Scopos.BabelFish.DataModel.SocialNetwork;
using Scopos.BabelFish.Requests.SocialNetworkAPI;
using Scopos.BabelFish.Responses.SocialNetworkAPI;
using Scopos.BabelFish.Runtime.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scopos.BabelFish.Tests.SocialNetwork
{
    [TestClass]
    public class ListSocialRelationshipsTests
    {
        private SocialNetworkAPIClient socialNetworkClient;


        [TestInitialize]
        public void InitClient()
        {
            socialNetworkClient = new SocialNetworkAPIClient(Constants.X_API_KEY, APIStage.BETA);
        }

        [TestMethod]
        public async Task ListAllFollowRelationships()
        {
            var userAuthentication = new UserAuthentication(
                Constants.TestDev3Credentials.Username,
                Constants.TestDev3Credentials.Password);
            await userAuthentication.InitializeAsync();

            var listRequest = new ListSocialRelationshipsAuthenticatedRequest(userAuthentication);
            listRequest.RelationshipName = SocialRelationshipName.FOLLOW;
            listRequest.AsPassive = true;
            listRequest.AsActive = true;
            listRequest.IncomingRequests = true; 
            listRequest.OutgoingRequests = true;

            var listResponse = await socialNetworkClient.ListSocialRelationshipsAuthenticatedAsync(listRequest);
            Assert.AreEqual(System.Net.HttpStatusCode.OK, listResponse.StatusCode);
        }


        [TestMethod]
        public async Task ListApprovedFollowers()
        {
            //get list of followers that have been approved by the calling user
            var userAuthentication = new UserAuthentication(
                Constants.TestDev3Credentials.Username,
                Constants.TestDev3Credentials.Password);
            await userAuthentication.InitializeAsync();

            var listRequest = new ListSocialRelationshipsAuthenticatedRequest(userAuthentication);
            listRequest.RelationshipName = SocialRelationshipName.FOLLOW;
            listRequest.Limit = 10;
            listRequest.AsPassive = true;
            

            var listResponse = await socialNetworkClient.ListSocialRelationshipsAuthenticatedAsync(listRequest);
            Assert.AreEqual(System.Net.HttpStatusCode.OK, listResponse.StatusCode);

            foreach (SocialRelationship sr in listResponse.SocialRelationshipList.Items)
            {
                Assert.AreEqual(sr.PassiveId, Constants.TestDev3UserId);
                Assert.IsTrue(sr.ActiveApproved && sr.PassiveApproved);
            }

        }

        [TestMethod]
        public async Task ListApprovedFollowing()
        {
            //get list of users that the calling user is following and is approved 
            var userAuthentication = new UserAuthentication(
                Constants.TestDev3Credentials.Username,
                Constants.TestDev3Credentials.Password);
            await userAuthentication.InitializeAsync();

            var listRequest = new ListSocialRelationshipsAuthenticatedRequest(userAuthentication);
            listRequest.RelationshipName = SocialRelationshipName.FOLLOW;
            listRequest.Limit = 10;
            listRequest.AsActive = true;

            var listResponse = await socialNetworkClient.ListSocialRelationshipsAuthenticatedAsync(listRequest);
            Assert.AreEqual(System.Net.HttpStatusCode.OK, listResponse.StatusCode);

            foreach (SocialRelationship sr in listResponse.SocialRelationshipList.Items)
            {
                Assert.AreEqual(sr.ActiveId, Constants.TestDev3UserId);
                Assert.IsTrue(sr.ActiveApproved && sr.PassiveApproved);
            }
        }

        [TestMethod]
        public async Task ListIncomingFollowRequests()
        {
            //get list of follow requests awaiting approval of the caller
            var userAuthentication = new UserAuthentication(
                Constants.TestDev3Credentials.Username,
                Constants.TestDev3Credentials.Password);
            await userAuthentication.InitializeAsync();

            var listRequest = new ListSocialRelationshipsAuthenticatedRequest(userAuthentication);
            listRequest.RelationshipName = SocialRelationshipName.FOLLOW;
            listRequest.Limit = 10;
            listRequest.IncomingRequests = true;

            var listResponse = await socialNetworkClient.ListSocialRelationshipsAuthenticatedAsync(listRequest);
            Assert.AreEqual(System.Net.HttpStatusCode.OK, listResponse.StatusCode);

            foreach (SocialRelationship sr in listResponse.SocialRelationshipList.Items)
            {
                Assert.AreEqual(sr.PassiveId, Constants.TestDev3UserId);
                Assert.IsTrue(sr.ActiveApproved && !sr.PassiveApproved);
            }

        }

        [TestMethod]
        public async Task ListOutgoingFollowRequests()
        {
            //get list of follow requests sent by the caller that have yet to be approved by the passive user
            var userAuthentication = new UserAuthentication(
                Constants.TestDev3Credentials.Username,
                Constants.TestDev3Credentials.Password);
            await userAuthentication.InitializeAsync();

            var listRequest = new ListSocialRelationshipsAuthenticatedRequest(userAuthentication);
            listRequest.RelationshipName = SocialRelationshipName.FOLLOW;
            listRequest.Limit = 10;
            listRequest.OutgoingRequests = true;

            var listResponse = await socialNetworkClient.ListSocialRelationshipsAuthenticatedAsync(listRequest);
            Assert.AreEqual(System.Net.HttpStatusCode.OK, listResponse.StatusCode);
            
            foreach (SocialRelationship sr in listResponse.SocialRelationshipList.Items)
            {
                Assert.AreEqual(sr.ActiveId, Constants.TestDev3UserId);
                Assert.IsTrue(sr.ActiveApproved && !sr.PassiveApproved);
            }
        }

        [TestMethod]
        public async Task ListAllFollowRelationshipsWithTokens()
        {
            var userAuthentication = new UserAuthentication(
                Constants.TestDev3Credentials.Username,
                Constants.TestDev3Credentials.Password);
            await userAuthentication.InitializeAsync();

            var listRequest = new ListSocialRelationshipsAuthenticatedRequest(userAuthentication);
            listRequest.RelationshipName = SocialRelationshipName.FOLLOW;
            listRequest.AsPassive = true;
            listRequest.AsActive = true;
            listRequest.IncomingRequests = true;
            listRequest.OutgoingRequests = true;
            listRequest.Limit = 20;

            ListSocialRelationshipsAuthenticatedResponse listResponse;
            List<SocialRelationship> myRelationships = new List<SocialRelationship>();
            bool moreData;
            string lastToken = "";

            do
            {
                listResponse = await socialNetworkClient.ListSocialRelationshipsAuthenticatedAsync(listRequest);
                myRelationships.AddRange(listResponse.SocialRelationshipList.Items);

                Assert.IsTrue(listResponse.SocialRelationshipList.Items.Count() <= listRequest.Limit);
                Assert.AreNotEqual(lastToken, listResponse.SocialRelationshipList.NextToken);
                Assert.AreEqual(System.Net.HttpStatusCode.OK, listResponse.StatusCode);

                lastToken = listResponse.SocialRelationshipList.NextToken;

                moreData = !string.IsNullOrEmpty(listResponse.SocialRelationshipList.NextToken);
                listRequest = listResponse.GetNextRequest();
            } while (moreData);
            

        }




    }
}
