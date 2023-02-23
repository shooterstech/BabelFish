using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace Scopos.BabelFish.DataModel.AttributeValue {

    [Serializable]
    public class ValidateUserID : BaseClass {
        public ValidateUserID() { }

        /// <summary>
        /// UserID
        /// </summary>
        [JsonProperty(Order = 1)]
        public string UserID { get; set; } = string.Empty;

        [JsonProperty(Order = 2)]
        public bool Valid{ get; set; } = false;

        public override string ToString()
        {
            StringBuilder foo = new StringBuilder();
            foo.Append("UserID status for ");
            foo.Append(UserID);
            return foo.ToString();
        }

    }
}