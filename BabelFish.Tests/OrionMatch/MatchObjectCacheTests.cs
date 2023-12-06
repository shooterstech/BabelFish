using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Scopos.BabelFish;
using Scopos.BabelFish.Helpers;
using Scopos.BabelFish.DataModel.Definitions;
using Scopos.BabelFish.APIClients;
using Scopos.BabelFish.Requests.DefinitionAPI;
using Scopos.BabelFish.Responses.DefinitionAPI;
using Scopos.BabelFish.DataModel.OrionMatch;

namespace Scopos.BabelFish.Tests.OrionMatch {

	[TestClass]
	public class MatchObjectCacheTests {

		[TestMethod]
		public async Task MatchObjectCacheTest() {

			var client = new OrionMatchAPIClient( Constants.X_API_KEY );
			var matchId = new MatchID( "1.2899.2023061619492879.0" );

			//The initial request should cache the response.
			var responseNoCache = await client.GetMatchDetailPublicAsync( matchId );
			Assert.AreEqual( HttpStatusCode.OK, responseNoCache.StatusCode );
			Assert.IsFalse( responseNoCache.CachedResponse );

			//The second request should use the cache
			var responseWithCache = await client.GetMatchDetailPublicAsync( matchId );
			Assert.AreEqual( HttpStatusCode.OK, responseWithCache.StatusCode );
			Assert.IsTrue( responseWithCache.CachedResponse );

			//The amount of time it took for the response should be very different 
			Assert.IsTrue( responseNoCache.TimeToRun > responseWithCache.TimeToRun * 100 );

			//And of course the Match object should be equal
			var matchNoCache = JsonConvert.SerializeObject( responseNoCache.Match );
			var matchWithCache = JsonConvert.SerializeObject( responseWithCache.Match );
			Assert.AreEqual( matchNoCache, matchWithCache );

		}
	}
}
