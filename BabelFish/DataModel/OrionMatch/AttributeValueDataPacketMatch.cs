using System;
using System.Collections.Generic;
using System.Text;
using Scopos.BabelFish.DataModel.AttributeValue;

namespace Scopos.BabelFish.DataModel.OrionMatch {
    /// <summary>
    /// An AttributeValue that helps describe a Partipant in a match.
    /// </summary>
    public class AttributeValueDataPacketMatch : AttributeValueDataPacket {

        /// <summary>
        /// Public constructor
        /// </summary>
        public AttributeValueDataPacketMatch() {

        }

        /// <summary>
        /// Some AttributeValues are specific to a Participants ReentryTag. An emptry string value ("") or a value of "No Reentry" means the same thing.
        /// </summary>
        public string ReentryTag { get; set; }
    }
}
