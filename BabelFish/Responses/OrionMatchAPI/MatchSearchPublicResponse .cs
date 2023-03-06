using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scopos.BabelFish.DataModel.OrionMatch;
using Scopos.BabelFish.Requests.OrionMatchAPI;

namespace Scopos.BabelFish.Responses.OrionMatchAPI
{
    public class MatchSearchPublicResponse : Response<MatchSearchWrapper>, ITokenResponse<MatchSearchPublicRequest> {

        public MatchSearchPublicResponse( MatchSearchPublicRequest request ) : base() {
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
        public MatchSearchPublicRequest GetNextRequest() {
            var nextRequest = (MatchSearchPublicRequest)Request.Copy();
            nextRequest.Token = Value.MatchSearchList.NextToken;
            return nextRequest;
        }
    }
}
