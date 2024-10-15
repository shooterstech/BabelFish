using System;
using System.Collections.Generic;
using System.Text;
using Scopos.BabelFish.Responses;
using Scopos.BabelFish.DataModel.Clubs;
using Scopos.BabelFish.Requests.ClubsAPI;
using Scopos.BabelFish.Responses.ClubsAPI;

namespace Scopos.BabelFish.Responses.ClubsAPI
{
    public class GetCoachClubListPublicResponse : Response<ClubListWrapper>, ITokenResponse<GetCoachClubListPublicRequest>
    {

        public GetCoachClubListPublicResponse(GetCoachClubListPublicRequest request) : base()
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
        GetCoachClubListPublicRequest ITokenResponse<GetCoachClubListPublicRequest>.GetNextRequest()
        {
            var nextRequest = (GetCoachClubListPublicRequest)Request.Copy();
            nextRequest.Token = Value.ClubList.NextToken;
            return nextRequest;
        }

        /// <inheritdoc />
        protected internal override DateTime GetCacheValueExpiryTime() {

            return DateTime.UtcNow.AddMinutes( 10 );
        }
    }
}
