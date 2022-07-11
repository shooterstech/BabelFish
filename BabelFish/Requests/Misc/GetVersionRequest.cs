using ShootersTech.Helpers;
using ShootersTech.Requests.OrionMatchAPI;

namespace ShootersTech.Requests.Misc
{
    public class GetVersionRequest : Request
    {
        /// <summary>
        /// Public constructor. 
        /// User is encouraged (really you need to do this) to set the Request Properties at time of construction.
        /// </summary>
        public GetVersionRequest() { }

        /// <summary>
        /// List of VersionService enum value(s)
        /// </summary>
        public List<VersionService> services = new List<VersionService>();

        /// <summary>
        /// VersionLevel enum value
        /// </summary>
        public VersionLevel level = VersionLevel.none;

        /// <inheritdoc />
        public override string RelativePath
        {
            get { return $"/version"; }
        }

        public override Dictionary<string, List<string>> QueryParameters
        {
            get
            {
                if (services.Count() == 0)
                    throw new GetOrionMatchRequestException("Must have at least one VersionService.");

                Dictionary<string, List<string>> parameterList = new Dictionary<string, List<string>>();
                parameterList.Add("services", services.Select(s => s.ToString()).ToList() );
                parameterList.Add("level", new List<string>() { level.ToString() });

                return parameterList;
            }
        }

    }
}
