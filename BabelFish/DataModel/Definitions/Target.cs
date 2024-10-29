using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Scopos.BabelFish.DataModel.Definitions {

    /// <summary>
    /// A TARGET defines the appearance and scoring ring dimensions of a physical and scoring target.
    /// </summary>
    [Serializable]
    public class Target : Definition, ICopy<Target> {

        /// <summary>
        /// Public constructor
        /// </summary>
        public Target() : base() {
            Type = DefinitionType.TARGET;
        }

        /// <inheritdoc />
        public Target Copy() {
            Target copy = new Target();

            base.Copy(copy);

            copy.InnerTen = this.InnerTen.Copy();
            copy.BackgroundColor = this.BackgroundColor;
            copy.Distance = this.Distance;
            foreach( var sr in this.ScoringRings) 
                copy.ScoringRings.Add(sr.Copy());
            foreach (var am in this.AimingMarks)
                copy.AimingMarks.Add( am.Copy() );

            return copy;
        }

        [OnDeserialized]
        internal new void OnDeserializedMethod(StreamingContext context) {
            base.OnDeserializedMethod(context);
        }

        /// <summary>
        /// An ordered list, highest to lowest, of the scoring rings on the target.
        /// </summary>
        [JsonProperty(Order = 10)]
        public List<ScoringRing> ScoringRings { get; set; } = new List<ScoringRing>();

        /// <summary>
        /// Definition of the inner ten ring (sometimes called 'X's or "Center Tens").
        /// </summary>
        [JsonProperty(Order = 11)]
        public ScoringRing InnerTen { get; set; } = new ScoringRing();

        /// <summary>
        /// An ordered list, largest to smallest, of the aiming marks on the physical target.
        /// </summary>
        [JsonProperty(Order = 12)]
        public List<AimingMark> AimingMarks { get; set; } = new List<AimingMark>();

        /// <summary>
        /// The background color of the target.
        /// </summary>
        [JsonConverter( typeof( StringEnumConverter ) )]
        [JsonProperty(Order = 13)]
        public AimingMarkColor BackgroundColor { get; set; } = AimingMarkColor.WHITE;

        /// <summary>
        /// The expected distance that this target is shot at. Measured in mm.
        /// </summary>
        [JsonProperty( Order = 14 )]
        [DefaultValue( 10000 )]
        public int Distance { get; set; } = 10000;
    }
}
