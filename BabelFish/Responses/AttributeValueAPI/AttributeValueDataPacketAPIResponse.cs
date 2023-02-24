using System;
using System.Collections.Generic;
using System.Text;
using Scopos.BabelFish.DataModel.AttributeValue;
using Scopos.BabelFish.DataModel.Definitions;

namespace Scopos.BabelFish.Responses.AttributeValueAPI {

    public class AttributeValueDataPacketAPIResponse : AttributeValueDataPacket {

        public AttributeValueDataPacketAPIResponse() { }

        public System.Net.HttpStatusCode StatusCode { get; set; }

        public string Message { get; set; }
    }
}
