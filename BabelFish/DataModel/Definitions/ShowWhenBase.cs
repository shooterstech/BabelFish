using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Scopos.BabelFish.Converters;
using Scopos.BabelFish.DataModel.Definitions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Scopos.BabelFish.DataModel.Definitions {


    [JsonConverter( typeof( ShowWhenBaseConverter ) )]
    [Serializable]
    public abstract class ShowWhenBase: IReconfigurableRulebookObject {

        public abstract ShowWhenBase Copy();

        [JsonConverter( typeof( StringEnumConverter ) )]
        [JsonProperty( DefaultValueHandling = DefaultValueHandling.Include )]
        public ShowWhenOperation Operation { get; protected set; } = ShowWhenOperation.VARIABLE;

        /// <inheritdoc/>
        [JsonProperty( Order = 99, DefaultValueHandling = DefaultValueHandling.Ignore )]
        [DefaultValue( "" )]
        public string Comment { get; set; } = string.Empty;
    }
}
