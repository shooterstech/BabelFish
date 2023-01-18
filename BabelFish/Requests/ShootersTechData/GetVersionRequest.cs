using Scopos.BabelFish.DataModel.ShootersTechData;
using Scopos.BabelFish.Requests.OrionMatchAPI;
using Scopos.BabelFish.Helpers;

namespace Scopos.BabelFish.Requests.ShootersTechData {
    public class GetVersionRequest : Request {
        /// <summary>
        /// Public constructor. 
        /// User is encouraged (really you need to do this) to set the Request Properties at time of construction.
        /// </summary>
        public GetVersionRequest() : base( "GetVersion" ) { }

        /// <summary>
        /// List of VersionService enum value(s)
        /// </summary>
        public List<VersionService> Services { get; set; } = new List<VersionService>();

        /// <summary>
        /// VersionLevel enum value
        /// </summary>
        public VersionLevel Level { get; set; } = VersionLevel.NONE;

        /// <inheritdoc />
        public override string RelativePath {
            get { return $"/version"; }
        }

        public override Dictionary<string, List<string>> QueryParameters {
            get {
                if (Services.Count() == 0)
                    throw new GetOrionMatchRequestException( "Must have at least one VersionService." );

                Dictionary<string, List<string>> parameterList = new Dictionary<string, List<string>>();
                parameterList.Add( "services", Services.Select( s => s.Description() ).ToList() );
                parameterList.Add( "level", new List<string>() { Level.Description() } );

                return parameterList;
            }
        }

    }
}
