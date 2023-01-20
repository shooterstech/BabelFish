using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text;
using System.Threading.Tasks;
using Scopos.BabelFish.Runtime;
using Scopos.BabelFish.Tests;
using Scopos.BabelFish.Requests.ClubsAPI;
using Scopos.BabelFish.DataModel.Clubs;
using Scopos.BabelFish.APIClients;
using Scopos.BabelFish.Runtime.Authentication;

namespace Scopos.BabelFish.Tests.Clubs {

    [TestClass]
    public class ClubsTests {

        /// <summary>
        /// Unit test to confirm the Constructors set the api key and API stage as expected.
        /// </summary>
        [TestMethod]
        public void BasicConstructorTests() {
          
            var defaultConstructorClient = new ClubsAPIClient( Constants.X_API_KEY );
            var apiStageConstructorClient = new ClubsAPIClient( Constants.X_API_KEY, APIStage.BETA );

            Assert.AreEqual( Constants.X_API_KEY, defaultConstructorClient.XApiKey );
            Assert.AreEqual( APIStage.PRODUCTION, defaultConstructorClient.ApiStage );

            Assert.AreEqual( Constants.X_API_KEY, apiStageConstructorClient.XApiKey );
            Assert.AreEqual( APIStage.BETA, apiStageConstructorClient.ApiStage );
        }

        [TestMethod]
        public async Task GetClubListSmallList() {

            var client = new ClubsAPIClient( Constants.X_API_KEY, APIStage.BETA );

            //Test Dev 7 is associated with two clubs.
            var userAuthentication = new UserAuthentication(
                Constants.TestDev7Credentials.Username,
                Constants.TestDev7Credentials.Password );
            var request = new GetClubListAuthenticatedRequest( userAuthentication );

            var response = await client.GetClubListAsync( request );

            Assert.AreEqual( response.StatusCode, System.Net.HttpStatusCode.OK );

            var clubList = response.ClubList;

            Assert.IsTrue( clubList.Items.Count > 0, "The response's ClubList should have a length greather than zero." );

            Assert.AreEqual( response.MessageResponse.NextToken, "", "Expecting NextToken to be an empty string with user test_dev_7." );

        }

        [TestMethod]
        public async Task GetClubListLargeList() {

            //Test Dev 1 is associated with a whole lot of clubs.
            var client = new ClubsAPIClient( Constants.X_API_KEY, APIStage.BETA );

            //Perform the initial request. The default consgruction sets Token to an empty string.
            var userAuthentication = new UserAuthentication(
                Constants.TestDev1Credentials.Username,
                Constants.TestDev1Credentials.Password );
            var request1 = new GetClubListAuthenticatedRequest( userAuthentication );

            var response1 = await client.GetClubListAsync( request1 );

            Assert.AreEqual( response1.StatusCode, System.Net.HttpStatusCode.OK );

            var clubList = response1.ClubList;

            Assert.IsTrue( clubList.Items.Count > 0, "The response's ClubList should have a length greather than zero." );
            Assert.AreNotEqual( response1.MessageResponse.NextToken, "", "Expecting NextToken to be an non empty string with user test_dev_1." );

            //Perform the follow up request, with the toekn value
            var request2 = new GetClubListAuthenticatedRequest( userAuthentication );
            request2.Token = response1.NextToken;

            var response2 = await client.GetClubListAsync( request2 );
            Assert.AreEqual( response2.StatusCode, System.Net.HttpStatusCode.OK );

            Assert.AreNotEqual( response1.MessageResponse.NextToken, response2.MessageResponse.NextToken );
        }

        [TestMethod]
        public async Task GetClubDetail() {


            var client = new ClubsAPIClient( Constants.X_API_KEY, APIStage.BETA );

            var ownerId = "OrionAcct002001";
            var userAuthentication = new UserAuthentication(
                Constants.TestDev1Credentials.Username,
                Constants.TestDev1Credentials.Password );
            var request = new GetClubDetailAuthenticatedRequest( ownerId, userAuthentication );

            var response = await client.GetClubDetailAsync( request );

            Assert.AreEqual( System.Net.HttpStatusCode.OK, response.StatusCode );

            var clubDetail = response.ClubDetail;

            Assert.AreEqual( ownerId, clubDetail.OwnerId, "Expecting the OwnerId to match, what was sent." );

            Assert.IsTrue( clubDetail.LicenseList.Count > 0, "Expecting the length of the license list is greather than zero." );

            Assert.IsTrue( clubDetail.Options.Count > 0, "Expecting at least one ClubOption." );
        }
    }
}
