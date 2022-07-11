using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShootersTech.BabelFish.DataModel.Athena.AbstractEST
{
    public class DisplayStrip
    {

        public DisplayStrip()
        {
            DisplayEntityName = "MatchInfoStrip";
            Config = "{}";
        }

        public string DisplayEntityName { get; set; }

        /// <summary>
        /// String is formatted as JSON
        /// </summary>
        public string Config { get; set; }
    }
}