using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShootersTech.DataModel.Athena.AbstractEST
{
    public class FiringPoint
    {

        public FiringPoint()
        {

        }

        /// <summary>
        /// A unique identifier within the ESTUnit.Owner for this range. Usually represented as a single character, 'A', 'B', 'C', etc.
        /// </summary>
        public string RangeID { get; set; }

        /// <summary>
        /// Human readable name given to this range.
        /// </summary>
        public string RangeName { get; set; }

        /// <summary>
        /// When firing points are grouped together, for example in ISSF 25m pistol, this is the group identifier. Usually represented as a single character, 'A', 'B', 'C', etc.
        /// </summary>
        public string Group { get; set; }

        /// <summary>
        /// The unique Firing Point number given to this firing point. Usually represented as a number, but may also be a charcter or a string. 
        /// </summary>
        public string FiringPointNumber { get; set; }

        /// <summary>
        /// The IOT ThingName and address for the ESTTarget assigned to this firing point
        /// </summary>
        public string TargetStateAddress { get; set; }

        /// <summary>
        /// The IOT ThingName and address for the ESTMonitor assigned to this firing point
        /// </summary>
        public string MonitorStateAddress { get; set; }

        /// <summary>
        /// The actual distance and unit of measure between the target and firing line. For example "10m" or "50ft".
        /// </summary>
        public string RangeDistance { get; set; }
    }
}