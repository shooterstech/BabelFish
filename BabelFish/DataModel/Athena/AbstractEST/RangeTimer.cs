using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShootersTech.BabelFish.DataModel.Athena.AbstractEST
{
    public class RangeTimer
    {

        public RangeTimer()
        {

        }

        public string StartTime { get; set; }

        public string StopTime { get; set; }

        public string PausedTime { get; set; }

        public float TimerLength { get; set; }

        public float PausedLength { get; set; }

        public bool CountDown { get; set; }

        public string TimerState { get; set; }
    }
}