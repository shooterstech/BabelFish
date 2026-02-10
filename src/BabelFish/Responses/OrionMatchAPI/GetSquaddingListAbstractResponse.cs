using Scopos.BabelFish.APIClients;
using Scopos.BabelFish.DataModel.OrionMatch;
using Scopos.BabelFish.Requests.OrionMatchAPI;

namespace Scopos.BabelFish.Responses.OrionMatchAPI {
    public abstract class GetSquaddingListAbstractResponse : Response<SquaddingListWrapper>, ITokenResponse<GetSquaddingListAbstractRequest> {

        public GetSquaddingListAbstractResponse() : base() {

        }

        /// <summary>
        /// Facade function that returns the same as this.Value
        /// </summary>
        public SquaddingList SquaddingList {
            get { return Value.SquaddingList; }
        }

        /// <inheritdoc />
        public GetSquaddingListAbstractRequest GetNextRequest() {
            if (!this.HasMoreItems)
                throw new NoMoreItemsException( "GetNextRequest() can not return a new request object because there are no more items to return. Always check .HasMoreItems before calling .GetNextRequest()." );

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
        public bool HasMoreItems {
            get {
                return this.HasOkStatusCode && !string.IsNullOrEmpty( Value.SquaddingList.NextToken );
            }
        }

        /// <summary>
        /// Deserilization of a AttributeValueDataPacket is handled by the overridden ReadJson()
        /// method of AttributeValueDataPacketConverter class. Because to deserialize an AttributeValue
        /// the Definition of the Attribute must be known. And reading the Definition is an IO bound
        /// Async call. But ReadJson() is not Async and can't be made async because it is overridden.
        /// To get around this limitation, the Task is assigned to AttributeValueTask (instead
        /// of awaiting and assigning to AttributeValue. The awaiting of AttributeValueTask
        /// is then handled in an async call sepeartly.
        /// </summary>
        /// <returns></returns>
        protected internal async Task PostResponseProcessingAsync() {
            //Value (and subsequently Value.ResultCOF) could be null if the asked for result cof id doesn't exist or can not be converted.
            if (Value != null) {
                foreach (var squadding in Value.SquaddingList.Items) {
                    await PostResponseProcessingForResultEventsAsync( squadding );
                }
            }
        }

        private async Task PostResponseProcessingForResultEventsAsync( Squadding squadding ) {
            //If the Attr Definition can not be found, remove it.
            List<AttributeValueDataPacketMatch> avToRemove = new List<AttributeValueDataPacketMatch>();
            foreach (var attributeValue in squadding.Participant.AttributeValues) {
                try {
                    if (attributeValue.AttributeValueTask != null) {
                        await attributeValue.FinishInitializationAsync();
                    }
                } catch (DefinitionNotFoundException nfe) {
                    avToRemove.Add( attributeValue );
                }
            }

            foreach (var attributeValue in avToRemove) {
                squadding.Participant.AttributeValues.Remove( attributeValue );
            }

            //Question, will we ever have to handle Teams ? 

        }

        /// <inheritdoc />
        protected internal override DateTime GetCacheValueExpiryTime() {

            try {
                //if today is before end then timeout is 1 min, else, make is 5 min
                if (DateTime.Today <= SquaddingList.EndDate) {
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
