using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Scopos.BabelFish.DataModel.AttributeValue;
using Scopos.BabelFish.DataModel.OrionMatch;
using Scopos.BabelFish.Requests.OrionMatchAPI;

namespace Scopos.BabelFish.Responses.OrionMatchAPI
{
    public class GetResultCOFDetailPublicResponse : GetResultCOFDetailResponse {

        public GetResultCOFDetailPublicResponse( GetResultCOFDetailPublicRequest request ) : base() {
            this.Request = request;
        }

        /// <inheritdoc />
        protected internal override DateTime GetCacheValueExpiryTime() {

            try {
                var timeSinceLastUpdate = DateTime.UtcNow - ResultCOF.LastUpdated;

                //If it was recently updated, set the expiry time fairly quickly, as more changes may be coming.
                if (timeSinceLastUpdate.TotalMinutes < 15)
                    return DateTime.UtcNow.AddSeconds( 30 );

                if (timeSinceLastUpdate.TotalMinutes < 60)
                    return DateTime.UtcNow.AddMinutes( 1 );

                if (timeSinceLastUpdate.TotalHours < 10)
                    return DateTime.UtcNow.AddMinutes( 5 );

                return DateTime.UtcNow.AddMinutes( 10 );
            } catch (Exception ex) {
                //Likely will never get here, if so, likely from a very old match.
                return DateTime.UtcNow.AddMinutes( 10 );
            }
        }
    }
}
