using ShootersTech.DataModel.Misc;
using ShootersTech.Requests.Misc;

namespace ShootersTech.Responses.Misc
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