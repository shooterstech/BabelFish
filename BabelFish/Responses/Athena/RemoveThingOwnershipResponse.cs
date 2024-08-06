using Scopos.BabelFish.Requests.Athena;
using Scopos.BabelFish.Responses;
using System;
using System.Collections.Generic;
using System.Text;

namespace Scopos.BabelFish.Responses.Athena
{
    public class RemoveThingOwnershipResponse : Response<RemoveThingOwnershipWrapper>
    {
        public RemoveThingOwnershipResponse(RemoveThingOwnershipRequest request) : base()
        {
            Request = request;
        }

        public Dictionary<string, object> ThingData => Value.ThingData;
        public string Region => Value.Region;
    }
}
