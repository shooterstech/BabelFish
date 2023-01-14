using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Scopos.BabelFish.DataModel.Athena.AbstractEST
{
    public class ViewDefinitionConfiguration
    {

        public ViewDefinitionConfiguration()
        {

        }

        public ViewDefinitionConfiguration(ViewDefinitionConfiguration vdc)
        {
            this.DefinitionName = vdc.DefinitionName;
            this.Duration = vdc.Duration;
        }

        public string DefinitionName { get; set; }

        /// <summary>
        /// The time in seconds that this ViewDefinition is shown on the Display.
        /// The default value is 15 seconds.
        /// NOTE: The default value is set in the source code for Athena.
        /// </summary>
        [DefaultValue(15)]
        public int Duration { get; set; }

        public override string ToString()
        {
            return DefinitionName;
        }
    }
}