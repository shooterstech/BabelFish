using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShootersTech.BabelFish.DataModel.Athena.Shot
{

    /// <summary>
    /// ShotUpdateRequest messages are sent to the Target on the topic {Owner}/{FP}/shot/update.
    /// </summary>
    public class ShotUpdateRequest
    {

        /// <summary>
        /// Shot is the requested updated value / properties of the shot.
        /// Unless RestoreToUpdate >= 0.
        /// </summary>
        public Shot Shot { get; set; }

        /// <summary>
        /// If >= 0, this ShotUpdateRequest means to restore the value of the shot to this specified Update version.
        /// </summary>
        [DefaultValue(-1)]
        public int RestoreToUpdate { get; set; }
    }
}