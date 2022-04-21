using System;
using System.Collections.Generic;
using System.Text;

namespace ShootersTech.DataModel.Athena.DataFormat {
    public class ShotGraphicDisplay {

        public string DisplayName { get; set; } = String.Empty;

        /// <summary>
        /// Human readable description of what this ShotGraphicDisplay do.
        /// </summary>
        public string Description { get; set; } = String.Empty;

        public ShotGraphicShow Show{ get; set; } = new ShotGraphicShow();

        public EventTypeNames CurrentEvents { get; set; } = new EventTypeNames();

        public BabelFish.DataModel.Definitions.AbbreviatedFormat AbbreviatedFormat { get; set; } = new BabelFish.DataModel.Definitions.AbbreviatedFormat();
    }
}
