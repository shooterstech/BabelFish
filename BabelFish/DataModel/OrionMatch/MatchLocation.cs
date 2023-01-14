using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Scopos.BabelFish.DataModel.OrionMatch
{
    [Serializable]
    public class MatchLocation : Location {

        public MatchLocation() { }

        /// <summary>
        /// After an object is deserialized form JSON,
        /// adds defaults to empty properties
        /// </summary>
        /// <param name="context"></param>
        [OnDeserialized()]
        public void OnDeserialized(StreamingContext context) { }

        [JsonProperty(Order = 1)]
        public string Name { get; set; } = string.Empty;

        [JsonProperty(Order = 2)]
        public string MatchName { get; set; } = string.Empty;

        [JsonProperty(Order = 3)]
        public string HostingClub { get; set; } = string.Empty;

        [JsonProperty(Order = 4)]
        public string StartDate { get; set; } = string.Empty;

        [JsonProperty(Order = 5)]
        public string EndDate { get; set; } = string.Empty;

        [JsonProperty(Order = 6)]
        public string ZipCode { get; set; } = string.Empty;

        [JsonProperty(Order = 7)]
        public string ShootingStyle { get; set; } = string.Empty;

        /// <summary>
        /// Contact information for the match, i.e. person's name, phone, email.
        /// </summary>
        [JsonProperty(Order = 8)]
        public POCContact POCContactInformation { get; set; } = new POCContact();

        public override string ToString() {
            StringBuilder foo = new StringBuilder();
            foo.Append("MatchLocation for ");
            foo.Append(MatchName);
            return foo.ToString();
        }
    }

    public class POCContact : MatchContact
    {
        public string PhoneNumber { get; set; } = string.Empty;
    }
}
