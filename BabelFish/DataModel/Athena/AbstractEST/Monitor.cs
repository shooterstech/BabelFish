using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scopos.BabelFish.DataModel.Athena.AbstractEST
{
    public class Monitor
    {

        public Monitor()
        {
            Bullet = new Bullet();
            FiringPoint = new FiringPoint();
        }

        public bool Capability { get; set; }

        /// <summary>
        /// Formatted as a SetName
        /// </summary>
        public string TargetDefinition { get; set; }

        /// <summary>
        /// Formatted as a SetName
        /// </summary>
        public CourseOfFire CourseOfFire { get; set; }

        public FiringPoint FiringPoint { get; set; }

        public Competitor Competitor { get; set; }

        public RangeTimer RangeTimer { get; set; }

        public Bullet Bullet { get; set; }

        public List<string> DisplayAddresses { get; set; }
    }
}