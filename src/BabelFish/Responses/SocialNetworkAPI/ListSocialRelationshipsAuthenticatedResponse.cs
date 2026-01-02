using System;
using System.Collections.Generic;
using System.Text;
using Scopos.BabelFish.APIClients;
using Scopos.BabelFish.DataModel.SocialNetwork;
using Scopos.BabelFish.Requests.SocialNetworkAPI;

namespace Scopos.BabelFish.Responses.SocialNetworkAPI
{
    public class ListSocialRelationshipsAuthenticatedResponse : Response<ListSocialRelationshipsWrapper>, ITokenResponse<ListSocialRelationshipsAuthenticatedRequest>
    {
        public ListSocialRelationshipsAuthenticatedResponse(ListSocialRelationshipsAuthenticatedRequest request) : base()
        {
            Request = request;
        }

        public SocialRelationshipList SocialRelationshipList
        {
            get { return Value.SocialRelationshipList; }
        }

        /// <inheritdoc/>
        public ListSocialRelationshipsAuthenticatedRequest GetNextRequest() {
            if (!this.HasMoreItems)
                throw new NoMoreItemsException( "GetNextRequest() can not return a new request object because there are no more items to return. Always check .HasMoreItems before calling .GetNextRequest()." );

            var nextRequest = (ListSocialRelationshipsAuthenticatedRequest)Request.Copy();
            nextRequest.Token = Value.SocialRelationshipList.NextToken;
            return nextRequest;
		}

		/// <inheritdoc />
		public bool HasMoreItems {
			get {
				return this.HasOkStatusCode && !string.IsNullOrEmpty( Value.SocialRelationshipList.NextToken );
			}
		}
	}
}
