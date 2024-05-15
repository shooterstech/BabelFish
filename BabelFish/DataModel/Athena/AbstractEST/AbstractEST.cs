using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scopos.BabelFish.DataModel.Athena.AbstractEST
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

        /// <summary>
        /// Errors and warnings detected externally about the state of the Target.
        /// </summary>
        public List<string> ExternalErrors { get; set; } = new List<string>();
    }
}