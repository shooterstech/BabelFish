using System;
using System.Collections.Generic;
using System.Text;
using Scopos.BabelFish.DataModel.AthenaLogin;
using Scopos.BabelFish.DataModel.Common;

namespace Scopos.BabelFish.Responses.Athena
{
    public class ESTUnitLogoutWrapper : BaseClass
    {


        public ESTUnitLogout ESTUnitLogout { get; set; } = new ESTUnitLogout();

        public override string ToString()
        {
            return $"EST Unit Logout count of {ESTUnitLogout.Items.Count}";
        }
    }
}
