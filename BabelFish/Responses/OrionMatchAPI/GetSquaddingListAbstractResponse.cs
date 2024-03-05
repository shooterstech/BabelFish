using System;
using System.Collections.Generic;
using System.Text;
using Scopos.BabelFish.DataModel.AttributeValue;
using Scopos.BabelFish.DataModel.OrionMatch;
using Scopos.BabelFish.Requests.OrionMatchAPI;

namespace Scopos.BabelFish.Responses.OrionMatchAPI {
    public abstract class GetSquaddingListAbstractResponse : Response<SquaddingListWrapper>, ITokenResponse<GetSquaddingListAbstractRequest> {

        public GetSquaddingListAbstractResponse() : base () {

        }

        /// <summary>
        /// Facade function that returns the same as this.Value
        /// </summary>
        public SquaddingList SquaddingList {
            get { return Value.SquaddingList; }
        }

        public GetSquaddingListAbstractRequest GetNextRequest() {
            if (Request is GetSquaddingListPublicRequest) {
                var nextRequest = (GetSquaddingListPublicRequest)Request.Copy();
                nextRequest.Token = Value.SquaddingList.NextToken;
                return nextRequest;
            } else if (Request is GetSquaddingListAuthenticatedRequest) {
                var nextRequest = (GetSquaddingListAuthenticatedRequest)Request.Copy();
                nextRequest.Token = Value.SquaddingList.NextToken;
                return nextRequest;
            } else {
                throw new ArgumentException( $"Parameter Request is of unexpected type ${Request.GetType()}." );
            }
        }
    }
}
