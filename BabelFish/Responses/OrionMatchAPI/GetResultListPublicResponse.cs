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
    }
}
