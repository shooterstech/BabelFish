namespace Scopos.BabelFish.DataModel.OrionMatch {

    /// <summary>
    /// An AttributeFilter describes how a ResultList should be filtered. That is to say, of the
    /// participants who shot a Course of Fire, which of those should be included in a ResultList.
    /// <para>For example, a Result List could show all the Sporter Air Rifle marksmen (excluding
    /// the Precision Air Rifle marksmen).</para>
    /// </summary>
    [Serializable]
    [G_STJ_SER.JsonConverter( typeof( G_BF_STJ_CONV.AttributeFilterConverter ) )]
    [G_NS.JsonConverter( typeof( G_BF_NS_CONV.AttributeFilterConverter ) )]
    public abstract class AttributeFilter {

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
        /// Concret class identifier. 
        /// </summary>
        /// <remarks>Concrete class implementations should set this value in their constructors.</remarks>
        [G_NS.JsonProperty( Order = 1, DefaultValueHandling = G_NS.DefaultValueHandling.Include )]
        public AttributeFilterOperation Operation { get; protected set; } = AttributeFilterOperation.ATTRIBUTE_VALUE;
    }
}
