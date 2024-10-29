using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Scopos.BabelFish.DataModel.Definitions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Scopos.BabelFish.DataModel.Definitions {
    public abstract class ShowWhenBase: IReconfigurableRulebookObject {

        public abstract ShowWhenBase Copy();

        [JsonConverter( typeof( StringEnumConverter ) )]
        public ShowWhenOperation Operation { get; protected set; } = ShowWhenOperation.VARIABLE;

        /// <summary>
        /// Indicates if the value of the Show When operation should be inverted.
        /// <para>Default value is false, meaning not to invert the operation.</para>
        /// </summary>
        public bool Not { get; set; } = false;

        /// <inheritdoc/>
        [JsonProperty( Order = 99, DefaultValueHandling = DefaultValueHandling.Ignore )]
        [DefaultValue( "" )]
        public string Comment { get; set; } = string.Empty;
    }
}
