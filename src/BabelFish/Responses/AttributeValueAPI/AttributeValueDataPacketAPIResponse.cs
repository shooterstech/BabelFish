using System;
using System.Collections.Generic;
using System.Text;
using Scopos.BabelFish.DataModel.AttributeValue;
using Scopos.BabelFish.DataModel.Definitions;

namespace Scopos.BabelFish.Responses.AttributeValueAPI {

    public class AttributeValueDataPacketAPIResponse : AttributeValueDataPacket {

        public const int CONCRETE_CLASS_ID = 1;
        public const System.Net.HttpStatusCode DEFAULT_STATUS_CODE = System.Net.HttpStatusCode.NotImplemented;

        public AttributeValueDataPacketAPIResponse() {
            this.ConcreteClassId = CONCRETE_CLASS_ID;
        }

        public System.Net.HttpStatusCode StatusCode { get; set; } = DEFAULT_STATUS_CODE;

        public string Message { get; set; } = string.Empty;
    }
}
