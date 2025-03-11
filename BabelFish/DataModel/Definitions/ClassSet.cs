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

        [G_NS.JsonProperty( Order = 1 )]
        public string Name { get; set; } = string.Empty;

        [G_NS.JsonProperty( Order = 2 )]
        public ShowWhenBase ShowWhen { get; set; } = ShowWhenVariable.ALWAYS_SHOW.Clone();

        /// <inheritdoc/>
        [DefaultValue("")]
        [G_NS.JsonProperty( Order = 99  )]
        public string Comment { get; set; } = string.Empty;

        public override string ToString()
        {
            return this.Name;
        }
    }
}
