using Scopos.BabelFish.DataModel.Definitions;
using Scopos.BabelFish.Requests.DefinitionAPI;

namespace Scopos.BabelFish.Responses.DefinitionAPI
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T">Should be a concrete implementation of abstract class .DataModel.Definitions.Definition</typeparam>
    public class GetDefinitionListPublicResponse : Response<SparseDefinitionListWrapper>, ITokenResponse<GetDefinitionListPublicRequest> {

        public GetDefinitionListPublicResponse( GetDefinitionListPublicRequest request) : base() {
            this.Request = request;

            this.DefinitionType = request.DefinitionType;
        }

        public DefinitionType DefinitionType { get; private set; }

		/// <summary>
		/// Facade function that returns the same as this.Value
		/// </summary>
		/// 
		public SparseDefinitionList DefinitionList { 
            get { return Value.DefinitionList; }
		}

		/// <inheritdoc/>
		public GetDefinitionListPublicRequest GetNextRequest() {
			var nextRequest = (GetDefinitionListPublicRequest)Request.Copy();
			nextRequest.Token = Value.DefinitionList.NextToken;
			return nextRequest;
		}

		/// <inheritdoc />
		protected internal override DateTime GetCacheValueExpiryTime() {
            //Definition files don't change often, so we can set the expiration time well into the future.
			return DateTime.UtcNow.AddDays(1);
		}
	}
}
