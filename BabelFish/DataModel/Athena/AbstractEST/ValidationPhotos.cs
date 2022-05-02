using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShootersTech.DataModel.Athena.AbstractEST
{
    public class ValidationPhotos
    {

        public ValidationPhotos()
        {

        }

        /// <summary>
        /// True, if this Target has the capability of taking photos of the aiming area within the target.
        /// </summary>
        public bool Capability { get; set; }

        /// <summary>
        /// True, if this Target is currently taking photos of the aiming area and saving them to disk.
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        /// The directory on the local machine where the photos will be saved. All photos are timed stamped.
        /// </summary>
        public string Directory { get; set; }

        /// <summary>
        /// How often, in seconds, a validation photo is take and saved to disk.
        /// </summary>
        public int Frequency { get; set; }

        /// <summary>
        /// The full path of the last validation photo that was taken.
        /// </summary>
        public string LastPhoto { get; set; }

        /// <summary>
        /// If the validation photo camera is in an error state (i.e. something is wrong with it), this field contains a descripiton of the problem. If it is blank, the validation photo camera is working as expected.
        /// </summary>
        public string Error { get; set; }

        /// <summary>
        /// The event within the target that caused a change to the state.
        /// </summary>
        public string Event { get; set; }
    }
}