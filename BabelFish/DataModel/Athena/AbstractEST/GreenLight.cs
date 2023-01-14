using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scopos.BabelFish.DataModel.Athena.AbstractEST
{
    public class GreenLight
    {

        public GreenLight()
        {

        }

        public bool Capability { get; set; }

        public int DefaultIllumination { get; set; }

        /// <summary>
        /// On a scale of 0 to 100, the amount of light the green O is currently illuminating.
        /// </summary>
        public int Illumination { get; set; }
    }
}