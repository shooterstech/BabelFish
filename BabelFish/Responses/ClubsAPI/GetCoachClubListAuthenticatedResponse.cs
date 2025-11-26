using System;
using System.Collections.Generic;
using System.Text;
using Scopos.BabelFish.Responses;
using Scopos.BabelFish.DataModel.Clubs;
using Scopos.BabelFish.Requests.ClubsAPI;
using Scopos.BabelFish.Responses.ClubsAPI;
using Scopos.BabelFish.APIClients;

namespace Scopos.BabelFish.Responses.ClubsAPI {
    public class GetCoachClubListAuthenticatedResponse : Response<ClubListWrapper>, ITokenResponse<GetCoachClubListAuthenticatedRequest> {

        public GetCoachClubListAuthenticatedResponse( GetCoachClubListAuthenticatedRequest request ) : base() {
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
        GetCoachClubListAuthenticatedRequest ITokenResponse<GetCoachClubListAuthenticatedRequest>.GetNextRequest() {
            if (!this.HasMoreItems)
                throw new NoMoreItemsException( "GetNextRequest() can not return a new request object because there are no more items to return. Always check .HasMoreItems before calling .GetNextRequest()." );

            var nextRequest = (GetCoachClubListAuthenticatedRequest)Request.Copy();
            nextRequest.Token = Value.ClubList.NextToken;
            return nextRequest;
        }

        /// <inheritdoc />
        public bool HasMoreItems {
            get {
                return this.HasOkStatusCode && !string.IsNullOrEmpty( Value.ClubList.NextToken );
            }
        }
    }
}
