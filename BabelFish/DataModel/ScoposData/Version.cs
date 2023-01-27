using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Scopos.BabelFish.Helpers;
using Scopos.BabelFish.DataModel;

namespace Scopos.BabelFish.DataModel.ScoposData {
    public class VersionsList : BaseClass {
        public List<VersionInfo> Versions = new List<VersionInfo>();
    }

    [Serializable]
    public class VersionInfo {

        /// <summary>
        /// Version number. in the form of a.b.c.d
        /// a = major version
        /// b = minor version
        /// c = bug fix version
        /// d = internal build
        /// </summary>
        [JsonProperty( Order = 1 )]
        public string Version { get; set; } = string.Empty;


        [JsonProperty( Order = 2 )]
        [JsonConverter( typeof( StringEnumConverter ) )]
        public VersionService Service { get; set; }

        [JsonProperty( Order = 3 )]
        [JsonConverter( typeof( StringEnumConverter ) )]
        public VersionLevel Level { get; set; }

        [JsonProperty( Order = 4 )]
        public List<string> ReleaseNotes { get; set; } = new List<string>();

        [JsonProperty( Order = 5 )]
        public List<string> Enhancements { get; set; } = new List<string>();

        [JsonProperty( Order = 6 )]
        public List<string> BugFixes { get; set; } = new List<string>();

        [JsonProperty( Order = 7 )]
        public DateTime Datetime { get; set; } = new DateTime();

        public override string ToString() {
            StringBuilder foo = new StringBuilder();
            foo.Append( "Version " );
            foo.Append( Version );
            return foo.ToString();
        }
    }
}
