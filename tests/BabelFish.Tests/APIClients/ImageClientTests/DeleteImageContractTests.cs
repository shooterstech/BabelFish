using System.Net;
using System.Net.Http;
using System.Runtime.CompilerServices;
using Scopos.BabelFish.APIClients;
using Scopos.BabelFish.DataModel.Image;
using Scopos.BabelFish.Requests;
using Scopos.BabelFish.Requests.ImageAPI;
using Scopos.BabelFish.Responses.ImageAPI;
using Scopos.BabelFish.Runtime.Authentication;

namespace Scopos.BabelFish.Tests.APIClients.ImageClientTests {
    // Deliberately independent of BaseTestClass: no API keys, login, or cloud setup.
    [TestClass]
    public class DeleteImageContractTests {
        private const string ImageId = "00000000-0000-4000-8000-000000000002";

        private static UserAuthentication Credentials() =>
            (UserAuthentication)RuntimeHelpers.GetUninitializedObject( typeof( UserAuthentication ) );

        [TestMethod]
        public async System.Threading.Tasks.Task RequestUsesAuthenticatedDeleteWithPathIdOnly() {
            var credentials = Credentials();
            var request = new DeleteImageAuthenticatedRequest( ImageId, credentials );

            Assert.AreEqual( "DeleteImage", request.OperationId );
            Assert.AreEqual( HttpMethod.Delete, request.HttpMethod );
            Assert.AreEqual( APISubDomain.AUTHAPI, request.SubDomain );
            Assert.AreSame( credentials, request.Credentials );
            Assert.IsTrue( request.RequiresCredentials );
            Assert.AreEqual( $"/image/{ImageId}", request.RelativePath );
            Assert.AreEqual( 0, request.QueryParameters.Count );
            Assert.AreEqual( string.Empty, request.QueryString );
            using var content = request.PostContent;
            Assert.AreEqual( string.Empty, await content.ReadAsStringAsync() );
            Assert.IsTrue( request.IgnoreInMemoryCache );
            Assert.IsTrue( request.IgnoreFileSystemCache );
        }

        [TestMethod]
        [DataRow( null )]
        [DataRow( "" )]
        [DataRow( " " )]
        [DataRow( "not-a-uuid" )]
        [DataRow( "00000000-0000-4000-8000-000000000002/other" )]
        [DataRow( "00000000-0000-4000-8000-000000000002?image-id=other" )]
        public void RequestRejectsMissingOrUnsafeImageId( string? imageId ) {
            var request = new DeleteImageAuthenticatedRequest( Credentials() ) { ImageId = imageId! };
            Assert.Throws<APIRequestParameterException>( () => _ = request.RelativePath );
        }

        [TestMethod]
        public void RequestCanonicalizesUuidAndRequiresCredentials() {
            var request = new DeleteImageAuthenticatedRequest(
                "ABCDEF01000040008000000000000002", Credentials() );
            Assert.AreEqual( "/image/abcdef01-0000-4000-8000-000000000002", request.RelativePath );
            Assert.Throws<ArgumentNullException>( () => new DeleteImageAuthenticatedRequest( null! ) );
        }

        [TestMethod]
        public void SuccessfulResponseConvertsNamedPayloadAndExposesFacade() {
            SerializerOptions.InitSerializers();
            var request = new DeleteImageAuthenticatedRequest( ImageId, Credentials() );
            var response = new DeleteImageAuthenticatedResponse( request );
            Assert.AreSame( request, response.Request );
            Assert.IsNull( response.DeletedImage );

            // Exercise the shared Response.Body conversion, without invoking authentication or transport.
            using var body = G_STJ.JsonDocument.Parse(
                "{\"Message\":[],\"Permissions\":{},\"DeletedImage\":{\"ImageId\":\"" + ImageId +
                "\",\"Caption\":\"Caption\",\"AltText\":\"Alt text\",\"Url\":\"https://example.test/image.jpeg\",\"CreatedAt\":\"2026-10-19T12:00:00.000000\",\"FutureField\":true}}" );
            SetBody( response, HttpStatusCode.OK, body );

            Assert.IsNotNull( response.DeletedImage );
            Assert.AreSame( response.Value.DeletedImage, response.DeletedImage );
            Assert.AreEqual( ImageId, response.DeletedImage.ImageId );
            Assert.IsInstanceOfType<ScoposImageAbbr>( response.DeletedImage );
            Assert.AreEqual( "Caption", response.DeletedImage.Caption );
            Assert.AreEqual( "Alt text", response.DeletedImage.AltText );
            Assert.AreEqual( "https://example.test/image.jpeg", response.DeletedImage.Url );
            // Match the existing converter's handling of GET's timezone-less timestamp.
            Assert.AreEqual( new DateTime( 2026, 10, 19, 12, 0, 0 ).ToUniversalTime(), response.DeletedImage.CreatedAt );
            Assert.IsTrue( response.DeletedImage.IsValid );

            var newtonsoft = G_NS.JsonConvert.DeserializeObject<DeletedImageWrapper>(
                body.RootElement.GetRawText(), SerializerOptions.NewtonsoftJsonSerializer );
            Assert.IsNotNull( newtonsoft?.DeletedImage );
            Assert.AreEqual( response.DeletedImage.ImageId, newtonsoft.DeletedImage.ImageId );
            Assert.AreEqual( response.DeletedImage.Url, newtonsoft.DeletedImage.Url );
            Assert.AreEqual( response.DeletedImage.Caption, newtonsoft.DeletedImage.Caption );
            Assert.AreEqual( response.DeletedImage.AltText, newtonsoft.DeletedImage.AltText );
            Assert.AreEqual( response.DeletedImage.CreatedAt, newtonsoft.DeletedImage.CreatedAt.ToUniversalTime() );

            // GET and DELETE must populate the same existing image type, including the wire field "Url".
            string imageJson = body.RootElement.GetProperty( "DeletedImage" ).GetRawText();
            var list = G_STJ.JsonSerializer.Deserialize<ImageListWrapper>(
                "{\"BulkImages\":{\"Items\":[" + imageJson + "]}}", SerializerOptions.SystemTextJsonDeserializer );
            Assert.IsNotNull( list );
            Assert.AreEqual( response.DeletedImage.Url, list.BulkImages.Items[0].Url );
            Assert.AreEqual( response.DeletedImage.CreatedAt, list.BulkImages.Items[0].CreatedAt );
            var roundTrip = G_STJ.JsonSerializer.Deserialize<ScoposImageAbbr>(
                G_STJ.JsonSerializer.Serialize( response.DeletedImage, SerializerOptions.SystemTextJsonDeserializer ),
                SerializerOptions.SystemTextJsonDeserializer );
            Assert.AreEqual( response.DeletedImage.Url, roundTrip!.Url );
        }

        [TestMethod]
        [DataRow( 400 )]
        [DataRow( 401 )]
        [DataRow( 403 )]
        [DataRow( 404 )]
        [DataRow( 500 )]
        public void ErrorResponseDoesNotExposeDeletionConfirmation( int statusCode ) {
            var response = new DeleteImageAuthenticatedResponse(
                new DeleteImageAuthenticatedRequest( ImageId, Credentials() ) );
            using var body = G_STJ.JsonDocument.Parse( "{\"Message\":[\"Request failed\"]}" );
            SetBody( response, (HttpStatusCode)statusCode, body );

            Assert.IsNotNull( response.Value );
            Assert.IsNull( response.DeletedImage );
        }

        private static void SetBody( DeleteImageAuthenticatedResponse response, HttpStatusCode statusCode, G_STJ.JsonDocument body ) {
            // Internal setters belong to the transport; reflection keeps production visibility unchanged.
            var type = typeof( DeleteImageAuthenticatedResponse );
            type.GetProperty( "RestApiStatusCode" )!.SetValue( response, statusCode );
            type.GetProperty( "Body" )!.SetValue( response, body );
        }
    }
}
