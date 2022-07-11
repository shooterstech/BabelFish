using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using ShootersTech.BabelFish.Helpers;

namespace ShootersTech.BabelFish.DataModel.Misc
{
    public class VersionsList {
        public List<VersionInfo> Versions = new List<VersionInfo>();
    }

    [Serializable]
    public class VersionInfo
    {
        [JsonProperty(Order = 1)]
        public string Version { get; set; } = string.Empty;

        [JsonProperty(Order = 2)]
        [JsonConverter(typeof(StringEnumConverter))]
        public VersionService Service { get; set; }

        [JsonProperty(Order = 3)]
        [JsonConverter(typeof(StringEnumConverter))]
        public VersionLevel Level { get; set; }

        [JsonProperty(Order = 4)]
        public List<string> ReleaseNotes { get; set; } = new List<string>();

        [JsonProperty(Order = 5)]
        public List<string> Enhancements { get; set; } = new List<string>();

        [JsonProperty(Order = 6)]
        public List<string> BugFixes { get; set; } = new List<string>();

        [JsonProperty(Order = 7)]
        public DateTime Datetime { get; set; } = new DateTime();

        public override string ToString()
        {
            StringBuilder foo = new StringBuilder();
            foo.Append("Version ");
            foo.Append(Version);
            return foo.ToString();
        }
    }
}
