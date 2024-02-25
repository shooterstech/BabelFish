using System;
using System.Collections.Generic;
using System.Text;
using Scopos.BabelFish.DataModel.AttributeValue;
using Scopos.BabelFish.DataModel.OrionMatch;
using Scopos.BabelFish.Requests.OrionMatchAPI;

namespace Scopos.BabelFish.Responses.OrionMatchAPI {
    public abstract class GetResultCOFDetailResponse : Response<CourseOfFireWrapper> {


        public GetResultCOFDetailResponse() : base() {
        }

        /// <summary>
        /// Facade function that returns the same as this.Value
        /// </summary>
        public ResultCOF ResultCOF {
            get { return Value.ResultCOF; }
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
				//If the Attr Definition can not be found, remove it.
				List<AttributeValueDataPacketMatch> avToRemove = new List<AttributeValueDataPacketMatch>();
				foreach (var attributeValue in ResultCOF.Participant.AttributeValues) {
					try {
						if (attributeValue.AttributeValueTask != null) {
							await attributeValue.FinishInitializationAsync();
						}
					} catch (AttributeNotFoundException nfe) {
						avToRemove.Add( attributeValue );
					}
				}

				foreach (var attributeValue in avToRemove) {
					ResultCOF.Participant.AttributeValues.Remove( attributeValue );
				}
			}
		}
	}
}
