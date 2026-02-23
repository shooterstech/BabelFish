using Scopos.BabelFish.DataModel.Definitions;

namespace Scopos.BabelFish.DataModel.OrionMatch {

    /// <summary>
    /// An AttributeFilterEquation is a concrete class implementation for AttributeFilter. An Equation
    /// specifies the boolean logic of how multiple AttributeFilter instances are combined.
    ///
    /// </summary>
    public class AttributeFilterEquation : AttributeFilter {

        /// <summary>
        /// Constructor
        /// </summary>
        public AttributeFilterEquation() {
            this.Operation = AttributeFilterOperation.EQUATION;
        }

        /*
        {
            "Operation" : "EQUATION", //Consistent with ShowWhen
            "Boolean" : "AND", //Consistent with ShowWhen
            "Arguments" : [ //Consistent with ShowWhen
                {
                    "Operation" : "ATTRIBUTE_VALUE",
                    "AttributeValue" : {
                        "AttributeDef": "v1.0:ntparc:Three-Position Air Rifle Type",
                        "Visibility": "PUBLIC",
                        "AttributeValue": {
                            "Three-Position Air Rifle Type": "Sporter"
                        },
                        "ConcreteClassId": 2
                    }
                }
            ]
        }
        */

        /// <summary>
        /// The boolean operation to apply.
        /// </summary>
        /// <remarks>Using the existing enum ShowWhenBoolean, which has all of the boolean operations defined.
        /// Choosing not to rename it (for now) to be more generic, as that would be a breaking change.</remarks>
        public ShowWhenBoolean Boolean { get; set; } = ShowWhenBoolean.AND;

        /// <summary>
        /// The list of AttributeFilter to apply the boolean logic too.
        /// </summary>
        public List<AttributeFilter> Arguments { get; set; } = new List<AttributeFilter>();
    }
}
