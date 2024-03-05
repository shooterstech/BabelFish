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
    public class GetMatchPublicResponse : GetMatchAbstractResponse
    {

        public GetMatchPublicResponse(GetMatchPublicRequest request ) : base() {
            this.Request = Request;
        }

		/// <inheritdoc />
		protected internal override DateTime GetCacheValueExpiryTime() {

            try {
                var timeSinceLastUpdate = DateTime.UtcNow - Match.LastUpdated;

                //If it was recently updated, set the expiry time fairly quickly, as more changes may be coming.
                if (timeSinceLastUpdate.TotalMinutes < 5)
                    return DateTime.UtcNow.AddMinutes( 1 );

                if (timeSinceLastUpdate.TotalMinutes < 60)
					return DateTime.UtcNow.AddMinutes( 2 );

				return DateTime.UtcNow.AddMinutes( 10 );
			} catch (Exception ex) {
                //Likely will never get here, if so, likely from a very old match.
                return DateTime.UtcNow.AddMinutes( 10 );
            }
		}
	}
}