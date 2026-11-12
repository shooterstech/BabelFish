using Scopos.BabelFish.DataModel.Common;
using Scopos.BabelFish.DataModel.Image;
using Scopos.BabelFish.Requests.ImageAPI;

namespace Scopos.BabelFish.Responses.ImageAPI {
    internal class GetPresignedUrlResponse : Response<PresignedUrlWrapper> {

        internal GetPresignedUrlResponse( GetPresignedUrlRequest request ) : base() {
            this.Request = request;
        }

        /// <summary>
        /// Facade function that returns the same as this.Value.PresignedUrl
        /// </summary>
        public PresignedUrl PresignedUrl {
            get { return Value.PresignedUrl; }
        }

        /// <inheritdoc />
        protected internal override DateTime GetCacheValueExpiryTime() {
            // We don't want caching for this response, since the presigned URL is only valid for a short time.
            return DateTime.UtcNow;
        }
    }

    internal class PresignedUrlWrapper : BaseClass {
        public PresignedUrl PresignedUrl { get; set; } = new PresignedUrl();
    }
}
