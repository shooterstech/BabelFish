using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShootersTech.DataModel.Athena.Range
{
    public class Target
    {

        public Target()
        {

        }

        /// <summary>
        /// The network address (IoT thing name) of Target
        /// </summary>
        public string TargetStateAddress { get; set; }

        /// <summary>
        /// The TargetLineLables that this target can be a part of. In most cases exactly one TargetLineLabel will be listed.
        /// </summary>
        public List<string> TargetLineLabels { get; set; }

        /// <summary>
        /// Human readable name for this target.
        /// </summary>
        public string Nickname { get; set; }
    }
}