using System;
using System.Collections.Generic;
using System.Text;
using Scopos.BabelFish.DataModel.Clubs;
using Scopos.BabelFish.Requests.ClubsAPI;

namespace Scopos.BabelFish.Responses.ClubsAPI {
    public class GetClubDetailAuthenticatedResponse : Response<ClubDetailWrapper> {

		public GetClubDetailAuthenticatedResponse( GetClubDetailAuthenticatedRequest request ) : base() {
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
