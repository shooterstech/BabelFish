using System;
using System.Collections.Generic;
using System.Text;

namespace Scopos.BabelFish.DataModel.OrionMatch
{
    /// <summary>
    /// A ShotGraphicDisplay describes how both the target image graphic should be displayed, what shots
    /// should be displayed on it, and the text representation of the scores for the Competitor.
    /// </summary>
    public class ShotGraphicDisplay
    {
        /// <summary>
        /// Human Readable name used only in Post Displays
        /// </summary>
        public string PostDisplayName { get; set; } = String.Empty;

        /// <summary>
        /// Human readable description of what this ShotGraphicDisplay do.
        /// </summary>
        public string Description { get; set; } = String.Empty;

        /// <summary>
        /// Describes which shots should be displayed graphically.
        /// </summary>
        public ShotGraphicShow Show { get; set; } = new ShotGraphicShow();

        /// <summary>
        /// In a Live event, lists the current EventNames for each EventType. In a event that is
        /// already concluded, this data structure will not have any meaning.
        /// </summary>
        public EventTypeNames CurrentEvents { get; set; } = new EventTypeNames();

        /// <summary>
        /// Describites what (text) scores should be displayed along witgh the target image graphics.
        /// </summary>
        public AbbreviatedScoreDisplay AbbreviatedFormat { get; set; } = new AbbreviatedScoreDisplay();
    }
}