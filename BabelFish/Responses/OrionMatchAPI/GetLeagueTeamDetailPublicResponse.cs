using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scopos.BabelFish.Helpers;
using Scopos.BabelFish.DataModel.OrionMatch;
using Scopos.BabelFish.Requests.OrionMatchAPI;

namespace Scopos.BabelFish.Responses.OrionMatchAPI
{
    public class GetLeagueTeamDetailPublicResponse : Response<LeagueTeamDetailWrapper>
    {

        public GetLeagueTeamDetailPublicResponse( GetLeagueTeamDetailPublicRequest request ) : base() {
            this.Request = Request;
        }

        /// <summary>
        /// Facade function that returns the same as this.Value
        /// </summary>
        public LeagueTeamDetail LeagueTeamDetail
        {
            get { return Value.LeagueTeamDetail; }
		}

        public LeagueTeam LeagueTeam {
            get { return Value.LeagueTeamDetail.LeagueTeam; }
        }

		/// <inheritdoc />
		protected internal override DateTime GetCacheValueExpiryTime() {

            return DateTime.UtcNow.AddMinutes( 10 );
		}
	}
}