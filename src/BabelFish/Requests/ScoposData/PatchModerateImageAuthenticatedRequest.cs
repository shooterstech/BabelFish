using Scopos.BabelFish.APIClients;
using Scopos.BabelFish.Runtime.Authentication;

namespace Scopos.BabelFish.Requests.ScoposData {
    public class PatchModerateImageAuthenticatedRequest : Request {

        public PatchModerateImageAuthenticatedRequest( UserAuthentication credentials ) : base( "PatchModerateImage", credentials ) {
            HttpMethod = new HttpMethod( "PATCH" );
            RequiresCredentials = true;
            SubDomain = APISubDomain.INTERNAL;
        }

        public byte[] ImageBytes { get; set; } = Array.Empty<byte>();

        public string Caption { get; set; } = "";

        public string FileName { get; set; } = "";

        public string ContentType {
            get {
                var extension = System.IO.Path.GetExtension( FileName ).ToLowerInvariant();

                return extension switch {
                    ".jpg" => "image/jpeg",
                    ".jpeg" => "image/jpeg",
                    ".png" => "image/png",
                    _ => "application/octet-stream"
                };
            }
        }

        public string AltText { get; set; } = "";

        public string PrimaryKey { get; set; } = "";

        public string SubKey { get; set; } = "";

        public string GroupKey { get; set; } = "";

        /// <inheritdoc />
        public override string RelativePath {
            get { return "/image/moderate"; }
        }

        /// <inheritdoc />
        public override Dictionary<string, List<string>> QueryParameters {
            get {
                return new Dictionary<string, List<string>>() {
                    { "caption", new List<string>() { Caption } },
                    { "file-name", new List<string>() { FileName } },
                    { "alt-text", new List<string>() { AltText } },
                    { "primary-key", new List<string>() { PrimaryKey } },
                    { "sub-key", new List<string>() { SubKey } },
                    { "group-key", new List<string>() { GroupKey } }
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
