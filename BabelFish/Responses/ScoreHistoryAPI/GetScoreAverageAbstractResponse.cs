using System;
using System.Collections.Generic;
using System.Text;
using Scopos.BabelFish.Responses;
using Scopos.BabelFish.DataModel.ScoreHistory;
using Scopos.BabelFish.Requests;
using Scopos.BabelFish.Requests.ScoreHistoryAPI;

namespace Scopos.BabelFish.Responses.ScoreHistoryAPI {
	public abstract class GetScoreAverageAbstractResponse : Response<ScoreAverageWrapper>, ITokenResponse<GetScoreHistoryAbstractRequest> {

		

        public ScoreAverageList ScoreAverageList {
            get { return Value.ScoreAverageList; }
        }

		/// <inheritdoc/>
		public GetScoreHistoryAbstractRequest GetNextRequest() {
			if (Request is GetScoreAveragePublicRequest) {
				var nextRequest = (GetScoreAveragePublicRequest)Request.Copy();
				nextRequest.Token = Value.ScoreAverageList.NextToken;
				return nextRequest;
			} else if (Request is GetScoreAverageAuthenticatedRequest) {
				var nextRequest = (GetScoreAverageAuthenticatedRequest)Request.Copy();
				nextRequest.Token = Value.ScoreAverageList.NextToken;
				return nextRequest;
			} else {
				throw new ArgumentException( $"Parameter Request is of unexpected type ${Request.GetType()}." );
			}
        }

        /// <inheritdoc />
        protected internal override DateTime GetCacheValueExpiryTime() {

            return DateTime.UtcNow.AddSeconds( 10 );
        }
    }
}
