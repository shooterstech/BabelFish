using System.Threading.Tasks;
using Scopos.BabelFish.APIClients;
using Scopos.BabelFish.Requests.Athena;
using Scopos.BabelFish.Runtime.Authentication;

namespace Scopos.BabelFish.Tests.Athena {

    [TestClass]
    public class LoginTest : BaseTestClass {
        
        /// <summary>
         /// Unit test to confirm the Constructors set the api key and API stage as expected.
         /// </summary>
        [TestMethod]
        public void BasicConstructorTests() {

            var defaultConstructorClient = new AthenaAPIClient( );
            var apiStageConstructorClient = new AthenaAPIClient( APIStage.BETA );

            Assert.AreEqual( APIStage.PRODUCTION, defaultConstructorClient.ApiStage );

            Assert.AreEqual( APIStage.BETA, apiStageConstructorClient.ApiStage );
        }

        [TestMethod]
        public async Task LoginToTargetHappyPath() {

            var client = new AthenaAPIClient( APIStage.BETA );

            //Test Dev 3 is associated with something ...
            var userAuthentication = new UserAuthentication(
                Constants.TestDev3Credentials.Username,
                Constants.TestDev3Credentials.Password );
            await userAuthentication.InitializeAsync();

            var authCode = "zzzzzz"; //test authcode that returns fake data with a 200 success code.
            var request = new AthenaEmployLoginCodeAuthenticatedRequest( authCode, userAuthentication );

            var response = await client.AthenaEmployLoginCodeAuthenticatedAsync( request );

            Assert.AreEqual( System.Net.HttpStatusCode.OK, response.StatusCode );

            var estTarget = response.ESTUnitLogin.ThingName;
            var loggedInUserId = response.ESTUnitLogin.Session.User.UserID;

            Assert.AreEqual( estTarget, "NotARealTarget" );

            Assert.AreEqual( Constants.TestDev3UserId, loggedInUserId );

        }

        [TestMethod]
        public async Task LoginToTargetSadPath() {

            var client = new AthenaAPIClient( APIStage.BETA );

            //Test Dev 3 is associated with something ...
            var userAuthentication = new UserAuthentication(
                Constants.TestDev3Credentials.Username,
                Constants.TestDev3Credentials.Password );
            await userAuthentication.InitializeAsync();

            //Pass in a code that is not real, and check the response comes back as .NotFound
            var authCode = "fakecode";
            var request = new AthenaEmployLoginCodeAuthenticatedRequest( authCode, userAuthentication );

            var response = await client.AthenaEmployLoginCodeAuthenticatedAsync( request );

            //When the auth code is wrong, unknown, or expired, and 404 is returned.
            Assert.AreEqual( System.Net.HttpStatusCode.NotFound, response.StatusCode );

        }

        [TestMethod]
        public async Task ActiveLoginSessions() {

            var client = new AthenaAPIClient( APIStage.BETA );

            //Test Dev 3 is associated with something ...
            var userAuthentication = new UserAuthentication(
                Constants.TestDev3Credentials.Username,
                Constants.TestDev3Credentials.Password );
            await userAuthentication.InitializeAsync();

            var request = new AthenaListActiveSessionsAuthenticatedRequest( userAuthentication );
            var response = await client.AthenaListActiveSessionsAuthenticatedAsync( request );

            //NOTE this is kinda difficult to write a unit test for, since there is no way to control
            //if there are legit active user sessions. At best we can test the response was expected.
            Assert.AreEqual( System.Net.HttpStatusCode.OK, response.StatusCode );

        }

        [TestMethod]
        public async Task LogoutSessions() {

            var client = new AthenaAPIClient( APIStage.BETA );

            //Test Dev 3 is associated with something ...
            var userAuthentication = new UserAuthentication(
                Constants.TestDev3Credentials.Username,
                Constants.TestDev3Credentials.Password );
            await userAuthentication.InitializeAsync();

            var request = new AthenaLogoutSessionAuthenticatedRequest( userAuthentication ) {
                ThingNames = new List<string>() { "ESTTarget-000000001" }
            };

            var response = await client.AthenaLogoutSessionAuthenticatedAsync( request );

            Assert.AreEqual( System.Net.HttpStatusCode.OK, response.StatusCode );

        }
    }
}
