using System;
using System.Collections.Generic;
using System.Text;
using Scopos.BabelFish.DataModel.AttributeValue;
using Scopos.BabelFish.DataModel.OrionMatch;
using Scopos.BabelFish.Requests.OrionMatchAPI;

namespace Scopos.BabelFish.Responses.OrionMatchAPI {
    public abstract class GetMatchParticipantListAbstractResponse : Response<MatchParticipantListWrapper>, ITokenResponse<GetMatchParticipantListAbstractRequest> {

        public GetMatchParticipantListAbstractResponse() : base() {

        }

        /// <summary>
        /// Facade function that returns the same as this.Value
        /// </summary>
        public MatchParticipantList MatchParticipantList {
            get { return Value.MatchParticipantList; }
        }

        public GetMatchParticipantListAbstractRequest GetNextRequest() {
            if (Request is GetMatchParticipantListPublicRequest) {
                var nextRequest = (GetMatchParticipantListPublicRequest)Request.Copy();
                nextRequest.Token = Value.MatchParticipantList.NextToken;
                return nextRequest;
            } else if (Request is GetMatchParticipantListAuthenticatedRequest) {
                var nextRequest = (GetMatchParticipantListAuthenticatedRequest)Request.Copy();
                nextRequest.Token = Value.MatchParticipantList.NextToken;
                return nextRequest;
            } else {
                throw new ArgumentException( $"Parameter Request is of unexpected type ${Request.GetType()}." );
            }
        }

    }
}
