using System;
using System.Collections.Generic;
using System.Text;
using Scopos.BabelFish.DataModel.Clubs;
using Scopos.BabelFish.Requests.ClubsAPI;
using Scopos.BabelFish.Responses.ClubsAPI;

namespace Scopos.BabelFish.Responses.ClubsAPI {
    public class GetClubListAuthenticatedResponse : Response<ClubListWrapper> {

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
    }
}
