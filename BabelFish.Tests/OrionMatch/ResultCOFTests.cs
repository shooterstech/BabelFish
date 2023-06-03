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
using System.Runtime.CompilerServices;

namespace Scopos.BabelFish.Tests.OrionMatch {

    [TestClass]
    public class ResultCOFTests {

        [TestMethod]
        public async Task TranslateShotsToDictionaryUsingEventNames() {

            var client = new OrionMatchAPIClient( Constants.X_API_KEY, APIStage.PRODUCTION );
            var cofResponse = await client.GetResultCourseOfFireDetailPublicAsync( "91eafa91-6569-434f-85ca-4d99a2f9bc74" );
            var resultCof = cofResponse.ResultCOF;

            var shots = resultCof.GetShotsByEventName();

            Assert.AreEqual( 60, shots.Count );

        }
    }
}
