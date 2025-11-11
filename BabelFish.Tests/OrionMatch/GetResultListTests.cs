using System.Diagnostics;
using System.Net;
using System.Threading.Tasks;
using Scopos.BabelFish.APIClients;
using Scopos.BabelFish.DataModel.OrionMatch;
using Scopos.BabelFish.Requests.OrionMatchAPI;
using Scopos.BabelFish.Runtime.Authentication;

namespace Scopos.BabelFish.Tests.OrionMatch {
    [TestClass]
    public class GetResultListTests : BaseTestClass {

        [TestMethod]
        public async Task GetResultListBasicPublicTest() {

            var client = new OrionMatchAPIClient( APIStage.BETA );

            //This match id has three relays of 20 athletes
            var matchId = new MatchID( "1.1.2023011915575119.0" );
            var resultListName = "Individual - All";

            var getResultListResponse = await client.GetResultListPublicAsync( matchId, resultListName );

            Assert.AreEqual( HttpStatusCode.OK, getResultListResponse.RestApiStatusCode );
            var resultList = getResultListResponse.ResultList;

            Assert.AreEqual( matchId.ToString(), resultList.MatchID );
            Assert.AreEqual( resultListName, resultList.ResultName );

            Assert.IsTrue( resultList.Items.Count > 0 );
        }

        [TestMethod]
        public async Task GetResultListBasicAuthenticatedTest() {

            var client = new OrionMatchAPIClient( APIStage.BETA );
            var userAuthentication = new UserAuthentication(
                Constants.TestDev7Credentials.Username,
                Constants.TestDev7Credentials.Password );
            await userAuthentication.InitializeAsync();

            //This match id has three relays of 20 athletes
            var matchId = new MatchID( "1.1.2023011915575119.0" );
            var resultListName = "Individual - All";

            var taskResultListResponse = client.GetResultListAuthenticatedAsync( matchId, resultListName, userAuthentication );
            var resultListResponse = taskResultListResponse.Result;

            Assert.AreEqual( HttpStatusCode.OK, resultListResponse.RestApiStatusCode );
            var resultList = resultListResponse.ResultList;

            Assert.AreEqual( matchId.ToString(), resultList.MatchID );
            Assert.AreEqual( resultListName, resultList.ResultName );

            Assert.IsTrue( resultList.Items.Count > 0 );
        }

        [TestMethod]
        public void GetResultListTokenizedTest() {

            var client = new OrionMatchAPIClient( APIStage.BETA );

            //This match id has three relays of 20 athletes
            var matchId = new MatchID( "1.1.2023011915575119.0" ); 
            var resultListName = "Individual - All";

            var requestInit = new GetResultListPublicRequest( matchId, resultListName ) {
                Limit = 2 //Setting to a small value so we get the next token in the call.
            };
            var taskResultListResponseInit = client.GetResultListPublicAsync( requestInit );
            var resultListResponseInit = taskResultListResponseInit.Result;

            Assert.AreEqual( HttpStatusCode.OK, resultListResponseInit.RestApiStatusCode );
            var resultListInit = resultListResponseInit.ResultList;

            Assert.AreEqual( matchId.ToString(), resultListInit.MatchID );
            Assert.AreEqual( resultListName, resultListInit.ResultName );
            Assert.IsTrue( resultListInit.Items.Count > 0 );
            Assert.AreNotEqual( "", resultListInit.NextToken );

            //Set up the next request
            var requestNext = (GetResultListPublicRequest) resultListResponseInit.GetNextRequest();
            Assert.AreEqual( resultListInit.NextToken, requestNext.Token );

            var taskResultListResponseNext = client.GetResultListPublicAsync( requestNext );
            var resultListResponseNext = taskResultListResponseNext.Result;

            Assert.AreEqual( HttpStatusCode.OK, resultListResponseNext.RestApiStatusCode );
            var resultListNext = resultListResponseNext.ResultList;

            Assert.AreEqual( matchId.ToString(), resultListNext.MatchID );
            Assert.AreEqual( resultListName, resultListNext.ResultName );
            Assert.IsTrue( resultListNext.Items.Count > 0 );
            Assert.AreNotEqual( "", resultListNext.NextToken );
        }

        [TestMethod]
        public async Task LiamsPlayground()
        {

            var client = new OrionMatchAPIClient(APIStage.PRODUCTION);

            var matchId = new MatchID("1.15.2025102016585824.0");
            //var resultListName = "Individual - All";
            //var resultListName = "Individual - Sporter";
            var resultListName = "Individual - Precision";

            var response = await client.GetResultListPublicAsync(matchId, resultListName);
            DataModel.Definitions.CommandAutomationRemark automation = new DataModel.Definitions.CommandAutomationRemark() {
                ParticipantRanks = "1..8"
            };
            var listOfParticipantsInRL = automation.GetParticipantListToApply(response.ResultList);
            foreach (var m in listOfParticipantsInRL)
            {
                Debug.WriteLine(m.ToString());
            }

        }

        [TestMethod]
        public async Task EriksPlayground() {

            var client = new OrionMatchAPIClient( APIStage.PRODUCTION );

            var matchId = new MatchID( "1.3987.2025012213525977.0" );
            var resultListName = "Individual - All";
           
            var response = await client.GetResultListPublicAsync( matchId, resultListName );
            foreach (var m in response.MessageResponse.Message) {
                Debug.WriteLine( m );
            }

        }
    }
}
