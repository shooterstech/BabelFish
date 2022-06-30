using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ShootersTech.DataModel.Definitions {
    /// <summary>
    /// An event within a COURSE OF FIRE that does not have any children. Almost always refers to a single shot. 
    /// Singulars are not defined individually, but as a group.As an example, in a three position match, 
    /// one Singular object define the shots fired in kneeling, a second Singular object defines the shots 
    /// fired in prone, and a third object defines the shots fired in standing.
    /// </summary>
    public class Singular {

        [JsonConverter(typeof(StringEnumConverter))]
        public enum ShotMappingMethodType { SEQUENTIAL }

        private List<string> validationErrorList = new List<string>();

        public Singular () {

            Type = "Shot";
            EventName = "";
            Values = "";
            ScoreFormat = "d";
            StageLabel = "";
            ShotMappingMethod = ShotMappingMethodType.SEQUENTIAL;
        }

        /// <summary>
        /// The type of singular event this is. Must be one of the following:
        ///  * Shot
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// The format for the EventName. The compiled EventName must be unique within the COURSE OF FIRE. 
        /// Traditionally the EventName for Singulars is the StageLable value concatenated with the place holder '{}'.
        /// </summary>
        public string EventName { get; set; }

        /// <summary>
        /// The integer values to use to generate the EventNames dynamically. Must be formatted as a Values Series
        /// </summary>
        public string Values { get; set; }

        /// <summary>
        /// The Score Format to use when displaying the Singular event during a match. 
        /// </summary>
        public string ScoreFormat { get; set; }

        /// <summary>
        /// A unique value that is used in the mapping process of shots to events. StageLabels are assigned to shots:
        /// * In an Athena compliant EST system via the StageLabel value in Segment.
        /// * In a paper target system via the StageLabel value in a BarcodeLabel.
        /// StageLabel values are traditionally a single character.
        /// </summary>
        public string StageLabel { get; set; }

        /// <summary>
        /// The method to use to map shots to events. Must be one of the following values:
        /// * Sequential
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public ShotMappingMethodType ShotMappingMethod { get; set; }

        /// <summary>
        /// Generates a list of Events based on the definition of this Singular
        /// </summary>
        /// <returns></returns>
        public List<Event> GetSingularEventList() {
            List<Event> list = new List<Event>();

            ValueSeries vs = new ValueSeries(Values);

            for (int i = vs.StartValue; i <= vs.EndValue; i += vs.Step) {
                Event e = new Event() {
                    EventName = EventName.Replace("{}", i.ToString()),
                    ScoreFormat = ScoreFormat,
                    Children = new List<string>()
                };
                list.Add(e);
            }            

            return list;
        }
    }
}
