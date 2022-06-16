using BabelFish.Helpers;

namespace BabelFish.Requests.Misc
{
    public class GetVersionRequest : Request
    {
        private const string ParamName = "services";
        private Dictionary<string, List<string>> queryParameters = new Dictionary<string, List<string>>();

        public GetVersionRequest(List<VersionService> services, VersionLevel level)
        {

            queryParameters.Add(ParamName, new List<string>());
            services.ForEach(x => queryParameters[ParamName].Add(x.ToString()));

            queryParameters.Add("level", new List<string>());
            queryParameters["level"].Add(level.ToString());
        }

        /// <inheritdoc />
        public override string RelativePath
        {
            get { return $"/version"; }
        }

        public override Dictionary<string, List<string>> QueryParameters
        {
            get
            {
                return queryParameters;
            }
        }

    }
}
