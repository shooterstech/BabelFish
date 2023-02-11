using Scopos.BabelFish.DataModel.AttributeValue;
using Scopos.BabelFish.DataModel.Definitions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Scopos.BabelFish.Responses.AttributeValueAPI {
    public class AttributeValueDataPacket {

        public AttributeValueDataPacket() { }

        public System.Net.HttpStatusCode StatusCode { get; set; }

        public SetName SetName { get; set; }

        public AttributeValue AttributeValue { get; set; }

        public VisibilityOption Visibility { get; set; }

        public string Message { get; set; }
    }
}
