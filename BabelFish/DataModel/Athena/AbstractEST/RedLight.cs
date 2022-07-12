using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShootersTech.BabelFish.DataModel.Athena.AbstractEST
{
    public class RedLight
    {

        public RedLight()
        {

        }

        public bool Capability { get; set; }

        public int DefaultIllumination { get; set; }

        /// <summary>
        /// On a scale of 0 to 100, the amount of light the Red X is currently illuminating.
        /// </summary>
        public int Illumination { get; set; }
    }
}