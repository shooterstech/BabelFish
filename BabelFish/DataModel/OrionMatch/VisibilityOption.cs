using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ShootersTech.BabelFish.DataModel.OrionMatch {

    [JsonConverter( typeof( StringEnumConverter ) )]
    public enum VisibilityOption { PRIVATE, INTERNAL, PUBLIC, PROTECTED };
}
