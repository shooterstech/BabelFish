using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scopos.BabelFish.APIClients;
using Scopos.BabelFish.Requests.OrionMatchAPI;
using Scopos.BabelFish.Runtime.Authentication;

namespace Scopos.BabelFish.Tests.OrionMatch {

    [TestClass]
    public class MatchSearchUnitTests {

        /// <summary>
        /// Conducts a default search and checks that soemthing comes back.
        /// </summary>
        [TestMethod]
        public async Task BasicTestSearchPublic() {

            //Conducting test in production since the development database doesn't always have entries in it.
            var client = new OrionMatchAPIClient( Constants.X_API_KEY, APIStage.PRODUCTION );

            var request = new MatchSearchPublicRequest();

            var matchSearchResponse = await client.GetMatchSearchPublicAsync( request );

            Assert.AreEqual( HttpStatusCode.OK, matchSearchResponse.StatusCode );

            //It is possible, however unlikely, that the search comes back without any items.
            Assert.IsTrue( matchSearchResponse.MatchSearchList.Items.Count > 0 );
        }

        [TestMethod]
        public async Task BasicTestSearchAuthenticated() {

            //Conducting test in production since the development database doesn't always have entries in it.
            var client = new OrionMatchAPIClient( Constants.X_API_KEY, APIStage.PRODUCTION );
            var userAuthentication = new UserAuthentication(
                Constants.TestDev7Credentials.Username,
                Constants.TestDev7Credentials.Password );

            var request = new MatchSearchAuthenticatedRequest(userAuthentication);

            Assert.IsTrue( request.RequiresCredentials );

            var matchSearchResponse = await client.GetMatchSearchAuthenticatedAsync( request );

            Assert.AreEqual( HttpStatusCode.OK, matchSearchResponse.StatusCode );

            //It is possible, however unlikely, that the search comes back without any items.
            Assert.IsTrue( matchSearchResponse.MatchSearchList.Items.Count > 0 );
        }

        /// <summary>
        /// Specifies good set of input parameters to the serach, and checks they match the output parameters.
        /// </summary>
        [TestMethod]
        public async Task MatchSearchInputMatchesOutput() {

            //Conducting test in production since the development database doesn't always have entries in it.
            var client = new OrionMatchAPIClient( Constants.X_API_KEY, APIStage.PRODUCTION );

            var distance = 325;
            var startDate = new DateTime( 2022, 01, 01 );
            var endDate = new DateTime( 2022, 01, 31 );
            var limit = 7;
            var longitude = -77.5;
            var latitude = 38.7;
            var shootingStyle = "Air Rifle";
            var request = new MatchSearchPublicRequest() {
                Distance = distance,
                StartDate = startDate,
                EndDate = endDate,
                Limit = limit,
                Longitude = longitude,
                Latitude = latitude,
                ShootingStyle = new List<string>() { shootingStyle }
            };

            var matchSearchResponse = await client.GetMatchSearchPublicAsync( request );

            Assert.AreEqual( HttpStatusCode.OK, matchSearchResponse.StatusCode );

            var matchSearchList = matchSearchResponse.MatchSearchList;
            Assert.AreEqual( limit, matchSearchList.Items.Count );
            Assert.AreEqual( distance, matchSearchList.Distance );
            Assert.AreEqual( latitude, matchSearchList.Latitude );
            Assert.AreEqual(longitude, matchSearchList.Longitude );
            Assert.AreEqual( limit, matchSearchList.Limit );
            Assert.AreEqual( startDate.ToString( Scopos.BabelFish.DataModel.Athena.DateTimeFormats.DATE_FORMAT ), matchSearchList.StartDate );
            Assert.AreEqual( endDate.ToString( Scopos.BabelFish.DataModel.Athena.DateTimeFormats.DATE_FORMAT ), matchSearchList.EndDate );
            Assert.IsTrue( matchSearchList.ShootingStyles.Contains( shootingStyle ) );

            foreach( var matchSearchAbbr in matchSearchList.Items ) {
                Assert.AreEqual( shootingStyle, matchSearchAbbr.ShootingStyle );
                Assert.IsFalse( string.IsNullOrEmpty( matchSearchAbbr.MatchID ) );
                Assert.IsFalse( string.IsNullOrEmpty( matchSearchAbbr.MatchName ) );
                Assert.IsFalse( string.IsNullOrEmpty( matchSearchAbbr.OwnerId ) );
                Assert.IsFalse( string.IsNullOrEmpty( matchSearchAbbr.OwnerName ) );

                var returnedStartDate = DateTime.Parse( matchSearchAbbr.StartDate );
                var returnedEndDate = DateTime.Parse( matchSearchAbbr.EndDate );

                Assert.IsTrue( returnedStartDate <= endDate, $"{matchSearchAbbr.MatchName} start date {matchSearchAbbr.StartDate} is after the search's end date {endDate}." );
                Assert.IsTrue( returnedEndDate >= startDate, $"{matchSearchAbbr.MatchName} end date {matchSearchAbbr.EndDate} is before the search's start date {startDate}." );
            }
        }

        /// <summary>
        /// Tests that the Token / NextToken / .GetNextRequest() methods are all working as expected.
        /// </summary>
        [TestMethod]
        public void NextTokenTest() {

            //Conducting test in production since the development database doesn't always have entries in it.
            var client = new OrionMatchAPIClient( Constants.X_API_KEY, APIStage.PRODUCTION );

            var distance = 500;
            var startDate = new DateTime( 2022, 01, 01 );
            var endDate = new DateTime( 2022, 12, 31 );
            var limit = 7;
            var longitude = -77.5;
            var latitude = 38.7;
            var request1 = new MatchSearchPublicRequest() {
                Distance = distance,
                StartDate = startDate,
                EndDate = endDate,
                Limit = limit,
                Longitude = longitude,
                Latitude = latitude
            };

            var taskMatchSearchResponse1 = client.GetMatchSearchPublicAsync( request1 );
            var matchSearchResponse1 = taskMatchSearchResponse1.Result;

            Assert.AreEqual( HttpStatusCode.OK, matchSearchResponse1.StatusCode );
            Assert.AreEqual( limit, matchSearchResponse1.MatchSearchList.Items.Count );

            var request2 = matchSearchResponse1.GetNextRequest();

            var taskMatchSearchResponse2 = client.GetMatchSearchPublicAsync( request2 );
            var matchSearchResponse2 = taskMatchSearchResponse2.Result;

            Assert.AreEqual( HttpStatusCode.OK, matchSearchResponse2.StatusCode );
            Assert.AreEqual( limit, matchSearchResponse2.MatchSearchList.Items.Count );

            var request3 = matchSearchResponse2.GetNextRequest();
            Assert.AreNotEqual( request1.Token, request2.Token );
            Assert.AreNotEqual( request2.Token, request3.Token );
        }
    }
}
