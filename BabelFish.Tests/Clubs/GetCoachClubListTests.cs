using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scopos.BabelFish.APIClients;
using Scopos.BabelFish.Requests.ClubsAPI;
using Scopos.BabelFish.Runtime.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scopos.BabelFish.Tests.Clubs
{
    [TestClass]
    public class GetCoachClubListTests
    {
        private ClubsAPIClient clubsClient;


        [TestInitialize]
        public void InitClient()
        {
            clubsClient = new ClubsAPIClient(Constants.X_API_KEY, APIStage.BETA);
        }

        [TestMethod]
        public async Task AuthenticatedPrivateTest()
        {
            //TestDev1 has private profile
            var userAuthentication = new UserAuthentication(
                Constants.TestDev1Credentials.Username,
                Constants.TestDev1Credentials.Password);
            await userAuthentication.InitializeAsync();

            var getRequest = new GetCoachClubListAuthenticatedRequest(userAuthentication);
            var getResponse = await clubsClient.GetCoachClubListAuthenticatedAsync(getRequest);
            Assert.AreEqual(System.Net.HttpStatusCode.OK, getResponse.StatusCode);
            Assert.IsTrue(getResponse.ClubList.Items.Count > 0);
        }

        [TestMethod]
        public async Task PublicGetPrivateTest()
        {
            var getRequest = new GetCoachClubListPublicRequest(Constants.TestDev1UserId);
            var getResponse = await clubsClient.GetCoachClubListPublicAsync(getRequest);
            Assert.AreEqual(System.Net.HttpStatusCode.NotFound, getResponse.StatusCode);
        }

        [TestMethod]
        public async Task PublicGetNotExistTest()
        {
            var getRequest = new GetCoachClubListPublicRequest("fakeuserid");
            var getResponse = await clubsClient.GetCoachClubListPublicAsync(getRequest);
            Assert.AreEqual(System.Net.HttpStatusCode.NotFound, getResponse.StatusCode);
        }

        [TestMethod]
        public async Task PublicGetGoodTest()
        {
            var getRequest = new GetCoachClubListPublicRequest(Constants.TestDev3UserId); //TestDev3 has public profile
            var getResponse = await clubsClient.GetCoachClubListPublicAsync(getRequest);
            Assert.AreEqual(System.Net.HttpStatusCode.OK, getResponse.StatusCode);
            Assert.IsTrue(getResponse.ClubList.Items.Count > 0);
        }

        [TestMethod]
        public async Task PublicGetNonCoachTest()
        {
            var getRequest = new GetCoachClubListPublicRequest(Constants.TestDev7UserId); //TestDev7 has public profile but is not an assigned coach
            var getResponse = await clubsClient.GetCoachClubListPublicAsync(getRequest);
            Assert.AreEqual(System.Net.HttpStatusCode.OK, getResponse.StatusCode);
            Assert.IsTrue(getResponse.ClubList.Items.Count == 0);
        }

        [TestMethod]
        public async Task AuthenticatedGetNonCoachTest()
        {
            //TestDev7 has public profile but is not an assigned coach
            var userAuthentication = new UserAuthentication(
                Constants.TestDev7Credentials.Username,
                Constants.TestDev7Credentials.Password);
            await userAuthentication.InitializeAsync();

            var getRequest = new GetCoachClubListAuthenticatedRequest(userAuthentication);
            var getResponse = await clubsClient.GetCoachClubListAuthenticatedAsync(getRequest);
            Assert.AreEqual(System.Net.HttpStatusCode.OK, getResponse.StatusCode);
            Assert.IsTrue(getResponse.ClubList.Items.Count == 0);
        }



    }
}