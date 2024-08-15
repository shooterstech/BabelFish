using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using Scopos.BabelFish.Requests.Athena;
using Scopos.BabelFish.DataModel.AthenaOwner;

namespace Scopos.BabelFish.Responses.Athena
{
    public class GetUserOwnershipValuesAuthenticatedResponse : Response<AthenaOwnerValuesWrapper>
    {
        public GetUserOwnershipValuesAuthenticatedResponse(GetUserOwnershipValuesAuthenticatedRequest request) : base()
        {
            Request = request;
        }

        /// <summary>
        /// Facade function that returns the same as this.Value
        /// </summary>
        public AthenaOwnerValues AthenaOwnerValues
        {
            get { return Value.AthenaOwnerValues; }
        }
    }
}
