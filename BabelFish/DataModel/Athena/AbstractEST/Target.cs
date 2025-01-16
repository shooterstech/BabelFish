using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scopos.BabelFish.DataModel.Athena.AbstractEST
{
    public class Target
    {

        public Target()
        {

            FiringPoint = new FiringPoint();
            Competitor = new Competitor();
            Bullet = new Bullet();
        }

        public bool Capability { get; set; }

        public string TargetDefinition { get; set; }

        public CourseOfFire CourseOfFire { get; set; }

        /// <summary>
        /// The firing point information for this target.
        /// </summary>
        public FiringPoint FiringPoint { get; set; }

        /// <summary>
        /// Information on the competitor who is shooting at the target
        /// </summary>
        public Competitor Competitor { get; set; }

        public RangeTimer RangeTimer { get; set; }

        /// <summary>
        /// The type of bullet that is being fired at the target.
        /// </summary>
        public Bullet Bullet { get; set; }

        public ShotSimulation ShotSimulation { get; set; }
    }
}