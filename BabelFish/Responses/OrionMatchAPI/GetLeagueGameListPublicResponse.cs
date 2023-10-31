using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scopos.BabelFish.DataModel.OrionMatch;
using Scopos.BabelFish.Requests.OrionMatchAPI;

namespace Scopos.BabelFish.Responses.OrionMatchAPI
{
    public class GetLeagueGameListPublicResponse : Response<LeagueGameListWrapper>, ITokenResponse<GetLeagueGameListPublicRequest> {

        public GetLeagueGameListPublicResponse( GetLeagueGameListPublicRequest request ) : base() {
            this.Request = request;
        }

        /// <summary>
        /// Facade function that returns the same as this.Value
        /// </summary>
        public LeagueGameList LeagueGames
        {
            get { return Value.LeagueGames; }
        }

        /// <inheritdoc/>
        public GetLeagueGameListPublicRequest GetNextRequest() {
            var nextRequest = (GetLeagueGameListPublicRequest)Request.Copy();
            nextRequest.Token = Value.LeagueGames.NextToken;
            return nextRequest;
		}
	}
}
