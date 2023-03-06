using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scopos.BabelFish.APIClients;
using Scopos.BabelFish.Requests.OrionMatchAPI;

namespace Scopos.BabelFish.Tests.OrionMatch {

    [TestClass]
    public class MatchSearchUnitTests {

        [TestMethod]
        public void BasicTestSearch() {

            var client = new OrionMatchAPIClient( Constants.X_API_KEY, APIStage.PRODUCTION );

            var request = new MatchSearchPublicRequest();

            var taskMatchSearchResponse = client.GetMatchSearchPublicAsync( request );
            var matchSearchResponse = taskMatchSearchResponse.Result;

            Assert.AreEqual( HttpStatusCode.OK, matchSearchResponse.StatusCode );

            Assert.IsTrue( matchSearchResponse.MatchSearchList.Items.Count > 0 );
        }
    }
}
