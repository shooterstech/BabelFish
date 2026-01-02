using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Scopos.BabelFish.DataModel.Athena.AbstractEST
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


        public Marquee Marquee { get; set; }

        public ActiveViewValues ActiveViewValues { get; set; }

        /// <summary>
        /// Formatted as a SetName
        /// </summary>
        public CourseOfFire CourseOfFire { get; set; }

        public RangeTimer RangeTimer { get; set; }

        public Logging Logging { get; set; }

        public List<AssignedTarget> AssignedTargets { get; set; }
    }
}