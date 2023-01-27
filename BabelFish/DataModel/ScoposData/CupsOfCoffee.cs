using System;
using System.Collections.Generic;
using System.Text;
using Scopos.BabelFish.DataModel;

namespace Scopos.BabelFish.DataModel.ScoposData {
    public class CupsOfCoffee : BaseClass {

        public CupsOfCoffee() { }

        /// <summary>
        /// Cups of coffee consumed by Shooter's Tech developers since they started working on Athena
        /// </summary>
        public int CupsOfCoffeeConsumed { get; set; }
    }
}
