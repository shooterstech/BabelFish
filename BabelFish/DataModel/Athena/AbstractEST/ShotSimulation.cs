using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShootersTech.DataModel.Athena.AbstractEST
{
    public class ShotSimulation
    {

        public ShotSimulation()
        {

            Enabled = false;
            Frequency = 60;
            StandardDeviation = 10;
            FrameHitPercent = 0;
        }

        public bool Enabled { get; set; }

        public int Frequency { get; set; }

        public float StandardDeviation { get; set; }

        public float FrameHitPercent { get; set; }
    }
}