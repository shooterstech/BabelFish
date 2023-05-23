using System;
using System.Collections.Generic;
using System.Text;
using Scopos.BabelFish.DataModel;
using Scopos.BabelFish.DataModel.AthenaLogin;

namespace Scopos.BabelFish.Responses.AthenaLogin
{
    public class ESTUnitLogoutWrapper : BaseClass
    {


        public ESTUnitLogout ESTUnitLogout = new ESTUnitLogout();  

        public override string ToString()
        {
            return "logout list";
        }
    }
}
