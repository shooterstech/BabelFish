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

            OrionMatchAPIClient _apiClient = new OrionMatchAPIClient();

            var getTournamentResponse = await _apiClient.GetTournamentPublicAsync( new MatchID( "1.1.2025100211025190.2" ) );
            var tournamentMerger = await Scopos.BabelFish.DataActors.Tournaments.TournamentMerger.FactoryAsync( getTournamentResponse.Tournament, "Individual Rankings" );

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
