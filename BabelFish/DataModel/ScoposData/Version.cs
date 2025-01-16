using System.Text;
using System.Text.Json.Serialization;

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
        [JsonPropertyOrder ( 1 )]
        public string Version { get; set; } = string.Empty;


        [JsonPropertyOrder ( 2 )]
        
        public VersionService Service { get; set; }

        [JsonPropertyOrder ( 3 )]
        
        public VersionLevel Level { get; set; }

        [JsonPropertyOrder ( 4 )]
        public List<string> ReleaseNotes { get; set; } = new List<string>();

        [JsonPropertyOrder ( 5 )]
        public List<string> Enhancements { get; set; } = new List<string>();

        [JsonPropertyOrder ( 6 )]
        public List<string> BugFixes { get; set; } = new List<string>();

        [JsonPropertyOrder ( 7 )]
        public DateTime Datetime { get; set; } = new DateTime();

        public override string ToString() {
            StringBuilder foo = new StringBuilder();
            foo.Append( "Version " );
            foo.Append( Version );
            return foo.ToString();
        }
    }
}
