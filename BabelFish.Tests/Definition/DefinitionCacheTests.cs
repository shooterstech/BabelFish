﻿using System;
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

namespace Scopos.BabelFish.Tests.Definition {

    [TestClass]
    public class DefinitionCacheTests {

        /// <summary>
        /// Testing that the first request, that is not using cache, is faster than the second test that is. 
        /// </summary>
        [TestMethod]
        public async Task GetAttributeAirRifleTest() {

            var client = new DefinitionAPIClient( Constants.X_API_KEY );
            var setName = SetName.Parse( "v1.0:ntparc:Three-Position Air Rifle Type" );

            var requestNoCache = new GetDefinitionPublicRequest( setName, DefinitionType.ATTRIBUTE );
            var requestWithCache = new GetDefinitionPublicRequest( setName, DefinitionType.ATTRIBUTE );
            requestNoCache.IgnoreLocalCache = true;
            requestWithCache.IgnoreLocalCache = false;
            var responseNoCache = new GetDefinitionPublicResponse<Scopos.BabelFish.DataModel.Definitions.Attribute>( requestNoCache );
            var responseWithCache = new GetDefinitionPublicResponse<Scopos.BabelFish.DataModel.Definitions.Attribute>( requestWithCache );

            var resultNoCache = await client.GetDefinitionAsync<Scopos.BabelFish.DataModel.Definitions.Attribute>( requestNoCache, responseNoCache );
            Assert.AreEqual( HttpStatusCode.OK, resultNoCache.StatusCode, $"Expecting and OK status code, instead received {resultNoCache.StatusCode}." );
            //The non-cached response should tell us it wasn't from cache
            Assert.IsFalse( resultNoCache.CachedResponse );


            var resultWithCache = await client.GetDefinitionAsync<Scopos.BabelFish.DataModel.Definitions.Attribute>( requestWithCache, responseWithCache );
            Assert.AreEqual( HttpStatusCode.OK, resultWithCache.StatusCode, $"Expecting and OK status code, instead received {resultWithCache.StatusCode}." );

            //With cache should be quite a bit faster than without
            Assert.IsTrue( resultNoCache.TimeToRun > resultWithCache.TimeToRun * 100 );
			//The Cached response should tell us it's from cache
			Assert.IsTrue( resultWithCache.CachedResponse );

            //The definitions should be the same. Serialize the definitions to check
            var attributeNoCache = JsonConvert.SerializeObject( resultNoCache.Definition );
            var attributeWithCache = JsonConvert.SerializeObject( resultWithCache.Definition );
            Assert.AreEqual( attributeWithCache, attributeNoCache );
        }

        [TestMethod]
        public async Task GetCourseOfFireTest() {

            var client = new DefinitionAPIClient( Constants.X_API_KEY );
            var setName = SetName.Parse( "v2.0:ntparc:Three-Position Air Rifle 3x10" );

            var requestNoCache = new GetDefinitionPublicRequest( setName, DefinitionType.COURSEOFFIRE );
            var requestWithCache = new GetDefinitionPublicRequest( setName, DefinitionType.COURSEOFFIRE );
            requestNoCache.IgnoreLocalCache = true;
            requestWithCache.IgnoreLocalCache = false;
            var responseNoCache = new GetDefinitionPublicResponse<CourseOfFire>( requestNoCache );
            var responseWithCache = new GetDefinitionPublicResponse<CourseOfFire>( requestWithCache );


            var resultNoCache = await client.GetDefinitionAsync<CourseOfFire>( requestNoCache, responseNoCache );
            Assert.AreEqual( HttpStatusCode.OK, resultNoCache.StatusCode, $"Expecting and OK status code, instead received {resultNoCache.StatusCode}." );
			//The non-cached response should tell us it wasn't from cache
			Assert.IsFalse( resultNoCache.CachedResponse );


			var resultWithCache = await client.GetDefinitionAsync<CourseOfFire>( requestWithCache, responseWithCache );
            Assert.AreEqual( HttpStatusCode.OK, resultWithCache.StatusCode, $"Expecting and OK status code, instead received {resultWithCache.StatusCode}." );

            //With cache should be quite a bit faster than without
            Assert.IsTrue( resultNoCache.TimeToRun > resultWithCache.TimeToRun * 100 );
			//The Cached response should tell us it's from cache
			Assert.IsTrue( resultWithCache.CachedResponse );

			//The definitions should be the same. Serialize the definitions to check
			var cofNoCache = JsonConvert.SerializeObject( resultNoCache.Definition );
            var cofWithCache = JsonConvert.SerializeObject( resultWithCache.Definition );
            Assert.AreEqual( cofWithCache, cofNoCache );
        }

        [TestMethod]
        public async Task RequestsThatShouldNotGetCached() {

			var client = new DefinitionAPIClient( Constants.X_API_KEY );
			var setName = SetName.Parse( "v2.0:ntparc:Three-Position Air Rifle 3x10" );

            //The first request should cache it's value
			var firstRequest = new GetDefinitionPublicRequest( setName, DefinitionType.COURSEOFFIRE );
            //The second request, b/c we are marking .IgnoreLocalCache should not use the cached value
			var secondRequest = new GetDefinitionPublicRequest( setName, DefinitionType.COURSEOFFIRE );
			secondRequest.IgnoreLocalCache = true;

			var firstResponse = new GetDefinitionPublicResponse<CourseOfFire>( firstRequest );
			var secondResponse = new GetDefinitionPublicResponse<CourseOfFire>( secondRequest );


			var firstResult = await client.GetDefinitionAsync<CourseOfFire>( firstRequest, firstResponse );
			Assert.AreEqual( HttpStatusCode.OK, firstResult.StatusCode, $"Expecting and OK status code, instead received {firstResult.StatusCode}." );
			//The non-cached response should tell us it wasn't from cache
			Assert.IsFalse( firstResult.CachedResponse );
            //The size of the cache should be 1
            Assert.AreEqual( 1, ResponseCache.CACHE.Count );

            var secondResult = await client.GetDefinitionAsync<CourseOfFire>( secondRequest, secondResponse );
			Assert.AreEqual( HttpStatusCode.OK, secondResult.StatusCode, $"Expecting and OK status code, instead received {secondResult.StatusCode}." );
			//The non-cached response should tell us it wasn't from cache
			Assert.IsFalse( secondResult.CachedResponse );

		}
    }
}
