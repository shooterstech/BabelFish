using System;
using System.Collections.Generic;
using System.Text;

namespace Scopos.BabelFish.DataModel.Athena.AbstractEST {
    public abstract class DisplayEntityConfiguration {

        /// <summary>
        /// Acts as the concrete class identifier.
        /// </summary>
        public DisplayEntityType DisplayEntity { get; protected set; }
    }

    public class AthleteDisplayConfiguration : DisplayEntityConfiguration {
        /*
         * Documentation at https://support.scopos.tech/index.html?display-entity-athlete-display.html
         */

        public AthleteDisplayConfiguration() {
            this.DisplayEntity = DisplayEntityType.AthleteDisplay;
        }

        public string FiringPoints { get; set; } = "*";

        public string ResultList { get; set; } = string.Empty;

        public string ShotPresentation { get; set; } = "ALL";
    }

    public class ImageDisplayConfiguration : DisplayEntityConfiguration {
        /*
         * Documentation at https://support.scopos.tech/index.html?display-entity-image-display.html
         */

        public ImageDisplayConfiguration() {
            this.DisplayEntity = DisplayEntityType.ImageDisplay;
        }

        public string ImageKey { get; set; } = string.Empty;

        //Should be an enum
        public string Presentation { get; set; } = "Alternate";
        
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

        public List<string> ResultLists { get; set; } = new List<string>();

        public bool ByRelay { get; set; } = true;
    }

    public class SquaddingListConfiguration : DisplayEntityConfiguration {
        /*
         * Documentation at https://support.scopos.tech/index.html?squadding-list2.html
         */

        public SquaddingListConfiguration() {
            this.DisplayEntity = DisplayEntityType.SquaddingList;
        }

        public string FiringPoints { get; set; } = "*";

        public string Relay { get; set; } = "CURRENT";

        public bool Scrolling { get; set; } = false;
    }
}
