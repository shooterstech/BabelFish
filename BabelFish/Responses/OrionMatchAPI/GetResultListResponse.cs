using System;
using System.Collections.Generic;
using System.Text;
using Scopos.BabelFish.DataModel.AttributeValue;
using Scopos.BabelFish.DataModel.OrionMatch;
using Scopos.BabelFish.Requests.OrionMatchAPI;

namespace Scopos.BabelFish.Responses.OrionMatchAPI {

    public abstract class GetResultListResponse : Response<ResultListWrapper> {
		/*
         * TODO: Figure out the best way to make GetResultListResponse implement the interface ITokenResponse<>
         * ITokenResponse<GetResultListAbstractRequest>
         */

		public GetResultListResponse() : base() {
        }

        /// <summary>
        /// Facade function that returns the same as this.Value
        /// </summary>
        public ResultList ResultList {
            get { return Value.ResultList; }
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
				foreach (var resultEvent in Value.ResultList.Items) {
					//If the Attr Definition can not be found, remove it.
					List<AttributeValueDataPacketMatch> avToRemove = new List<AttributeValueDataPacketMatch>();
					foreach (var attributeValue in resultEvent.Participant.AttributeValues) {
						try {
							if (attributeValue.AttributeValueTask != null) {
								await attributeValue.FinishInitializationAsync();
							}
						} catch (AttributeNotFoundException nfe) {
							avToRemove.Add( attributeValue );
						}
					}

					foreach (var attributeValue in avToRemove) {
						resultEvent.Participant.AttributeValues.Remove( attributeValue );
					}
				}
			}
		}
	}
}
