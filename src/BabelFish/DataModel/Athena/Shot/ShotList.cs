using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Scopos.BabelFish.DataModel.Athena.Shot
{
    /// <summary>
    /// A ShotList is sent by an ESTTarget in response to a Request Shots command.
    /// </summary>
    public class ShotList
    {

        public ShotList()
        {

            Shots = new List<Shot>();
        }

        [OnDeserialized]
        internal void OnDeserializedMethod(StreamingContext context)
        {
            if (Shots == null)
                Shots = new List<Shot>();
        }

        /// <summary>
        /// A ShotList object should always be limited to at least a specific Match ID.
        /// </summary>
        [DefaultValue("")]
        public string MatchID { get; set; }

        /// <summary>
        /// A ShtoList object may additionally be limted to a specific ResultCOFID. If an empty string or missing, then the ShotList is for the entire match.
        /// </summary>
        [DefaultValue("")]
        public string ResultCOFID { get; set; }

        /// <summary>
        /// The thing name (target's state adddress) that sent this ShotList
        /// </summary>
        [DefaultValue("")]
        public string TargetName { get; set; }

        /// <summary>
        /// The list of Shots sent by the Target.
        /// May not be the complete list. Check NextToken if the target has more.
        /// </summary>
        public List<Shot> Shots { get; set; }

        /// <summary>
        /// If after receiving a ShotList from a target, there are more shots to send, NextToken will be set. 
        /// A null value or empty string means all shots have been sent. 
        /// </summary>
        [DefaultValue("")]
        public string NextToken { get; set; }
    }
}