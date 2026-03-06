namespace Scopos.BabelFish.DataModel.OrionMatch {
    /// <summary>
    /// An AttributeFilterAttibuteValue is a concrete class implementation for AttributeFilter. It specifies a
    /// condition where the participant must have (or must not have) specific <seealso cref="AttributeValue"/> field values.
    /// </summary>
    public class AttributeFilterAttributeValue : AttributeFilter, IEquatable<AttributeFilterAttributeValue>, IEqualityComparer<AttributeFilterAttributeValue> {

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
        /// The filter rule to apply.
        /// </summary>
        public AttributeFilterRule FilterRule { get; set; } = AttributeFilterRule.HAVE_ALL;

        /// <summary>
        /// A list of Attribute Values to test against the participant's Attributes. The interpretation of this list depends on the FilterRule.
        /// </summary>
        public List<AttributeValueDataPacketMatch> Values { get; set; } = new List<AttributeValueDataPacketMatch>();

        /// <summary>
        /// Returns a hash code unique ideifying this AttributeFiler. Incorporating the Operation, Boolean, and </summary>
        /// <inheritdoc />
        public override int GetHashCode() {
            StringBuilder sb = new StringBuilder();
            sb.Append( this.Operation.ToString() );
            sb.Append( this.FilterRule.ToString() );
            //Since the order of the valued does not matter, we will use a bitwas xor operator.
            int temp = 0;
            foreach (var val in this.Values) {
                temp ^= val.GetHashCode();
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
