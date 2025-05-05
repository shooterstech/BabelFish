using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Scopos.BabelFish.DataModel.ScoposData
{
    public class ReleaseInfo
    {

        /// <summary>
        /// Version number. in the form of a.b.c.d
        /// a = major version
        /// b = minor version
        /// c = bug fix version
        /// d = internal build
        /// </summary>
        [JsonPropertyOrder(1)]
        public string Version { get; set; } = string.Empty;


        [JsonPropertyOrder(2)]

        public ApplicationName Application { get; set; }

        [JsonPropertyOrder(3)]

        public ReleasePhase ReleasePhase { get; set; }

        [JsonPropertyOrder(4)]
        public string DownloadFile { get; set; } = "";

        [JsonPropertyOrder(5)]
        public List<string> Enhancements { get; set; } = new List<string>();

        [JsonPropertyOrder(6)]
        public List<string> BugFixes { get; set; } = new List<string>();

        [JsonPropertyOrder(7)]
        public ReleaseNote ReleaseNotes { get; set; } = new ReleaseNote();

        [JsonPropertyOrder(8)]
        public List<ReleaseRequirements> Requires { get; set; } = new List<ReleaseRequirements>();

        public override string ToString()
        {
            StringBuilder foo = new StringBuilder();
            foo.Append("Version ");
            foo.Append(Version);
            return foo.ToString();
        }
    }

    public class ReleaseNote
    {
        [JsonPropertyOrder(1)]
        public List<string> Enhancements { get; set; } = new List<string>();

        [JsonPropertyOrder(2)]
        public List<string> BugFixes { get; set; } = new List<string>();
    }

    public class ReleaseRequirements
    {
        public ApplicationName Application { get; set; }
        public string Version { get; set; } = string.Empty;
    }
}
