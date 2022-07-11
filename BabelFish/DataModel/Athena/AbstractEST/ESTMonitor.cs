using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShootersTech.BabelFish.DataModel.Athena.AbstractEST
{
    public class ESTMonitor : AbstractEST
    {

        public ESTMonitor()
        {
            Monitor = new Monitor();
        }

        public Monitor Monitor { get; set; }
    }
}