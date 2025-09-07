using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scopos.BabelFish.APIClients;
using Scopos.BabelFish.DataActors.Tournaments;
using Scopos.BabelFish.DataModel.OrionMatch;

namespace Scopos.BabelFish.Tests.DataActors.TournamentMerger {

    [TestClass]
    public class TournamentMergerTests : BaseTestClass {

        [TestMethod]
        public async Task EriksPlayground() {

            List<string> matchIds = new List<string>() { "1.1.2025090710280588.0", "1.1.2025090710380473.0", "1.1.2025090710463248.0" };
            OrionMatchAPIClient _apiClient = new OrionMatchAPIClient();

            var tournament = new Match();
            tournament.MatchID = "1.1.2025090710351868.2";
            var tournamentMerger = new Scopos.BabelFish.DataActors.Tournaments.TournamentMerger( tournament, "Individual - All" );

            foreach (var matchId in matchIds) {
                var getMatchResponse = await _apiClient.GetMatchPublicAsync( new MatchID( matchId ) );
                tournamentMerger.AddMatch( getMatchResponse.Match );
            }

            var mergedResultList = await tournamentMerger.MergeAsync();
            Assert.IsNotNull( mergedResultList );
        }
    }
}
