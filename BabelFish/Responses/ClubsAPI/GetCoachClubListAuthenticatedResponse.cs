using System;
using System.Collections.Generic;
using System.Text;
using Scopos.BabelFish.Responses;
using Scopos.BabelFish.DataModel.Clubs;
using Scopos.BabelFish.Requests.ClubsAPI;
using Scopos.BabelFish.Responses.ClubsAPI;

namespace Scopos.BabelFish.Responses.ClubsAPI
{
    public class GetCoachClubListAuthenticatedResponse : Response<ClubListWrapper>, ITokenResponse<GetCoachClubListAuthenticatedRequest>
    {

        public GetCoachClubListAuthenticatedResponse(GetCoachClubListAuthenticatedRequest request) : base()
        {
            this.Request = request;
        }

        /// <summary>
        /// Facade function that returns the same as this.Value
        /// </summary>
        /// 
        public ClubList ClubList
        {
            get { return Value.ClubList; }
        }

        /// <inheritdoc/>
        GetCoachClubListAuthenticatedRequest ITokenResponse<GetCoachClubListAuthenticatedRequest>.GetNextRequest()
        {
            var nextRequest = (GetCoachClubListAuthenticatedRequest)Request.Copy();
            nextRequest.Token = Value.ClubList.NextToken;
            return nextRequest;
        }
    }
}
