using System.ComponentModel;

namespace Scopos.BabelFish.DataModel.Definitions {

    /// <summary>
    /// ShowWhen operations describe logic for when a <seealso cref="ResultListFormat">RESULT LIST FORMAT</seealso>
    /// <seealso cref="ResultListDisplayColumn"/>, <seealso cref="ClassSet"/>, or SpanningText is included and displayed.
    /// <para>ShowWhenBase is the abstract base class for describing this logic.</para>
    /// </summary>
    [Serializable]
    [G_NS.JsonConverter( typeof( G_BF_NS_CONV.ShowWhenBaseConverter ) )]
    public abstract class ShowWhenBase : IReconfigurableRulebookObject {

        /// <summary>
        /// Concret class identifier. 
        /// </summary>
        [G_NS.JsonProperty( Order = 1, DefaultValueHandling = G_NS.DefaultValueHandling.Include )]
        public ShowWhenOperation Operation { get; protected set; } = ShowWhenOperation.VARIABLE;

        /// <inheritdoc/>
        [DefaultValue( "" )]
        [G_NS.JsonProperty( Order = 99 )]
        public string Comment { get; set; } = string.Empty;
    }
}
