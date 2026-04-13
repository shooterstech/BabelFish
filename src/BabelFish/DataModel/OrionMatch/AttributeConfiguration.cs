using Scopos.BabelFish.APIClients;
using Scopos.BabelFish.DataModel.Definitions;

namespace Scopos.BabelFish.DataModel.OrionMatch {

    /// <summary>
    /// An AttributeConfiguration is not conceptually an AttributeValue, despite the fact from a programming point of view it inhertis from AttributeValueDataPacket.
    /// Instead it is a class that describes a Attribute that is used in a match. It is used to specify the AttributeDef and possible values for an Attribute that is used in a match.
    /// It is used in the MatchConfiguration to specify the Attributes that are used in a match, and the possible values for those Attributes.
    /// <para>Unless you are a JSON deserializer, it is generally best to instantiate a new AttributeConfiguration using
    /// one of the FactoryAsync() methods.</para>
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
        /// <para>Unless you are a JSON deserializer, it is generally best to instantiate a new AttributeConfiguration using
        /// one of the FactoryAsync() methods.</para>
        /// </summary>
        public AttributeConfiguration() {
            this.ConcreteClassId = CONCRETE_CLASS_ID;
            this.Type = AttributeValueType.CONFIGURATION;
        }

        /// <summary>
        /// Public constructor initializaing the instance with the passed in AttributeValue, which must be a <see cref="Definitions.Attribute.ReallySimpleAttribute"/>.
        /// </summary>
        /// <param name="attrValue">The AttributeValue to construct this AttributeConfiguration out of.
        /// The AttributeValue's AttributeDef specifies the ATTRIBUTE for this AttributeConfiguration.</param>
        /// <param name="constant">if true, then the values listed in the parameter attrValue initialize the values of this AttributeConfiguration. If false, then no Attribute Values are included.</param>
        /// <exception cref="ArgumentException">Thrown if the AttributeValue passed is not a <see cref="Definitions.Attribute.ReallySimpleAttribute"/>.</exception>"
        public static async Task<AttributeConfiguration> CreateAsync( AttributeValue.AttributeValue attrValue, bool constant = false ) {

            AttributeConfiguration configuration = new AttributeConfiguration();

            var attribute = attrValue.Attribute;

            if (!attribute.ReallySimpleAttribute) {
                throw new ArgumentException( "To construct a new AttributeConfiguration, the ATTRIBUTE must be a SimpleStringAttributes." );
            }

            configuration.AttributeDef = attrValue.SetName;
            configuration.Constant = constant;
            configuration.AttributeValue = await DataModel.AttributeValue.AttributeValue.CreateAsync( attrValue.SetName );

            if (constant) {
                //Should I clone or copy the AttributeValue here to avoid potential issues with mutability?
                configuration.AttributeValue = attrValue;
            }

            configuration.IsForIndividuals = attribute.Designation.Contains( AttributeDesignation.ATHLETE );
            configuration.IsForTeams = attribute.Designation.Contains( AttributeDesignation.TEAM );

            return configuration;
        }

        public static async Task<AttributeConfiguration> CreateAsync( SetName setName ) {
            if (setName.IsDefault)
                throw new DefinitionNotFoundException( "Can not create a instance of AttributeConfiguration using the default Attribute." );
            var attribute = await DefinitionCache.GetAttributeDefinitionAsync( setName );

            if (!attribute.SimpleAttribute) {
                throw new ArgumentException( "To construct a new AttributeConfiguration, the ATTRIBUTE must be a SimpleAttributes." );
            }

            AttributeConfiguration configuration = new AttributeConfiguration();
            configuration.AttributeDef = setName;
            configuration.Constant = false;
            configuration.AttributeValue = await DataModel.AttributeValue.AttributeValue.CreateAsync( setName );

            configuration.IsForIndividuals = attribute.Designation.Contains( AttributeDesignation.ATHLETE );
            configuration.IsForTeams = attribute.Designation.Contains( AttributeDesignation.TEAM );

            return configuration;
        }

        public override async Task FinishInitializationAsync() {
            await base.FinishInitializationAsync();
            var attribute = await DefinitionCache.GetAttributeDefinitionAsync( this.AttributeDef );

            this.IsForIndividuals = attribute.Designation.Contains( AttributeDesignation.ATHLETE );
            this.IsForTeams = attribute.Designation.Contains( AttributeDesignation.TEAM );
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

        /// <summary>
        /// Helper property to indicate whether this AttributeConfiguration is intended to be used to describe Individuals within a Match
        /// </summary>
        /// <remarks>Values are set during the CreateAsync methods.</remarks>
        [G_NS.JsonIgnore]
        public bool IsForIndividuals { get; private set; }

        /// <summary>
        /// Helper property to indicate whether this AttributeConfiguration is intended to be used to describe Teams within a Match
        /// </summary>
        /// <remarks>Values are set during the CreateAsync methods.</remarks>
        [G_NS.JsonIgnore]
        public bool IsForTeams { get; private set; }

        /// <summary>
        /// Creates a copy of this instance as a <see cref="AttributeValueDataPacketMatch"/>. 
        /// </summary>
        /// <returns></returns>
        public AttributeValueDataPacketMatch GetAsAttributeValueDataPacketMatch() {
            AttributeValueDataPacketMatch dataPacket = new AttributeValueDataPacketMatch();
            dataPacket.AttributeDef = this.AttributeDef;
            dataPacket.AttributeValue = this.AttributeValue.Copy();
            dataPacket.Visibility = this.Visibility;
            return dataPacket;
        }

        /// <summary>
        /// Returns a string that provides a concise description of the current AttributeConfiguration instance,
        /// including its attribute definition and constant value.
        /// </summary>
        /// <returns>A string formatted as 'AttributeConfiguration: AttributeDef={AttributeDef}, Constant={Constant}',
        /// representing the attribute definition and constant value of this instance.</returns>
        public override string ToString() {
            return $"AttributeConfiguration: AttributeDef={AttributeDef}, Constant={Constant}";
        }
    }
}
