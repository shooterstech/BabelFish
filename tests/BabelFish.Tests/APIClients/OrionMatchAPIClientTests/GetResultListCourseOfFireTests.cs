using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using Scopos.BabelFish.APIClients;
using Scopos.BabelFish.DataModel.OrionMatch;
using Scopos.BabelFish.Requests;
using Scopos.BabelFish.Requests.OrionMatchAPI;
using Scopos.BabelFish.Responses;
using Scopos.BabelFish.Responses.OrionMatchAPI;
using Scopos.BabelFish.Runtime.Authentication;

namespace Scopos.BabelFish.Tests.APIClients.OrionMatchAPIClientTests {
    [TestClass]
    public class GetResultListCourseOfFireTests {

        private static GetResultListAbstractRequest CreateRequest( bool authenticated ) {
            var matchId = new MatchID( "1.1.2026102300000000.1" );
            // Only a credential reference is needed; bypass Cognito construction/authentication.
            var credentials = authenticated
                ? (UserAuthentication)RuntimeHelpers.GetUninitializedObject( typeof( UserAuthentication ) )
                : null;
            return GetResultListAbstractRequest.Factory( matchId, "Individual - All", credentials );
        }

        private static GetResultListAbstractResponse CreateResponse( GetResultListAbstractRequest request, string token ) {
            GetResultListAbstractResponse response = request is GetResultListPublicRequest publicRequest
                ? new GetResultListPublicResponse( publicRequest )
                : new GetResultListAuthenticatedResponse( (GetResultListAuthenticatedRequest)request );
            // Inject completed response state without HTTP, global initialization, or a production transport seam.
            var responseType = typeof( Response<ResultListWrapper> );
            responseType.GetProperty( nameof( response.Value ) )!.SetValue( response,
                new ResultListWrapper { ResultList = new ResultList { CourseOfFireId = request.CourseOfFireId, NextToken = token } } );
            responseType.GetProperty( nameof( response.OverallStatusCode ) )!.SetValue( response, RequestStatusCode.OK );
            return response;
        }

        [TestMethod]
        [DataRow( false )]
        [DataRow( true )]
        public void ExistingRequestsDefaultToCourseOne( bool authenticated ) {
            var request = CreateRequest( authenticated );

            Assert.AreEqual( 1, request.CourseOfFireId );
            Assert.AreEqual( "1", request.QueryParameters["course-of-fire-id"].Single() );
            Assert.AreEqual( "/match/1.1.2026102300000000.1/result-list/Individual - All", request.RelativePath );
            Assert.AreEqual( HttpMethod.Get, request.HttpMethod );
            Assert.AreEqual( authenticated ? APISubDomain.AUTHAPI : APISubDomain.API, request.SubDomain );
            Assert.AreEqual( authenticated, request.RequiresCredentials );
        }

        [TestMethod]
        [DataRow( false )]
        [DataRow( true )]
        public void SelectedCourseIsSentAlongsidePaginationParameters( bool authenticated ) {
            var request = CreateRequest( authenticated );
            var courseOneQuery = request.QueryString;
            request.CourseOfFireId = 2;
            request.Token = "50";
            request.Limit = 25;

            Assert.AreEqual( "2", request.QueryParameters["course-of-fire-id"].Single() );
            Assert.AreEqual( "50", request.QueryParameters["token"].Single() );
            Assert.AreEqual( "25", request.QueryParameters["limit"].Single() );
            StringAssert.Contains( request.QueryString, "course-of-fire-id=2" );
            Assert.AreNotEqual( courseOneQuery, request.QueryString );
        }

        [TestMethod]
        [DataRow( false, 0 )]
        [DataRow( false, -1 )]
        [DataRow( true, 0 )]
        [DataRow( true, -1 )]
        public void NonpositiveCourseCannotBeSent( bool authenticated, int courseOfFireId ) {
            var request = CreateRequest( authenticated );
            request.CourseOfFireId = courseOfFireId;

            Assert.Throws<ArgumentOutOfRangeException>( () => { _ = request.QueryString; } );
        }

        [TestMethod]
        [DataRow( false )]
        [DataRow( true )]
        public void NextPagePreservesCourseCredentialsAndCallerSettings( bool authenticated ) {
            var request = CreateRequest( authenticated );
            request.CourseOfFireId = 2;
            request.Limit = 25;
            request.IgnoreInMemoryCache = true;
            request.IgnoreFileSystemCache = true;
            request.Timeout = 45;
            request.HeaderKeyValuePairs["X-Test"] = "original";
            if (request is GetResultListPublicRequest publicRequest) {
                publicRequest.Preliminary = true;
            }

            var next = CreateResponse( request, "25" ).GetNextRequest();

            Assert.AreEqual( request.GetType(), next.GetType() );
            Assert.AreEqual( request.MatchID, next.MatchID );
            Assert.AreEqual( request.ResultListName, next.ResultListName );
            Assert.AreEqual( 2, next.CourseOfFireId );
            Assert.AreEqual( "25", next.Token );
            Assert.AreEqual( string.Empty, request.Token );
            Assert.AreEqual( 25, next.Limit );
            Assert.AreSame( request.Credentials, next.Credentials );
            Assert.AreEqual( request.RequiresCredentials, next.RequiresCredentials );
            Assert.AreEqual( request.SubDomain, next.SubDomain );
            Assert.IsTrue( next.IgnoreInMemoryCache );
            Assert.IsTrue( next.IgnoreFileSystemCache );
            Assert.AreEqual( 45, next.Timeout );
            Assert.AreEqual( "2", next.QueryParameters["course-of-fire-id"].Single() );
            if (next is GetResultListPublicRequest nextPublic) {
                Assert.IsTrue( nextPublic.Preliminary );
                Assert.AreEqual( "True", next.QueryParameters["preliminary"].Single() );
            }
            next.HeaderKeyValuePairs["X-Test"] = "changed";
            Assert.AreEqual( "original", request.HeaderKeyValuePairs["X-Test"] );
        }

        [TestMethod]
        [DataRow( false )]
        [DataRow( true )]
        public void FinalPageCannotGenerateAnotherRequest( bool authenticated ) {
            var response = CreateResponse( CreateRequest( authenticated ), string.Empty );

            Assert.IsFalse( response.HasMoreItems );
            Assert.Throws<NoMoreItemsException>( () => response.GetNextRequest() );
        }
    }
}
