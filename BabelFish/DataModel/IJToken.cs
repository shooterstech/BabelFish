using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Scopos.BabelFish.DataModel {

    /// <summary>
    /// Interface defines functions a class that needs to control how it is serialized into json.
    /// Not 100% sure this really needs to be an interface class.
    /// Custom conversion to and from JSON is usually doen via custom convertrers. https://www.newtonsoft.com/json/help/html/CustomJsonConverter.htm
    /// However, i'm choosing not to follow this pattern because ... well at the moment I think a ToJToken() function will be easier to write.
    /// </summary>
    [Obsolete("Use custom converters instead")]
    interface IJToken {

        /// <summary>
        /// Returns a JToken object representing the class instance.
        /// Normally a class could implement this like JsonConvert.Serialize(this)
        /// </summary>
        /// <returns></returns>
        JToken ToJToken();
    }
}
