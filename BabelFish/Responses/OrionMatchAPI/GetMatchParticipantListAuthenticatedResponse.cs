using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scopos.BabelFish.DataModel.OrionMatch;
using Scopos.BabelFish.Requests.OrionMatchAPI;

namespace Scopos.BabelFish.Responses.OrionMatchAPI
{
    public class GetMatchParticipantListAuthenticatedResponse : Response<MatchParticipantListWrapper>, ITokenResponse<GetMatchParticipantListAuthenticatedRequest> {

        public GetMatchParticipantListAuthenticatedResponse( GetMatchParticipantListAuthenticatedRequest request ) : base() {
            this.Request = request;
        }

        /// <summary>
        /// Facade function that returns the same as this.Value
        /// </summary>
        public MatchParticipantList MatchParticipantList {
            get { return Value.MatchParticipantList; }
        }

        /// <inheritdoc/>
        public GetMatchParticipantListAuthenticatedRequest GetNextRequest() {
            var nextRequest = (GetMatchParticipantListAuthenticatedRequest)Request.Copy();
            nextRequest.Token = Value.MatchParticipantList.NextToken;
            return nextRequest;
        }
    }
}