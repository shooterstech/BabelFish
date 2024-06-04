using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scopos.BabelFish.DataModel.OrionMatch;
using Scopos.BabelFish.Requests.OrionMatchAPI;

namespace Scopos.BabelFish.Responses.OrionMatchAPI {
    public class GetLeagueTeamsPublicResponse : Response<LeagueTeamListWrapper>, ITokenResponse<GetLeagueTeamsPublicRequest> {

        public GetLeagueTeamsPublicResponse( GetLeagueTeamsPublicRequest request ) : base() {
            this.Request = request;
        }

        /// <summary>
        /// Facade function that returns the same as this.Value
        /// </summary>
        public LeagueTeamList LeagueTeams {
            get { return Value.LeagueTeams; }
        }

        /// <inheritdoc/>
        public GetLeagueTeamsPublicRequest GetNextRequest() {
            var nextRequest = (GetLeagueTeamsPublicRequest)Request.Copy();
            nextRequest.Token = Value.LeagueTeams.NextToken;
            return nextRequest;
        }

        /// <inheritdoc />
        protected internal override DateTime GetCacheValueExpiryTime() {

            return DateTime.UtcNow.AddMinutes( 10 );
        }
    }
}
