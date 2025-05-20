using System;
using System.Collections.Generic;
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
        public string ShotPresentation { get; set; } = "ALL";

		[G_NS.JsonProperty( Order = 5 )]
		public string PaintGraphic { get; set; } = string.Empty;
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
