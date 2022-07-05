using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ShootersTech.DataModel.Athena.AbstractEST
{
    public class ESTDisplay : AbstractEST
    {

        public ESTDisplay()
        {
            Marquee = new Marquee();
            ActiveViewValues = new ActiveViewValues();
        }

        [OnDeserialized]
        internal void OnDeserialized(StreamingContext context)
        {
            //Providate default values if they were not read during deserialization

            if (Marquee == null)
            {
                Marquee = new Marquee();
            }
        }


        [JsonProperty( Order = 2 )]
        public Marquee Marquee { get; set; }

        public ActiveViewValues ActiveViewValues { get; set; }

        /// <summary>
        /// Formatted as a SetName
        /// </summary>
        [JsonProperty( Order = 8 )]
        public CourseOfFire CourseOfFire { get; set; }

        [JsonProperty( Order = 9 )]
        public RangeTimer RangeTimer { get; set; }

        [JsonProperty( Order = 10 )]
        public Logging Logging { get; set; }

        [JsonProperty( Order = 11 )]
        public List<AssignedTarget> AssignedTargets { get; set; }
    }
}