using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ShootersTech.DataModel.Athena.AbstractEST
{
    public class ViewDefinition
    {

        public enum ReplaceVariableOptions { ResultList, ResultLists };

        public ViewDefinition()
        {

            ViewName = "Default";
            EntityName = "AthleteDisplay";
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

        [JsonProperty(Order = 1)]
        public string ViewName { get; set; }

        [JsonProperty(Order = 2)]
        public string Description { get; set; }

        [JsonProperty(Order = 3)]
        public string EntityName { get; set; }

        /// <summary>
        /// EntityName specific configurations.
        /// </summary>
        [JsonProperty(Order = 4)]
        public dynamic Config { get; set; }

        /// <summary>
        /// If the Config attribute is a dictionary with Key type of string (which it should be)
        /// the attributes within Config will get replaced with Match specific values by Orion
        /// beofre they are sent to the Display. These values must be
        /// ResultLists: Which is a list of strings
        /// ResultList: Which is a string
        /// </summary>
        [JsonProperty(Order = 5, ItemConverterType = typeof(StringEnumConverter))]
        public List<ReplaceVariableOptions> ReplaceAttributes { get; set; }

        [JsonProperty(Order = 6)]
        public bool ShowTopStrip { get; set; }

        [JsonProperty(Order = 7)]
        public bool ShowBottomStrip { get; set; }

        public override string ToString()
        {
            return $"{ViewName} ({EntityName})";
        }
    }
}