using Scopos.BabelFish.APIClients;
using Scopos.BabelFish.DataModel.Common;
using Scopos.BabelFish.DataModel.Definitions;

namespace Scopos.BabelFish.DataModel.AttributeValue {

    /// <summary>
    /// Abstract base class for serializing an AttributeValue. Contains both a referrence to the Attribute
    /// <seealso cref="AttributeDef"/> that defines the data, as well as the value <seealso cref="AttributeValue"/>
    /// </summary>
    [G_NS.JsonConverter( typeof( G_BF_NS_CONV.AttributeValueDataPacketConverter ) )]
    public abstract class AttributeValueDataPacket : IDeserializableAbstractClass, IGetAttributeDefinition {

        public const int CONCRETE_CLASS_ID = 1;

        /// <summary>
        /// Default constructor.
        /// </summary>
        public AttributeValueDataPacket() {
            this.ConcreteClassId = CONCRETE_CLASS_ID;
        }

        /// <summary>
        /// the SetName, formatted as a string, of the Attribute definition.
        /// </summary>
        public string AttributeDef { get; set; }

        /// <summary>
        /// Property that contains the value.
        /// </summary>
        public AttributeValue AttributeValue { get; set; }

        /// <summary>
        /// Property that holds the task to finish deserializing an AttributeValue.
        /// See also <seealso cref="FinishInitializationAsync"/>
        /// </summary>
        protected internal Task<AttributeValue> AttributeValueTask { get; set; }

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
        protected internal async Task FinishInitializationAsync() {
            AttributeValue = await AttributeValueTask;
        }


        /// <inheritdoc />
        /// <remarks>
        /// Returns the ATTRIBUTE definition from .DefaultAttributeDef
        /// <para>It is a best practice to check for null or empty string on .DefaultAttributeDef before calling this method.</para>
        /// </remarks>
        /// <exception cref="XApiKeyNotSetException" />
        /// <exception cref="DefinitionNotFoundException" />
        /// <exception cref="ScoposAPIException" />
        public async Task<Definitions.Attribute> GetAttributeDefinitionAsync() {

            if (string.IsNullOrEmpty( AttributeDef ))
                throw new ArgumentNullException( $"The value for .DefaultSttributeDef is empty. Which is allowed." );

            var setName = Definitions.SetName.Parse( AttributeDef );
            return await DefinitionCache.GetAttributeDefinitionAsync( setName );
        }

        /// <summary>
        /// Property storing how broadly this AttributeValue may be shared.
        /// </summary>
        public VisibilityOption Visibility { get; set; }

        /// <summary>
        /// Implementation of the IDeserializableAbstractClass interface.
        /// To have added control over the Deserialization of abstract classes, in to
        /// Concrete classes, the JSON should include a ConcreteClassId that specifies
        /// the Concrete class.
        /// </summary>
        public int ConcreteClassId { get; set; }
    }
}
