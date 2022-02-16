using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace BabelFish.DataModel.OrionMatch {
    /// <summary>
    /// A Participant is anyone who has a role in a Match. This includes athletes, teams, match officials, and coaches.
    /// </summary>
    [Serializable]
    public abstract class Participant {

        public Participant() { }

        /// <summary>
        /// A unique, human readable, value assigned to all Participants in a match.
        /// 
        /// In most cases the CompetitorNumber will be numeric, but it can also be alphabetical.
        /// </summary>
        public string CompetitorNumber { get; set; } = string.Empty;

        public List<AttributeValue> AttributeValues { get; set; } = new List<AttributeValue>();

        /// <summary>
        /// When a competitor's name is displayed, this is the default display value.
        /// </summary>
        public string DisplayName { get; set; } = string.Empty;

        /// <summary>
        /// The three letter country code the participant is from.
        /// </summary>
        public string Country { get; set; } = string.Empty;

        /// <summary>
        /// The Hometown the participant is from.
        /// </summary>
        public string HomeTown { get; set; } = string.Empty;

        /// <summary>
        /// $type coming from API data
        /// </summary>
        [JsonProperty(PropertyName = "$type")]
        public string type{ get; set; } = string.Empty;

        /// <summary>
        /// When a competitor's name is displayed, and there is limited number of characters, use this value. 
        /// 
        /// There is no rule as to how long the Short value could be, but by convention 12 characters or less.
        /// </summary>
        public string DisplayNameShort { get; set; } = string.Empty;

        //TODO: Club, ReentryTag not in API return data
        /// <summary>
        /// The Hometown Club the Participant represents. Note, this is NOT the same as any team the Participant is shooting with. 
        /// </summary>
        public string Club { get; set; } = string.Empty;

        public string ReentryTag { get; set; } = string.Empty;

        public override string ToString() {
            return this.DisplayName;
        }

    }
}
