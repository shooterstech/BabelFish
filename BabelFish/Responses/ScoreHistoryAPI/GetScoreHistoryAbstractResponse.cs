using System;
using System.Collections.Generic;
using System.Text;
using Scopos.BabelFish.DataModel.ScoreHistory;
using Scopos.BabelFish.Requests.ScoreHistoryAPI;

namespace Scopos.BabelFish.Responses.ScoreHistoryAPI {
	public abstract class GetScoreHistoryAbstractResponse : Response<ScoreHistoryWrapper>, ITokenResponse<GetScoreHistoryAbstractRequest> {

		public ScoreHistoryList ScoreHistoryList {
			get { return Value.ScoreHistoryList; }
		}

		/// <inheritdoc/>
		public GetScoreHistoryAbstractRequest GetNextRequest() {
			if (Request is GetScoreHistoryPublicRequest) {
				var nextRequest = (GetScoreHistoryPublicRequest)Request.Copy();
				nextRequest.Token = Value.ScoreHistoryList.NextToken;
				return nextRequest;
			} else if (Request is GetScoreHistoryAuthenticatedRequest) {
				var nextRequest = (GetScoreHistoryAuthenticatedRequest)Request.Copy();
				nextRequest.Token = Value.ScoreHistoryList.NextToken;
				return nextRequest;
			} else {
				throw new ArgumentException( $"Parameter Request is of unexpected type ${ Request.GetType() }.");
			}
        }

        /// <inheritdoc />
        protected internal override DateTime GetCacheValueExpiryTime() {

            return DateTime.UtcNow.AddSeconds( 10 );
        }
    }
}
