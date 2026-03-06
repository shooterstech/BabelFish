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

    /// <summary>
    /// An AttributeConfiguration is not conceptually an AttributeValue, despite the face from a programming point of view it inhertis from AttributeValueDataPacket.
    /// Instead it is a class that describes a Attribute that is used in a match. It is used to specify the AttributeDef and possible values for an Attribute that is used in a match.
    /// It is used in the MatchConfiguration to specify the Attributes that are used in a match, and the possible values for those Attributes.
    /// </summary>
    /// <remarks>AttributeConfiguration may not be constructed out of Attributes that allow MultipleValues or have fields that allow MultipleValues.</remarks>
    [Serializable]
    public class AttributeConfiguration : AttributeValueDataPacketMatch {

        /// <summary>
        /// Concrete class identifier.
        /// </summary>
        public const int CONCRETE_CLASS_ID = 3;

        /// <summary>
        /// Public constructor.
        /// <para>Unless you are a JSON deserializer, it is generally best to instantiate a new AttributeValueDataPacketMatch using the althernative
        /// constructor where you pass in an AttributeValue.</para>
        /// </summary>
        public AttributeConfiguration() {
            this.ConcreteClassId = CONCRETE_CLASS_ID;
        }

        /// <summary>
        /// Public constructor initializaing the instance with the passed in AttributeValue.
        /// </summary>
        /// <param name="attrValue">The AttributeValue to construct this AttributeConfiguration out of.
        /// The AttributeValue's AttributeDef specifies the ATTRIBUTE for this AttributeConfiguration.</param>
        /// <param name="constant">if true, then the values listed in the parameter attrValue initialize the values of this AttributeConfiguration. If false, then no Attribute Values are included.</param>
        /// <exception cref="ArgumentException">Thrown if the AttributeValue passed in has an Attribute that allows MultipleValues or has fields that allow MultipleValues, as AttributeConfigurations cannot be constructed out of these types of Attributes.</exception>"
        public AttributeConfiguration( AttributeValue.AttributeValue attrValue, bool constant = false ) {
            this.ConcreteClassId = CONCRETE_CLASS_ID;

            var attribute = attrValue.Attribute;
            if (attribute.MultipleValues || attribute.Fields.Any( f => f.MultipleValues )) {
                throw new ArgumentException( "AttributeValue passed in to AttributeConfiguration constructor cannot have an Attribute that allows MultipleValues or has fields that allow MultipleValues." );
            }

            this.AttributeDef = attrValue.SetName;
            this.Constant = constant;

            if (constant) {
                //Should I clone or copy the AttributeValue here to avoid potential issues with mutability?
                this.AttributeValue = attrValue;
            }
        }

        /// <summary>
        /// A Constant (value true) AttributeConfiguration means that the AttributeValue specified in this AttributeConfiguration is applied to all participants in the match.
        /// A non-constant (value false) AttributeConfiguration means that the AttributeValue specified in this AttributeConfiguration is not a constant,
        /// and each participant in the match may have a different value.
        /// </summary>
        /// <remarks>If Constant is true then <see cref="AttributeValue"/> must contain the values (as they are the constant AttributeValues that everyone will have.
        /// <para>If Constnat is false, the <see cref="AttributeValue"/> should ben an empty list.</para></remarks>
        [G_NS.JsonProperty( DefaultValueHandling = G_NS.DefaultValueHandling.Include )]
        public bool Constant { get; set; } = false;
    }
}
