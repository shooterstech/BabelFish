using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.VisualBasic.FileIO;
using Scopos.BabelFish.APIClients;
using Scopos.BabelFish.DataModel.Newsfeed;
using Scopos.BabelFish.Requests.NewsfeedAPI;
using Scopos.BabelFish.Responses;
using Scopos.BabelFish.Runtime.Authentication;

namespace Scopos.BabelFish.Tests.APIClients.NewsfeedClientTests {
    [TestClass]
    [TestCategory( "LiveBeta" )]
    [DoNotParallelize]
    public class NewsfeedPostBetaTests {
        public TestContext TestContext { get; set; } = null!;

        [TestMethod]
        public async Task PersonalPostCreateDeleteAndRetryThroughBetaClient() {
            // Explicit opt-in prevents an ordinary test run from creating remote fixtures.
            if (Environment.GetEnvironmentVariable( "SCOPOS_NEWSFEED_LIVE_BETA" ) != "1")
                Assert.Inconclusive( "Set SCOPOS_NEWSFEED_LIVE_BETA=1 to authorize this Beta create/soft-delete round trip." );
            var csvPath = Environment.GetEnvironmentVariable( "SCOPOS_TEST_USERS_CSV" );
            var apiKey = Environment.GetEnvironmentVariable( "ScoposXApiKey" );
            if (string.IsNullOrEmpty( apiKey ) || string.IsNullOrEmpty( csvPath ) || !File.Exists( csvPath ))
                Assert.Inconclusive( "ScoposXApiKey and SCOPOS_TEST_USERS_CSV must identify configured local test inputs." );
            var alias = Environment.GetEnvironmentVariable( "SCOPOS_NEWSFEED_TEST_ALIAS" ) ?? "test_dev_7";
            var account = ReadAccount( csvPath!, alias );
            Initializer.Initialize( apiKey!, false );
            var credentials = new UserAuthentication( account.Username, account.Password );
            try {
                await credentials.InitializeAsync();
            } catch (Exception ex) {
                // Do not put credential-bearing exception details into the test report.
                var types = new List<string>();
                for (Exception? error = ex; error != null; error = error.InnerException)
                    types.Add( error.GetType().Name );
                Assert.Fail( $"Test-account authentication failed ({string.Join( " -> ", types )})." );
            }

            // Application calls are pinned to Beta; never fall back to another stage.
            var client = new NewsfeedAPIClient( APIStage.BETA );
            var text = $"  BabelFish Beta newsfeed test {Guid.NewGuid():N}\r\nVerbatim é😀 <text> & + % \"quotes\"  ";
            string? postId = null;
            Post? created = null;
            Post? firstDeletion = null;
            var cleanupComplete = false;
            try {
                var create = await client.CreateNewsfeedPostAuthenticatedAsync( text, credentials );
                // Capture the ID before assertions so a malformed success can still be cleaned up.
                postId = create.Post?.PostId;
                if (string.IsNullOrEmpty( postId ) && create.Body != null &&
                    create.Body.RootElement.TryGetProperty( "Post", out var rawPost ) &&
                    rawPost.TryGetProperty( "PostId", out var rawId ) && rawId.ValueKind == G_STJ.JsonValueKind.String)
                    postId = rawId.GetString();
                AssertResponse( create );
                created = create.Post;
                Assert.IsNotNull( created );
                Assert.IsFalse( string.IsNullOrEmpty( postId ) );
                TestContext.WriteLine( $"Beta POST HTTP 200; fixture {postId}; account alias {alias}." );
                Assert.AreEqual( text, created.BodyText );
                Assert.AreEqual( account.UserId, created.UserId );
                Assert.IsNull( created.LicenseNumber );
                Assert.IsNull( created.DeletedAt );
                Assert.IsNull( created.DeletedByUserId );
                Assert.AreEqual( DateTimeKind.Utc, created.CreatedAt.Kind );
                Assert.IsTrue( created.CreatedAt > DateTime.UtcNow.AddMinutes( -5 ) );

                var delete = await client.DeleteNewsfeedPostAuthenticatedAsync( new DeleteNewsfeedPostAuthenticatedRequest( postId!, credentials ) );
                AssertResponse( delete );
                firstDeletion = delete.DeletedPost;
                AssertDeletion( created, firstDeletion, account.UserId );
                cleanupComplete = true;
                TestContext.WriteLine( "Beta DELETE HTTP 200; complete post and deletion audit verified." );

                var retry = await client.DeleteNewsfeedPostAuthenticatedAsync( postId!, credentials );
                AssertResponse( retry );
                AssertDeletion( created, retry.DeletedPost, account.UserId );
                Assert.AreEqual( firstDeletion!.DeletedAt, retry.DeletedPost!.DeletedAt );
                Assert.AreEqual( firstDeletion.DeletedByUserId, retry.DeletedPost.DeletedByUserId );
                TestContext.WriteLine( "Beta repeated DELETE HTTP 200; first deletion audit preserved." );
            } finally {
                // Never repeat a POST after an uncertain result. DELETE is idempotent for this captured ID.
                if (!cleanupComplete && !string.IsNullOrEmpty( postId )) {
                    var cleanup = await client.DeleteNewsfeedPostAuthenticatedAsync( postId, credentials );
                    AssertResponse( cleanup );
                    Assert.IsNotNull( cleanup.DeletedPost?.DeletedAt, $"Cleanup unconfirmed for fixture {postId}." );
                    TestContext.WriteLine( $"Beta fixture {postId} soft-deleted during cleanup." );
                }
            }
        }

        private static void AssertResponse<T>( Response<T> response ) where T : Scopos.BabelFish.DataModel.Common.BaseClass, new() {
            // Avoid outputting raw response bodies or exception strings on failure.
            Assert.AreEqual( HttpStatusCode.OK, response.RestApiStatusCode, $"Beta HTTP {(int)response.RestApiStatusCode}; client status {response.OverallStatusCode}." );
            Assert.IsTrue( response.HasOkStatusCode, $"Client status {response.OverallStatusCode}." );
            Assert.IsFalse( response.InMemoryCachedResponse );
            Assert.IsFalse( response.FileSystemCachedResponse );
        }

        private static void AssertDeletion( Post created, Post? deleted, string userId ) {
            Assert.IsNotNull( deleted );
            Assert.AreEqual( created.PostId, deleted.PostId );
            Assert.AreEqual( created.BodyText, deleted.BodyText );
            Assert.AreEqual( created.CreatedAt, deleted.CreatedAt );
            Assert.AreEqual( created.UserId, deleted.UserId );
            Assert.AreEqual( created.LicenseNumber, deleted.LicenseNumber );
            Assert.IsNotNull( deleted.DeletedAt );
            Assert.AreEqual( DateTimeKind.Utc, deleted.DeletedAt.Value.Kind );
            Assert.IsTrue( deleted.DeletedAt >= created.CreatedAt );
            Assert.AreEqual( userId, deleted.DeletedByUserId );
        }

        private static (string Username, string Password, string UserId) ReadAccount( string path, string alias ) {
            // The supplied CSV has introductory notes and duplicate username columns. Use the first login column.
            using var parser = new TextFieldParser( path, Encoding.UTF8 );
            parser.SetDelimiters( "," );
            parser.HasFieldsEnclosedInQuotes = true;
            string[]? header = null;
            while (!parser.EndOfData) {
                var row = parser.ReadFields()!;
                if (header == null) {
                    if (row.Length > 0 && row[0] == "Alias Accounts" && row.Contains( "User Name" ) && row.Contains( "Password" ) && row.Contains( "User ID" ))
                        header = row;
                } else if (row.Length >= header.Length && row[0] == alias) {
                    return (row[Array.IndexOf( header, "User Name" )], row[Array.IndexOf( header, "Password" )], row[Array.IndexOf( header, "User ID" )]);
                }
            }
            Assert.Inconclusive( "Configured test alias was not found in the supplied account table." );
            return default;
        }
    }
}
