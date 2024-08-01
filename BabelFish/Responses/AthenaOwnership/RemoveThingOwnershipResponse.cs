using Scopos.BabelFish.Requests.AthenaOwnership;
using System;
using System.Collections.Generic;
using System.Text;

namespace Scopos.BabelFish.Responses.AthenaOwnership
{
    public class RemoveThingOwnershipResponse : Response<RemoveThingOwnershipWrapper>
    {
        public RemoveThingOwnershipResponse(RemoveThingOwnershipRequest request) : base()
        {
            this.Request = request;
        }

        public Dictionary<string, object> ThingData => Value.ThingData;
        public string Region => Value.Region;
    }
}
