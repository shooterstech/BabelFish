using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Scopos.BabelFish.APIClients;
using Scopos.BabelFish.DataActors.OrionMatch;
using Scopos.BabelFish.DataModel.OrionMatch;

namespace Scopos.BabelFish.Tests.OrionMatch {
    [TestClass]
    public class ResultListExtensionsTests : BaseTestClass {

        [TestMethod]
        public async Task GetResultListBasicPublicTest() {

            var client = new OrionMatchAPIClient( APIStage.BETA );

            //This match id has three relays of 20 athletes
            var matchId = new MatchID( "1.1.2023011915575119.0" );
            var resultListName = "Individual - All";

            var getResultListResponse = await client.GetResultListPublicAsync( matchId, resultListName );

            Assert.AreEqual( HttpStatusCode.OK, getResultListResponse.RestApiStatusCode );
            var resultList = getResultListResponse.ResultList;
            ResultStatus status = ResultStatus.FUTURE;
            //resultList.CalculateResultListStatus(status);

            Assert.AreEqual( matchId.ToString(), resultList.MatchID );
            Assert.AreEqual( resultListName, resultList.ResultName );

            Assert.IsTrue( resultList.Items.Count > 0 );
        }

        /// <summary>
        /// Tests the exstention method, for a SquaddingList, .ListOfRelays(), that returns
        /// a list of relay names that one more more participants in the squadding list is
        /// assigned to.
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task ListOfRelaysTests() {

            //
			var client = new OrionMatchAPIClient( );
			var matchId = new MatchID( "1.1.2025081213222434.0" );
            var getSquaddingListResponse = await client.GetSquaddingListPublicAsync( matchId, "Qualification" );

            var squaddingList = getSquaddingListResponse.SquaddingList;

            var referenceList = new List<string>() { "1", "2", "3", "4" };
            var list1 = squaddingList.ListOfRelays();

            Assert.IsTrue( referenceList.SequenceEqual( list1 ) );

            var sorter = new CompareSquadding( CompareSquadding.CompareMethod.GIVENNAME_FAMILYNAME, Scopos.BabelFish.Helpers.SortBy.DESCENDING );
            squaddingList.Items.Sort( sorter );

            var list2 = squaddingList.ListOfRelays();

			Assert.IsTrue( referenceList.SequenceEqual( list1 ) );
		}
    }
}
