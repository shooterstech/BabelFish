using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scopos.BabelFish.APIClients;
using Scopos.BabelFish.DataModel.OrionMatch;
using Scopos.BabelFish.Requests.OrionMatchAPI;

namespace Scopos.BabelFish.Responses.OrionMatchAPI
{
    public class GetLeagueGamesPublicResponse : Response<LeagueGameListWrapper>, ITokenResponse<GetLeagueGamesPublicRequest> {

        public GetLeagueGamesPublicResponse( GetLeagueGamesPublicRequest request ) : base() {
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
        public GetLeagueGamesPublicRequest GetNextRequest() {
            if (!this.HasMoreItems)
                throw new NoMoreItemsException( "GetNextRequest() can not return a new request object because there are no more items to return. Always check .HasMoreItems before calling .GetNextRequest()." );

            var nextRequest = (GetLeagueGamesPublicRequest)Request.Copy();
            nextRequest.Token = Value.LeagueGames.NextToken;
            return nextRequest;
		}

		/// <inheritdoc />
		public bool HasMoreItems {
			get {
				return this.HasOkStatusCode && !string.IsNullOrEmpty( Value.LeagueGames.NextToken );
			}
		}

		/// <inheritdoc />
		protected internal override DateTime GetCacheValueExpiryTime() {

			return DateTime.UtcNow.AddMinutes( 1 );
		}
	}
}
