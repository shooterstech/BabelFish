using Scopos.BabelFish.DataModel.AttributeValue;

namespace Scopos.BabelFish.Responses.AttributeValueAPI {

    /// <summary>
    /// An AttributeValue that helps describe a user or club.
    /// </summary>
    public class AttributeValueDataPacketAPIResponse : AttributeValueDataPacket {

        /// <summary>
        /// Concrete class identifier for AttributeValueDataPacketAPIResponse
        /// </summary>
        public const int CONCRETE_CLASS_ID = 1;

        /// <summary>
        /// Public constructor.
        /// <para>Unless you are a JSON deserializer, it is generally best to instantiate a new AttributeValueDataPacketAPIResponse using the althernative
        /// constructor where you pass in an AttributeValue.</para>
        /// </summary>
        public AttributeValueDataPacketAPIResponse() {
            this.ConcreteClassId = CONCRETE_CLASS_ID;
        }

        /// <summary>
        /// Public constructor initializaing the instance with the passed in AttributeValue.
        /// </summary>
        public AttributeValueDataPacketAPIResponse( Scopos.BabelFish.DataModel.AttributeValue.AttributeValue attrValue ) {
            this.ConcreteClassId = CONCRETE_CLASS_ID;

            this.AttributeValue = attrValue;
            this.AttributeDef = attrValue.SetName;
        }

        /// <summary>
        /// Specific status code for this AttributeValue request (and not the status code of the overall API call).
        /// </summary>
        public System.Net.HttpStatusCode StatusCode { get; set; } = System.Net.HttpStatusCode.NotImplemented;

        /// <summary>
        /// What, if any, message returned from the REST API fot his specfic AttributeValue request (and nto the overall message for the overall API CAll).
        /// </summary>
        public string Message { get; set; } = string.Empty;
    }
}
