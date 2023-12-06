using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scopos.BabelFish.DataModel.OrionMatch;
using Scopos.BabelFish.Requests.OrionMatchAPI;

namespace Scopos.BabelFish.Responses.OrionMatchAPI
{
    public class MatchSearchAuthenticatedResponse : Response<MatchSearchWrapper>, ITokenResponse<MatchSearchAuthenticatedRequest> {

        public MatchSearchAuthenticatedResponse( MatchSearchAuthenticatedRequest request ) : base() {
            Request = request;
        }

        /// <summary>
        /// Facade function that returns the same as this.Value
        /// </summary>
        public MatchSearchList MatchSearchList
        {
            get { return Value.MatchSearchList; }
        }

        /// <inheritdoc/>
        public MatchSearchAuthenticatedRequest GetNextRequest() {
            var nextRequest = (MatchSearchAuthenticatedRequest)Request.Copy();
            nextRequest.Token = Value.MatchSearchList.NextToken;
            return nextRequest;
        }
    }
}
