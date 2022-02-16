using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace BabelFish.DataModel.Definitions {
    /// <summary>
    /// A COURSE OF FIRE defines the structure of events that make up a competition. This structure includes:
    /// * Defining each event, both composite events and singular events. A composite event is made up of other events. A singular event is made up only of itself, and is almost always just a shot.
    /// * EVENT STYLE and STAGE STYLE assignment to events. 
    /// * Range officer command script.
    /// * EST Target configuration.
    /// * Mapping of shots to singular events.
    /// A COURSE OF FIRE should only describe an event that can be completed with one outing to the range. In other words, an athlete should be able to complete the course of fire with one trip to the range. A multi-day event is the combination of two or more COURSE OF FIRE, that is defined outside of this type.
    /// </summary>
    public class CourseOfFire : Definition {

        [JsonConverter(typeof(StringEnumConverter))]
        public enum COFTypeOptions { COMPETITION, FORMALPRACTICE, INFORMALPRACTICE, DRILL, GAME };

        public CourseOfFire() : base() {
            Type = Definition.DefinitionType.COURSEOFFIRE;

            RangeScripts = new List<RangeScript>();

            AbbreviatedFormats = new List<AbbreviatedFormat>();

            RequiredAttributes = new List<string>();

            Events = new List<Event>();

            Singulars = new List<Singular>();
        }


        [OnDeserialized]
        internal new void OnDeserializedMethod(StreamingContext context) {
            base.OnDeserializedMethod(context);
        }

        [JsonProperty(Order = 9)]
        [JsonConverter(typeof(StringEnumConverter))]
        public COFTypeOptions COFType { get; set; }

        /// <summary>
        /// A human readable short name.
        /// </summary>
        [JsonProperty(Order = 10)]
        public string CommonName { get; set; }

        /// <summary>
        /// A list of ATTRIBUTES that each competitor needs to have in order for the mapping to EVENT STYLE or STAGE STYLE to work. Used when a COURSE OF FIRE is defined for multiple equipment classes.
        /// </summary>
        [JsonProperty(Order = 11)]
        public List<string> RequiredAttributes { get; set; }

        /// <summary>
        /// Formatted as a SetName, the TargetCollectionDef to use to score shots. The specific TARGET definition to use is calculated with the 
        /// SegementGroupSegment.TargetSetIndex.
        /// Required.
        /// </summary>
        [JsonProperty(Order = 12)]
        public string TargetCollectionDef { get; set; }

        /// <summary>
        /// The default expected diameter of the bullet shot at the target.
        /// </summary>
        [JsonProperty(Order = 13)]
        [DefaultValue(4.5)]
        public float DefaultExpectedDiameter { get; set; }

        /// <summary>
        /// The default bullet diameter to use for scoring, measured in mm.
        /// </summary>
        [JsonProperty(Order=14)]
        [DefaultValue(4.5)]
        public float DefaultScoringDiameter { get; set; }

        /// <summary>
        /// Formatted as a SetName, the ScoreFormatCollectionDef to use to display results to athletes and spectators. 
        /// </summary>
        [JsonProperty(Order = 15)]
        public string ScoreFormatCollectionDef { get; set; }

        /// <summary>
        /// The default ScoreConfig to use, within the ScoreFormatCollection. 
        /// </summary>
        [JsonProperty(Order = 16)]
        public string ScoreConfigDefault { get; set; }

        /// <summary>
        /// Range command script with Paper Targets or EST Configuration. 
        /// </summary>
        [JsonProperty(Order = 20)]
        public List<RangeScript> RangeScripts { get; set; }

        /// <summary>
        /// A list of Events that make up this COURSE OF FIRE. These are the composite events, those that are made up of other child events.
        /// </summary>
        [JsonProperty(Order = 21)]
        public List<Event> Events { get; set; }

        /// <summary>
        /// A list of Singulars that make up this COURSE OF FIRE. These are the singular events, those that are not made up of other events. Almost always represents a singular shot.
        /// </summary>
        [JsonProperty(Order = 22)]
        public List<Singular> Singulars { get; set; }

        /// <summary>
        /// A list of AbbreviatedFormats.
        /// </summary>
        [JsonProperty(Order = 23)]
        public List<AbbreviatedFormat> AbbreviatedFormats { get; set; }

    }
}
