using Scopos.BabelFish.DataModel.AttributeValue;

namespace Scopos.BabelFish.DataModel.OrionMatch {
    /// <summary>
    /// An AttributeValue that helps describe a Partipant in a match.
    /// </summary>
    [Serializable]
    public class AttributeValueDataPacketMatch : AttributeValueDataPacket {

        /// <summary>
        /// Concrete class identifier.
        /// </summary>
        public const int CONCRETE_CLASS_ID = 2;

        /// <summary>
        /// Public constructor.
        /// <para>Unless you are a JSON deserializer, it is generally best to instantiate a new AttributeValueDataPacketMatch using the althernative
        /// constructor where you pass in an AttributeValue.</para>
        /// </summary>
        public AttributeValueDataPacketMatch() {
            this.ConcreteClassId = CONCRETE_CLASS_ID;
        }

        /// <summary>
        /// Public constructor initializaing the instance with the passed in AttributeValue.
        /// </summary>
        public AttributeValueDataPacketMatch( AttributeValue.AttributeValue attrValue ) {
            this.ConcreteClassId = CONCRETE_CLASS_ID;

            this.AttributeValue = attrValue;
            this.AttributeDef = attrValue.SetName.ToString();
        }

        /// <summary>
        /// Some AttributeValues are specific to a Participants ReentryTag. An emptry string value ("") or a value of "No Reentry" means the same thing.
        /// </summary>
        public string ReentryTag { get; set; }
    }
}
