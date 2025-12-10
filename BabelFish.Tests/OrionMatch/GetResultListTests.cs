using System.Diagnostics;
using System.Net;
using System.Threading.Tasks;
using Scopos.BabelFish.APIClients;
using Scopos.BabelFish.DataActors.ResultListFormatter;
using Scopos.BabelFish.DataActors.ResultListFormatter.UserProfile;
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
                ParticipantRanks = new DataModel.Definitions.ValueSeries( "1..8" )
            };
            var listOfParticipantsInRL = automation.GetParticipantListToApply(response.ResultList);
            foreach (var m in listOfParticipantsInRL)
            {
                Debug.WriteLine(m.ToString());
            }

        }

        [TestMethod]
        public async Task EriksPlayground() {

            var client = new OrionMatchAPIClient();

            var matchId = new MatchID( "1.1.2025100109364878.1" );

            //Retreives information about the match
            var getMatchResponse = await client.GetMatchAsync(matchId);
            if (getMatchResponse.HasOkStatusCode) {
                var match = getMatchResponse.Match;
                Console.WriteLine( match.Name );
                Console.WriteLine( match.StartDate );
                Console.WriteLine( match.EndDate );

                //Loop through and find the primary result lists
                foreach( var resultEvent in match.ResultEvents ) {
                    foreach (var resultListAbbr in resultEvent.ResultLists) {
                        if (resultListAbbr.Primary) {

                            //Retreives the result list (note this command only reteives the start of the list).
                            var getResultListResponse = await client.GetResultListAsync( matchId, resultListAbbr.ResultName );
                            if (getResultListResponse.HasOkStatusCode) {

                                var resultList = getResultListResponse.ResultList;

                                //Reteive the recommended RESULT LIST FORMAT to use on this Result List
                                var resultListFormat = await resultList.GetResultListFormatDefinitionAsync();

                                //Instantiate a Result List Intermediate Formatted instance, to easily allow us to print out the results.
                                var rlif = new ResultListIntermediateFormatted( resultList, resultListFormat, new BaseUserProfileLookup() );
                                await rlif.InitializeAsync();

                                //For demo purposes, just show the top 3 participants.
                                rlif.ShowNumberOfBodyRows = 0;
                                rlif.ShowNumberOfChildRows = 0;
                                rlif.ShowRanks = 3;

                                //Pretend we are on a wide screen.
                                rlif.ResolutionWidth = 5000;


                                Console.WriteLine( $"Show results for {resultList.Name}" );

                                //Print the header row
                                foreach (var colIndex in rlif.GetShownColumnIndexes()) {
                                    Console.Write( rlif.GetColumnHeaderCell( colIndex ).Text );
                                    Console.Write( "  " );
                                }
                                Console.WriteLine();

                                //Print the results, one row at a time.
                                foreach (var row in rlif.ShownRows) {
                                    foreach( var colIndex in rlif.GetShownColumnIndexes() ) {
                                        Console.Write( row.GetColumnBodyCell( colIndex ).Text );
                                        Console.Write( "  " );
                                    }
                                    Console.WriteLine();
                                }

                            }
                            Console.WriteLine();
                        }
                    }
                }
            }
        }
    }
}
