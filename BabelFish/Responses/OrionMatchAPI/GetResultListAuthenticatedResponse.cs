using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scopos.BabelFish.DataModel.OrionMatch;
using Scopos.BabelFish.Requests.OrionMatchAPI;

namespace Scopos.BabelFish.Responses.OrionMatchAPI
{
    public class GetResultListAuthenticatedResponse : GetResultListResponse, ITokenResponse<GetResultListAuthenticatedRequest> {

        public GetResultListAuthenticatedResponse( GetResultListAuthenticatedRequest request ) : base() {
            this.Request = request;
        }

        /// <inheritdoc/>
        public GetResultListAuthenticatedRequest GetNextRequest() {
            var nextRequest = (GetResultListAuthenticatedRequest)Request.Copy();
            nextRequest.Token = Value.ResultList.NextToken;
            return nextRequest;
        }
    }
}
