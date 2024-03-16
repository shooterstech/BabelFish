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
        [TestMethod]
        public async Task CreateFollowRelationshipRole()
        {
            var socialNetworkClient = new SocialNetworkAPIClient(Constants.X_API_KEY, APIStage.BETA);
            var userAuthentication = new UserAuthentication(
                Constants.TestDev7Credentials.Username,
                Constants.TestDev7Credentials.Password);
            await userAuthentication.InitializeAsync();

            var createRequest = new CreateRelationshipRoleAuthenticatedRequest(userAuthentication);
            createRequest.RelationshipName = SocialRelationshipName.FOLLOW; 
            createRequest.PassiveId = Constants.TestDev3UserId;

            var createResponse = await socialNetworkClient.CreateRelationshipRoleAuthenticatedAsync(createRequest);
            
            Assert.AreEqual(System.Net.HttpStatusCode.OK, createResponse.StatusCode);
        }
    }
}
