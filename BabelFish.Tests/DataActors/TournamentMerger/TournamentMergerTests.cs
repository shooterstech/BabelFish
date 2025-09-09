using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scopos.BabelFish.APIClients;
using Scopos.BabelFish.DataActors.OrionMatch;
using Scopos.BabelFish.DataActors.ResultListFormatter;
using Scopos.BabelFish.DataActors.Tournaments;
using Scopos.BabelFish.DataModel.Definitions;
using Scopos.BabelFish.DataModel.OrionMatch;

namespace Scopos.BabelFish.Tests.DataActors.TournamentMerger {

    [TestClass]
    public class TournamentMergerTests : BaseTestClass {

        [TestMethod]
        public async Task EriksPlayground() {

            List<string> matchIds = new List<string>() { "1.1.2025090710463248.0", "1.1.2025090710380473.0", "1.1.2025090710280588.0" };
            OrionMatchAPIClient _apiClient = new OrionMatchAPIClient();

            var tournament = new Match();
            tournament.MatchID = "1.1.2025090710351868.2";
            var tournamentMerger = new Scopos.BabelFish.DataActors.Tournaments.TournamentMerger( tournament, "Aggregate" );

            foreach (var matchId in matchIds) {
                var getMatchResponse = await _apiClient.GetResultListPublicAsync( new MatchID( matchId ), "Individual - All" );
                tournamentMerger.AddResultList( getMatchResponse.ResultList );
            }

            tournamentMerger.AutoGenerateRankingRule();
            tournamentMerger.AutoGenerateResultListFormat();
            var mergedResultList = await tournamentMerger.MergeAsync();
            Assert.IsNotNull( mergedResultList );

            ResultEngine re = new ResultEngine( mergedResultList, tournamentMerger.RankingRule );
            var fakeCof = await DefinitionCache.GetCourseOfFireDefinitionAsync( SetName.Parse( "v1.0:ntparc:40 Shot Standing" ) );
            ProjectorOfScores projectorOfScores = new ProjectScoresByNull( fakeCof );
            await re.SortAsync( projectorOfScores, false );
            Assert.IsNotNull( mergedResultList );

            //Test that the conversion was successful and has the same number of objects.
            ResultListIntermediateFormatted rlf = new ResultListIntermediateFormatted( mergedResultList, tournamentMerger.ResultListFormat, null );
            await rlf.InitializeAsync( false );
            Assert.IsNotNull( rlf );


            //await rlf.LoadSquaddingListAsync();

            rlf.Engagable = false;
            rlf.ResolutionWidth = 1200;
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
