using System;
using System.Collections.Generic;
using System.Text;
using Scopos.BabelFish.DataModel;
using Scopos.BabelFish.DataModel.AthenaLogin;

namespace Scopos.BabelFish.Responses.Athena
{
    public class ESTUnitActiveSessionsWrapper : BaseClass
    {


        public ESTUnitActiveSessions ESTUnitActiveSessions = new ESTUnitActiveSessions();

        public override string ToString()
        {
            return $"EST Unit Active Session count of {ESTUnitActiveSessions.Items.Count}";
        }
    }
}
