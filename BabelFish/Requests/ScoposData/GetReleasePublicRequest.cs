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
        /// List of application names as strings the caller would like to recieve release notes for
        /// </summary>
        public List<string> ApplicationItems { get; set; } = new List<string>( ["orion", "athena"] );
        /// <summary>
        /// The ThingName of whoever is making the call, not required. 
        /// </summary>
        public string ThingName { get; set; } = "";

        /// <summary>
        /// The ThingVersion of whoever is making the call, not required.
        /// </summary>
        /// <remarks>ThingVersion may be null.</remarks>
        public DataModel.Common.Version? ThingVersion { get; set; } = null;

        /// <summary>
        /// boolean to track if the user has accepted the Orion EULA, without this there will be no download link in the response.
        /// </summary>
        public bool OrionEulaAccepted { get; set; } = false;

        /// <summary>
        /// boolean to track if the user has accepted the Athena EULA, without this there will be no download link in the response.
        /// </summary>
        public bool AthenaEulaAccepted { get; set; } = false;

        /// <summary>
        /// Owner ID of the caller, to track their EULA accepted-ness, without this the DB table will not update and there will be no download link in the response.
        /// </summary>
        public string OwnerID { get; set; } = "";

        /// <inheritdoc />
        public override string RelativePath
        {
            get { return $"/release"; }
        }

        /*
           {
                "release-phase" : "production",
                "application-items" : [ "orion", "athena" ],
                "thing-name" : "000015-orion-001",
                "thing-version" : "2.21.1",
                "orion-eula-accepted" : False,
                "athena-eula-accepted" : False,
                "owner-id" : "OrionAcct000015"
            }
         */
        public override Dictionary<string, List<string>> QueryParameters
        {
            get
            {

                Dictionary<string, List<string>> parameterList = new Dictionary<string, List<string>>();

                parameterList.Add("release-phase", new List<string>() { ReleasePhase.Description() });

                parameterList.Add("application-items", ApplicationItems );

                if ( !string.IsNullOrEmpty( this.ThingName ) )
                    parameterList.Add("thing-name", new List<string>() { ThingName });

                if ( !(ThingVersion is null) ) {
                    parameterList.Add( "thing-version", new List<string>() { ThingVersion.ToString() } );
                }

                parameterList.Add("orion-eula-accepted", new List<string>() { OrionEulaAccepted.ToString() });

                parameterList.Add("athena-eula-accepted", new List<string>() { AthenaEulaAccepted.ToString() });

                if ( !string.IsNullOrEmpty( this.OwnerID ) )
                {
                    parameterList.Add("owner-id", new List<string>() { OwnerID.ToString() });
                }

                return parameterList;
            }
        }
    }
}
