using System.IO;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scopos.BabelFish.APIClients;
using Scopos.BabelFish.Converters;
using Scopos.BabelFish.DataModel.Definitions;
using Scopos.BabelFish.Requests.DefinitionAPI;
using Scopos.BabelFish.Responses.DefinitionAPI;

namespace Scopos.BabelFish.Tests.Definition {

    [TestClass]
    public class DefinitionCacheTests {

        [TestInitialize]
        public void InitializeTest() {
            Scopos.BabelFish.Runtime.Settings.XApiKey = Constants.X_API_KEY;
        }

        /// <summary>
        /// Testing that the first request, that is not using cache, is faster than the second test that is. 
        /// </summary>
        [TestMethod]
        public async Task InMemoryCacheTest() {

            var client = new DefinitionAPIClient();
            var setName = SetName.Parse( "v1.0:ntparc:Three-Position Air Rifle Type" );

            DefinitionAPIClient.LocalStoreDirectory = new System.IO.DirectoryInfo( @"g:\My Drive\Definitions" );
            var requestNoCache = new GetDefinitionPublicRequest( setName, DefinitionType.ATTRIBUTE );
            var requestWithCache = new GetDefinitionPublicRequest( setName, DefinitionType.ATTRIBUTE );
            requestNoCache.IgnoreFileSystemCache = true;
            requestNoCache.IgnoreInMemoryCache = true;
            requestWithCache.IgnoreFileSystemCache = true;
            requestWithCache.IgnoreInMemoryCache = false;
            var responseNoCache = new GetDefinitionPublicResponse<Scopos.BabelFish.DataModel.Definitions.Attribute>( requestNoCache );
            var responseWithCache = new GetDefinitionPublicResponse<Scopos.BabelFish.DataModel.Definitions.Attribute>( requestWithCache );

            var resultNoCache = await client.GetDefinitionAsync<Scopos.BabelFish.DataModel.Definitions.Attribute>( requestNoCache, responseNoCache );
            Assert.AreEqual( HttpStatusCode.OK, resultNoCache.StatusCode, $"Expecting and OK status code, instead received {resultNoCache.StatusCode}." );
            //The non-cached response should tell us it wasn't from cache
            Assert.IsFalse( resultNoCache.InMemoryCachedResponse );


            var resultWithCache = await client.GetDefinitionAsync<Scopos.BabelFish.DataModel.Definitions.Attribute>( requestWithCache, responseWithCache );
            Assert.AreEqual( HttpStatusCode.OK, resultWithCache.StatusCode, $"Expecting and OK status code, instead received {resultWithCache.StatusCode}." );

            //With cache should be quite a bit faster than without
            Assert.IsTrue( resultNoCache.TimeToRun > resultWithCache.TimeToRun * 100 );
            //The Cached response should tell us it's from cache
            Assert.IsTrue( resultWithCache.InMemoryCachedResponse );

            //The definitions should be the same. Serialize the definitions to check
            var attributeNoCache = JsonSerializer.Serialize( resultNoCache.Definition, G_BF_STJ_CONV.SerializerOptions.APIClientSerializer );
            var attributeWithCache = JsonSerializer.Serialize( resultWithCache.Definition, G_BF_STJ_CONV.SerializerOptions.APIClientSerializer );
            Assert.AreEqual( attributeWithCache, attributeNoCache );
        }

        /// <summary>
        /// Testing that the first request, that is using file system cache, is faster than the second test that should use in memory. 
        /// </summary>
        [TestMethod]
        public async Task FileSystemCacheTest() {

            var client = new DefinitionAPIClient();
            var setName = SetName.Parse( "v1.0:ntparc:Three-Position Air Rifle Type" );

            //TODO figure out how to read the value of the definition directory from a config file.
            DefinitionAPIClient.LocalStoreDirectory = new System.IO.DirectoryInfo( @"C:\Users\erikkanderson\Documents\My Matches\DATABASE\DEFINITIONS\" ); // @"g:\My Drive\Definitions" );
            var attributeRequest = new GetDefinitionPublicRequest( setName, DefinitionType.ATTRIBUTE );
            attributeRequest.IgnoreFileSystemCache = false;

            var attributeResponse = new GetDefinitionPublicResponse<Scopos.BabelFish.DataModel.Definitions.Attribute>( attributeRequest );

            var attributeResult = await client.GetDefinitionAsync<Scopos.BabelFish.DataModel.Definitions.Attribute>( attributeRequest, attributeResponse );
            Assert.AreEqual( HttpStatusCode.OK, attributeResult.StatusCode, $"Expecting and OK status code, instead received {attributeResult.StatusCode}." );

            //The non-cached response should tell us it was from file system
            Assert.IsTrue( attributeResult.FileSystemCachedResponse );
            Assert.IsFalse( attributeResult.InMemoryCachedResponse );

            var attribute = attributeResult.Value;
            Assert.AreEqual( "v1.0:ntparc:Three-Position Air Rifle Type", attribute.SetName );
            Assert.AreEqual( 1, attribute.Fields.Count );

            //If we repeat the request, should pull from in-memory
            var attributeRequest2 = new GetDefinitionPublicRequest( setName, DefinitionType.ATTRIBUTE );

            var attributeResponse2 = new GetDefinitionPublicResponse<Scopos.BabelFish.DataModel.Definitions.Attribute>( attributeRequest2 );

            var attributeResult2 = await client.GetDefinitionAsync<Scopos.BabelFish.DataModel.Definitions.Attribute>( attributeRequest2, attributeResponse2 );
            Assert.AreEqual( HttpStatusCode.OK, attributeResult2.StatusCode, $"Expecting and OK status code, instead received {attributeResult.StatusCode}." );

            //The non-cached response should tell us it was from file system
            Assert.IsFalse( attributeResult2.FileSystemCachedResponse );
            Assert.IsTrue( attributeResult2.InMemoryCachedResponse );

            var attribute2 = attributeResult2.Value;
            Assert.AreEqual( "v1.0:ntparc:Three-Position Air Rifle Type", attribute2.SetName );
            Assert.AreEqual( 1, attribute2.Fields.Count );
        }

        [TestMethod]
        public async Task GetCourseOfFireTest() {

            var client = new DefinitionAPIClient();
            var setName = SetName.Parse( "v2.0:ntparc:Three-Position Air Rifle 3x10" );

            var requestNoCache = new GetDefinitionPublicRequest( setName, DefinitionType.COURSEOFFIRE );
            var requestWithCache = new GetDefinitionPublicRequest( setName, DefinitionType.COURSEOFFIRE );
            requestNoCache.IgnoreInMemoryCache = true;
            requestWithCache.IgnoreInMemoryCache = false;
            var responseNoCache = new GetDefinitionPublicResponse<CourseOfFire>( requestNoCache );
            var responseWithCache = new GetDefinitionPublicResponse<CourseOfFire>( requestWithCache );


            var resultNoCache = await client.GetDefinitionAsync<CourseOfFire>( requestNoCache, responseNoCache );
            Assert.AreEqual( HttpStatusCode.OK, resultNoCache.StatusCode, $"Expecting and OK status code, instead received {resultNoCache.StatusCode}." );
            //The non-cached response should tell us it wasn't from cache
            Assert.IsFalse( resultNoCache.InMemoryCachedResponse );


            var resultWithCache = await client.GetDefinitionAsync<CourseOfFire>( requestWithCache, responseWithCache );
            Assert.AreEqual( HttpStatusCode.OK, resultWithCache.StatusCode, $"Expecting and OK status code, instead received {resultWithCache.StatusCode}." );

            //With cache should be quite a bit faster than without
            Assert.IsTrue( resultNoCache.TimeToRun > resultWithCache.TimeToRun * 10 );
            //The Cached response should tell us it's from cache
            Assert.IsTrue( resultWithCache.InMemoryCachedResponse );

            //The definitions should be the same. Serialize the definitions to check
            var cofNoCache = JsonSerializer.Serialize( resultNoCache.Definition, G_BF_STJ_CONV.SerializerOptions.APIClientSerializer );
            var cofWithCache = JsonSerializer.Serialize( resultWithCache.Definition, G_BF_STJ_CONV.SerializerOptions.APIClientSerializer );
            Assert.AreEqual( cofWithCache, cofNoCache );
        }

        [TestMethod]
        public async Task RequestsThatShouldNotGetCached() {

            var client = new DefinitionAPIClient();
            var setName = SetName.Parse( "v2.0:ntparc:Three-Position Air Rifle 3x10" );

            //The first request should cache it's value
            var firstRequest = new GetDefinitionPublicRequest( setName, DefinitionType.COURSEOFFIRE );
            //The second request, b/c we are marking .IgnoreLocalCache should not use the cached value
            var secondRequest = new GetDefinitionPublicRequest( setName, DefinitionType.COURSEOFFIRE );
            secondRequest.IgnoreInMemoryCache = true;

            var firstResponse = new GetDefinitionPublicResponse<CourseOfFire>( firstRequest );
            var secondResponse = new GetDefinitionPublicResponse<CourseOfFire>( secondRequest );


            var firstResult = await client.GetDefinitionAsync<CourseOfFire>( firstRequest, firstResponse );
            Assert.AreEqual( HttpStatusCode.OK, firstResult.StatusCode, $"Expecting and OK status code, instead received {firstResult.StatusCode}." );
            //The non-cached response should tell us it wasn't from cache
            Assert.IsFalse( firstResult.InMemoryCachedResponse );
            //The size of the cache should be 1
            Assert.AreEqual( 1, ResponseCache.CACHE.Count );

            var secondResult = await client.GetDefinitionAsync<CourseOfFire>( secondRequest, secondResponse );
            Assert.AreEqual( HttpStatusCode.OK, secondResult.StatusCode, $"Expecting and OK status code, instead received {secondResult.StatusCode}." );
            //The non-cached response should tell us it wasn't from cache
            Assert.IsFalse( secondResult.InMemoryCachedResponse );

        }

        [TestMethod]
        public async Task SaveToFileTest() {


			var client = new DefinitionAPIClient();
			var setName = SetName.Parse( "v1.0:ntparc:Sporter Air Rifle Kneeling" );

			var response = await client.GetStageStyleDefinitionAsync( setName );
            var stageStyle = response.Definition;

            DirectoryInfo temp = new DirectoryInfo( @"c:\temp" );
            stageStyle.SaveToFile( temp );
		}

        [TestMethod] 
        public async Task ReadFromFileTest() {

            string path = @"G:\My Drive\Definitions\COURSE OF FIRE\v1.0 usas Smallbore Rifle 60 Prone.json";

            string json = File.ReadAllText( path );

            var definition = JsonSerializer.Deserialize<Scopos.BabelFish.DataModel.Definitions.Definition> ( json );

            Assert.AreEqual( DefinitionType.COURSEOFFIRE, definition.Type );
		}
    }
}
