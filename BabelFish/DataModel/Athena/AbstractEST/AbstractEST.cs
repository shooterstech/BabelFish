using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShootersTech.DataModel.Athena.AbstractEST
{
    public abstract class AbstractEST
    {

        public AbstractEST()
        {
            ESTUnit = new ESTUnit();
            Logging = new Logging();
        }

        public ESTUnit ESTUnit { get; set; }

        public Logging Logging { get; set; }
    }
}