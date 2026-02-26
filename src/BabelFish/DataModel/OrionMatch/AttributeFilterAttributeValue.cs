using Scopos.BabelFish.APIClients;
using Scopos.BabelFish.DataModel.AttributeValue;
using Scopos.BabelFish.DataModel.Definitions;
using Scopos.BabelFish.DataModel.OrionMatch;
using Attribute = Scopos.BabelFish.DataModel.Definitions.Attribute;

namespace BabelFish.DataModel.OrionMatch {
    /// <summary>
    /// An AttributeFilterAttibuteValue is a concrete class implementation for AttributeFilter. It specifies a
    /// condition where the participant must have (or must not have) specific <seealso cref="AttributeValue"/> field values.
    /// </summary>
    public class AttributeFilterAttributeValue : AttributeFilter, IGetAttributeDefinition, IEquatable<AttributeFilterAttributeValue>, IEqualityComparer<AttributeFilterAttributeValue> {

        /*
        {
            "Operation" : "EQUATION", //Consistent with ShowWhen
            "Boolean" : "AND", //Consistent with ShowWhen
            "Arguments" : [ //Consistent with ShowWhen
                {
                    "Operation" : "ATTRIBUTE_VALUE",
                    "AttributeDef": "v1.0:ntparc:Three-Position Air Rifle Type",
                    "Values": {
                        "Three-Position Air Rifle Type": "Sporter"
                    },
                    "FilterRule" : "EQUAL" //HAVE_ONE, HAVE_ALL, NOT_EQUAL, NOT_HAVE_ANY
                }
            ]
        }
        */

        /// <summary>
        /// Constructor
        /// </summary>
        public AttributeFilterAttributeValue() {
            this.Operation = AttributeFilterOperation.ATTRIBUTE_VALUE;
        }

        /// <summary>
        /// The SetName of the <seealso cref="Attribute">ATTRIBUTE</seealso> to test values against.
        /// </summary>
        public SetName AttributeDef { get; set; } = SetName.Parse( "v1.0:orion:Default" );

        /// <summary>
        /// The filter rule to apply.
        /// </summary>
        public AttributeFilterRule FilterRule { get; set; } = AttributeFilterRule.HAVE_ALL;

        /// <summary>
        /// A list of field name and filed value pairs that we will test against.
        /// <para>Item1 is the Attribute's FieldName, Item2 is the Attribute's FieldValue</para>
        /// </summary>
        public ConstantFieldValueList Values { get; set; } = new ConstantFieldValueList();

        /// <inheritdoc />
        /// <remarks>
        /// Returns the ATTRIBUTE definition from .DefaultAttributeDef
        /// <para>It is a best practice to check for null or empty string on .DefaultAttributeDef before calling this method.</para>
        /// </remarks>
        /// <exception cref="XApiKeyNotSetException" />
        /// <exception cref="DefinitionNotFoundException" />
        /// <exception cref="ScoposAPIException" />
        public async Task<Attribute> GetAttributeDefinitionAsync() {

            return await DefinitionCache.GetAttributeDefinitionAsync( this.AttributeDef );
        }

        /// <summary>
        /// Returns a hash code unique ideifying this AttributeFiler. Incorporating the Operation, Boolean, and </summary>
        /// <inheritdoc />
        public override int GetHashCode() {
            StringBuilder sb = new StringBuilder();
            sb.Append( this.Operation.ToString() );
            sb.Append( this.AttributeDef.ToString() );
            sb.Append( this.FilterRule.ToString() );
            //Since the order of the valued does not matter, we will use a bitwas xor operator.
            int temp = 0;
            foreach (var val in this.Values) {
                temp ^= val.FieldName.GetHashCode();
                temp ^= val.Value.GetHashCode();
            }
            sb.Append( temp );
            return sb.ToString().GetHashCode();
        }

        /// <inheritdoc />
        public override bool Equals( object obj ) {
            if (obj is AttributeFilterAttributeValue afe) {
                return Equals( afe );
            }
            return false;
        }

        /// <inheritdoc />
        public bool Equals( AttributeFilterAttributeValue other ) {
            return this.GetHashCode() == other.GetHashCode();
        }

        /// <inheritdoc />
        public bool Equals( AttributeFilterAttributeValue x, AttributeFilterAttributeValue y ) => x.Equals( y );

        /// <inheritdoc />
        public int GetHashCode( AttributeFilterAttributeValue obj ) => obj.GetHashCode();
    }
}
