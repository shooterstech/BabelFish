using System;
using System.Threading.Tasks;
using Scopos.BabelFish.APIClients;
using Scopos.BabelFish.DataActors.OrionMatch;
using Scopos.BabelFish.DataActors.ResultListFormatter;
using Scopos.BabelFish.DataActors.ResultListMerger;
using Scopos.BabelFish.DataModel.Common;
using Scopos.BabelFish.Requests.OrionMatchAPI;
using Scopos.BabelFish.Runtime.Authentication;
using Scopos.BabelFish.DataModel.OrionMatch;

namespace Scopos.BabelFish.Tests.OrionMatch.Tournament {
    [TestClass]
    public class GetTournamentTests : BaseTestClass {

        [TestMethod]
        public async Task BasicHappyPathGetTournamentTest() {

            var client = new OrionMatchAPIClient();
            var mId = new MatchID( "1.1.2025100211025190.2" );

            var request = new GetTournamentPublicRequest( mId );
            var response = await client.GetTournamentPublicAsync( request );

            Assert.IsTrue( response.HasOkStatusCode );

            var tournament = response.Tournament;
            Assert.AreEqual( mId, tournament.MatchId );
            Assert.AreEqual( mId, tournament.TournamentId );

            Assert.AreEqual( MatchType.TOURNAMENT, tournament.MatchType );

            Assert.IsTrue( tournament.TournamentMembers.Count > 0 );
            Assert.IsTrue( tournament.MergedResultLists.Count > 0 );
            Assert.IsTrue( tournament.MergedResultLists[0].Configuration is AverageMethodConfiguration );
        }

        [TestMethod]
        public async Task BasicHappyPathCreateTournamentWithRequestTest() {
            var client = new OrionMatchAPIClient(APIStage.BETA);
            var userAuthentication = new UserAuthentication(
                Constants.TestDev7Credentials.Username,
                Constants.TestDev7Credentials.Password );
            await userAuthentication.InitializeAsync();

            var tournamentName = $"BabelFish API Create Request Test {DateTime.UtcNow:yyyyMMddHHmmss}";
            var request = new CreateTournamentAuthenticatedRequest( userAuthentication);
            request.TournamentName = tournamentName;
            request.OwnerId = 2;
            request.Visibility = VisibilityOption.PUBLIC;
            request.ShowOnSearch = true;


            var response = await client.CreateTournamentAuthenticatedAsync( request );

            Assert.IsTrue( response.HasOkStatusCode );
            Assert.IsNotNull( response.Tournament );
            Assert.AreEqual( tournamentName, response.Tournament.TournamentName );
            Assert.AreEqual( "OrionAcct000002", response.Tournament.OwnerId );
            Assert.IsTrue( response.Tournament.IncludeInSearchResults );
            Assert.IsTrue( response.Tournament.TournamentId.ToString().EndsWith( ".2" ) );
        }


        [TestMethod]
        public async Task BasicHappyPathCreateTournamentWithTournamentObjectTest() {
            var client = new OrionMatchAPIClient(APIStage.BETA);
            var userAuthentication = new UserAuthentication(
                Constants.TestDev7Credentials.Username,
                Constants.TestDev7Credentials.Password );
            await userAuthentication.InitializeAsync();

            var tournamentName = $"BabelFish API Create Tournament Object Test {DateTime.UtcNow:yyyyMMddHHmmss}";
            var tournament = new Scopos.BabelFish.DataModel.OrionMatch.Tournament() {
                MatchName = tournamentName,
                OwnerId = "OrionAcct0002255",
                Visibility = VisibilityOption.PROTECTED,
                IncludeInSearchResults = true,
                MemberPolicy = MemberPolicyOption.INVITE
            };

            var response = await client.CreateTournamentAuthenticatedAsync( tournament, userAuthentication );

            Assert.IsTrue( response.HasOkStatusCode );
            Assert.IsNotNull( response.Tournament );
            Assert.AreEqual( tournamentName, response.Tournament.TournamentName );
            Assert.AreEqual( "2255", response.Tournament.OwnerId );
            Assert.IsTrue( response.Tournament.IncludeInSearchResults );
            Assert.IsTrue( response.Tournament.TournamentId.ToString().EndsWith( ".2" ) );
        }

        [TestMethod]
        public async Task BasicHappyPathDeleteTournamentWithRequestTest() {
            var client = new OrionMatchAPIClient( APIStage.BETA );
            var userAuthentication = new UserAuthentication(
                Constants.TestDev7Credentials.Username,
                Constants.TestDev7Credentials.Password );
            await userAuthentication.InitializeAsync();

            var tournamentName = $"BabelFish API Delete Request Test {DateTime.UtcNow:yyyyMMddHHmmss}";
            var createRequest = new CreateTournamentAuthenticatedRequest( userAuthentication );
            createRequest.TournamentName = tournamentName;
            createRequest.OwnerId = 2;
            createRequest.Visibility = VisibilityOption.PUBLIC;
            createRequest.ShowOnSearch = true;

            var createResponse = await client.CreateTournamentAuthenticatedAsync( createRequest );
            Assert.IsTrue( createResponse.HasOkStatusCode );
            Assert.IsNotNull( createResponse.Tournament );

            var deleteRequest = new DeleteTournamentAuthenticatedRequest( userAuthentication, createResponse.Tournament.TournamentId );
            var deleteResponse = await client.DeleteTournamentAuthenticatedAsync( deleteRequest );

            Assert.IsTrue( deleteResponse.HasOkStatusCode );
            Assert.IsNotNull( deleteResponse.DeleteTournamentResponse );
            Assert.AreEqual( createResponse.Tournament.TournamentId, deleteResponse.DeleteTournamentResponse.TournamentId );
            Assert.AreEqual( 2, deleteResponse.DeleteTournamentResponse.LicenseNumber );
        }

        [TestMethod]
        public async Task BasicHappyPathDeleteTournamentWithTournamentIdTest() {
            var client = new OrionMatchAPIClient( APIStage.BETA );
            var userAuthentication = new UserAuthentication(
                Constants.TestDev7Credentials.Username,
                Constants.TestDev7Credentials.Password );
            await userAuthentication.InitializeAsync();

            var tournamentName = $"BabelFish API Delete TournamentId Test {DateTime.UtcNow:yyyyMMddHHmmss}";
            var createRequest = new CreateTournamentAuthenticatedRequest( userAuthentication );
            createRequest.TournamentName = tournamentName;
            createRequest.OwnerId = 2;
            createRequest.Visibility = VisibilityOption.PUBLIC;
            createRequest.ShowOnSearch = true;

            var createResponse = await client.CreateTournamentAuthenticatedAsync( createRequest );
            Assert.IsTrue( createResponse.HasOkStatusCode );
            Assert.IsNotNull( createResponse.Tournament );

            var deleteResponse = await client.DeleteTournamentAuthenticatedAsync( createResponse.Tournament.TournamentId, userAuthentication );

            Assert.IsTrue( deleteResponse.HasOkStatusCode );
            Assert.IsNotNull( deleteResponse.DeleteTournamentResponse );
            Assert.AreEqual( createResponse.Tournament.TournamentId, deleteResponse.DeleteTournamentResponse.TournamentId );
            Assert.AreEqual( 2, deleteResponse.DeleteTournamentResponse.LicenseNumber );
        }

        [TestMethod]
        public async Task EriksPlayground() {

            var client = new OrionMatchAPIClient();
            //var mId = new MatchID( "1.1.2025100211025190.2" );
            var mId = new MatchID( "1.2255.2025111109531371.2" );

            var request = new GetTournamentPublicRequest( mId );
            var response = await client.GetTournamentPublicAsync( request );

            Assert.IsTrue( response.HasOkStatusCode );

            var tournament = response.Tournament;
            var mergedResultList = tournament.MergedResultLists[0];
            var configuration = (ReentryMethodConfiguration)mergedResultList.Configuration;
            var tournamentMerger = await ResultListMergerEngine.FactoryAsync( tournament, mergedResultList.ResultName );

            var resultList = await tournamentMerger.MergeAsync();

            ResultEngine re = new ResultEngine( resultList, tournamentMerger.RankingRule );
            //ProjectorOfScores projectorOfScores = new ProjectScoresByNull( await configuration.GetCourseOfFireDefinitionAsync() );
            //await re.SortAsync( projectorOfScores, false );

            var rlf = new ResultListIntermediateFormatted( resultList, tournamentMerger.ResultListFormat, null );
            await rlf.InitializeAsync( false );
            rlf.SetShowValuesToDefault();
            rlf.RefreshAllRowsParticipantAttributeFields();


            CellValues tryCellValues, cellValues;
            foreach (int i in rlf.GetShownColumnIndexes()) {
                Console.Write( $"{rlf.GetColumnHeaderCell( i ).Text}, " );
            }
            Console.WriteLine();

            foreach (var row in rlf.ShownRows) {
                foreach (int i in rlf.GetShownColumnIndexes()) {
                    var cell = row.GetColumnBodyCell( i );

                    Console.Write( $"{cell.Text}, " );
                }
                Console.Write( " : " );
                Console.Write( row.GetParticipant().RemarkList.ToString() );
                Console.Write( " : " );
                Console.Write( string.Join( ", ", row.GetClassList() ) );
                Console.WriteLine();
            }

        }
    }
}
