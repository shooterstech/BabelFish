using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scopos.BabelFish.APIClients;
using Scopos.BabelFish.DataActors.OrionMatch;
using Scopos.BabelFish.DataActors.ResultListFormatter;
using Scopos.BabelFish.DataActors.Tournaments;
using Scopos.BabelFish.DataModel.OrionMatch;
using Scopos.BabelFish.Requests.OrionMatchAPI;
using Scopos.BabelFish.Tests;

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

            var tournament= response.Tournament;
            Assert.AreEqual( mId, tournament.MatchId );
            Assert.AreEqual( mId, tournament.TournamentId );

            Assert.AreEqual( MatchType.TOURNAMENT, tournament.MatchType );

            Assert.IsTrue( tournament.TournamentMembers.Count > 0 );
            Assert.IsTrue( tournament.MergedResultLists.Count > 0 );
            Assert.IsTrue( tournament.MergedResultLists[0].Configuration is AverageMethodConfiguration );
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
            var configuration = (ReentryMethodConfiguration) mergedResultList.Configuration;
            var tournamentMerger = await TournamentMerger.FactoryAsync( tournament, mergedResultList.ResultName );

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
