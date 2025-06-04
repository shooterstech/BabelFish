using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Scopos.BabelFish.DataModel.Athena.AbstractEST {
    public abstract class DisplayEntityConfiguration {

        /// <summary>
        /// Acts as the concrete class identifier.
        /// </summary>
        [G_NS.JsonProperty( Order = 1, DefaultValueHandling = Newtonsoft.Json.DefaultValueHandling.Include )]
        public DisplayEntityType DisplayEntity { get; protected set; }
    }

    public class AthleteDisplayConfiguration : DisplayEntityConfiguration {
        /*
         * Documentation at https://support.scopos.tech/index.html?display-entity-athlete-display.html
         */

        public AthleteDisplayConfiguration() {
            this.DisplayEntity = DisplayEntityType.AthleteDisplay;
        }

        [G_NS.JsonProperty( Order = 2 )]
        public List<string> FiringPoints { get; set; } = new List<string>();

        [G_NS.JsonProperty( Order = 3 )]
        public string ResultList { get; set; } = string.Empty;

        [G_NS.JsonProperty( Order = 4 )]
		//Should be an enum
		public string ShotPresentation { get; set; } = "ALL"; 

        [G_NS.JsonProperty(Order = 4, DefaultValueHandling = Newtonsoft.Json.DefaultValueHandling.Include)]
        [DefaultValue(true)]
        public bool ShowRank { get; set; } = true;

        [G_NS.JsonProperty(Order = 5, DefaultValueHandling = Newtonsoft.Json.DefaultValueHandling.Include)]
        public PaintGraphic PaintGraphic { get; set; } = PaintGraphic.TargetAndResultCOF; //default is first enum, TargetAndResultCOF

		[G_NS.JsonProperty( Order = 6, DefaultValueHandling = Newtonsoft.Json.DefaultValueHandling.Include )]
        [DefaultValue( ViewDefinitionShotDisplay.NONE )]
		public ViewDefinitionShotDisplay ShotDisplayModifier { get; set; } = ViewDefinitionShotDisplay.NONE;

		[G_NS.JsonProperty( Order = 7, DefaultValueHandling = Newtonsoft.Json.DefaultValueHandling.Include )]
		[DefaultValue( ViewDefinitionShotDisplay.SIGHTER_NUMBER )]
		public ViewDefinitionShotDisplay SighterDisplayModifier { get; set; } = ViewDefinitionShotDisplay.SIGHTER_NUMBER;

        [G_NS.JsonProperty(Order = 8, DefaultValueHandling = Newtonsoft.Json.DefaultValueHandling.Include)]
        [DefaultValue(NeedsDisplayOptions.NONE)]
        public NeedsDisplayOptions NeedsToLeadDisplay { get; set; } = NeedsDisplayOptions.NONE;

        [G_NS.JsonProperty(Order = 9, DefaultValueHandling = Newtonsoft.Json.DefaultValueHandling.Include)]
        [DefaultValue(NeedsDisplayOptions.NONE)]
        public NeedsDisplayOptions NeedsToSurviveDisplay { get; set; } = NeedsDisplayOptions.NONE;

    }

    public class ImageDisplayConfiguration : DisplayEntityConfiguration {
        /*
         * Documentation at https://support.scopos.tech/index.html?display-entity-image-display.html
         */

        public ImageDisplayConfiguration() {
            this.DisplayEntity = DisplayEntityType.ImageDisplay;
        }

        [G_NS.JsonProperty( Order = 2, DefaultValueHandling = Newtonsoft.Json.DefaultValueHandling.Include )]
        public string ImageKey { get; set; } = string.Empty;

        [G_NS.JsonProperty( Order = 3 )]
        //Should be an enum
        public string Presentation { get; set; } = "Alternate";

        [G_NS.JsonProperty( Order = 4 )]
        //Should be an enum
        public string ScreenFormat { get; set; } = "FIT";
    }

    public class ResultListConfiguration : DisplayEntityConfiguration {
        /*
         * Documentation at https://support.scopos.tech/index.html?display-entity-result-list.html
         */

        public ResultListConfiguration() {
            this.DisplayEntity = DisplayEntityType.ResultList;
        }

        [G_NS.JsonProperty( Order = 2 )]
        public List<string> ResultLists { get; set; } = new List<string>();

        [G_NS.JsonProperty( Order = 3 )]
        public bool ByRelay { get; set; } = true;

        [G_NS.JsonProperty( Order = 4, DefaultValueHandling = Newtonsoft.Json.DefaultValueHandling.Include )]
        public int AnimatedRows { get; set; } = 3;
    }

    public class SquaddingListConfiguration : DisplayEntityConfiguration {
        /*
         * Documentation at https://support.scopos.tech/index.html?squadding-list2.html
         */

        public SquaddingListConfiguration() {
            this.DisplayEntity = DisplayEntityType.SquaddingList;
        }

        [G_NS.JsonProperty( Order = 2 )]
        public List<string> FiringPoints { get; set; } = new List<string>();

        [G_NS.JsonProperty( Order = 3 )]
        public string Relay { get; set; } = "CURRENT";

        [G_NS.JsonProperty( Order = 4 )]
        public bool Scrolling { get; set; } = false;
    }
}
