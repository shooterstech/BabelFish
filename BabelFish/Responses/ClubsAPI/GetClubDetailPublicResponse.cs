using System;
using System.Collections.Generic;
using System.Text;
using Scopos.BabelFish.DataModel.Clubs;
using Scopos.BabelFish.Requests.ClubsAPI;

namespace Scopos.BabelFish.Responses.ClubsAPI {
    public class GetClubDetailPublicResponse : Response<ClubDetailWrapper> {

		public GetClubDetailPublicResponse( GetClubDetailPublicRequest request ) : base() {
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
