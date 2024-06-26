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
using Scopos.BabelFish.DataActors.OrionMatch;
using System.Collections;

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
            await userAuthentication.InitializeAsync();
            var request = new GetClubListAuthenticatedRequest( userAuthentication );

            var response = await client.GetClubListAuthenticatedAsync( request );

            Assert.AreEqual( System.Net.HttpStatusCode.OK, response.StatusCode );

            var clubList = response.ClubList;

            Assert.IsTrue( clubList.Items.Count > 0, "The response's ClubList should have a length greather than zero." );

            Assert.AreEqual( clubList.NextToken, "", "Expecting NextToken to be an empty string with user test_dev_7." );

        }

        [TestMethod]
        public async Task GetClubListLargeList() {

            //Test Dev 1 is associated with a whole lot of clubs.
            var client = new ClubsAPIClient( Constants.X_API_KEY, APIStage.BETA );

            //Perform the initial request. The default consgruction sets Token to an empty string.
            var userAuthentication = new UserAuthentication(
                Constants.TestDev1Credentials.Username,
                Constants.TestDev1Credentials.Password );
            await userAuthentication.InitializeAsync();
            var request1 = new GetClubListAuthenticatedRequest( userAuthentication );

            var response1 = await client.GetClubListAuthenticatedAsync( request1 );

            Assert.AreEqual( response1.StatusCode, System.Net.HttpStatusCode.OK );

            var clubList = response1.ClubList;

            Assert.IsTrue( clubList.Items.Count > 0, "The response's ClubList should have a length greather than zero." );
            Assert.AreNotEqual( clubList.NextToken, "", "Expecting NextToken to be an non empty string with user test_dev_1." );

            //Perform the follow up request, with the toekn value
            var request2 = new GetClubListAuthenticatedRequest( userAuthentication );
            request2.Token = clubList.NextToken;

            var response2 = await client.GetClubListAuthenticatedAsync( request2 );
            Assert.AreEqual( response2.StatusCode, System.Net.HttpStatusCode.OK );

            Assert.AreNotEqual( response1.ClubList.NextToken, response2.ClubList.NextToken );
        }

        [TestMethod]
        public async Task GetClubDetailAuthenticated() {


            var client = new ClubsAPIClient( Constants.X_API_KEY, APIStage.BETA );

            var ownerId = "OrionAcct002001";
            var userAuthentication = new UserAuthentication(
                Constants.TestDev1Credentials.Username,
                Constants.TestDev1Credentials.Password );
            await userAuthentication.InitializeAsync();
            var request = new GetClubDetailAuthenticatedRequest( ownerId, userAuthentication );

            var response = await client.GetClubDetailAuthenticatedAsync( request );

            Assert.AreEqual( System.Net.HttpStatusCode.OK, response.StatusCode );

            var clubDetail = response.ClubDetail;

            Assert.AreEqual( ownerId, clubDetail.OwnerId, "Expecting the OwnerId to match, what was sent." );

            Assert.IsTrue( clubDetail.LicenseList.Count > 0, "Expecting the length of the license list is greather than zero." );

            Assert.IsTrue( clubDetail.Options.Count > 0, "Expecting at least one ClubOption." );
        }

        [TestMethod]
        public async Task GetClubDetailPublic() {


            var client = new ClubsAPIClient( Constants.X_API_KEY, APIStage.BETA );

            var ownerId = "OrionAcct000001";
            var request = new GetClubDetailPublicRequest( ownerId );

            var response = await client.GetClubDetailPublicAsync( request );

            Assert.AreEqual( System.Net.HttpStatusCode.OK, response.StatusCode );

            var clubDetail = response.ClubDetail;

            Assert.AreEqual( ownerId, clubDetail.OwnerId, "Expecting the OwnerId to match, what was sent." );
        }

        [TestMethod]
        public async Task GetClubListPublic() {

            //Using production, to get more real values.
            var client = new ClubsAPIClient( Constants.X_API_KEY, APIStage.PRODUCTION );

            //Should return all clubs without any parameters ... or at least up to the token limit
            var request = new GetClubListPublicRequest( );
            request.CurrentlyShooting = false;

            var getAllClubsResponse = await client.GetClubListPublicAsync( request );

            Assert.AreEqual( System.Net.HttpStatusCode.OK, getAllClubsResponse.StatusCode );

            var clubList = getAllClubsResponse.ClubList;

            Assert.IsTrue( clubList.Items.Count == 50, "The response's ClubList should have 50 clubs." );
            Assert.AreNotEqual( clubList.NextToken, "", "Expecting NextToken to be a non empty string." );

        }

        [TestMethod]
        public async Task GetClubCurrentlyShooting() {

            //Using production, to get more real values.
            var client = new ClubsAPIClient( Constants.X_API_KEY, APIStage.PRODUCTION );

            //as this call requires clubs to be shooting, the list may be empty (b/c no one is shooting)
            var request = new GetClubListPublicRequest();
            request.CurrentlyShooting = true;

            var getClubCurrentlyShooting = await client.GetClubListPublicAsync( request );

            Assert.AreEqual( System.Net.HttpStatusCode.OK, getClubCurrentlyShooting.StatusCode );

            var clubList = getClubCurrentlyShooting.ClubList.Items;

            //All clubs in the returned list (if there are any) should have a shot fired within the last 10 minutes.
            foreach( var club in clubList ) {
                Assert.IsTrue( (DateTime.UtcNow - club.LastPublicShot).TotalMinutes < 11.0);
            }

        }

        [TestMethod]
        public async Task CompareGetClubAbbr()
        {
            var comparerAcctNum = new CompareClubAbbr(CompareClubAbbr.CompareMethod.ACCOUNT_NUMBER, Scopos.BabelFish.Helpers.SortBy.ASCENDING);

            var comparerName = new CompareClubAbbr(CompareClubAbbr.CompareMethod.NAME, Scopos.BabelFish.Helpers.SortBy.ASCENDING);

            var comparerShooting = new CompareClubAbbr(CompareClubAbbr.CompareMethod.IS_SHOOTING, Scopos.BabelFish.Helpers.SortBy.ASCENDING);
            var clubAbbr1 = new ClubAbbr
            {
                AccountNumber = 1,
                Name = "AAAA",
                LastPublicShot = DateTime.UtcNow.AddMinutes(-30) //isCurrentlyShooting = false
            };
            var clubAbbr2 = new ClubAbbr
            {
                AccountNumber = 15,
                Name = "BBBB",
                LastPublicShot = DateTime.UtcNow.AddMinutes(-2) //isCurrentlyShooting = true
            };
            var clubAbbr3 = new ClubAbbr
            {
                AccountNumber = 2035,
                Name = "CCCC",
                LastPublicShot = DateTime.UtcNow.AddMinutes(-120) //isCurrentlyShooting = false
            };
            var clubAbbr4 = new ClubAbbr
            {
                AccountNumber = 3,
                Name = "DDDD",
                LastPublicShot = DateTime.UtcNow.AddMinutes(-0) //isCurrentlyShooting = true
            };

            Assert.IsTrue(comparerAcctNum.Compare(clubAbbr1, clubAbbr2) < 0); // -1 = 1 compareTo 2
            Assert.IsTrue(comparerAcctNum.Compare(clubAbbr1, clubAbbr3) < 0); // -1 = 1 compareTo 3
            Assert.IsTrue(comparerAcctNum.Compare(clubAbbr1, clubAbbr4) < 0); // -1 = 1 compareTo 4
            Assert.IsTrue(comparerAcctNum.Compare(clubAbbr3, clubAbbr2) > 0); // 1 = 3 compareTo 2

            Assert.IsTrue(comparerName.Compare(clubAbbr1, clubAbbr2) < 0); // -1 = 1 compareTo 2
            Assert.IsTrue(comparerName.Compare(clubAbbr1, clubAbbr3) < 0); // -1 = 1 compareTo 3
            Assert.IsTrue(comparerName.Compare(clubAbbr1, clubAbbr4) < 0); // -1 = 1 compareTo 4
            Assert.IsTrue(comparerName.Compare(clubAbbr3, clubAbbr2) > 0); // 1 = 3 compareTo 2

            Assert.IsTrue(comparerShooting.Compare(clubAbbr1, clubAbbr2) < 0); // -1 = 1 compareTo 2
            Assert.IsTrue(comparerShooting.Compare(clubAbbr1, clubAbbr3) == 0); // -1 = 1 compareTo 3
            Assert.IsTrue(comparerShooting.Compare(clubAbbr1, clubAbbr4) < 0); // -1 = 1 compareTo 4
            Assert.IsTrue(comparerShooting.Compare(clubAbbr3, clubAbbr2) < 0); // 1 = 3 compareTo 2

        }

    }
}
