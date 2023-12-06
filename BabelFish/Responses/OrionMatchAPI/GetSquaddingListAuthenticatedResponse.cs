using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scopos.BabelFish.Responses;
using Scopos.BabelFish.DataModel.OrionMatch;
using Scopos.BabelFish.Helpers;
using Scopos.BabelFish.Requests.OrionMatchAPI;

namespace Scopos.BabelFish.Responses.OrionMatchAPI
{
    public class GetSquaddingListAuthenticatedResponse : Response<SquaddingListWrapper>, ITokenResponse<GetSquaddingListAuthenticatedRequest>
    {

        public GetSquaddingListAuthenticatedResponse( GetSquaddingListAuthenticatedRequest request ) : base() {
            this.Request = request;
        }

        /// <summary>
        /// Facade function that returns the same as this.Value
        /// </summary>
        public SquaddingList SquaddingList {
            get { return Value.SquaddingList; }
        }

        /// <inheritdoc/>
        public GetSquaddingListAuthenticatedRequest GetNextRequest() {
            var nextRequest = (GetSquaddingListAuthenticatedRequest) Request.Copy();
            nextRequest.Token = Value.SquaddingList.NextToken;
            return nextRequest;
        }
    }
}
