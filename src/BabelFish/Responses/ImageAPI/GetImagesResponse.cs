using Scopos.BabelFish.APIClients;
using Scopos.BabelFish.DataModel.Image;
using Scopos.BabelFish.Requests.ImageAPI;

namespace Scopos.BabelFish.Responses.ImageAPI {
    public class GetImagesResponse : Response<ImageListWrapper>, ITokenResponse<GetImagesRequest> {

        public GetImagesResponse( GetImagesRequest request ) : base() {
            this.Request = request;
        }

        /// <summary>
        /// Facade function that returns the same as this.Value
        /// </summary>
        /// 
        public ImageList ImageList {
            get { return Value.BulkImages; }
        }

        /// <inheritdoc/>
        public GetImagesRequest GetNextRequest() {
            if (!this.HasMoreItems)
                throw new NoMoreItemsException( "GetNextRequest() can not return a new request object because there are no more items to return. Always check .HasMoreItems before calling .GetNextRequest()." );

            var nextRequest = (GetImagesRequest)Request.Copy();
            nextRequest.Token = Value.BulkImages.NextToken;
            return nextRequest;
        }

        /// <inheritdoc />
		public bool HasMoreItems {
            get {
                return this.HasOkStatusCode && !string.IsNullOrEmpty( Value.BulkImages.NextToken );
            }
        }

        /// <inheritdoc />
        protected internal override DateTime GetCacheValueExpiryTime() {

            return DateTime.UtcNow.AddMinutes( 10 );
        }

    }
}
