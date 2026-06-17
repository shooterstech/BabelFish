using System.Threading.Tasks;
using Scopos.BabelFish.APIClients;
using Scopos.BabelFish.DataActors.Clubs;
using Scopos.BabelFish.DataModel.Clubs;
using Scopos.BabelFish.Requests.ClubsAPI;
using Scopos.BabelFish.Runtime.Authentication;

namespace Scopos.BabelFish.Tests.APIClients.ClubAPIClientTests {

    [TestClass]
    public class ClubApiClientTests : BaseTestClass {

        /* 
         * Orion Account Number 16 is the official unit testing account for the Clubs API.
         */

        /// <summary>
        /// Unit test to confirm the Constructors set the api key and API stage as expected.
        /// </summary>
        [TestMethod]
        public void BasicConstructorTests() {

            var defaultConstructorClient = new ClubsAPIClient();
            var apiStageConstructorClient = new ClubsAPIClient( APIStage.BETA );

            Assert.AreEqual( APIStage.PRODUCTION, defaultConstructorClient.ApiStage );

            Assert.AreEqual( APIStage.BETA, apiStageConstructorClient.ApiStage );
        }

        [TestMethod]
        public async Task GetClubListSmallList() {

            var client = new ClubsAPIClient( APIStage.PRODUCTION );

            //Test Dev 7 is associated with two clubs.
            var userAuthentication = new UserAuthentication(
                Constants.TestDev7Credentials.Username,
                Constants.TestDev7Credentials.Password );
            await userAuthentication.InitializeAsync();
            var request = new GetClubListAuthenticatedRequest( userAuthentication );

            var response = await client.GetClubListAuthenticatedAsync( request );

            Assert.AreEqual( System.Net.HttpStatusCode.OK, response.RestApiStatusCode );

            var clubList = response.ClubList;

            Assert.IsTrue( clubList.Items.Count > 0, "The response's ClubList should have a length greather than zero." );

            Assert.AreEqual( string.Empty, clubList.NextToken, "Expecting NextToken to be an empty string with user test_dev_7." );

        }


        [TestMethod]
        public async Task GetClubDetailAuthenticated() {

            var client = new ClubsAPIClient( APIStage.PRODUCTION );

            var ownerId = "OrionAcct000016";
            var userAuthentication = new UserAuthentication(
                Constants.TestDev1Credentials.Username,
                Constants.TestDev1Credentials.Password );
            await userAuthentication.InitializeAsync();
            var request = new GetClubDetailAuthenticatedRequest( ownerId, userAuthentication );

            var response = await client.GetClubDetailAuthenticatedAsync( request );

            Assert.AreEqual( System.Net.HttpStatusCode.OK, response.RestApiStatusCode );

            var clubDetail = response.ClubDetail;

            Assert.AreEqual( ownerId, clubDetail.OwnerId, "Expecting the OwnerId to match, what was sent." );

            Assert.IsTrue( clubDetail.LicenseList.Count > 0, "Expecting the length of the license list is greather than zero." );
        }

        [TestMethod]
        public async Task GetClubDetailPublic() {


            var client = new ClubsAPIClient( APIStage.PRODUCTION );

            var ownerId = "OrionAcct000016";
            var request = new GetClubDetailPublicRequest( ownerId );

            var response = await client.GetClubDetailPublicAsync( request );

            Assert.AreEqual( System.Net.HttpStatusCode.OK, response.RestApiStatusCode );

            var clubDetail = response.ClubDetail;

            Assert.AreEqual( ownerId, clubDetail.OwnerId, "Expecting the OwnerId to match, what was sent." );
        }

        [TestMethod]
        public async Task GetClubListPublic() {

            //Using production, to get more real values.
            var client = new ClubsAPIClient( APIStage.PRODUCTION );

            //Should return all clubs without any parameters ... or at least up to the token limit
            var request = new GetClubListPublicRequest();
            request.CurrentlyShooting = GetClubListPublicRequest.SearchParameterState.IGNORE;

            var getAllClubsResponse = await client.GetClubListPublicAsync( request );

            Assert.AreEqual( System.Net.HttpStatusCode.OK, getAllClubsResponse.RestApiStatusCode );

            var clubList = getAllClubsResponse.ClubList;

            Assert.AreEqual( 200, clubList.Items.Count, "The response's ClubList should have 200 clubs." );
            Assert.AreNotEqual( string.Empty, clubList.NextToken, "Expecting NextToken to be a non empty string." );

        }

        [TestMethod]
        public async Task GetClubCurrentlyShooting() {

            //Using production, to get more real values.
            var client = new ClubsAPIClient( APIStage.PRODUCTION );

            //as this call requires clubs to be shooting, the list may be empty (b/c no one is shooting)
            var request = new GetClubListPublicRequest();
            request.CurrentlyShooting = GetClubListPublicRequest.SearchParameterState.MUST_HAVE;

            var getClubCurrentlyShooting = await client.GetClubListPublicAsync( request );

            Assert.AreEqual( System.Net.HttpStatusCode.OK, getClubCurrentlyShooting.RestApiStatusCode );

            var clubList = getClubCurrentlyShooting.ClubList.Items;

            //All clubs in the returned list (if there are any) should have a shot fired within the last 10 minutes.
            foreach (var club in clubList) {
                Assert.IsTrue( (DateTime.UtcNow - club.LastPublicShot).TotalMinutes < 11.0 );
            }

        }

        [TestMethod]
        public async Task GetClubSearchStuff() {

            //Using production, to get more real values.
            var client = new ClubsAPIClient( APIStage.PRODUCTION );

            //as this call requires clubs to be shooting, the list may be empty (b/c no one is shooting)
            var request = new GetClubListPublicRequest();
            request.ShowAll = GetClubListPublicRequest.SearchParameterState.IGNORE;
            request.EnabledRezults = GetClubListPublicRequest.SearchParameterState.MUST_HAVE;
            request.ActiveLicense = GetClubListPublicRequest.SearchParameterState.MUST_HAVE;
            request.OrionForClubs = GetClubListPublicRequest.SearchParameterState.IGNORE;
            request.OrionAtHome = GetClubListPublicRequest.SearchParameterState.IGNORE;
            request.AthenaForClubs = GetClubListPublicRequest.SearchParameterState.IGNORE;
            request.CurrentlyShooting = GetClubListPublicRequest.SearchParameterState.IGNORE;

            var getClubCurrentlyShooting = await client.GetClubListPublicAsync( request );

            Assert.AreEqual( System.Net.HttpStatusCode.OK, getClubCurrentlyShooting.RestApiStatusCode );

            var clubList = getClubCurrentlyShooting.ClubList.Items;

            Assert.IsTrue( clubList.Count == 50, "The response's ClubList should have 50 clubs." );

            var club1 = clubList[0]; //This should just be eriks account.
            Console.WriteLine( club1 );
            Assert.IsNotNull( club1.Location );
            Assert.IsTrue( club1.Location.IsKnown );
            Assert.IsNotNull( club1.Location.Longitude );
            Assert.IsNotNull( club1.Location.Latitude );

        }

        [TestMethod]
        public async Task CompareGetClubAbbr() {

            var comparerAcctNum = new CompareClubAbbr( CompareClubAbbr.CompareMethod.ACCOUNT_NUMBER, Scopos.BabelFish.Helpers.SortBy.ASCENDING );

            var comparerName = new CompareClubAbbr( CompareClubAbbr.CompareMethod.NAME, Scopos.BabelFish.Helpers.SortBy.ASCENDING );

            var comparerShooting = new CompareClubAbbr( CompareClubAbbr.CompareMethod.IS_SHOOTING, Scopos.BabelFish.Helpers.SortBy.ASCENDING );
            var clubAbbr1 = new ClubAbbr {
                AccountNumber = 1,
                Name = "AAAA",
                LastPublicShot = DateTime.UtcNow.AddMinutes( -30 ) //isCurrentlyShooting = false
            };
            var clubAbbr2 = new ClubAbbr {
                AccountNumber = 15,
                Name = "BBBB",
                LastPublicShot = DateTime.UtcNow.AddMinutes( -2 ) //isCurrentlyShooting = true
            };
            var clubAbbr3 = new ClubAbbr {
                AccountNumber = 2035,
                Name = "CCCC",
                LastPublicShot = DateTime.UtcNow.AddMinutes( -120 ) //isCurrentlyShooting = false
            };
            var clubAbbr4 = new ClubAbbr {
                AccountNumber = 3,
                Name = "DDDD",
                LastPublicShot = DateTime.UtcNow.AddMinutes( -0 ) //isCurrentlyShooting = true
            };

            Assert.IsTrue( comparerAcctNum.Compare( clubAbbr1, clubAbbr2 ) < 0 ); // -1 = 1 compareTo 2
            Assert.IsTrue( comparerAcctNum.Compare( clubAbbr1, clubAbbr3 ) < 0 ); // -1 = 1 compareTo 3
            Assert.IsTrue( comparerAcctNum.Compare( clubAbbr1, clubAbbr4 ) < 0 ); // -1 = 1 compareTo 4
            Assert.IsTrue( comparerAcctNum.Compare( clubAbbr3, clubAbbr2 ) > 0 ); // 1 = 3 compareTo 2

            Assert.IsTrue( comparerName.Compare( clubAbbr1, clubAbbr2 ) < 0 ); // -1 = 1 compareTo 2
            Assert.IsTrue( comparerName.Compare( clubAbbr1, clubAbbr3 ) < 0 ); // -1 = 1 compareTo 3
            Assert.IsTrue( comparerName.Compare( clubAbbr1, clubAbbr4 ) < 0 ); // -1 = 1 compareTo 4
            Assert.IsTrue( comparerName.Compare( clubAbbr3, clubAbbr2 ) > 0 ); // 1 = 3 compareTo 2

            Assert.IsTrue( comparerShooting.Compare( clubAbbr1, clubAbbr2 ) < 0 ); // -1 = 1 compareTo 2
            Assert.IsTrue( comparerShooting.Compare( clubAbbr1, clubAbbr3 ) == 0 ); // -1 = 1 compareTo 3
            Assert.IsTrue( comparerShooting.Compare( clubAbbr1, clubAbbr4 ) < 0 ); // -1 = 1 compareTo 4
            Assert.IsTrue( comparerShooting.Compare( clubAbbr3, clubAbbr2 ) < 0 ); // 1 = 3 compareTo 2

        }

    }
}
