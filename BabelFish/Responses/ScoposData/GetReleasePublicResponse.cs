using Newtonsoft.Json.Linq;
using Scopos.BabelFish.DataModel.ScoposData;
using Scopos.BabelFish.Requests.ScoposData;

namespace Scopos.BabelFish.Responses.ScoposData
{
    public class GetReleasePublicResponse : Response<ApplicationReleaseWrapper>
    {
        public GetReleasePublicResponse(GetReleasePublicRequest request)
        {
            this.Request = request;
        }

        /// <summary>
        /// Facade function that returns the same as this.Value
        /// </summary>
        public ApplicationReleaseList ApplicationRelease
        {
            get { return Value.ApplicationRelease; }
        }
    }
}