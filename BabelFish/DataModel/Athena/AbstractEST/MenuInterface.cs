using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scopos.BabelFish.DataModel.Athena.AbstractEST
{
    public class MenuInterface
    {

        public MenuInterface()
        {

        }

        public bool Capability { get; set; }

        /// <summary>
        /// is the menu interface is having an error... usually only having to do with the I2C interface being missing and it being set to 4 button.
        /// </summary>
        public string Error { get; set; }

        /// <summary>
        /// which of the button styles.
        /// "1 Button" - one button, no LCD screen
        /// "4 Button" - four button, LCD screen using I2C (Address 0x3C) for menu interaction
        /// </summary>
        public string InterfaceType { get; set; }
    }
}