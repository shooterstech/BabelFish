using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;
using Scopos.BabelFish.APIClients;


namespace Scopos.BabelFish.DataModel.Definitions {
    /// <summary>
    /// An event within a COURSE OF FIRE that does not have any children. Almost always refers to a single shot. 
    /// Singulars are not defined individually, but as a group.As an example, in a three position match, 
    /// one Singular object define the shots fired in kneeling, a second Singular object defines the shots 
    /// fired in prone, and a third object defines the shots fired in standing.
    /// </summary>
    public class Singular : IReconfigurableRulebookObject {

        private List<string> validationErrorList = new List<string>();

        public Singular () {

            Type = SingularType.SHOT;
            EventName = "";
            Values = new ValueSeries( "1" );
            ScoreFormat = "Shots";
            StageLabel = "";
            ShotMappingMethod = ShotMappingMethodType.SEQUENTIAL;
        }

        /// <summary>
        /// The format for the EventName. The compiled EventName must be unique within the COURSE OF FIRE. 
        /// Traditionally the EventName for Singulars is the StageLable value concatenated with the place holder '{}'.
        /// </summary>
        [JsonPropertyOrder( 1 )]
        [G_NS.JsonProperty( Order = 1 )]
        public string EventName { get; set; }

        /// <summary>
        /// The type of singular event this is. Must be one of the following:
        ///  * Shot
        ///  * Test
        /// </summary>
        [JsonPropertyOrder( 2 )]
        [G_NS.JsonProperty( Order = 2 )]
        public SingularType Type { get; set; } = SingularType.SHOT;

        /// <summary>
        /// The integer values to use to generate the EventNames dynamically. 
        /// </summary>
        /// <remarks>Default value is "1"</remarks>
        [G_NS.JsonConverter( typeof( G_BF_NS_CONV.ValueSeriesConverter ) )]
        [G_STJ_SER.JsonConverter( typeof( G_BF_STJ_CONV.ValueSeriesConverter ) )]
        [JsonPropertyOrder( 3 )]
        [G_NS.JsonProperty( Order = 3 )]
        public ValueSeries Values { get; set; } = new ValueSeries( "1" );

        /// <summary>
        /// Newtonsoft.json helper method to determine if .Values should be serialized.
        /// If .EventName contains the interpolation variable "{}" then .Values will be serialized.
        /// </summary>
        /// <returns></returns>
        public bool ShouldSerializeValues() {
            return EventName?.Contains( "{}" ) ?? false;
        }

        /// <summary>
        /// The Score Format to use when displaying the Singular event during a match. 
        /// </summary>
        [JsonPropertyOrder( 4 )]
        [G_NS.JsonProperty( Order = 4 )]
        public string ScoreFormat { get; set; } = "Shots";

        /// <summary>
        /// A unique value that is used in the mapping process of shots to events. StageLabels are assigned to shots:
        /// * In an Athena compliant EST system via the StageLabel value in Segment.
        /// * In a paper target system via the StageLabel value in a BarcodeLabel.
        /// StageLabel values are traditionally a single character.
        /// </summary>
        [JsonPropertyOrder( 5 )]
        [G_NS.JsonProperty( Order = 5 )]
        public string StageLabel { get; set; }

		/// <summary>
		/// The method to use to map shots to events. Must be one of the following values:
		/// * Sequential
		/// </summary>
        [G_NS.JsonProperty( DefaultValueHandling = Newtonsoft.Json.DefaultValueHandling.Include, Order = 6 )]
        [JsonPropertyOrder( 6 )]
        public ShotMappingMethodType ShotMappingMethod { get; set; } = ShotMappingMethodType.SEQUENTIAL;

        /// <summary>
        /// Specifies the *expected* TargetDef to use during this Segment. Specifically, this is the index into the 
        /// CourseOfFire.TargetCollectionDef.TargetCollections.TargetDefs array.
        /// <para>This value has no precise programmatic control. Instead it is used as a helper to more easily learn
        /// what type of Target this singular is being shot on. Programmatically, the SegmentGroupSegment, in the 
        /// range script controls the selected target. The definition author  is expected to compose the values
        /// so they are consistent.</para>
        /// </summary>
        [DefaultValue( 0 )]
        [JsonPropertyOrder( 7 )]
        [G_NS.JsonProperty( DefaultValueHandling = Newtonsoft.Json.DefaultValueHandling.Include, Order = 7 )]
        public int TargetCollectionIndex { get; set; } = 0;

        public async Task<SetName> GetTargetCollectionAsync( SetName courseOfFire, string targetCollectionName ) {
            var cofDefinition = await DefinitionCache.GetCourseOfFireDefinitionAsync( courseOfFire );
            var targetCollectionDefinition = await cofDefinition.GetTargetCollectionDefinitionAsync();
            var targetCollection = targetCollectionDefinition.GetTargetCollection( targetCollectionName );
            return targetCollection.TargetDefs[TargetCollectionIndex];
        }

        /// <inheritdoc/>
        [JsonPropertyOrder ( 99 )]
        [G_NS.JsonProperty( Order = 99 )]
        [DefaultValue( "" )]
        public string Comment { get; set; } = string.Empty;

        /// <summary>
        /// Generates a list of Events based on the definition of this Singular
        /// </summary>
        /// <returns></returns>
        public List<Event> GetSingularEventList() {
            List<Event> list = new List<Event>();

            for (int i = Values.StartValue; i <= Values.EndValue; i += Values.Step) {
                Event e = new EventExplicit() {
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
