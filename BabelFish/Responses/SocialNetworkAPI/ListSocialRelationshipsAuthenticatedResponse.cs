using System;
using System.Collections.Generic;
using System.Text;
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

        public ListSocialRelationshipsAuthenticatedRequest GetNextRequest()
        {
            var nextRequest = (ListSocialRelationshipsAuthenticatedRequest)Request.Copy();
            nextRequest.Token = Value.SocialRelationshipList.NextToken;
            return nextRequest;
		}

		/// <inheritdoc />
		public bool HasMoreItems {
			get {
				return !string.IsNullOrEmpty( Value.SocialRelationshipList.NextToken );
			}
		}
	}
}
