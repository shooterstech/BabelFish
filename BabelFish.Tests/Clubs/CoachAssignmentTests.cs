using Amazon.DynamoDBv2.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scopos.BabelFish.APIClients;
using Scopos.BabelFish.Requests.ClubsAPI;
using Scopos.BabelFish.Responses.ClubsAPI;
using Scopos.BabelFish.Runtime.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scopos.BabelFish.Tests.Clubs
{
    [TestClass]
    public class CoachAssignmentTests
    {
        private ClubsAPIClient clubsClient;
        private APIStage stage;


        [TestInitialize]
        public void InitClient()
        {
            stage = APIStage.PRODUCTION;
            clubsClient = new ClubsAPIClient(Constants.X_API_KEY, stage);
        }

        public async Task DeleteAllCoachAssignments(int licenseNumber, UserAuthentication userAuthentication)
        {
            //list coach assignments for license number 7
            var getResponse = await clubsClient.GetCoachAssignmentAuthenticatedAsync(licenseNumber, userAuthentication);
            Assert.AreEqual(System.Net.HttpStatusCode.OK, getResponse.StatusCode);

            if (getResponse.CoachAssignmentList.Items.Count == 0) return;

            //delete any existing assignments
            var deleteAllRequest = new DeleteCoachAssignmentAuthenticatedRequest(userAuthentication);
            deleteAllRequest.LicenseNumber = licenseNumber;
            deleteAllRequest.UserId = getResponse.CoachAssignmentList.Items;
            var deleteAllResponse = await clubsClient.DeleteCoachAssignmentAuthenticatedAsync(deleteAllRequest);
            Assert.AreEqual(System.Net.HttpStatusCode.OK, deleteAllResponse.StatusCode);
            Assert.IsTrue(deleteAllResponse.CoachAssignmentList.Items.Count == 0);
        }

        [TestMethod]
        public async Task TestUnauthorizedCoachAssignmentCRUD()
        {
            var userAuthentication = new UserAuthentication(
                Constants.TestDev3Credentials.Username,
                Constants.TestDev3Credentials.Password);
            await userAuthentication.InitializeAsync();
            int licenseNumber = 7;

            //TestDev3 is not a POC for license 7, and should not be able to perform any CRUD functions
            var getResponse = await clubsClient.GetCoachAssignmentAuthenticatedAsync(licenseNumber, userAuthentication);
            Assert.AreEqual(System.Net.HttpStatusCode.Unauthorized, getResponse.StatusCode);

            var postRequest = new CreateCoachAssignmentAuthenticatedRequest(userAuthentication);
            postRequest.LicenseNumber = licenseNumber;
            postRequest.UserId.Add(Constants.TestDev3UserId);//try to add themself as designated coach
            var postResponse = await clubsClient.CreateCoachAssignmentAuthenticatedAsync(postRequest);
            Assert.AreEqual(System.Net.HttpStatusCode.Unauthorized, postResponse.StatusCode);

            var deleteRequest = new DeleteCoachAssignmentAuthenticatedRequest(userAuthentication);
            deleteRequest.LicenseNumber = licenseNumber;
            deleteRequest.UserId.Add(Constants.TestDev3UserId); //try to delete themself as designated coach
            var deleteResponse = await clubsClient.DeleteCoachAssignmentAuthenticatedAsync(deleteRequest);
            Assert.AreEqual(System.Net.HttpStatusCode.Unauthorized, deleteResponse.StatusCode);

        }

        [TestMethod]
        public async Task TestAuthorizeButLicenseExpired()
        {
            var userAuthentication = new UserAuthentication(
                Constants.TestDev1Credentials.Username,
                Constants.TestDev1Credentials.Password);
            await userAuthentication.InitializeAsync();
            int licenseNumber = 8;//stage!=APIStage.PRODUCTION? 2:8 ; //license 2 is expired in dev db, 8 is expired in production

            //TestDev1 is a POC for license 2, but license 2 is expired, so none of the operations should succeed
            var getResponse = await clubsClient.GetCoachAssignmentAuthenticatedAsync(licenseNumber, userAuthentication);
            Assert.AreEqual(System.Net.HttpStatusCode.BadRequest, getResponse.StatusCode);

            var postRequest = new CreateCoachAssignmentAuthenticatedRequest(userAuthentication);
            postRequest.LicenseNumber = licenseNumber;
            postRequest.UserId.Add(Constants.TestDev3UserId);//try to add new designated user
            var postResponse = await clubsClient.CreateCoachAssignmentAuthenticatedAsync(postRequest);
            Assert.AreEqual(System.Net.HttpStatusCode.BadRequest, postResponse.StatusCode);

            var deleteRequest = new DeleteCoachAssignmentAuthenticatedRequest(userAuthentication);
            deleteRequest.LicenseNumber = licenseNumber;
            deleteRequest.UserId.Add(Constants.TestDev3UserId); //try to delete existing designated user
            var deleteResponse = await clubsClient.DeleteCoachAssignmentAuthenticatedAsync(deleteRequest);
            Assert.AreEqual(System.Net.HttpStatusCode.BadRequest, deleteResponse.StatusCode);

        }

        [TestMethod]
        public async Task TestMaximumAssignment()
        {
            var userAuthentication = new UserAuthentication(
                Constants.TestDev1Credentials.Username,
                Constants.TestDev1Credentials.Password);
            await userAuthentication.InitializeAsync();
            int licenseNumber = 7;
            //Delete any existing assignments
            await DeleteAllCoachAssignments(licenseNumber, userAuthentication);

            //add exactly the maximum allowed
            var postRequest = new CreateCoachAssignmentAuthenticatedRequest(userAuthentication);
            postRequest.LicenseNumber = licenseNumber;
            postRequest.UserId = new List<string> { 
                Constants.TestDev2UserId,  
                Constants.TestDev3UserId,
                Constants.TestDev7UserId,
                Constants.TestDev9UserId
            };

            var postResponse = await clubsClient.CreateCoachAssignmentAuthenticatedAsync(postRequest);
            Assert.AreEqual(System.Net.HttpStatusCode.OK, postResponse.StatusCode);

            //try to add all of them again, should still be ok because we didnt add anyone new
            postResponse = await clubsClient.CreateCoachAssignmentAuthenticatedAsync(postRequest);
            Assert.AreEqual(System.Net.HttpStatusCode.OK, postResponse.StatusCode);


            //try to add new user as coach, this should not work because we have exceeded the limit
            postRequest.UserId = new List<string> { Constants.TestDev1UserId };
            postResponse = await clubsClient.CreateCoachAssignmentAuthenticatedAsync(postRequest);
            Assert.AreEqual(System.Net.HttpStatusCode.BadRequest, postResponse.StatusCode);
            Assert.AreEqual(postResponse.MessageResponse.Message[0], "Maximum of 4 coach designtions allowed per license");

            //delete all assignments
            await DeleteAllCoachAssignments(licenseNumber, userAuthentication);
            
            //add half of the maximum
            postRequest.UserId = new List<string> {
                Constants.TestDev2UserId,
                Constants.TestDev3UserId
            };
            postResponse = await clubsClient.CreateCoachAssignmentAuthenticatedAsync(postRequest);
            Assert.AreEqual(System.Net.HttpStatusCode.OK, postResponse.StatusCode);

            //try to more than allowed all at once
            postRequest.UserId = new List<string> {
                Constants.TestDev7UserId,
                Constants.TestDev9UserId,
                Constants.TestDev1UserId
            };
            postResponse = await clubsClient.CreateCoachAssignmentAuthenticatedAsync(postRequest);
            Assert.AreEqual(System.Net.HttpStatusCode.BadRequest, postResponse.StatusCode);
            Assert.AreEqual(postResponse.MessageResponse.Message[0], "Maximum of 4 coach designtions allowed per license");

            //delete all assignments
            await DeleteAllCoachAssignments(licenseNumber, userAuthentication);


        }

        [TestMethod]
        public async Task TestCoachAssignmentCRUD()
        {
            var userAuthentication = new UserAuthentication(
                Constants.TestDev1Credentials.Username,
                Constants.TestDev1Credentials.Password);
            await userAuthentication.InitializeAsync();
            int licenseNumber = 7;
            //Delete any existing assignments
            await DeleteAllCoachAssignments(licenseNumber, userAuthentication);

            //Assign a single user as coach
            var postRequest = new CreateCoachAssignmentAuthenticatedRequest(userAuthentication);
            postRequest.LicenseNumber = licenseNumber;
            postRequest.UserId.Add(Constants.TestDev9UserId);
            var postResponse = await clubsClient.CreateCoachAssignmentAuthenticatedAsync(postRequest);
            Assert.AreEqual(System.Net.HttpStatusCode.OK, postResponse.StatusCode);
            Assert.IsTrue(Enumerable.SequenceEqual(postResponse.CoachAssignmentList.Items, postRequest.UserId));

            //read single user
            var getResponse = await clubsClient.GetCoachAssignmentAuthenticatedAsync(licenseNumber, userAuthentication);
            Assert.AreEqual(System.Net.HttpStatusCode.OK, getResponse.StatusCode);
            Assert.IsTrue(Enumerable.SequenceEqual(getResponse.CoachAssignmentList.Items, postRequest.UserId));


            //Add two more users
            postRequest.UserId.Add(Constants.TestDev7UserId);
            postRequest.UserId.Add(Constants.TestDev3UserId);
            postResponse = await clubsClient.CreateCoachAssignmentAuthenticatedAsync(postRequest);
            Assert.AreEqual(System.Net.HttpStatusCode.OK, postResponse.StatusCode);
            postRequest.UserId.Sort();
            postResponse.CoachAssignmentList.Items.Sort();
            Assert.IsTrue(Enumerable.SequenceEqual(postResponse.CoachAssignmentList.Items, postRequest.UserId));

            //read multiple user
            getResponse = await clubsClient.GetCoachAssignmentAuthenticatedAsync(licenseNumber, userAuthentication);
            Assert.AreEqual(System.Net.HttpStatusCode.OK, getResponse.StatusCode);
            getResponse.CoachAssignmentList.Items.Sort();
            Assert.IsTrue(Enumerable.SequenceEqual(getResponse.CoachAssignmentList.Items, postRequest.UserId));

            //delete single user
            var deleteRequest = new DeleteCoachAssignmentAuthenticatedRequest(userAuthentication);
            deleteRequest.LicenseNumber = licenseNumber;
            deleteRequest.UserId.Add(Constants.TestDev7UserId);
            var deleteResponse = await clubsClient.DeleteCoachAssignmentAuthenticatedAsync(deleteRequest);
            Assert.AreEqual(System.Net.HttpStatusCode.OK, deleteResponse.StatusCode);
            List<string> expected = new List<string> { Constants.TestDev3UserId, Constants.TestDev9UserId };
            expected.Sort();
            deleteResponse.CoachAssignmentList.Items.Sort();
            Assert.IsTrue(Enumerable.SequenceEqual(deleteResponse.CoachAssignmentList.Items, expected));

            //read deletion
            getResponse = await clubsClient.GetCoachAssignmentAuthenticatedAsync(licenseNumber, userAuthentication);
            Assert.AreEqual(System.Net.HttpStatusCode.OK, getResponse.StatusCode);
            getResponse.CoachAssignmentList.Items.Sort();
            Assert.IsTrue(Enumerable.SequenceEqual(getResponse.CoachAssignmentList.Items, expected));

            //delete remaining users
            deleteRequest.UserId = getResponse.CoachAssignmentList.Items;
            deleteResponse = await clubsClient.DeleteCoachAssignmentAuthenticatedAsync(deleteRequest);
            Assert.AreEqual(System.Net.HttpStatusCode.OK, getResponse.StatusCode);
            Assert.IsTrue(deleteResponse.CoachAssignmentList.Items.Count == 0);

        }
    }
}
