using System.Net;
using System.Threading.Tasks;
using Scopos.BabelFish.APIClients;
using Scopos.BabelFish.DataModel.OrionMatch;

namespace Scopos.BabelFish.Tests.OrionMatch {

    [TestClass]
	public class MatchObjectCacheTests : BaseTestClass {

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
			var matchNoCache = G_NS.JsonConvert.SerializeObject( responseNoCache.Match, SerializerOptions.NewtonsoftJsonDeserializer );
			var matchWithCache = G_NS.JsonConvert.SerializeObject( responseWithCache.Match, SerializerOptions.NewtonsoftJsonDeserializer );
			Assert.AreEqual( matchNoCache, matchWithCache );

		}
	}
}
