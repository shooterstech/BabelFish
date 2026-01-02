using Scopos.BabelFish.Converters;
using Scopos.BabelFish.DataModel.Definitions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Scopos.BabelFish.DataModel.Definitions {


    [Serializable]
    [G_NS.JsonConverter( typeof( G_BF_NS_CONV.ShowWhenBaseConverter ) )]
    public abstract class ShowWhenBase: IReconfigurableRulebookObject {

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
