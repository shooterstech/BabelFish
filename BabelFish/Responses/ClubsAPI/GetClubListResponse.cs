using System;
using System.Collections.Generic;
using System.Text;
using ShootersTech.BabelFish.ClubsAPI;
using ShootersTech.BabelFish.DataModel.Clubs;
using ShootersTech.BabelFish.Requests.ClubsAPI;

namespace ShootersTech.BabelFish.Responses.ClubsAPI {
    public class GetClubListResponse : Response<ClubListWrapper> {

        public GetClubListResponse( GetClubListRequest request ) : base(  ) {
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
