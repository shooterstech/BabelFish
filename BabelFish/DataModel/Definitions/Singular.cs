using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Scopos.BabelFish.DataModel.Definitions {
    /// <summary>
    /// An event within a COURSE OF FIRE that does not have any children. Almost always refers to a single shot. 
    /// Singulars are not defined individually, but as a group.As an example, in a three position match, 
    /// one Singular object define the shots fired in kneeling, a second Singular object defines the shots 
    /// fired in prone, and a third object defines the shots fired in standing.
    /// </summary>
    public class Singular : IReconfigurableRulebookObject, ICopy<Singular> {

        private List<string> validationErrorList = new List<string>();

        public Singular () {

            Type = SingularType.SHOT;
            EventName = "";
            Values = "";
            ScoreFormat = "d";
            StageLabel = "";
            ShotMappingMethod = ShotMappingMethodType.SEQUENTIAL;
        }

        /// <inheritdoc/>
        public Singular Copy() {
            Singular s = new Singular();
            s.Type = this.Type;
            s.EventName = this.EventName;
            s.Values = this.Values;
            s.ScoreFormat = this.ScoreFormat;
            s.StageLabel = this.StageLabel;
            s.ShotMappingMethod = this.ShotMappingMethod;
            s.Comment = this.Comment;

            return s;
        }

        /// <summary>
        /// The type of singular event this is. Must be one of the following:
        ///  * Shot
        ///  * Test
        /// </summary>
        public SingularType Type { get; set; } = SingularType.SHOT;

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
        public string ScoreFormat { get; set; } = "Shots";

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
        [JsonConverter( typeof( StringEnumConverter ) )]
        public ShotMappingMethodType ShotMappingMethod { get; set; } = ShotMappingMethodType.SEQUENTIAL;

        /// <inheritdoc/>
        [JsonProperty( Order = 99, DefaultValueHandling = DefaultValueHandling.Ignore )]
        [DefaultValue( "" )]
        public string Comment { get; set; } = string.Empty;

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
