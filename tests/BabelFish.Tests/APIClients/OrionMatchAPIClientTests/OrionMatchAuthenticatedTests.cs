using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Scopos.BabelFish.APIClients;
using Scopos.BabelFish.DataModel.Common;
using Scopos.BabelFish.DataModel.OrionMatch;
using Scopos.BabelFish.Responses.OrionMatchAPI;
using Scopos.BabelFish.Runtime.Authentication;

namespace Scopos.BabelFish.Tests.APIClients.OrionMatchAPIClientTests {
    [TestClass]
    public class OrionMatchAuthenticatedUnitTests : BaseTestClass {

        [TestMethod]
        public async Task OrionMatchAPI_GetAMatch() {

            var client = new OrionMatchAPIClient( APIStage.BETA );
            var userAuthentication = new UserAuthentication(
                Constants.TestDev7Credentials.Username,
                Constants.TestDev7Credentials.Password );
            await userAuthentication.InitializeAsync();

            var matchId = new MatchID( "1.1.2023011915575119.0" );
            var response = await client.GetMatchAuthenticatedAsync( matchId, userAuthentication );

            Assert.AreEqual( HttpStatusCode.OK, response.RestApiStatusCode );

            var match = response.Match;

            //Perform some simple tests on the returned data.
            Assert.AreEqual( matchId.ToString(), match.MatchID.ToString() );
            Assert.AreEqual( "Unit Test Match", match.Name );
            Assert.AreEqual( VisibilityOption.PUBLIC, match.Visibility );
            Assert.AreEqual( "2023-01-19", match.StartDate.ToString( DateTimeFormats.DATE_FORMAT ) );
        }

        [TestMethod]
        public async Task OrionMatchAPI_GetAMatchIncludesApprovedTournamentMembershipInMetadata() {

            var client = new OrionMatchAPIClient( APIStage.BETA );
            var userAuthentication = new UserAuthentication(
                Constants.TestDev7Credentials.Username,
                Constants.TestDev7Credentials.Password );
            await userAuthentication.InitializeAsync();

            var matchId = new MatchID( "1.1.2020090810262303.1" );
            var response = await client.GetMatchAuthenticatedAsync( matchId, userAuthentication );

            Assert.AreEqual( HttpStatusCode.OK, response.RestApiStatusCode );
            Assert.IsInstanceOfType( response.MetaData, typeof( MatchDetailMetaData ) );

            var matchMetaData = (MatchDetailMetaData)response.MetaData;
            Assert.IsTrue( matchMetaData.Tournaments.Count > 0, "Expected the match metadata to include at least one tournament." );

            var tournament = matchMetaData.Tournaments.FirstOrDefault( x =>
                x.TournamentMembers.Any( member => member.MatchID.Equals( matchId ) ) );
            Assert.IsNotNull( tournament, "Expected one of the metadata tournaments to include the requested match as a member." );

            var tournamentMember = tournament.TournamentMembers.FirstOrDefault( member => member.MatchID.Equals( matchId ) );
            Assert.IsNotNull( tournamentMember, "Expected to find the requested match in the tournament member list." );
            Assert.AreEqual( ApprovalStatus.APPROVED, tournamentMember.ApprovalStatus );
        }
    }
}
