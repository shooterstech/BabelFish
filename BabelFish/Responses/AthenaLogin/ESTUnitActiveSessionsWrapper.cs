using System;
using System.Collections.Generic;
using System.Text;
using Scopos.BabelFish.DataModel;
using Scopos.BabelFish.DataModel.AthenaLogin;

namespace Scopos.BabelFish.Responses.AthenaLogin
{
    public class ESTUnitActiveSessionsWrapper : BaseClass
    {


        public ESTUnitActiveSessions ESTUnitActiveSessions = new ESTUnitActiveSessions();

        public override string ToString()
        {
            return "active session list";
        }
    }
}
