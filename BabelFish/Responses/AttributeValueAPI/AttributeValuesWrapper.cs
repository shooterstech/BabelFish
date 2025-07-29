using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;
using Scopos.BabelFish.DataModel;

namespace Scopos.BabelFish.Responses.AttributeValueAPI {
    public class AttributeValuesWrapper : BaseClass {

        public AttributeValuesWrapper() {
            AttributeValues = new Dictionary<string, AttributeValueDataPacketAPIResponse>();
        }

        /// <summary>
        /// The Key is the set name of the attribute value.
        /// The Value is the server response (which includes the AttributeValue's value).
        /// </summary>
        public Dictionary<string, AttributeValueDataPacketAPIResponse> AttributeValues { get; set; }
    }
}
