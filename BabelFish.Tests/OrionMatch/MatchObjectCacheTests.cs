using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scopos.BabelFish.APIClients;
using Scopos.BabelFish.Converters;
using Scopos.BabelFish.DataModel.OrionMatch;

namespace Scopos.BabelFish.Tests.OrionMatch {

    [TestClass]
	public class MatchObjectCacheTests {

        [TestInitialize]
        public void InitializeTest() {
            Scopos.BabelFish.Runtime.Settings.XApiKey = Constants.X_API_KEY;
        }

        [TestMethod]
		public async Task MatchObjectCacheTest() {

			var client = new OrionMatchAPIClient();
			var matchId = new MatchID( "1.2899.2023061619492879.0" );

			//The initial request should cache the response.
			var responseNoCache = await client.GetMatchPublicAsync( matchId );
			Assert.AreEqual( HttpStatusCode.OK, responseNoCache.StatusCode );
			Assert.IsFalse( responseNoCache.InMemoryCachedResponse );

			//The second request should use the cache
			var responseWithCache = await client.GetMatchPublicAsync( matchId );
			Assert.AreEqual( HttpStatusCode.OK, responseWithCache.StatusCode );
			Assert.IsTrue( responseWithCache.InMemoryCachedResponse );

			//The amount of time it took for the response should be very different 
			Assert.IsTrue( responseNoCache.TimeToRun > responseWithCache.TimeToRun * 100 );

			//And of course the Match object should be equal
			var matchNoCache = JsonSerializer.Serialize( responseNoCache.Match, SerializerOptions.APIClientSerializer );
			var matchWithCache = JsonSerializer.Serialize( responseWithCache.Match, SerializerOptions.APIClientSerializer );
			Assert.AreEqual( matchNoCache, matchWithCache );

		}
	}
}
