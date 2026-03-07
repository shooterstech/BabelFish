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
            this.AttributeDef = attrValue.SetName;
        }

        /// <summary>
        /// Some AttributeValues are specific to a specific course of fire within a Match. A value of 0 means the AttributeValue is not specific to a course of fire, and applies to the whole match. Any other value should be the CourseOfFireId of the course of fire that this AttributeValue is specific to.
        /// <para></para>
        /// </summary>
        public int CourseOfFireId { get; set; } = 0;
    }
}
