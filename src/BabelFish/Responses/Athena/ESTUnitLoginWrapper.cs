using System;
using System.Collections.Generic;
using System.Text;
using Scopos.BabelFish.DataModel;
using Scopos.BabelFish.DataModel.AthenaLogin;

namespace Scopos.BabelFish.Responses.Athena
{
    public class ESTUnitLoginWrapper : BaseClass
    {


        public ESTUnitLogin ESTUnitLogin { get; set; } = new ESTUnitLogin();

        public override string ToString()
        {
            return $"EST Unit Login for {ESTUnitLogin.ThingName}";
        }
    }
}
