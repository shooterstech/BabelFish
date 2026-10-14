using Version = Scopos.BabelFish.DataModel.Common.Version;

namespace Scopos.BabelFish.DataModel.ScoposData {
    public class ReleaseInfo {

        /// <summary>
        /// Version number
        /// <list type="bullet">Is in the form of x.y.z.b.
        /// <item>x is the Major Version</item>
        /// <item>y is the Minor Version</item>
        /// <item>z is the Patch Version</item>
        /// <item>p is the Build version. Defaults to 0 if not included in the string.</item>
        /// </list>
        /// </summary>
        [G_NS.JsonProperty( Order = 1 )]
        public Version Version { get; set; } = Version.DEFAULT;


        [G_NS.JsonProperty( Order = 2 )]

        public ApplicationName Application { get; set; }

        [G_NS.JsonProperty( Order = 3 )]

        public ReleasePhase ReleasePhase { get; set; }


        /// <summary>
        /// The URL to download the build from.
        /// </summary>
        [G_NS.JsonProperty( Order = 4 )]
        public string DownloadFileUrl { get; set; } = "";

        /// <summary>
        /// The URL of the EULA's PDF.
        /// </summary>
        [G_NS.JsonProperty( Order = 5 )]
        public string EULAUrl { get; set; } = "";

        /// <summary>
        /// Enhancement and bug fix notes for this release.
        /// </summary>
        [G_NS.JsonProperty( Order = 6 )]
        public ReleaseNote ReleaseNotes { get; set; } = new ReleaseNote();

        /// <summary>
        /// Other applicaiton version requirements to run this release.
        /// </summary>
        [G_NS.JsonProperty( Order = 7 )]
        public List<ReleaseRequirements> Requires { get; set; } = new List<ReleaseRequirements>();

        /// <inheritdoc />
        public override string ToString() {
            StringBuilder foo = new StringBuilder();
            foo.Append( "Version " );
            foo.Append( Version );
            return foo.ToString();
        }
    }

    public class ReleaseNote {
        /// <summary>
        /// Enhancement notes
        /// </summary>
        [G_NS.JsonProperty( Order = 1 )]
        public List<string> Enhancements { get; set; } = new List<string>();

        /// <summary>
        /// Bug fixe notes
        /// </summary>
        [G_NS.JsonProperty( Order = 2 )]
        public List<string> BugFixes { get; set; } = new List<string>();
    }

    public class ReleaseRequirements {

        [G_NS.JsonProperty( Order = 1 )]
        public ApplicationName Application { get; set; }

        /// <summary>
        /// The minimum version of the Applicaiton that is required.
        /// </summary>
        [G_NS.JsonProperty( Order = 2 )]
        [G_NS.JsonConverter( typeof( G_BF_NS_CONV.VersionConverter ) )]
        [G_STJ_SER.JsonConverter( typeof( G_BF_STJ_CONV.VersionConverter ) )]
        public Version Version { get; set; } = Version.DEFAULT;
    }
}
