using System.Collections.Concurrent;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Scopos.BabelFish.APIClients;
using Scopos.BabelFish.DataModel.Common;
using Scopos.BabelFish.DataModel.OrionMatch;
using Scopos.BabelFish.Helpers;
using Scopos.BabelFish.Requests.OrionMatchAPI;
using Scopos.BabelFish.Responses;
using Scopos.BabelFish.Responses.OrionMatchAPI;
using Scopos.BabelFish.Runtime;
using Scopos.BabelFish.Runtime.Authentication;

namespace Scopos.BabelFish.Tests.APIClients.OrionMatchAPIClientTests {
    [TestClass]
    [DoNotParallelize]
    public class ResultListCourseOfFireContractTests {
        private const string ParentId = "1.1.2026102300000000.1";
        private const string ChildId = "1.1.2026102300000000.1001";
        private string _originalApiKey = string.Empty;
        private ResponseCache _originalCache = null!;
        private DirectoryInfo? _originalDirectory;

        [TestInitialize]
        public void InitializeOfflineContractTests() {
            // Initialize converters only: no application, Cognito, SQL, or definition lookup.
            SerializerOptions.InitSerializers();
            _originalApiKey = Settings.XApiKey;
            _originalCache = ResponseCache.CACHE;
            _originalDirectory = OrionMatchAPIClient.LocalStoreDirectory;
            Settings.XApiKey = "offline-contract-test-key";
            ResponseCache.CACHE = (ResponseCache)Activator.CreateInstance( typeof( ResponseCache ), true )!;
        }

        [TestCleanup]
        public void RestoreProcessSettings() {
            Settings.XApiKey = _originalApiKey;
            ResponseCache.CACHE = _originalCache;
            OrionMatchAPIClient.LocalStoreDirectory = _originalDirectory;
        }

        private static GetResultListAbstractRequest ResultRequest( bool authenticated, int course = 2 ) {
            GetResultListAbstractRequest request = authenticated
                ? new GetResultListAuthenticatedRequest( new MatchID( ParentId ), "Individual - All",
                    (UserAuthentication)RuntimeHelpers.GetUninitializedObject( typeof( UserAuthentication ) ) )
                : new GetResultListPublicRequest( new MatchID( ParentId ), "Individual - All" );
            request.CourseOfFireId = course;
            return request;
        }

        private static GetResultListAbstractResponse ReadResultResponse( bool authenticated, string json ) {
            var request = ResultRequest( authenticated );
            GetResultListAbstractResponse response = request is GetResultListPublicRequest publicRequest
                ? new GetResultListPublicResponse( publicRequest )
                : new GetResultListAuthenticatedResponse( (GetResultListAuthenticatedRequest)request );
            // Exercise Response<T>.Body and the registered API converters, rather than injecting Value.
            SetResponseBody( response, json );
            return response;
        }

        private static void SetResponseBody<T>( Response<T> response, string json ) where T : BaseClass, new() {
            typeof( Response<T> ).GetProperty( nameof( response.RestApiStatusCode ) )!.SetValue( response, HttpStatusCode.OK );
            typeof( Response<T> ).GetProperty( nameof( response.OverallStatusCode ) )!.SetValue( response, RequestStatusCode.OK );
            typeof( Response<T> ).GetProperty( nameof( response.Body ) )!.SetValue( response, JsonDocument.Parse( json ) );
        }

        // Synthetic HTTP body after GetResultList_v3 strips DynamoDB/S3 storage fields.
        // NextToken is numeric on the partial-list branch and a string on ordinary pages.
        private static string ResultBody( int course, string tokenJson = "null", int rank = 1 ) => $$"""
            {
              "Message": [], "Permissions": {}, "MetaData": {},
              "ResultList": {
                "MatchID": "{{ParentId}}", "ResultName": "Individual - All",
                "CourseOfFireId": {{course}}, "JSONVersion": "2.0.0",
                "NextToken": {{tokenJson}}, "Partial": false,
                "Metadata": {"{{ParentId}}": {
                  "MatchID": "{{ParentId}}", "OwnerId": "offline-owner", "Creator": "Orion",
                  "Status": "OFFICIAL", "StartDate": "2026-10-23", "EndDate": "2026-10-24",
                  "LastUpdated": "2026-10-24T12:00:00Z"
                }
                },
                "Items": [{
                  "MatchID": "{{ParentId}}", "Rank": {{rank}},
                  "ResultCOFID": "course-{{course}}-rank-{{rank}}",
                  "Participant": {"ParticipantType": "Individual", "DisplayName": "Offline athlete", "AttributeValues": []}
                }]
              }
            }
            """;

        [TestMethod]
        [DataRow( false, "50" )]
        [DataRow( true, "50" )]
        [DataRow( false, "\"50\"" )]
        [DataRow( true, "\"50\"" )]
        public void CourseTwoResponseDeserializesAndKeepsCourseOnNextPage( bool authenticated, string tokenJson ) {
            var response = ReadResultResponse( authenticated, ResultBody( 2, tokenJson ) );

            Assert.AreEqual( 2, response.ResultList.CourseOfFireId );
            Assert.AreEqual( new MatchID( ParentId ), response.ResultList.MatchID );
            Assert.AreEqual( "Individual - All", response.ResultList.ResultName );
            Assert.AreEqual( "course-2-rank-1", response.ResultList.Items.Single().ResultCOFID );
            Assert.IsInstanceOfType<Individual>( response.ResultList.Items.Single().Participant );
            Assert.IsTrue( response.HasMoreItems );
            var next = response.GetNextRequest();
            Assert.AreEqual( "50", next.Token );
            Assert.AreEqual( 2, next.CourseOfFireId );
            Assert.AreEqual( authenticated, next.RequiresCredentials );
            Assert.AreSame( response.Request.Credentials, next.Credentials );
        }

        [TestMethod]
        [DataRow( false, "" )]
        [DataRow( true, "" )]
        [DataRow( false, ", \"NextToken\": null" )]
        [DataRow( true, ", \"NextToken\": null" )]
        [DataRow( false, ", \"NextToken\": \"\"" )]
        [DataRow( true, ", \"NextToken\": \"\"" )]
        public void LegacyEmptyResponseDefaultsToCourseOneAndHasNoNextPage( bool authenticated, string tokenField ) {
            var response = ReadResultResponse( authenticated, $$"""
                {
                  "ResultList": {
                    "MatchID": "{{ParentId}}", "ResultName": "Individual - All", "Items": []{{tokenField}}
                  }
                }
                """ );

            Assert.AreEqual( 1, response.ResultList.CourseOfFireId );
            Assert.HasCount( 0, response.ResultList.Items );
            Assert.IsFalse( response.HasMoreItems );
            Assert.Throws<NoMoreItemsException>( () => response.GetNextRequest() );
        }

        [TestMethod]
        [DataRow( 400 )]
        [DataRow( 401 )]
        [DataRow( 404 )]
        [DataRow( 500 )]
        public async Task ErrorResponseRetainsMessageAndCannotContinuePagination( int statusCode ) {
            using var transport = new OfflineTransport( _ => new HttpResponseMessage( (HttpStatusCode)statusCode ) {
                Content = new StringContent( "{\"Message\":[\"Offline parameter or permission failure\"],\"ResultList\":{\"NextToken\":\"50\"}}" )
            } );
            var client = CreateOfflineClient( transport );
            var response = await client.GetResultListPublicAsync( (GetResultListPublicRequest)ResultRequest( false ) );

            Assert.AreEqual( (HttpStatusCode)statusCode, response.RestApiStatusCode );
            Assert.AreEqual( RequestStatusCode.RestApiServerError, response.OverallStatusCode );
            Assert.AreEqual( "Offline parameter or permission failure", response.MessageResponse.Message.Single() );
            Assert.IsFalse( response.HasMoreItems );
            Assert.Throws<NoMoreItemsException>( () => response.GetNextRequest() );
            Assert.HasCount( 1, transport.Calls );
        }

        [TestMethod]
        public async Task MergedRetrievalSeparatesSameNamedCoursesAcrossEveryPage() {
            using var transport = new OfflineTransport( uri => {
                var query = System.Web.HttpUtility.ParseQueryString( uri.Query );
                int course = int.Parse( query["course-of-fire-id"]!, CultureInfo.InvariantCulture );
                bool firstPage = query["token"] == null;
                return JsonResponse( ResultBody( course, firstPage ? "\"1\"" : "null", firstPage ? 1 : 2 ) );
            } );
            var client = CreateOfflineClient( transport );
            var merged = new MergedResultList();
            foreach (int course in new[] { 1, 2 }) {
                merged.ResultListMembers.Add( new ResultListMember {
                    MatchId = new MatchID( ParentId ), CourseOfFireId = course, ResultName = "Individual - All"
                } );
            }

            var lists = await client.GetResultListsAsync( merged );

            Assert.HasCount( 2, lists );
            Assert.HasCount( 4, transport.Calls );
            foreach (int course in new[] { 1, 2 }) {
                var list = lists.Single( item => item.CourseOfFireId == course );
                CollectionAssert.AreEqual( new[] { $"course-{course}-rank-1", $"course-{course}-rank-2" },
                    list.Items.Select( item => item.ResultCOFID ).ToArray() );
                var calls = transport.Calls.Where( uri => System.Web.HttpUtility.ParseQueryString( uri.Query )["course-of-fire-id"] == course.ToString( CultureInfo.InvariantCulture ) ).ToArray();
                Assert.HasCount( 2, calls );
                Assert.AreEqual( 1, calls.Count( uri => System.Web.HttpUtility.ParseQueryString( uri.Query )["token"] == null ) );
                Assert.AreEqual( 1, calls.Count( uri => System.Web.HttpUtility.ParseQueryString( uri.Query )["token"] == "1" ) );
            }
            foreach (var member in merged.ResultListMembers) {
                Assert.AreEqual( "Individual - All", member.ResultName );
            }
        }

        [TestMethod]
        [DataRow( false )]
        [DataRow( true )]
        public async Task FilteredMatchResponsePreservesParentCoursesAndChildDetails( bool child ) {
            string matchId = child ? ChildId : ParentId;
            // Mirrors GetMatchDetail's public allowlist and parent-owned MatchStructure.
            // This fixture verifies client compatibility; backend filtering is tested in RestAPI.
            string json = $$"""
                {
                  "Message": [], "Permissions": {"{{ParentId}}": ["match.read"]},
                  "MetaData": {"Type": "Match", "Tournaments": []},
                  "Match": {
                    "MatchID": "{{matchId}}", "ParentID": "{{ParentId}}",
                    "Name": "{{(child ? "Child" : "Parent")}}", "OwnerId": "offline-owner",
                    "Visibility": "PUBLIC", "MemberPolicy": "OPEN", "JSONVersion": "2.0.0",
                    "ClubAddress": {"City": "{{(child ? "Child city" : "Parent city")}}"},
                    "MatchContacts": [{"Name": "Offline contact"}], "Officials": [],
                    "ScoringSystems": ["Orion Scoring System"],
                    "MatchStructure": {"CoursesOfFire": [
                      {"CourseOfFireId": 1, "CourseOfFireName": "Parent course one", "ResultLists": []},
                      {"CourseOfFireId": 2, "CourseOfFireName": "Parent course two", "ResultLists": []}
                    ], "GlobalAttributes": []}
                  }
                }
                """;
            using var transport = new OfflineTransport( _ => JsonResponse( json ) );
            var response = await CreateOfflineClient( transport ).GetMatchPublicAsync( new MatchID( matchId ) );

            Assert.AreEqual( RequestStatusCode.OK, response.OverallStatusCode, response.ExceptionMessage );
            Assert.AreEqual( new MatchID( matchId ), response.Match.MatchID );
            Assert.AreEqual( new MatchID( ParentId ), response.Match.MatchID.GetParentMatchID() );
            Assert.AreEqual( child ? "Child" : "Parent", response.Match.Name );
            Assert.AreEqual( child ? "Child city" : "Parent city", response.Match.ClubAddress.City );
            Assert.AreEqual( MemberPolicyOption.OPEN, response.Match.MemberPolicy );
            Assert.HasCount( 2, response.Match.MatchStructure.CoursesOfFire );
            CollectionAssert.AreEqual( new[] { 1, 2 }, response.Match.MatchStructure.CoursesOfFire.Select( cof => cof.CourseOfFireId ).ToArray() );
            Assert.AreEqual( "Parent course two", response.Match.MatchStructure.CoursesOfFire[1].CourseOfFireName );
            Assert.AreSame( response.Match, response.Match.MatchStructure.Match );
            Assert.AreSame( response.Match.MatchStructure, response.Match.MatchStructure.CoursesOfFire[1].MatchStructure );
            // Verify omission of this legacy field remains safe for the existing client model.
#pragma warning disable CS0618
            Assert.AreEqual( string.Empty, response.Match.SharedKey );
#pragma warning restore CS0618
            Assert.IsFalse( response.Body.RootElement.GetProperty( "Match" ).TryGetProperty( "SharedKey", out _ ) );
            Assert.AreEqual( "match.read", response.Permissions[ParentId].Single().ToString() );
            Assert.IsInstanceOfType<MatchDetailMetaData>( response.MetaData );
            Assert.HasCount( 1, transport.Calls );
        }

        private static HttpResponseMessage JsonResponse( string json ) => new HttpResponseMessage( HttpStatusCode.OK ) {
            Content = new StringContent( json, Encoding.UTF8, "application/json" )
        };

        private static OrionMatchAPIClient CreateOfflineClient( OfflineTransport transport ) {
            var client = new OrionMatchAPIClient( APIStage.BETA ) { IgnoreInMemoryCache = true, IgnoreFileSystemCache = true };
            // No public transport injection exists. Replace the existing timeout-15 client in tests only.
            // This uses the real CallAPIAsync pipeline and cannot fall through to a network handler.
            var clients = (ConcurrentDictionary<int, HttpClient>)typeof( APIClient<OrionMatchAPIClient> )
                .GetField( "_httpClients", BindingFlags.Instance | BindingFlags.NonPublic )!.GetValue( client )!;
            clients[15] = new HttpClient( transport, disposeHandler: false );
            transport.Client = clients[15];
            return client;
        }

        private sealed class OfflineTransport : HttpMessageHandler {
            private readonly Func<Uri, HttpResponseMessage> _respond;
            public ConcurrentQueue<Uri> Calls { get; } = new ConcurrentQueue<Uri>();
            public HttpClient? Client { get; set; }

            public OfflineTransport( Func<Uri, HttpResponseMessage> respond ) => _respond = respond;

            protected override Task<HttpResponseMessage> SendAsync( HttpRequestMessage request, CancellationToken cancellationToken ) {
                var uri = request.RequestUri!;
                Assert.AreEqual( HttpMethod.Get, request.Method );
                Assert.AreEqual( "api-stage.scopos.tech", uri.Host );
                StringAssert.StartsWith( uri.AbsolutePath, "/beta/match/" );
                Calls.Enqueue( uri );
                return Task.FromResult( _respond( uri ) );
            }

            protected override void Dispose( bool disposing ) {
                if (disposing) Client?.Dispose();
                base.Dispose( disposing );
            }
        }
    }
}
