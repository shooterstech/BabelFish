using Scopos.BabelFish.APIClients;
using Scopos.BabelFish.DataModel.ScoposData;
using Scopos.BabelFish.Runtime.Authentication;

namespace Scopos.BabelFish.Requests.ScoposData {
    public class PatchModerateImageAuthenticatedRequest : Request {

        public PatchModerateImageAuthenticatedRequest( UserAuthentication credentials ) : base( "PatchModerateImage", credentials ) {
            HttpMethod = new HttpMethod( "PATCH" );
            RequiresCredentials = true;
            SubDomain = APISubDomain.INTERNAL;
        }

        public byte[] ImageBytes { get; set; } = Array.Empty<byte>();

        public string ContentType {
            get {

                return FileType switch {
                    ImageFileType.JPEG => "image/jpeg",
                    ImageFileType.PNG => "image/png",
                    _ => "application/octet-stream"
                };
            }
        }

        public ImageFileType FileType { get; set; } = ImageFileType.JPEG;

        public ImageCategory ImageCategory { get; set; } = ImageCategory.CLUB;

        public string Caption { get; set; } = "";

        public string AltText { get; set; } = "";

        public string PrimaryKey { get; set; } = "";

        public string SubKey { get; set; } = "";

        public ImageGroupKeyType GroupKey { get; set; } = ImageGroupKeyType.HEADER;

        /// <inheritdoc />
        public override string RelativePath {
            get { return "/image/moderate"; }
        }

        /// <inheritdoc />
        public override Dictionary<string, List<string>> QueryParameters {
            get {
                return new Dictionary<string, List<string>>() {
                    { "caption", new List<string>() { Caption } },
                    { "alt-text", new List<string>() { AltText } },
                    { "file-type", new List<string>() { FileType.Description() } },
                    { "image-category", new List<string>() { ImageCategory.Description() } },
                    { "primary-key", new List<string>() { PrimaryKey } },
                    { "sub-key", new List<string>() { SubKey } },
                    { "group-key", new List<string>() { GroupKey.Description() } }
                };
            }
        }

        /// <inheritdoc />
        public override HttpContent PostContent {
            get {
                var content = new ByteArrayContent( ImageBytes );
                content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue( ContentType );
                return content;
            }
        }
    }
}
