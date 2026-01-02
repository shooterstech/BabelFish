using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Scopos.BabelFish.DataModel.Athena.Shot;

namespace Scopos.BabelFish.DataModel.Athena.AbstractEST
{
    public class ShotDetection
    {

        public ShotDetection()
        {

        }

        [OnDeserialized]
        internal void OnDeserialized(StreamingContext context)
        {
            if (RecentShotUpdates == null)
                RecentShotUpdates = new List<Shot.Shot>();
        }

        public bool Capability { get; set; }

        public bool Paused { get; set; }

        [Obsolete("Replaced with RecentShotUpdates")]
        public Scopos.BabelFish.DataModel.Athena.Shot.Shot LastShotDetected { get; set; }

        /// <summary>
        /// List of the last few shots that were scored or edited from the target.
        /// Almost always a list of 1.
        /// </summary>
        public List<Scopos.BabelFish.DataModel.Athena.Shot.Shot> RecentShotUpdates { get; set; }

        /// <summary>
        /// Shot Attributes that are appended on to each Shot, in the Shot.Attributes field
        /// </summary>
        public List<string> Attributes { get; set; }

        /// <summary>
        /// The label to apply to each shot when it is detected.
        /// </summary>
        public string StageLabel { get; set; }

        /// <summary>
        /// Each shot when it is detected it is labeld with an in order numbering, known as Sequence
        /// </summary>
        public int Sequence { get; set; }

        public string Event { get; set; }

        public string Error { get; set; }

        public string Warning { get; set; }

        public bool InternalPaused { get; set; }

        public bool UserPaused { get; set; }

        public string Settings { get; set; }
    }
}