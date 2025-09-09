using System.Net;
using System.Threading.Tasks;
using Scopos.BabelFish.APIClients;
using Scopos.BabelFish.Requests.OrionMatchAPI;
using Scopos.BabelFish.Runtime.Authentication;

namespace Scopos.BabelFish.Tests.OrionMatch {

    [TestClass]
    public class GetResultCOFTests : BaseTestClass {

        [TestMethod]
        public async Task GetResultCOFPublicTest() {

            var client = new OrionMatchAPIClient( APIStage.BETA );

            string resultCofId = "4608b306-8b6d-40c2-b608-e5375d05bd12";

            var response = await client.GetResultCourseOfFireDetailPublicAsync( resultCofId );
            Assert.AreEqual( HttpStatusCode.OK, response.RestApiStatusCode );

            //This result cof should of 30 shots, and 8 event scores
            var resultCof = response.ResultCOF;
            Assert.AreEqual( 30, resultCof.Shots.Count );
            Assert.AreEqual( 8, resultCof.EventScores.Count );
        }

        [TestMethod]
        public async Task GetResultCOFAuthenticatedTest() {

            var client = new OrionMatchAPIClient( APIStage.BETA );
            var userAuthentication = new UserAuthentication(
                Constants.TestDev7Credentials.Username,
                Constants.TestDev7Credentials.Password );
            await userAuthentication.InitializeAsync();

            string resultCofId = "4608b306-8b6d-40c2-b608-e5375d05bd12";

            var response = await client.GetResultCourseOfFireDetailAuthenticatedAsync( resultCofId, userAuthentication );
            Assert.AreEqual( HttpStatusCode.OK, response.RestApiStatusCode );

            var resultCof = response.ResultCOF;
            Assert.AreEqual( 30, resultCof.Shots.Count );
            Assert.AreEqual( 8, resultCof.EventScores.Count );
        }

        /// <summary>
        /// Testing that the request object gets marked to use authenticated calls
        /// </summary>
        [TestMethod]
        public void GetResultCOFDetailAuthenticatedRequestRequiresAuthenticationTest() {

            var userAuthentication = new UserAuthentication(
                Constants.TestDev7Credentials.Username,
                Constants.TestDev7Credentials.Password );

            string resultCofId = "4608b306-8b6d-40c2-b608-e5375d05bd12";

            var request = new GetResultCOFAuthenticatedRequest( resultCofId, userAuthentication );

            Assert.AreEqual( true, request.RequiresCredentials );
        }
    }
}
