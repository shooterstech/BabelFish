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
    public class GetLeaguePublicResponse : Response<LeagueWrapper>
    {

        public GetLeaguePublicResponse(GetLeaguePublicRequest request ) : base() {
            this.Request = Request;
        }

        /// <summary>
        /// Facade function that returns the same as this.Value
        /// </summary>
        public League League
        {
            get { return Value.League; }
		}

		/// <inheritdoc />
		protected internal override DateTime GetCacheValueExpiryTime() {

            return DateTime.UtcNow.AddMinutes( 10 );
		}
	}
}