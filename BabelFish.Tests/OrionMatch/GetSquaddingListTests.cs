using System.Net;
using System.Threading.Tasks;
using Scopos.BabelFish.APIClients;
using Scopos.BabelFish.DataModel.OrionMatch;
using Scopos.BabelFish.Requests.OrionMatchAPI;
using Scopos.BabelFish.Runtime.Authentication;

namespace Scopos.BabelFish.Tests.OrionMatch {

    [TestClass]
    public class GetSquaddingListTests : BaseTestClass {


        [TestMethod]
        public void GetSquaddingListBasicPublicTest() {

            var client = new OrionMatchAPIClient( APIStage.BETA );

            //This match id has three relays of 20 athletes
            var matchId = new MatchID( "1.1.2023022315342668.0" );
            var squaddingListName = "Individual";

            var taskSquaddingListResponse = client.GetSquaddingListPublicAsync( matchId, squaddingListName );
            var squaddingListResponse = taskSquaddingListResponse.Result;

            Assert.AreEqual( HttpStatusCode.OK, squaddingListResponse.StatusCode );
            var squaddingList = squaddingListResponse.SquaddingList;

            Assert.AreEqual( matchId.ToString(), squaddingList.MatchID );
            Assert.AreEqual( squaddingListName, squaddingList.EventName );

            Assert.IsTrue( squaddingList.Items.Count > 0 );
        }

        [TestMethod]
        public async Task GetSquaddingListBasicAuthenticatedTest() {

            var client = new OrionMatchAPIClient( APIStage.BETA );
            var userAuthentication = new UserAuthentication(
                Constants.TestDev7Credentials.Username,
                Constants.TestDev7Credentials.Password );
            await userAuthentication.InitializeAsync();

            //This match id has three relays of 20 athletes
            var matchId = new MatchID( "1.1.2023022315342668.0" );
            var squaddingListName = "Individual";

            var taskSquaddingListResponse = client.GetSquaddingListAuthenticatedAsync( matchId, squaddingListName, userAuthentication );
            var squaddingListResponse = taskSquaddingListResponse.Result;

            Assert.AreEqual( HttpStatusCode.OK, squaddingListResponse.StatusCode );
            var squaddingList = squaddingListResponse.SquaddingList;

            Assert.AreEqual( matchId.ToString(), squaddingList.MatchID );
            Assert.AreEqual( squaddingListName, squaddingList.EventName );

            Assert.IsTrue( squaddingList.Items.Count > 0 );
        }

        /// <summary>
        /// Tests that we can ask for a specific relay of athlete, and only get that relay in the response.
        /// </summary>
        [TestMethod]
        public void GetSquaddingListLimitToRelay() {

            var client = new OrionMatchAPIClient( APIStage.BETA );

            //This match id has three relays of 20 athletes
            var matchId = new MatchID( "1.1.2023022315342668.0" );
            var squaddingListName = "Individual";

            //Ask for all squadding assignments on relay 2
            var relayName = "2";
            var taskSquaddingListResponse = client.GetSquaddingListPublicAsync( matchId, squaddingListName, relayName );
            var squaddingListResponse = taskSquaddingListResponse.Result;

            Assert.AreEqual( HttpStatusCode.OK, squaddingListResponse.StatusCode );
            var squaddingList = squaddingListResponse.SquaddingList;

            Assert.IsTrue( squaddingList.Items.Count > 0 );
            foreach( var squadding in  squaddingList.Items ) {
                //Convert the SquaddingAssignment to their concrete class
                var fpAssignment = (SquaddingAssignmentFiringPoint)squadding.SquaddingAssignment;

                Assert.AreEqual( relayName, fpAssignment.Relay );
            }
        }

        /// <summary>
        /// Tests that the initial response indicates we have a partial list, 
        /// then uses the toekn to return the remaining list
        /// </summary>
        [TestMethod]
        public void GetSquaddingListTokenizedCalls() {

            var client = new OrionMatchAPIClient( APIStage.BETA );

            //This match id has three relays of 20 athletes
            var matchId = new MatchID( "1.1.2023022315342668.0" );
            var squaddingListName = "Individual";

            var taskSquaddingListResponse1 = client.GetSquaddingListPublicAsync( matchId, squaddingListName );
            var squaddingListResponse1 = taskSquaddingListResponse1.Result;

            Assert.AreEqual( HttpStatusCode.OK, squaddingListResponse1.StatusCode );
            var squaddingList1 = squaddingListResponse1.SquaddingList;

            //We should have a token, and should have 50 items inthe list.
            Assert.AreNotEqual( "", squaddingList1.NextToken );
            Assert.AreEqual( 50, squaddingList1.Items.Count );

            //Now use the next token to make the next call.
            var nextRequest = (GetSquaddingListPublicRequest) squaddingListResponse1.GetNextRequest();
            var taskSquaddingListResponse2 = client.GetSquaddingListPublicAsync( nextRequest );
            var squaddingListResponse2 = taskSquaddingListResponse2.Result;

            Assert.AreEqual( HttpStatusCode.OK, squaddingListResponse2.StatusCode );
            var squaddingList2 = squaddingListResponse2.SquaddingList;
            Assert.AreEqual( "", squaddingList2.NextToken );
            Assert.AreEqual( 10, squaddingList2.Items.Count );
        }
    }
}
