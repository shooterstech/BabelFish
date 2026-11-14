using System.Linq;
using System.Web;
using Scopos.BabelFish.DataModel.OrionMatch;
using Scopos.BabelFish.Requests;
using Scopos.BabelFish.Requests.OrionMatchAPI;
using Scopos.BabelFish.Runtime.Authentication;

namespace Scopos.BabelFish.Tests.DataModel.OrionMatchTests.RangeReporter {
    // Pure request tests: no BaseTestClass setup, authentication, or API calls.
    [TestClass]
    public class RangeReportEmailAdditionalAddressesTests {
        [TestMethod]
        [DataRow(false)]
        [DataRow(true)]
        public void AdditionalAddressesAreOptionalAndEncodedAsRepeatedQueryValues( bool league ) {
            var credentials = new UserAuthentication( "reporter@example.com", "unused" );
            var matchId = new MatchID( "1.1.2025111411171861.1" );
            var addresses = new List<string>();
            Request request = league
                ? new SendLeagueRangeReportEmailAuthenticatedRequest(
                    new MatchID( "1.1.2025071411032772.3" ), matchId, credentials ) {
                    AdditionalEmailAddresses = addresses
                }
                : new SendRangeReportEmailAuthenticatedRequest(
                    matchId, new[] { "Individual - All" }, credentials ) {
                    AdditionalEmailAddresses = addresses
                };

            Assert.IsFalse( request.QueryParameters.ContainsKey( "additional-email-address" ) );
            Assert.AreSame( credentials, request.Credentials );
            Assert.IsTrue( request.RequiresCredentials );

            // A literal plus must survive encoding and must not become a space.
            addresses.Add( "coach+reports@example.com" );
            AssertAddresses( request, addresses );
            addresses.Add( "press@example.com" );
            AssertAddresses( request, addresses );
            addresses.Clear();
            Assert.IsFalse( request.QueryParameters.ContainsKey( "additional-email-address" ) );
        }

        private static void AssertAddresses( Request request, List<string> expected ) {
            CollectionAssert.AreEqual( expected, request.QueryParameters["additional-email-address"] );
            var queryValues = request.QueryString.Split( '&' )
                .Where( pair => pair.StartsWith( "additional-email-address=" ) )
                .Select( pair => HttpUtility.UrlDecode( pair.Substring( pair.IndexOf( '=' ) + 1 ) ) )
                .ToList();
            CollectionAssert.AreEqual( expected, queryValues );
        }
    }
}
