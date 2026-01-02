using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scopos.BabelFish.DataModel.Athena.AbstractEST
{
    public class Bullet
    {

        public Bullet()
        {

            BulletDiameter = 4.5f;
            ScoringDiameter = 4.5f;
            ExpectedVelocity = 180f;
        }

        /// <summary>
        /// Measured in mm
        /// </summary>
        public float BulletDiameter { get; set; }

        /// <summary>
        /// Measured in mm
        /// </summary>
        public float ScoringDiameter { get; set; }

        /// <summary>
        /// Measured in m/s
        /// </summary>
        public float ExpectedVelocity { get; set; }
    }
}