using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scopos.BabelFish.DataModel.OrionMatch;
using Scopos.BabelFish.Requests.OrionMatchAPI;

namespace Scopos.BabelFish.Responses.OrionMatchAPI
{
    public class GetMatchParticipantListPublicResponse : GetMatchParticipantListAbstractResponse {

        public GetMatchParticipantListPublicResponse( GetMatchParticipantListPublicRequest request ) : base() {
            this.Request = request;
        }

		/// <inheritdoc />
		protected internal override DateTime GetCacheValueExpiryTime() {

			try {
				var lastUpdated = DateTime.MinValue;
				foreach ( var participant in MatchParticipantList.Items ) {
					if ( participant.LastUpdated > lastUpdated)
						lastUpdated = participant.LastUpdated;
				}

				var timeSinceLastUpdate = DateTime.UtcNow - lastUpdated;

				//If it was recently updated, set the expiry time fairly quickly, as more changes may be coming.
				if (timeSinceLastUpdate.TotalMinutes < 5)
					return DateTime.UtcNow.AddMinutes( 1 );

				if (timeSinceLastUpdate.TotalMinutes < 60)
					return DateTime.UtcNow.AddMinutes( 5 );

				if (timeSinceLastUpdate.TotalHours < 10)
					return DateTime.UtcNow.AddMinutes( 15 );

				if (timeSinceLastUpdate.TotalDays < 2)
					return DateTime.UtcNow.AddMinutes( 60 );

				return DateTime.UtcNow.AddDays( 1 );
			} catch (Exception ex) {
				//Likely will never get here, if so, likely from a very old match.
				return DateTime.UtcNow.AddDays( 1 );
			}
		}
	}
}