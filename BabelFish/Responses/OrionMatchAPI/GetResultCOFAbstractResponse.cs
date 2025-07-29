using System;
using System.Collections.Generic;
using System.Text;
using Scopos.BabelFish.APIClients;
using Scopos.BabelFish.DataModel.AttributeValue;
using Scopos.BabelFish.DataModel.OrionMatch;
using Scopos.BabelFish.Requests.OrionMatchAPI;

namespace Scopos.BabelFish.Responses.OrionMatchAPI {
    public abstract class GetResultCOFAbstractResponse : Response<CourseOfFireWrapper> {


        public GetResultCOFAbstractResponse() : base() {
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
					} catch (DefinitionNotFoundException nfe) {
						avToRemove.Add( attributeValue );
					}
				}

				foreach (var attributeValue in avToRemove) {
					ResultCOF.Participant.AttributeValues.Remove( attributeValue );
				}
			}
        }

        /// <inheritdoc />
        protected internal override DateTime GetCacheValueExpiryTime()
        {

            try
            {
                //NOTE: The cache refresh time on the the REST API is 10 seconds. By making the cache expiry tme slightly above that
                //We can avoide getting a cache hit and potentially old data.

                switch( ResultCOF.Status) {
                    case ResultStatus.FUTURE:
                    case ResultStatus.UNOFFICIAL:

                        //32s is 30s (3 * time of rest api cache) + 2 seconds
                        return DateTime.UtcNow.AddSeconds( 32 );

                    case ResultStatus.INTERMEDIATE:
                        //12s is 10s (time of rest api cache) + 2 seconds
                        return DateTime.UtcNow.AddSeconds( 12 );

                    case ResultStatus.OFFICIAL:
                    default:
                        //Official results shouldn't ever be changing ... in theory at least
                        return DateTime.UtcNow.AddMinutes( 5 );
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
