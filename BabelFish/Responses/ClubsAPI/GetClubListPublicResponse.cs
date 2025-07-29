using System;
using System.Collections.Generic;
using System.Text;
using Scopos.BabelFish.Responses;
using Scopos.BabelFish.DataModel.Clubs;
using Scopos.BabelFish.Requests.ClubsAPI;
using Scopos.BabelFish.Responses.ClubsAPI;

namespace Scopos.BabelFish.Responses.ClubsAPI {
    public class GetClubListPublicResponse : Response<ClubListWrapper>, ITokenResponse<GetClubListPublicRequest> {

        public GetClubListPublicResponse( GetClubListPublicRequest request ) : base(  ) {
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
        public GetClubListPublicRequest GetNextRequest() {
            var nextRequest = (GetClubListPublicRequest)Request.Copy();
            nextRequest.Token = Value.ClubList.NextToken;
            return nextRequest;
		}

        /// <inheritdoc />
		public bool HasMoreItems {
			get {
				return !string.IsNullOrEmpty( Value.ClubList.NextToken );
			}
		}

		/// <inheritdoc />
		protected internal override DateTime GetCacheValueExpiryTime() {

            return DateTime.UtcNow.AddMinutes( 10 );
        }
    }
}
