using Scopos.BabelFish.DataModel.ScoposData;
using Scopos.BabelFish.Requests.OrionMatchAPI;
using Scopos.BabelFish.Helpers;

namespace Scopos.BabelFish.Requests.ScoposData {
    public class GetVersionPublicRequest : Request {
        /// <summary>
        /// Public constructor. 
        /// User is encouraged (really you need to do this) to set the Request Properties at time of construction.
        /// </summary>
        public GetVersionPublicRequest() : base( "GetVersion" ) { }

        /// <summary>
        /// List of VersionService enum value(s)
        /// </summary>
        public List<VersionService> Services { get; set; } = new List<VersionService>();

        /// <summary>
        /// VersionLevel enum value
        /// </summary>
        public VersionLevel Level { get; set; } = VersionLevel.PRODUCTION;

        /// <inheritdoc />
        public override string RelativePath {
            get { return $"/version"; }
        }

        public override Dictionary<string, List<string>> QueryParameters {
            get {
                if (Services.Count() == 0)
                    throw new RequestException( "Must have at least one VersionService." );

                Dictionary<string, List<string>> parameterList = new Dictionary<string, List<string>>();
                parameterList.Add( "services", Services.Select( s => s.Description() ).ToList() );
                parameterList.Add( "level", new List<string>() { Level.Description() } );

                return parameterList;
            }
        }

    }
}
