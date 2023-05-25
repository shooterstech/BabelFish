using Scopos.BabelFish.DataModel.AthenaLogin;
using System;
using System.Collections.Generic;
using System.Text;

namespace Scopos.BabelFish.DataModel.AthenaLogin
{
    public class ESTUnitActiveSessions
    {
        public List<ESTUnitLogin> Items { get; set; } = new List<ESTUnitLogin>();

        public string NextToken { get; set; }
    }
}
