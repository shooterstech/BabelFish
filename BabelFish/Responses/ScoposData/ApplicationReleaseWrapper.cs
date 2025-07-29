using Scopos.BabelFish.DataModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Scopos.BabelFish.Responses.ScoposData
{
    public class ApplicationReleaseWrapper : BaseClass
    {
        public ApplicationReleaseList ApplicationRelease { get; set; } = new ApplicationReleaseList();
    }
}
