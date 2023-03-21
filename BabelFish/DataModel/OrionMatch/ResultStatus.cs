using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Scopos.BabelFish.DataModel.OrionMatch {

    /// <summary>
    /// FUTURE", "INTERMEDIATE", "UNOFFICIAL", "OFFICIAL
    /// </summary>
    [JsonConverter( typeof( StringEnumConverter ) )]
    public enum ResultStatus {
        FUTURE, INTERMEDIATE, UNOFFICIAL, OFFICIAL
    }
}
