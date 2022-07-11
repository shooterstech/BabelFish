using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShootersTech.BabelFish.DataModel.Athena.AbstractEST
{
    public class CameraCalibration
    {

        public bool Capability { get; set; }

        public string Directory { get; set; }

        public int BoardHeight { get; set; }

        public int BoardWidth { get; set; }

        public int NumberOfPhotos { get; set; }

        public string LastPhoto { get; set; }

        public string Error { get; set; }

        public string Event { get; set; }
    }
}