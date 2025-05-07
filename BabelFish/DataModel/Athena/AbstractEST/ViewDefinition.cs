using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using BabelFish.DataModel.Athena;

namespace Scopos.BabelFish.DataModel.Athena.AbstractEST
{
    public class ViewDefinition
    {

        public ViewDefinition()
        {

            ViewName = "Default";
            EntityName = DisplayEntityType.AthleteDisplay;
            //Config = new Dictionary<string, string>();
            ReplaceAttributes = new List<ReplaceVariableOptions>();
        }

        /// <summary>
        /// Creates a new ViewDefinition by copying the values of the passed in ViewDefinition
        /// </summary>
        /// <param name="vd"></param>
        public ViewDefinition(ViewDefinition vd)
        {
            this.ViewName = vd.ViewName;
            this.EntityName = vd.EntityName;
            this.Config = vd.Config;
            this.ShowTopStrip = vd.ShowTopStrip;
            this.ShowBottomStrip = vd.ShowBottomStrip;
        }

        public string ViewName { get; set; }

        public string Description { get; set; }

        public DisplayEntityType EntityName { get; set; }

        /// <summary>
        /// EntityName specific configurations.
        /// </summary>
        public DisplayEntityConfiguration Config { get; set; }

        /// <summary>
        /// If the Config attribute is a dictionary with Key type of string (which it should be)
        /// the attributes within Config will get replaced with Match specific values by Orion
        /// beofre they are sent to the Display. These values must be
        /// ResultLists: Which is a list of strings
        /// ResultList: Which is a string
        /// </summary>
        public List<ReplaceVariableOptions> ReplaceAttributes { get; set; }

        public bool ShowTopStrip { get; set; }

        public bool ShowBottomStrip { get; set; }

        public override string ToString()
        {
            return $"{ViewName} ({EntityName})";
        }
    }
}