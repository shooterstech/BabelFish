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
using System.Security.Cryptography.X509Certificates;

namespace Scopos.BabelFish.Tests.Definition {

    [TestClass]
    public class DefinitionTests {

        /// <summary>
        /// Testing that the first request, that is not using cache, is faster than the second test that is. 
        /// </summary>
        [TestMethod]
        public void GetAttributeAirRifleTest() {

            var client = new DefinitionAPIClient( Constants.X_API_KEY );
            var setName = SetName.Parse( "v1.0:ntparc:Three-Position Air Rifle Type" );

            var requestNoCache = new GetDefinitionPublicRequest( setName, DefinitionType.ATTRIBUTE );
            var requestWithCache = new GetDefinitionPublicRequest( setName, DefinitionType.ATTRIBUTE );
            requestNoCache.IgnoreLocalCache = true;
            requestWithCache.IgnoreLocalCache = false;
            var responseNoCache = new GetDefinitionPublicResponse<Scopos.BabelFish.DataModel.Definitions.Attribute>( requestNoCache );
            var responseWithCache = new GetDefinitionPublicResponse<Scopos.BabelFish.DataModel.Definitions.Attribute>( requestWithCache );

            var taskResponseNoCache = client.GetDefinitionAsync<Scopos.BabelFish.DataModel.Definitions.Attribute>( requestNoCache, responseNoCache );
            var resultNoCache = taskResponseNoCache.Result;
            Assert.AreEqual( HttpStatusCode.OK, resultNoCache.StatusCode, $"Expecting and OK status code, instead received {resultNoCache.StatusCode}." );


            var taskResponseWithCache = client.GetDefinitionAsync<Scopos.BabelFish.DataModel.Definitions.Attribute>( requestWithCache, responseWithCache );
            var resultWithCache = taskResponseWithCache.Result;
            Assert.AreEqual( HttpStatusCode.OK, resultWithCache.StatusCode, $"Expecting and OK status code, instead received {resultWithCache.StatusCode}." );

            //With cache should be quite a bit faster than without
            Assert.IsTrue( resultNoCache.TimeToRun > resultWithCache.TimeToRun * 100 );

            //The definitions should be the same. Serialize the definitions to check
            var attributeNoCache = JsonConvert.SerializeObject( resultNoCache.Definition );
            var attributeWithCache = JsonConvert.SerializeObject( resultWithCache.Definition );
            Assert.AreEqual( attributeWithCache, attributeNoCache );

            //The Cached response should tell us it's from cache
            Assert.IsTrue( resultWithCache.MessageResponse.Title.Contains( "Cache" ) );
        }

        [TestMethod]
        public void GetCourseOfFireTest() {

            var client = new DefinitionAPIClient( Constants.X_API_KEY );
            var setName = SetName.Parse( "v2.0:ntparc:Three-Position Air Rifle 3x10" );

            var requestNoCache = new GetDefinitionPublicRequest( setName, DefinitionType.COURSEOFFIRE );
            var requestWithCache = new GetDefinitionPublicRequest( setName, DefinitionType.COURSEOFFIRE );
            requestNoCache.IgnoreLocalCache = true;
            requestWithCache.IgnoreLocalCache = false;
            var responseNoCache = new GetDefinitionPublicResponse<CourseOfFire>( requestNoCache );
            var responseWithCache = new GetDefinitionPublicResponse<CourseOfFire>( requestWithCache );


            var taskResponseNoCache = client.GetDefinitionAsync<CourseOfFire>( requestNoCache, responseNoCache );
            var resultNoCache = taskResponseNoCache.Result;
            Assert.AreEqual( HttpStatusCode.OK, resultNoCache.StatusCode, $"Expecting and OK status code, instead received {resultNoCache.StatusCode}." );


            var taskResponseWithCache = client.GetDefinitionAsync<CourseOfFire>( requestWithCache, responseWithCache );
            var resultWithCache = taskResponseWithCache.Result;
            Assert.AreEqual( HttpStatusCode.OK, resultWithCache.StatusCode, $"Expecting and OK status code, instead received {resultWithCache.StatusCode}." );

            //With cache should be quite a bit faster than without
            Assert.IsTrue( resultNoCache.TimeToRun > resultWithCache.TimeToRun * 100 );

            //The definitions should be the same. Serialize the definitions to check
            var cofNoCache = JsonConvert.SerializeObject( resultNoCache.Definition );
            var cofWithCache = JsonConvert.SerializeObject( resultWithCache.Definition );
            Assert.AreEqual( cofWithCache, cofNoCache );

            //The Cached response should tell us it's from cache
            Assert.IsTrue( resultWithCache.MessageResponse.Title.Contains( "Cache" ) );
        }
    }
}
