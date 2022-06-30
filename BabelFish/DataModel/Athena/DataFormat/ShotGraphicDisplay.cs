using System;
using System.Collections.Generic;
using System.Text;

namespace ShootersTech.DataModel.Athena.DataFormat
{
    /// <summary>
    /// A ShotGraphicDisplay describes how both the target image graphic should be displayed, what shots
    /// should be displayed on it, and the text representation of the scores for the Competitor.
    /// </summary>
    public class ShotGraphicDisplay
    {

        public string DisplayName { get; set; } = String.Empty;

        /// <summary>
        /// Human readable description of what this ShotGraphicDisplay do.
        /// </summary>
        public string Description { get; set; } = String.Empty;

        /// <summary>
        /// Describes 1 to many target images that can be displayed, or rotated. In a Live event, only the 
        /// first (index == 0) object should be displayed. 
        /// </summary>
        public List<ShotGraphicShow> Show { get; set; } = new List<ShotGraphicShow>();

        /// <summary>
        /// In a Live event, lists the current EventNames for each EventType. In a event that is
        /// already concluded, this data structure will not have any meaning.
        /// </summary>
        public EventTypeNames CurrentEvents { get; set; } = new EventTypeNames();

        /// <summary>
        /// Describites what (text) scores should be displayed along witgh the target image graphics.
        /// </summary>
        public ShootersTech.DataModel.Definitions.AbbreviatedFormat AbbreviatedFormat { get; set; } = new ShootersTech.DataModel.Definitions.AbbreviatedFormat();
    }
}