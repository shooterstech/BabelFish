using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;
using Scopos.BabelFish.APIClients;
using Scopos.BabelFish.DataModel.Definitions;
using Scopos.BabelFish.Requests;

namespace   Scopos.BabelFish.Requests.Athena
{
    public class RemoveThingOwnershipRequest : Request
    {
        public RemoveThingOwnershipRequest() : base("RemoveThingOwnership")
        {
            RequiresCredentials = false;
            SubDomain = APISubDomain.INTERNAL;
            HttpMethod = HttpMethod.Delete;
        }

        public string OwnerId { get; set; }

        public string CpuSerial { get; set; }

        public string SharedKey { get; set; }

        /// <inheritdoc />
        public override string RelativePath
        {
            get { return $"/target/take-ownership"; }
        }

        public override Dictionary<string, List<string>> QueryParameters
        {
            get
            {

                Dictionary<string, List<string>> parameterList = new Dictionary<string, List<string>>();

                if (!string.IsNullOrEmpty(OwnerId))
                {
                    parameterList.Add("owner", new List<string> { OwnerId });
                }
                if (!string.IsNullOrEmpty(CpuSerial))
                {
                    parameterList.Add("cpu-serial", new List<string> { CpuSerial });
                }
                if (!string.IsNullOrEmpty(SharedKey))
                {
                    parameterList.Add("shared-key", new List<string> { SharedKey });
                }

                return parameterList;
            }
        }
    }
}
