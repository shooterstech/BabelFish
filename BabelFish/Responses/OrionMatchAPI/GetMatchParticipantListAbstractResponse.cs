using System;
using System.Collections.Generic;
using System.Text;
using Scopos.BabelFish.APIClients;
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

        /// <inheritdoc />
        public GetMatchParticipantListAbstractRequest GetNextRequest() {
            if (! this.HasMoreItems)
                throw new NoMoreItemsException( "GetNextRequest() can not return a new request object because there are no more items to return. Always check .HasMoreItems before calling .GetNextRequest()." );

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

		/// <inheritdoc />
		public bool HasMoreItems {
			get {
				return this.HasOkStatusCode && !string.IsNullOrEmpty( Value.MatchParticipantList.NextToken );
			}
		}

		/// <inheritdoc />
		protected internal override DateTime GetCacheValueExpiryTime() {

            try {
                //if today is before end then timeout is 1 min, else, make is 5 min
                if (DateTime.Today <= MatchParticipantList.EndDate) {
                    return DateTime.UtcNow.AddMinutes( 1 );
                } else {
                    return DateTime.UtcNow.AddMinutes( 5 );
                }
            } catch (Exception ex) {
                //Likely will never get here, if so, likely from a very old match.
                return DateTime.UtcNow.AddMinutes( 5 );
            }
        }

    }
}
