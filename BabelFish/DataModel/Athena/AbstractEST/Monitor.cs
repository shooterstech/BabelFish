using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ShootersTech.DataModel.Athena.AbstractEST
{
    public class Monitor
    {

        public Monitor()
        {
            Bullet = new Bullet();
            FiringPoint = new FiringPoint();
        }

        [JsonProperty(Order = 1)]
        public bool Capability { get; set; }

        /// <summary>
        /// Formatted as a SetName
        /// </summary>
        [JsonProperty(Order = 2)]
        public string TargetDefinition { get; set; }

        /// <summary>
        /// Formatted as a SetName
        /// </summary>
        [JsonProperty(Order = 3)]
        public CourseOfFire CourseOfFire { get; set; }

        [JsonProperty(Order = 4)]
        public FiringPoint FiringPoint { get; set; }

        [JsonProperty(Order = 5)]
        public Competitor Competitor { get; set; }

        [JsonProperty(Order = 6)]
        public RangeTimer RangeTimer { get; set; }

        [JsonProperty(Order = 7)]
        public Bullet Bullet { get; set; }

        [JsonProperty(Order = 8)]
        public List<string> DisplayAddresses { get; set; }
    }
}