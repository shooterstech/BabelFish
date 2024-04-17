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

        /// <inheritdoc />
        protected internal override DateTime GetCacheValueExpiryTime()
        {

            try
            {
                //if today is before end then timeout is 1 min, else, make is 5 min
                if (DateTime.UtcNow > SquaddingList.StartDate && DateTime.UtcNow < SquaddingList.EndDate)
                {
                    return DateTime.UtcNow.AddMinutes(1);
                }
                else
                {
                    return DateTime.UtcNow.AddMinutes(5);
                }
            }
            catch (Exception ex)
            {
                //Likely will never get here, if so, likely from a very old match.
                return DateTime.UtcNow.AddMinutes(10);
            }
        }
    }
}
