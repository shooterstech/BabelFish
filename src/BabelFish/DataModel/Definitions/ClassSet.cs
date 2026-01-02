using System.ComponentModel;

namespace Scopos.BabelFish.DataModel.Definitions {

    /// <summary>
    /// A ClassSet specifies the css class name to include and the (ShowWhen) condition to when to include it. 
    /// <para>Used on a ResultListDisplayColumn and ResultListDisplayPartition.</para>
    /// </summary>
    [G_NS.JsonConverter(typeof(G_BF_NS_CONV.ShowWhenBaseConverter))]
    public class ClassSet : IReconfigurableRulebookObject {
        public ClassSet() { }

        public ClassSet(string Name, ShowWhenBase showWhen)
        {
            this.Name = Name;
            this.ShowWhen = showWhen;
        }

        /// <summary>
        /// The name of the css class to apply, when ShowWhen evaluates to true.
        /// </summary>
        [G_NS.JsonProperty( Order = 1 )]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Conditional operation that determines if this css .Name is included.
        /// <para>Defaults to always including the css .Name.</para>
        /// </summary>
        [G_NS.JsonProperty( Order = 2 )]
        public ShowWhenBase ShowWhen { get; set; } = ShowWhenVariable.ALWAYS_SHOW.Clone();

        public bool ShouldSerializeShowWhen() {

            //Dont serialize ShowWhen if it says to always show
            if ( ShowWhen is ShowWhenVariable showWhen && showWhen.Condition == ShowWhenCondition.TRUE ) {
                return false;
            }

            return true;
        }

        /// <inheritdoc/>
        [DefaultValue("")]
        [G_NS.JsonProperty( Order = 99  )]
        public string Comment { get; set; } = string.Empty;

        /// <inheritdoc/>
        public override string ToString()
        {
            return this.Name;
        }
    }
}
