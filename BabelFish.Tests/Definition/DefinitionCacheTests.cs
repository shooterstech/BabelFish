using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Scopos.BabelFish.APIClients;
using Scopos.BabelFish.Converters;
using Scopos.BabelFish.DataModel.Definitions;
using Scopos.BabelFish.Helpers;
using Scopos.BabelFish.Requests.DefinitionAPI;
using Scopos.BabelFish.Responses.DefinitionAPI;

namespace Scopos.BabelFish.Tests.Definition {

    [TestClass]
    public class DefinitionCacheTests : BaseTestClass {

        /// <summary>
        /// Testing that the first request, that is not using cache, is faster than the second test that is. 
        /// </summary>
        [TestMethod]
        public async Task InMemoryCacheTest() {

            var client = new DefinitionAPIClient();
            var setName = SetName.Parse( "v1.0:ntparc:Three-Position Air Rifle Type" );

            DefinitionAPIClient.LocalStoreDirectory = new System.IO.DirectoryInfo( @"c:\\temp" );
            var requestNoCache = new GetDefinitionPublicRequest( setName, DefinitionType.ATTRIBUTE );
            var requestWithCache = new GetDefinitionPublicRequest( setName, DefinitionType.ATTRIBUTE );
            requestNoCache.IgnoreFileSystemCache = true;
            requestNoCache.IgnoreInMemoryCache = true;
            requestWithCache.IgnoreFileSystemCache = true;
            requestWithCache.IgnoreInMemoryCache = false;
            var responseNoCache = new GetDefinitionPublicResponse<Scopos.BabelFish.DataModel.Definitions.Attribute>( requestNoCache );
            var responseWithCache = new GetDefinitionPublicResponse<Scopos.BabelFish.DataModel.Definitions.Attribute>( requestWithCache );

            var resultNoCache = await client.GetDefinitionAsync<Scopos.BabelFish.DataModel.Definitions.Attribute>( requestNoCache, responseNoCache );
            Assert.AreEqual( HttpStatusCode.OK, resultNoCache.RestApiStatusCode, $"Expecting and OK status code, instead received {resultNoCache.RestApiStatusCode}." );
            //The non-cached response should tell us it wasn't from cache
            Assert.IsFalse( resultNoCache.InMemoryCachedResponse );


            var resultWithCache = await client.GetDefinitionAsync<Scopos.BabelFish.DataModel.Definitions.Attribute>( requestWithCache, responseWithCache );
            Assert.AreEqual( HttpStatusCode.OK, resultWithCache.RestApiStatusCode, $"Expecting and OK status code, instead received {resultWithCache.RestApiStatusCode}." );

            //With cache should be quite a bit faster than without
            Assert.IsTrue( resultNoCache.TimeToRun > resultWithCache.TimeToRun * 100 );
            //The Cached response should tell us it's from cache
            Assert.IsTrue( resultWithCache.InMemoryCachedResponse );

            //The definitions should be the same. Serialize the definitions to check
            var attributeNoCache = G_NS.JsonConvert.SerializeObject( resultNoCache.Definition, SerializerOptions.NewtonsoftJsonSerializer );
            var attributeWithCache = G_NS.JsonConvert.SerializeObject( resultWithCache.Definition, SerializerOptions.NewtonsoftJsonSerializer );
            Assert.AreEqual( attributeWithCache, attributeNoCache );
        }

        /// <summary>
        /// Testing that the first request, that is using file system cache, is faster than the second test that should use in memory. 
        /// </summary>
        [TestMethod]
        public async Task FileSystemCacheTest() {


            var client = new DefinitionAPIClient();
            var setName = SetName.Parse( "v1.0:ntparc:Three-Position Air Rifle Type" );

            //These next two lines (should) store the attribute in the local file system if it is not already there.
            DefinitionAPIClient.LocalStoreDirectory = new System.IO.DirectoryInfo( @"C:\temp" );
            await DefinitionCache.GetAttributeDefinitionAsync( setName );

            //Clear the cache so we are starting with a blank slate for the remainder of the test. 
            //Note clearing the cache does not delete files from the file system,, only memory
            Initializer.ClearCache( false );

            var attributeRequest = new GetDefinitionPublicRequest( setName, DefinitionType.ATTRIBUTE );
            attributeRequest.IgnoreFileSystemCache = false;

            var attributeResponse = new GetDefinitionPublicResponse<Scopos.BabelFish.DataModel.Definitions.Attribute>( attributeRequest );

            var attributeResult = await client.GetDefinitionAsync<Scopos.BabelFish.DataModel.Definitions.Attribute>( attributeRequest, attributeResponse );
            Assert.AreEqual( HttpStatusCode.OK, attributeResult.RestApiStatusCode, $"Expecting and OK status code, instead received {attributeResult.RestApiStatusCode}." );

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
            Assert.AreEqual( HttpStatusCode.OK, attributeResult2.RestApiStatusCode, $"Expecting and OK status code, instead received {attributeResult.RestApiStatusCode}." );

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
            Assert.AreEqual( HttpStatusCode.OK, resultNoCache.RestApiStatusCode, $"Expecting and OK status code, instead received {resultNoCache.RestApiStatusCode}." );
            //The non-cached response should tell us it wasn't from cache
            Assert.IsFalse( resultNoCache.InMemoryCachedResponse );


            var resultWithCache = await client.GetDefinitionAsync<CourseOfFire>( requestWithCache, responseWithCache );
            Assert.AreEqual( HttpStatusCode.OK, resultWithCache.RestApiStatusCode, $"Expecting and OK status code, instead received {resultWithCache.RestApiStatusCode}." );

            //With cache should be quite a bit faster than without
            Assert.IsTrue( resultNoCache.TimeToRun > resultWithCache.TimeToRun * 10 );
            //The Cached response should tell us it's from cache
            Assert.IsTrue( resultWithCache.InMemoryCachedResponse );

            //The definitions should be the same. Serialize the definitions to check
            var cofNoCache = G_NS.JsonConvert.SerializeObject( resultNoCache.Definition, SerializerOptions.NewtonsoftJsonSerializer ); 
            var cofWithCache = G_NS.JsonConvert.SerializeObject( resultWithCache.Definition, SerializerOptions.NewtonsoftJsonSerializer ); 
            Assert.AreEqual( cofWithCache, cofNoCache );
        }

        [TestMethod]
        public async Task RequestsThatShouldNotGetCached() {

            //To perform this test, need to clear cache first, as other tests may have populated it.
            Initializer.ClearCache(false);

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
            Assert.AreEqual( HttpStatusCode.OK, firstResult.RestApiStatusCode, $"Expecting and OK status code, instead received {firstResult.RestApiStatusCode}." );
            //The non-cached response should tell us it wasn't from cache
            Assert.IsFalse( firstResult.InMemoryCachedResponse );
            //The size of the cache should be 1
            Assert.AreEqual( 1, ResponseCache.CACHE.Count );

            var secondResult = await client.GetDefinitionAsync<CourseOfFire>( secondRequest, secondResponse );
            Assert.AreEqual( HttpStatusCode.OK, secondResult.RestApiStatusCode, $"Expecting and OK status code, instead received {secondResult.RestApiStatusCode}." );
            //The non-cached response should tell us it wasn't from cache
            Assert.IsFalse( secondResult.InMemoryCachedResponse );

        }

        /// <summary>
        /// Tests if the definition is saved to local file system after it is read from the Rest API
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task SaveToLocalStoreDirectoryAfterDownloadTest() {

            //to run this test, first need to delete the file that the call would normally try and read.
            DefinitionAPIClient.LocalStoreDirectory = new System.IO.DirectoryInfo( @"C:\temp" );
            var definitionFileName = $"c:\\temp\\DEFINITIONS\\STAGE STYLE\\v1.0 ntparc Sporter Air RIfle Kneeling.json";

            if ( File.Exists( definitionFileName ) ) 
                File.Delete( definitionFileName );

            //Now clear the cache so we are starting with a blank slate
            Initializer.ClearCache(false);

            //Now we can try and retreive it in the normal way, which should save it to file system within the (above) set LocalStoreDirectory
            var setName = SetName.Parse( "v1.0:ntparc:Sporter Air Rifle Kneeling" );
            await DefinitionCache.GetStageStyleDefinitionAsync( setName );

            Assert.IsTrue( File.Exists( definitionFileName ) );
        }

        [TestMethod]
        public async Task DefinitionNotFoundTest() {

            SetName notARealAttrDefinition = SetName.Parse( "v1.0:orion:not a real attribute" );

            //Now clear the cache so we are starting with a blank slate
            Initializer.ClearCache( false );

            var swFirstCall = Stopwatch.StartNew();
            try {
                await DefinitionCache.GetAttributeDefinitionAsync( notARealAttrDefinition );
            } catch ( DefinitionNotFoundException ) {
                //This is expected
                swFirstCall.Stop();
            }

            var swSecondCall = Stopwatch.StartNew();
            try {
                await DefinitionCache.GetAttributeDefinitionAsync( notARealAttrDefinition );
            } catch (DefinitionNotFoundException) {
                //This is expected
                swSecondCall.Stop();
            }

            //The second call, if it was cached, should be faster than the first
            //Due to the time it takes to throw and catch an exception, the benefit is minimal, but still there.
            Assert.IsTrue( swFirstCall.ElapsedMilliseconds > 2 * swSecondCall.ElapsedMilliseconds );
        }

        /// <summary>
        /// Tests that the DefinitionCache's PreLoad, actually loads the expected definitiions into cache. Runs it twice, first using Rest API, second using local file system.
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task DefinitionCachePreLoadTests() {

            //Clear out the definitions directory, and the cache 
            Directory.Delete( @"c:\temp\DEFINITIONS", true );
            Initializer.ClearCache( false );

            //Set the LocalStoreDirectory and preload. Which should also save to file system
            DefinitionAPIClient.LocalStoreDirectory = new System.IO.DirectoryInfo( @"c:\temp" );
            Stopwatch loadFromRestApi = Stopwatch.StartNew();
            await DefinitionCache.PreLoad();
            loadFromRestApi.Stop();

            Assert.AreEqual( 19, DefinitionCache.GetCacheSize( DefinitionType.ATTRIBUTE ) );
            Assert.AreEqual( 2, DefinitionCache.GetCacheSize( DefinitionType.TARGET ) );
            Assert.AreEqual( 2, DefinitionCache.GetCacheSize( DefinitionType.SCOREFORMATCOLLECTION ) );

            //Clear the cache and make sure it got cleared
            Initializer.ClearCache( false );
            Assert.AreEqual( 0, DefinitionCache.GetCacheSize( DefinitionType.ATTRIBUTE ) );
            Assert.AreEqual( 0, DefinitionCache.GetCacheSize( DefinitionType.TARGET ) );
            Assert.AreEqual( 0, DefinitionCache.GetCacheSize( DefinitionType.SCOREFORMATCOLLECTION ) );

            //Re-run preload, which will read the files from the local file system (since they should exist now)
            Stopwatch loadFromFileSystem = Stopwatch.StartNew();
            await DefinitionCache.PreLoad();
            loadFromFileSystem.Stop();

            Assert.AreEqual( 19, DefinitionCache.GetCacheSize( DefinitionType.ATTRIBUTE ) );
            Assert.AreEqual( 2, DefinitionCache.GetCacheSize( DefinitionType.TARGET ) );
            Assert.AreEqual( 2, DefinitionCache.GetCacheSize( DefinitionType.SCOREFORMATCOLLECTION ) );

            //Check that it is much faster
            Assert.IsTrue( loadFromFileSystem.ElapsedMilliseconds * 10 < loadFromRestApi.ElapsedMilliseconds );
        }

        /// <summary>
        /// Checks that the CheckForNewerVersion() method does indeed return true or false if there's a newer version
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task CheckForNewerVersionTest() {

            var setName = SetName.Parse( "v2.0:ntparc:Three-Position Air Rifle 3x10" );
            var cofDefinition = await DefinitionCache.GetCourseOfFireDefinitionAsync( setName );

            //Initially, since we just grabbed it, there should not be a newer version
            var hasNewerVersion = await cofDefinition.IsVersionUpdateAvaliableAsync();
            Assert.IsFalse( hasNewerVersion );

            var currentVersion = cofDefinition.GetDefinitionVersion();
            var manipulatedVersion = $"{currentVersion.MajorVersion}.{currentVersion.MinorVersion - 1}";
            cofDefinition.Version = manipulatedVersion;

            //Since we manipulated the cached version, we need to clear the cache before checking again.
            Initializer.ClearCache( false );

            //After manipulating the version, to be lower, should get a rresponse that there is a newer version.
            hasNewerVersion = await cofDefinition.IsVersionUpdateAvaliableAsync();
            Assert.IsTrue( hasNewerVersion );
        }

        /// <summary>
        /// Checks that the DownloadNewMinorVersionIfAvaliableAsync method does download a new version if one 
        /// is avaliable.
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task DownloadNewMinorVersionIfAvaliableTest() {

            /* TODO: This only tests the DownloadNewMinorVersionIfAvaliableAsync() method for Attributes. Should write other unit tests for the other definition types */             

            //Set to auto download
            DefinitionCache.AutoDownloadNewDefinitionVersions = true;

            var setName = SetName.Parse( "v1.0:orion:Collegiate Class" );
            var attrDefinition = await DefinitionCache.GetAttributeDefinitionAsync( setName );

            //Decrement the version so when IsNewerVersionAvaliable is called, it returns true.
            var currentVersion = attrDefinition.GetDefinitionVersion();
            var manipulatedVersion = $"{currentVersion.MajorVersion}.{currentVersion.MinorVersion - 1}";
            attrDefinition.Version = manipulatedVersion;

            //After manipulating the version, to be lower, should get a rresponse that there is a newer version.
            var hasNewerVersion = await attrDefinition.IsVersionUpdateAvaliableAsync();
            Assert.IsTrue( hasNewerVersion );

            //And we should also get a response back from DownloadNewMinorVersionIfAvaliableAsync() that the latest version was downloaded.
            var downloadedNewVersion = await DefinitionCache.DownloadNewMinorVersionIfAvaliableAsync( attrDefinition );
            Assert.IsTrue( downloadedNewVersion );

            //Next, if we call GetAttributeDefinitionAsync() again, we should get the original value of currentVersion
            var attrDefinitionSecondCall = await DefinitionCache.GetAttributeDefinitionAsync( setName );
            Assert.IsTrue( currentVersion.ToString() == attrDefinitionSecondCall.Version.ToString() );
        }

        [TestMethod]
        public async Task EriksPlayground() {
            var setName = SetName.Parse( "v1.0:orion:Test Informal Practice Air Rifle" );
            var definition = await DefinitionCache.GetDefinitionAsync( DefinitionType.COURSEOFFIRE, setName );

            Assert.AreEqual( DefinitionType.COURSEOFFIRE, definition.Type );
        }
    }
}
