using ShootersTech.BabelFish.DataModel.Misc;
using ShootersTech.BabelFish.Requests.Misc;

namespace ShootersTech.BabelFish.Responses.Misc
{
    public class GetVersionResponse : Response<VersionsList>
    {
        public GetVersionResponse(GetVersionRequest request)
        {
            this.Request = request;
        }

        /// <summary>
        /// Facade function that returns the same as this.Value
        /// </summary>
        public List<VersionInfo> VersionList
        {
            get { return Value.Versions; }
        }
    }
}