using System.Threading.Tasks;
using Scopos.BabelFish.APIClients;
using Scopos.BabelFish.Requests.ClubsAPI;
using Scopos.BabelFish.Runtime.Authentication;

namespace Scopos.BabelFish.Tests.Clubs {
    [TestClass]
    public class GetCoachClubListTests : BaseTestClass
    {
        private ClubsAPIClient clubsClient;


        [TestInitialize]
        public override void InitializeTest() {
            base.InitializeTest();

            clubsClient = new ClubsAPIClient( APIStage.PRODUCTION );
        }

        [TestMethod]
        public async Task AuthenticatedPrivateTest()
        {
            //TestDev1 has private profile
            var userAuthentication = new UserAuthentication(
                Constants.TestDev1Credentials.Username,
                Constants.TestDev1Credentials.Password);
            await userAuthentication.InitializeAsync();

            //TestDev1 assigns themself a coach under license 7 (they are a POC)
            var postRequest = new CreateCoachAssignmentAuthenticatedRequest(userAuthentication);
            postRequest.LicenseNumber = 7; 
            postRequest.UserId.Add(Constants.TestDev1UserId);//try to add new designated user
            var postResponse = await clubsClient.CreateCoachAssignmentAuthenticatedAsync(postRequest);
            Assert.AreEqual(System.Net.HttpStatusCode.OK, postResponse.StatusCode);

            //TestDev1 retrieves the list of clubs that assign them as coach
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
            var userAuthentication = new UserAuthentication(
                Constants.TestDev1Credentials.Username,
                Constants.TestDev1Credentials.Password);
            await userAuthentication.InitializeAsync();
            //TestDev1 assigns TestDev3 a coach under license 7 (they are a POC)
            var postRequest = new CreateCoachAssignmentAuthenticatedRequest(userAuthentication);
            postRequest.LicenseNumber = 7;
            postRequest.UserId.Add(Constants.TestDev3UserId);//try to add new designated user
            var postResponse = await clubsClient.CreateCoachAssignmentAuthenticatedAsync(postRequest);
            Assert.AreEqual(System.Net.HttpStatusCode.OK, postResponse.StatusCode);


            var getRequest = new GetCoachClubListPublicRequest(Constants.TestDev3UserId); //TestDev3 has public profile, no userAuthentication used
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