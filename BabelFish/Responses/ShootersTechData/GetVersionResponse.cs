using ShootersTech.BabelFish.DataModel.ShootersTechData;
using ShootersTech.BabelFish.Requests.ShootersTechData;

namespace ShootersTech.BabelFish.Responses.ShootersTechData
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