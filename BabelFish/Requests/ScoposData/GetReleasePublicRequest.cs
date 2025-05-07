using Scopos.BabelFish.DataModel.ScoposData;
using Scopos.BabelFish.Requests.OrionMatchAPI;
using Scopos.BabelFish.Helpers;


namespace Scopos.BabelFish.Requests.ScoposData
{
    public class GetReleasePublicRequest : Request
    {
        /// <summary>
        /// Public constructor. 
        /// User is encouraged (really you need to do this) to set the Request Properties at time of construction.
        /// </summary>
        public GetReleasePublicRequest() : base("GetRelease") { }

        /// <summary>
        /// VersionLevel to request
        /// </summary>
        public ReleasePhase ReleasePhase { get; set; } = ReleasePhase.PRODUCTION;

        /// <summary>
        /// The ThingName of whoever is making the call, not required. 
        /// </summary>
        public string ThingName { get; set; } = "";

        /// <summary>
        /// The ThingVersion of whoever is making the call, not required.
        /// </summary>
        /// <remarks>ThingVersion may be null.</remarks>
        public Version ? ThingVersion { get; set; } = null;

        /// <inheritdoc />
        public override string RelativePath
        {
            get { return $"/release"; }
        }

        public override Dictionary<string, List<string>> QueryParameters
        {
            get
            {

                Dictionary<string, List<string>> parameterList = new Dictionary<string, List<string>>();
                parameterList.Add("release-phase", new List<string>() { ReleasePhase.Description() });

                if ( !string.IsNullOrEmpty( this.ThingName ) )
                    parameterList.Add("thing-name", new List<string>() { ThingName });

                if (this.ThingVersion != null) {
                    parameterList.Add( "thing-version", new List<string>() { ThingVersion.ToString() } );
                }

                return parameterList;
            }
        }
    }
}
