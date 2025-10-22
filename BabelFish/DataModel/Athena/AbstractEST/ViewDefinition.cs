

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

        [G_NS.JsonProperty( Order = 1 )]
        public string ViewName { get; set; }


        [G_NS.JsonProperty( Order = 2, DefaultValueHandling = Newtonsoft.Json.DefaultValueHandling.Include )]
        public DisplayEntityType EntityName { get; set; }

        /// <summary>
        /// EntityName specific configurations.
        /// </summary>
        [G_NS.JsonProperty( Order = 3 )]
        public DisplayEntityConfiguration Config { get; set; }

        /// <summary>
        /// If the Config attribute is a dictionary with Key type of string (which it should be)
        /// the attributes within Config will get replaced with Match specific values by Orion
        /// beofre they are sent to the Display. These values must be
        /// ResultLists: Which is a list of strings
        /// ResultList: Which is a string
        /// </summary>
        [G_NS.JsonProperty( Order = 4 )]
        public List<ReplaceVariableOptions> ReplaceAttributes { get; set; }


        [G_NS.JsonProperty( Order = 5 )]
        public bool ShowTopStrip { get; set; }


        [G_NS.JsonProperty( Order = 6 )]
        public bool ShowBottomStrip { get; set; }


        [G_NS.JsonProperty( Order = 10 )]
        public string Description { get; set; }

        public override string ToString()
        {
            return $"{ViewName} ({EntityName})";
        }
    }
}