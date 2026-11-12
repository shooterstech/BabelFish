using Scopos.BabelFish.DataModel.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Scopos.BabelFish.Responses.Athena
{
    public class RemoveThingOwnershipWrapper : BaseClass
    {
        public Dictionary<string, object> ThingData { get; set; }
        public string Region { get; set; }
    }
}
