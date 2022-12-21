using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text;
using System.Threading.Tasks;
using ShootersTech.BabelFish.ClubsAPI;
using ShootersTech.BabelFish.Tests;
using ShootersTech.BabelFish.Requests.ClubsAPI;

namespace ShootersTech.BabelFish.Tests {

    [TestClass]
    public class ClubsTests {

        [TestMethod]
        public async Task GetClubListSmallList() {
            
            //Test Dev 7 is associated with two clubs.
            var client = new ClubsAPIClient( Constants.X_API_KEY, Constants.clientParamsTestDev7 );

            var request = new GetClubListRequest();
            request.ApiStage = ShootersTech.BabelFish.Helpers.APIStage.BETA;

            var response = await client.GetClubListAsync( request );

            Assert.AreEqual( response.StatusCode, System.Net.HttpStatusCode.OK );

            var clubList = response.ClubList;

            Assert.IsTrue( clubList.Items.Count > 0, "The response's ClubList should have a length greather than zero." );

            Assert.AreEqual( response.MessageResponse.NextToken, "", "Expecting NextToken to be an empty string with user test_dev_7." );

        }

        [TestMethod]
        public async Task GetClubListLargeList() {

            //Test Dev 1 is associated with a whole lot of clubs.
            var client = new ClubsAPIClient( Constants.X_API_KEY, Constants.clientParamsTestDev1 );

            //Perform the initial request. The default consgruction sets Token to an empty string.
            var request1 = new GetClubListRequest();
            request1.ApiStage = ShootersTech.BabelFish.Helpers.APIStage.BETA;

            var response1 = await client.GetClubListAsync( request1 );

            Assert.AreEqual( response1.StatusCode, System.Net.HttpStatusCode.OK );

            var clubList = response1.ClubList;

            Assert.IsTrue( clubList.Items.Count > 0, "The response's ClubList should have a length greather than zero." );
            Assert.AreNotEqual( response1.MessageResponse.NextToken, "", "Expecting NextToken to be an non empty string with user test_dev_1." );

            //Perform the follow up request, with the toekn value
            var request2 = new GetClubListRequest();
            request2.Token = response1.NextToken;
            request2.ApiStage = ShootersTech.BabelFish.Helpers.APIStage.BETA;

            var response2 = await client.GetClubListAsync( request2 );
            Assert.AreEqual( response2.StatusCode, System.Net.HttpStatusCode.OK );

            Assert.AreNotEqual( response1.MessageResponse.NextToken, response2.MessageResponse.NextToken );
        }

        [TestMethod]
        public async Task GetclubDetail() {


            var client = new ClubsAPIClient( Constants.X_API_KEY, Constants.clientParamsTestDev7 );

            var ownerId = "OrionAcct002001";
            var request = new GetClubDetailRequest( ownerId );
            request.ApiStage = ShootersTech.BabelFish.Helpers.APIStage.BETA;

            var response = await client.GetClubDetailAsync( request );

            Assert.AreEqual( response.StatusCode, System.Net.HttpStatusCode.OK );

            var clubDetail = response.ClubDetail;

            Assert.AreEqual( clubDetail.OwnerId, ownerId, "Expecting the OwnerId to match, what was sent." );

            Assert.IsTrue( clubDetail.LicenseList.Count > 0, "Expecting the length of the license list is greather than zero." );
        }
    }
}
