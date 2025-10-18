using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scopos.BabelFish.APIClients;
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
        }
    }
}
