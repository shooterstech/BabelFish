using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShootersTech.BabelFish.DataModel.Athena.Ping
{
    public class ESTStatus
    {

        public ESTStatus() { }

        public string ThingName { get; set; }

        public string CPUSerialNumber { get; set; }

        public string CPUBoard { get; set; }

        public float CPUTemperature { get; set; }

        public string Model { get; set; }

        public string Ping { get; set; }

        public bool ClockSyncronized { get; set; }

        public string IPAddress { get; set; }

        public float WiFiQuality { get; set; }

        public string Version { get; set; }

        public string FiringPoint { get; set; }

        public string Warning { get; set; }


    }
}