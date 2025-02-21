using System.Net;
using System.Threading.Tasks;
using Scopos.BabelFish.APIClients;
using Scopos.BabelFish.DataModel.OrionMatch;
using Scopos.BabelFish.Requests.OrionMatchAPI;
using Scopos.BabelFish.Runtime.Authentication;

namespace Scopos.BabelFish.Tests.OrionMatch {

    [TestClass]
    public class GetMatchParticipantListTests : BaseTestClass {


        [TestMethod]
        public void GetMatchParticipantListPublicTest() {

            var client = new OrionMatchAPIClient( APIStage.BETA );

            //This match id has three relays of 20 athletes
            var matchId = new MatchID( "1.1.2023022315342668.0" );

            var taskMatchParticipantListResponse = client.GetMatchParticipantListPublicAsync( matchId );
            var matchParticipantListResponse = taskMatchParticipantListResponse.Result;

            Assert.AreEqual( HttpStatusCode.OK, matchParticipantListResponse.StatusCode );
            var matchParticipantList = matchParticipantListResponse.MatchParticipantList;

            Assert.AreEqual( matchId.ToString(), matchParticipantList.MatchID );

            Assert.IsTrue( matchParticipantList.Items.Count > 0 );
        }

        [TestMethod]
        public async Task GetMatchParticipantListAuthenteicatedTest() {

            var client = new OrionMatchAPIClient( APIStage.BETA );
            var userAuthentication = new UserAuthentication(
                Constants.TestDev7Credentials.Username,
                Constants.TestDev7Credentials.Password );
            await userAuthentication.InitializeAsync();

            //This match id has three relays of 20 athletes
            var matchId = new MatchID( "1.1.2023022315342668.0" );

            var taskMatchParticipantListResponse = client.GetMatchParticipantListAuthenticatedAsync( matchId, userAuthentication );
            var matchParticipantListResponse = taskMatchParticipantListResponse.Result;

            Assert.AreEqual( HttpStatusCode.OK, matchParticipantListResponse.StatusCode );
            var matchParticipantList = matchParticipantListResponse.MatchParticipantList;

            Assert.AreEqual( matchId.ToString(), matchParticipantList.MatchID );

            Assert.IsTrue( matchParticipantList.Items.Count > 0 );
        }

        /// <summary>
        /// Tests that we can ask for a specific relay of athlete, and only get that relay in the response.
        /// </summary>
        [TestMethod]
        public void GetMatchParticipantListLimitToRelay() {

            var client = new OrionMatchAPIClient( APIStage.BETA );

            //This match id has three relays of 20 athletes
            var matchId = new MatchID( "1.1.2023022315342668.0" );

            //Ask for all matchParticipant assignments on relay 2
            var role = MatchParticipantRole.STATISTICAL_OFFICER;
            var taskMatchParticipantListResponse = client.GetMatchParticipantListPublicAsync( matchId, role );
            var matchParticipantListResponse = taskMatchParticipantListResponse.Result;

            Assert.AreEqual( HttpStatusCode.OK, matchParticipantListResponse.StatusCode );
            var matchParticipantList = matchParticipantListResponse.MatchParticipantList;

            Assert.IsTrue( matchParticipantList.Items.Count > 0 );
            foreach (var matchParticipantAssignment in matchParticipantList.Items) {

                Assert.IsTrue( matchParticipantAssignment.RoleList.Contains( role) );
            }
        }

        /// <summary>
        /// Tests that the initial response indicates we have a partial list, 
        /// then uses the toekn to return the remaining list
        /// </summary>
        [TestMethod]
        public void GetMatchParticipantListTokenizedCalls() {

            var client = new OrionMatchAPIClient( APIStage.BETA );

            //This match id has three relays of 20 athletes
            var matchId = new MatchID( "1.1.2023022315342668.0" );

            var taskMatchParticipantListResponse1 = client.GetMatchParticipantListPublicAsync( matchId );
            var matchParticipantListResponse1 = taskMatchParticipantListResponse1.Result;

            Assert.AreEqual( HttpStatusCode.OK, matchParticipantListResponse1.StatusCode );
            var matchParticipantList1 = matchParticipantListResponse1.MatchParticipantList;

            //We should have a token, and should have 50 items inthe list.
            Assert.AreNotEqual( "", matchParticipantList1.NextToken );
            Assert.AreEqual( 50, matchParticipantList1.Items.Count );

            //Now use the next token to make the next call.
            var nextRequest = (GetMatchParticipantListPublicRequest) matchParticipantListResponse1.GetNextRequest();
            var taskMatchParticipantListResponse2 = client.GetMatchParticipantListPublicAsync( nextRequest );
            var matchParticipantListResponse2 = taskMatchParticipantListResponse2.Result;

            Assert.AreEqual( HttpStatusCode.OK, matchParticipantListResponse2.StatusCode );
            var matchParticipantList2 = matchParticipantListResponse2.MatchParticipantList;
            Assert.AreEqual( "", matchParticipantList2.NextToken );
            Assert.AreEqual( 10, matchParticipantList2.Items.Count );
        }
    }
}
