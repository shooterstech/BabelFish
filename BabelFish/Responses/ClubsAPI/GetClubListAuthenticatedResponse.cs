using System;
using System.Collections.Generic;
using System.Text;
using Scopos.BabelFish.Responses;
using Scopos.BabelFish.DataModel.Clubs;
using Scopos.BabelFish.Requests.ClubsAPI;
using Scopos.BabelFish.Responses.ClubsAPI;

namespace Scopos.BabelFish.Responses.ClubsAPI {
    public class GetClubListAuthenticatedResponse : Response<ClubListWrapper>, ITokenResponse<GetClubListAuthenticatedRequest> {

        public GetClubListAuthenticatedResponse( GetClubListAuthenticatedRequest request ) : base(  ) {
            this.Request = request;
        }

        /// <summary>
        /// Facade function that returns the same as this.Value
        /// </summary>
        /// 
        public ClubList ClubList {
            get { return Value.ClubList; }
        }

        /// <inheritdoc/>
        public GetClubListAuthenticatedRequest GetNextRequest() {
            var nextRequest = (GetClubListAuthenticatedRequest)Request.Copy();
            nextRequest.Token = Value.ClubList.NextToken;
            return nextRequest;
        }
    }
}
