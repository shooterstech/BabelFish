using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Scopos.BabelFish.APIClients;
using Scopos.BabelFish.DataModel.Newsfeed;
using Scopos.BabelFish.Requests;
using Scopos.BabelFish.Requests.NewsfeedAPI;
using Scopos.BabelFish.Responses;
using Scopos.BabelFish.Responses.NewsfeedAPI;
using Scopos.BabelFish.Runtime.Authentication;

namespace Scopos.BabelFish.Tests.APIClients.NewsfeedClientTests {
    // No BaseTestClass, API key, authentication, or external services in these contract tests.
    [TestClass]
    public class NewsfeedPostContractTests {
        private static UserAuthentication Credentials() =>
            (UserAuthentication)RuntimeHelpers.GetUninitializedObject( typeof( UserAuthentication ) );

        [TestInitialize]
        public void InitializeSerializers() => SerializerOptions.InitSerializers();

        [TestMethod]
        public async Task CreateUsesAuthenticatedPostWithVerbatimJsonTextAndOptionalClubQuery() {
            var credentials = Credentials();
            var text = "  Quotes: \"\\\" & + % <p>é😀</p>\r\n{\"json\":true}\n  ";
            var request = new CreateNewsfeedPostAuthenticatedRequest( text, credentials );
            AssertRequest( request, credentials, "CreateNewsfeedPost", HttpMethod.Post, "/newsfeed/posts" );
            Assert.AreEqual( 0, request.QueryParameters.Count );
            using var content = request.PostContent;
            Assert.AreEqual( "application/json", content.Headers.ContentType!.MediaType );
            Assert.AreEqual( "utf-8", content.Headers.ContentType.CharSet );
            using var json = G_STJ.JsonDocument.Parse( await content.ReadAsStringAsync() );
            Assert.AreEqual( text, json.RootElement.GetProperty( "body-text" ).GetString() );
            Assert.AreEqual( 1, json.RootElement.EnumerateObject().Count() );

            // Query parameters must not depend on a caller's number formatting culture.
            var previous = CultureInfo.CurrentCulture;
            try {
                CultureInfo.CurrentCulture = new CultureInfo( "ar-SA" );
                request.LicenseNumber = int.MaxValue;
                Assert.AreEqual( "2147483647", request.QueryParameters["license-number"][0] );
                Assert.AreEqual( "license-number=2147483647", request.QueryString );
            } finally {
                CultureInfo.CurrentCulture = previous;
            }
        }

        [TestMethod]
        [DataRow( null )]
        [DataRow( "" )]
        [DataRow( " \t\r\n" )]
        public void CreateRejectsMissingOrBlankTextBeforeAuthentication( string? text ) {
            var request = new CreateNewsfeedPostAuthenticatedRequest( text!, Credentials() );
            Assert.Throws<APIRequestParameterException>( () => request.PreRequestMethodAsync() );
            Assert.Throws<APIRequestParameterException>( () => _ = request.PostParameters );
        }

        [TestMethod]
        public async Task CreateEnforcesDecodedUtf8ByteLimitWithoutCountingJsonEscapes() {
            // Quotes expand in JSON; emoji occupy four UTF-8 bytes but two UTF-16 code units.
            foreach (var text in new[] { new string( '"', 65535 ), new string( 'é', 32767 ) + "a", string.Concat( Enumerable.Repeat( "😀", 16383 ) ) + "abc" }) {
                var request = new CreateNewsfeedPostAuthenticatedRequest( text, Credentials() );
                await request.PreRequestMethodAsync();
                using var content = request.PostParameters;
                using var json = G_STJ.JsonDocument.Parse( await content.ReadAsStringAsync() );
                Assert.AreEqual( text, json.RootElement.GetProperty( "body-text" ).GetString() );
            }
            foreach (var text in new[] { new string( 'a', 65536 ), new string( 'é', 32768 ), string.Concat( Enumerable.Repeat( "😀", 16384 ) ), "invalid\ud800", "invalid\udc00" }) {
                var request = new CreateNewsfeedPostAuthenticatedRequest( text, Credentials() );
                Assert.Throws<APIRequestParameterException>( () => _ = request.PostParameters );
                Assert.Throws<APIRequestParameterException>( () => request.PreRequestMethodAsync() );
            }
        }

        [TestMethod]
        [DataRow( 0 )]
        [DataRow( -1 )]
        public void CreateRejectsNonpositiveClubLicense( int license ) {
            var request = new CreateNewsfeedPostAuthenticatedRequest( "Text", Credentials(), license );
            Assert.Throws<APIRequestParameterException>( () => _ = request.QueryString );
        }

        [TestMethod]
        [DataRow( "1" )]
        [DataRow( "9007199254740993" )]
        [DataRow( "18446744073709551615" )]
        public async Task DeleteUsesExactDecimalStringPathWithNoAttributionParameters( string id ) {
            var credentials = Credentials();
            var request = new DeleteNewsfeedPostAuthenticatedRequest( id, credentials );
            AssertRequest( request, credentials, "DeleteNewsfeedPost", HttpMethod.Delete, $"/newsfeed/posts/{id}" );
            Assert.AreEqual( string.Empty, request.QueryString );
            using var body = request.PostContent;
            Assert.AreEqual( string.Empty, await body.ReadAsStringAsync() );
        }

        [TestMethod]
        [DataRow( null )]
        [DataRow( "" )]
        [DataRow( "0" )]
        [DataRow( "01" )]
        [DataRow( "-1" )]
        [DataRow( "+1" )]
        [DataRow( " 1" )]
        [DataRow( "1\n" )]
        [DataRow( "1.0" )]
        [DataRow( "1e3" )]
        [DataRow( "١" )]
        [DataRow( "18446744073709551616" )]
        [DataRow( "100000000000000000000" )]
        [DataRow( "1/other?post-id=2" )]
        public void DeleteRejectsMissingOutOfRangeOrUnsafePathIds( string? id ) {
            var request = new DeleteNewsfeedPostAuthenticatedRequest( id!, Credentials() );
            Assert.Throws<APIRequestParameterException>( () => _ = request.RelativePath );
        }

        [TestMethod]
        public void BothOperationsRequireCredentials() {
            Assert.Throws<ArgumentNullException>( () => new CreateNewsfeedPostAuthenticatedRequest( null! ) );
            Assert.Throws<ArgumentNullException>( () => new DeleteNewsfeedPostAuthenticatedRequest( null! ) );
        }

        [TestMethod]
        [DataRow( false )]
        [DataRow( true )]
        public void ResponsesPreserveNamedPayloadCompletePostAndUtcMicroseconds( bool deleted ) {
            var json = "{\"PostId\":\"18446744073709551615\",\"LicenseNumber\":8,\"BodyText\":\" Text \"," +
                "\"CreatedAt\":\"2026-11-13T12:00:00.123456Z\",\"UserId\":\"creator\",\"FutureField\":true," +
                (deleted ? "\"DeletedAt\":\"2026-11-13T12:01:00.654321Z\",\"DeletedByUserId\":\"deleter\"}" : "\"DeletedAt\":null,\"DeletedByUserId\":null}");
            var key = deleted ? "DeletedPost" : "Post";
            using var body = G_STJ.JsonDocument.Parse( "{\"Message\":[],\"Permissions\":{},\"MetaData\":{},\"" + key + "\":" + json + "}" );
            Post post;
            if (deleted) {
                var request = new DeleteNewsfeedPostAuthenticatedRequest( "1", Credentials() );
                var response = new DeleteNewsfeedPostAuthenticatedResponse( request );
                Assert.IsNull( response.DeletedPost );
                SetBody( response, HttpStatusCode.OK, body );
                Assert.AreSame( request, response.Request );
                Assert.AreSame( response.Value.DeletedPost, response.DeletedPost );
                post = response.DeletedPost!;
            } else {
                var request = new CreateNewsfeedPostAuthenticatedRequest( "Text", Credentials() );
                var response = new CreateNewsfeedPostAuthenticatedResponse( request );
                Assert.IsNull( response.Post );
                SetBody( response, HttpStatusCode.OK, body );
                Assert.AreSame( request, response.Request );
                Assert.AreSame( response.Value.Post, response.Post );
                post = response.Post!;
            }
            Assert.AreEqual( "18446744073709551615", post.PostId );
            Assert.AreEqual( 8, post.LicenseNumber );
            Assert.AreEqual( " Text ", post.BodyText );
            Assert.AreEqual( "creator", post.UserId );
            Assert.AreEqual( new DateTime( 2026, 11, 13, 12, 0, 0, DateTimeKind.Utc ).AddTicks( 1234560 ), post.CreatedAt );
            Assert.AreEqual( DateTimeKind.Utc, post.CreatedAt.Kind );
            if (deleted) {
                Assert.AreEqual( new DateTime( 2026, 11, 13, 12, 1, 0, DateTimeKind.Utc ).AddTicks( 6543210 ), post.DeletedAt );
                Assert.AreEqual( DateTimeKind.Utc, post.DeletedAt!.Value.Kind );
                Assert.AreEqual( "deleter", post.DeletedByUserId );
            } else {
                Assert.IsNull( post.DeletedAt );
                Assert.IsNull( post.DeletedByUserId );
            }
            var newtonsoft = G_NS.JsonConvert.DeserializeObject<Post>( json, SerializerOptions.NewtonsoftJsonSerializer )!;
            Assert.AreEqual( post.PostId, newtonsoft.PostId );
            Assert.AreEqual( post.LicenseNumber, newtonsoft.LicenseNumber );
            Assert.AreEqual( post.BodyText, newtonsoft.BodyText );
            Assert.AreEqual( post.CreatedAt, newtonsoft.CreatedAt );
            Assert.AreEqual( post.DeletedAt, newtonsoft.DeletedAt );
            Assert.AreEqual( post.DeletedByUserId, newtonsoft.DeletedByUserId );
            var roundTrip = G_STJ.JsonSerializer.Deserialize<Post>(
                G_NS.JsonConvert.SerializeObject( post, SerializerOptions.NewtonsoftJsonSerializer ), SerializerOptions.SystemTextJsonDeserializer )!;
            Assert.AreEqual( post.PostId, roundTrip.PostId );
            Assert.AreEqual( post.CreatedAt, roundTrip.CreatedAt );
            Assert.AreEqual( post.DeletedAt, roundTrip.DeletedAt );
        }

        [TestMethod]
        public void PersonalPostSupportsOmittedNullableFieldsAndEmptyEnvelope() {
            var post = G_STJ.JsonSerializer.Deserialize<Post>(
                "{\"PostId\":\"9007199254740993\",\"BodyText\":\"Personal\",\"CreatedAt\":\"2026-11-13T12:00:00.000000Z\",\"UserId\":\"creator\"}",
                SerializerOptions.SystemTextJsonDeserializer )!;
            Assert.AreEqual( "9007199254740993", post.PostId );
            Assert.IsNull( post.LicenseNumber );
            Assert.IsNull( post.DeletedAt );
            Assert.IsNull( post.DeletedByUserId );
            using var empty = G_STJ.JsonDocument.Parse( "{}" );
            var response = new CreateNewsfeedPostAuthenticatedResponse( new CreateNewsfeedPostAuthenticatedRequest( Credentials() ) );
            SetBody( response, HttpStatusCode.OK, empty );
            Assert.IsNull( response.Post );
        }

        [TestMethod]
        [DataRow( 400 )]
        [DataRow( 401 )]
        [DataRow( 403 )]
        [DataRow( 404 )]
        [DataRow( 500 )]
        public void ErrorsDoNotExposeSuccessfulMutationPayloads( int status ) {
            using var body = G_STJ.JsonDocument.Parse( "{\"Message\":[\"Request failed\"]}" );
            var create = new CreateNewsfeedPostAuthenticatedResponse( new CreateNewsfeedPostAuthenticatedRequest( Credentials() ) );
            var delete = new DeleteNewsfeedPostAuthenticatedResponse( new DeleteNewsfeedPostAuthenticatedRequest( Credentials() ) );
            SetBody( create, (HttpStatusCode)status, body );
            SetBody( delete, (HttpStatusCode)status, body );
            Assert.IsNotNull( create.Value );
            Assert.IsNotNull( delete.Value );
            Assert.IsNull( create.Post );
            Assert.IsNull( delete.DeletedPost );
        }

        private static void AssertRequest( Request request, UserAuthentication credentials, string operation, HttpMethod method, string path ) {
            Assert.AreEqual( operation, request.OperationId );
            Assert.AreEqual( method, request.HttpMethod );
            Assert.AreEqual( path, request.RelativePath );
            Assert.AreEqual( APISubDomain.AUTHAPI, request.SubDomain );
            Assert.AreSame( credentials, request.Credentials );
            Assert.IsTrue( request.RequiresCredentials );
            Assert.IsTrue( request.IgnoreInMemoryCache );
            Assert.IsTrue( request.IgnoreFileSystemCache );
        }

        private static void SetBody<T>( Response<T> response, HttpStatusCode status, G_STJ.JsonDocument body )
            where T : Scopos.BabelFish.DataModel.Common.BaseClass, new() {
            // Exercise the actual shared response converter without changing transport setter visibility.
            response.GetType().GetProperty( "RestApiStatusCode" )!.SetValue( response, status );
            response.GetType().GetProperty( "Body" )!.SetValue( response, body );
        }
    }
}
