using System.Net;
using System.Threading.Tasks;
using Scopos.BabelFish.APIClients;
using Scopos.BabelFish.DataModel.Newsfeed;
using Scopos.BabelFish.Requests.NewsfeedAPI;
using Scopos.BabelFish.Responses;
using Scopos.BabelFish.Runtime.Authentication;

namespace Scopos.BabelFish.Tests.APIClients.NewsfeedClientTests {
    [TestClass]
    [TestCategory( "LiveBeta" )]
    [DoNotParallelize]
    public class NewsfeedPostBetaTests : BaseTestClass {
        public TestContext TestContext { get; set; } = null!;

        [TestMethod]
        public async Task PersonalPostCreateDeleteAndRetryThroughBetaClient() {
            var credentials = new UserAuthentication( Constants.TestDev7Credentials.Username, Constants.TestDev7Credentials.Password );
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
                TestContext.WriteLine( $"Beta POST HTTP 200; fixture {postId}; account alias test_dev_7." );
                Assert.AreEqual( text, created.BodyText );
                Assert.AreEqual( Constants.TestDev7UserId, created.UserId );
                Assert.IsNull( created.LicenseNumber );
                Assert.IsNull( created.DeletedAt );
                Assert.IsNull( created.DeletedByUserId );
                Assert.AreEqual( DateTimeKind.Utc, created.CreatedAt.Kind );
                Assert.IsTrue( created.CreatedAt > DateTime.UtcNow.AddMinutes( -5 ) );

                var delete = await client.DeleteNewsfeedPostAuthenticatedAsync( new DeleteNewsfeedPostAuthenticatedRequest( postId!, credentials ) );
                AssertResponse( delete );
                firstDeletion = delete.DeletedPost;
                AssertDeletion( created, firstDeletion, Constants.TestDev7UserId );
                cleanupComplete = true;
                TestContext.WriteLine( "Beta DELETE HTTP 200; complete post and deletion audit verified." );

                var retry = await client.DeleteNewsfeedPostAuthenticatedAsync( postId!, credentials );
                AssertResponse( retry );
                AssertDeletion( created, retry.DeletedPost, Constants.TestDev7UserId );
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

    }
}
