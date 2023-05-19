using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text;
using System.Threading.Tasks;
using Scopos.BabelFish.Runtime;
using Scopos.BabelFish.Tests;
using Scopos.BabelFish.Requests.AthenaLogin;
using Scopos.BabelFish.DataModel.AthenaLogin;
using Scopos.BabelFish.APIClients;
using Scopos.BabelFish.Runtime.Authentication;

namespace Scopos.BabelFish.Tests.AthenaLogin {

    [TestClass]
    public class LoginTest {

        /// <summary>
        /// Unit test to confirm the Constructors set the api key and API stage as expected.
        /// </summary>
        [TestMethod]
        public void BasicConstructorTests() {

            var defaultConstructorClient = new AthenaLoginAPIClient( Constants.X_API_KEY );
            var apiStageConstructorClient = new AthenaLoginAPIClient( Constants.X_API_KEY, APIStage.BETA );

            Assert.AreEqual( Constants.X_API_KEY, defaultConstructorClient.XApiKey );
            Assert.AreEqual( APIStage.PRODUCTION, defaultConstructorClient.ApiStage );

            Assert.AreEqual( Constants.X_API_KEY, apiStageConstructorClient.XApiKey );
            Assert.AreEqual( APIStage.BETA, apiStageConstructorClient.ApiStage );
        }

        [TestMethod]
        public async Task LoginToTargetHappyPath() {

            var client = new AthenaLoginAPIClient( Constants.X_API_KEY, APIStage.BETA );

            //Test Dev 3 is associated with something ...
            var userAuthentication = new UserAuthentication(
                Constants.TestDev3Credentials.Username,
                Constants.TestDev3Credentials.Password );

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

            var client = new AthenaLoginAPIClient( Constants.X_API_KEY, APIStage.BETA );

            //Test Dev 3 is associated with something ...
            var userAuthentication = new UserAuthentication(
                Constants.TestDev3Credentials.Username,
                Constants.TestDev3Credentials.Password );
            //Pass in a code that is not real, and check the response comes back as .NotFound
            var authCode = "fakecode";
            var request = new AthenaEmployLoginCodeAuthenticatedRequest( authCode, userAuthentication );

            var response = await client.AthenaEmployLoginCodeAuthenticatedAsync( request );

            //When the auth code is wrong, unknown, or expired, and 404 is returned.
            Assert.AreEqual( System.Net.HttpStatusCode.NotFound, response.StatusCode );

            //The message list dhould incldue the message it's expired or invalid.
            var message = response.MessageResponse.Message[0];
            Assert.IsTrue( message.Contains( "not recognized, expired or is invalid" ) );

        }
    }
}
