using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Scopos.BabelFish.DataModel.AttributeValue {

    [JsonConverter( typeof( StringEnumConverter ) )]
    public enum VisibilityOption { PRIVATE, PROTECTED, INTERNAL, PUBLIC };
}
