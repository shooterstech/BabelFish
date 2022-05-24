using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ShootersTech.DataModel.Athena.AbstractEST
{
    public class Target
    {

        public Target()
        {

            FiringPoint = new FiringPoint();
            Competitor = new Competitor();
            Bullet = new Bullet();
        }

        [JsonProperty(Order = 1)]
        public bool Capability { get; set; }

        [JsonProperty(Order = 2)]
        public string TargetDefinition { get; set; }

        [JsonProperty(Order = 3)]
        public CourseOfFire CourseOfFire { get; set; }

        /// <summary>
        /// The firing point information for this target.
        /// </summary>
        [JsonProperty(Order = 4)]
        public FiringPoint FiringPoint { get; set; }

        /// <summary>
        /// Information on the competitor who is shooting at the target
        /// </summary>
        [JsonProperty(Order = 5)]
        public Competitor Competitor { get; set; }

        [JsonProperty(Order = 6)]
        public RangeTimer RangeTimer { get; set; }

        /// <summary>
        /// The type of bullet that is being fired at the target.
        /// </summary>
        [JsonProperty(Order = 7)]
        public Bullet Bullet { get; set; }

        [JsonProperty(Order = 8)]
        public ShotSimulation ShotSimulation { get; set; }
    }
}