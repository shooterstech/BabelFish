using System;
using System.Collections.Generic;
using System.Text;
using ShootersTech.BabelFish.DataModel.Clubs;
using ShootersTech.BabelFish.Requests.ClubsAPI;

namespace ShootersTech.BabelFish.Responses.ClubsAPI {
    public class GetClubDetailResponse : Response<ClubDetailWrapper> {

		public GetClubDetailResponse( GetClubDetailRequest request ) : base() {
			this.Request = request;
		}

		/// <summary>
		/// Facade function that returns the same as this.Value
		/// </summary>
		/// 
		public ClubDetail ClubDetail {
			get { return Value.ClubDetail; }
		}
	}
}
