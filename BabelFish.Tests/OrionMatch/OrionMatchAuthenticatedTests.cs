using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scopos.BabelFish.APIClients;
using Scopos.BabelFish.Requests.OrionMatchAPI;
using Scopos.BabelFish.DataModel.OrionMatch;
using Scopos.BabelFish.DataModel.AttributeValue;
using Scopos.BabelFish.Runtime.Authentication;
using Scopos.BabelFish.DataModel.Athena;

namespace Scopos.BabelFish.Tests.OrionMatch {
    [TestClass]
    public class OrionMatchAuthenticatedUnitTests {

        [TestMethod]
        public async Task OrionMatchAPI_GetAMatch() {

            var client = new OrionMatchAPIClient( Constants.X_API_KEY, APIStage.BETA );
            var userAuthentication = new UserAuthentication(
                Constants.TestDev7Credentials.Username,
                Constants.TestDev7Credentials.Password );
            await userAuthentication.InitializeAsync();

            var matchId = new MatchID( "1.1.2023011915575119.0" );
            var response = await client.GetMatchDetailAuthenticatedAsync( matchId, userAuthentication );

            Assert.AreEqual( HttpStatusCode.OK, response.StatusCode );

            var match = response.Match;

            //Perform some simple tests on the returned data.
            Assert.AreEqual( matchId.ToString(), match.MatchID );
            Assert.AreEqual( "Unit Test Match", match.Name );
            Assert.AreEqual( VisibilityOption.PUBLIC, match.Visibility );
            Assert.AreEqual( "2023-01-19", match.StartDate.ToString( DateTimeFormats.DATE_FORMAT ) );
        }
    }
}