using System.ComponentModel;

namespace Scopos.BabelFish.DataModel.Definitions {

    [Serializable]
    [G_NS.JsonConverter(typeof(G_BF_NS_CONV.ShowWhenBaseConverter))]
    public class ClassSet : IReconfigurableRulebookObject {
        public ClassSet() { }

        public ClassSet(string Name, ShowWhenBase showWhen)
        {
            this.Name = Name;
            this.ShowWhen = showWhen;
        }

        /// <summary>
        /// The name of the css class to apply, when ShowWhen evalutes to true.
        /// </summary>
        [G_NS.JsonProperty( Order = 1 )]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Conditional opeartion that determiens if this Name is included.
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
