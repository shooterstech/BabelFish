using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scopos.BabelFish.DataModel.OrionMatch;
using Scopos.BabelFish.Requests.OrionMatchAPI;

namespace Scopos.BabelFish.Responses.OrionMatchAPI
{
    public class GetResultListPublicResponse : Response<ResultListWrapper>, ITokenResponse<GetResultListPublicRequest> {

        public GetResultListPublicResponse( GetResultListPublicRequest request ) : base() {
            this.Request = request;
        }

        /// <summary>
        /// Facade function that returns the same as this.Value
        /// </summary>
        public ResultList ResultList
        {
            get { return Value.ResultList; }
        }

        /// <inheritdoc/>
        public GetResultListPublicRequest GetNextRequest() {
            var nextRequest = (GetResultListPublicRequest)Request.Copy();
            nextRequest.Token = Value.ResultList.NextToken;
            return nextRequest;
		}

		/// <inheritdoc />
		protected internal override DateTime GetCacheValueExpiryTime() {

			try {
				var timeSinceLastUpdate = DateTime.UtcNow - ResultList.LastUpdated;

				//If it was recently updated, set the expiry time fairly quickly, as more changes may be coming.
				if (timeSinceLastUpdate.TotalMinutes < 5)
					return DateTime.UtcNow.AddSeconds( 30 );

				if (timeSinceLastUpdate.TotalMinutes < 60)
					return DateTime.UtcNow.AddMinutes( 1 );

				if (timeSinceLastUpdate.TotalHours < 10)
					return DateTime.UtcNow.AddMinutes( 5 );

				if (timeSinceLastUpdate.TotalDays < 7)
					return DateTime.UtcNow.AddMinutes( 30 );

				return DateTime.UtcNow.AddDays( 1 );
			} catch (Exception ex) {
				//Likely will never get here, if so, likely from a very old match.
				return DateTime.UtcNow.AddDays( 1 );
			}
		}
	}
}
