using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scopos.BabelFish.DataModel.Athena.AbstractEST
{
    /// <summary>
    /// Represents that data is part of all EST Units, and is part of the state of ESTMonitor, ESTTarget, and ESTDisplay
    /// </summary>
    public class ESTUnit
    {

        public ESTUnit()
        {
            Version = "1.0.0";
        }
        public string Name { get; set; }

        /// <summary>
        /// IP Address of the EST Unit.
        /// If hte value is 127.0.0.1, it means the device could not learn it's own IP.
        /// </summary>
        public string IP { get; set; }

        public string RootCAPath { get; set; }

        public string CertificatePath { get; set; }

        public string PrivateKeyPath { get; set; }

        public string GreenGrassGroup { get; set; }

        /// <summary>
        /// The Orion Account that owns this EST Unit, formatted as OrionAcct000001
        /// </summary>
        public string Owner { get; set; }

        /// <summary>
        /// The CPU and Board model. Likely a version of a Raspberry Pi
        /// </summary>
        public string CPUModel { get; set; }

        /// <summary>
        /// The EST Unit device model. For example NC100, DoW100, MM100
        /// </summary>
        public string Model { get; set; }

        public string IoTHost { get; set; }

        public int IoTPort { get; set; }

        public string Qos { get; set; }

        public bool Simulation { get; set; }

        /// <summary>
        /// The software version the EST Unit is running.
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// Reported CPU Temperature
        /// </summary>
        [DefaultValue(0)]
        public float Temperature { get; set; }

        /// <summary>
        /// The WiFi Link quality as reported by the OS.
        /// From the command iwconfig wlan0 | grep -i quality
        /// </summary>
        [DefaultValue(-1)]
        public float WiFiQuality { get; set; }

        /// <summary>
        /// The CPU's Serial Number
        /// </summary>
        [DefaultValue("")]
        public string Serial { get; set; }

		/// <summary>
		/// True if the CPU is reporting low voltage.
		/// </summary>
		[G_STJ_SER.JsonConverter( typeof( G_BF_STJ_CONV.BooleanConverter ) )]
		[DefaultValue(false)]
        public bool LowVoltage { get; set; }

		/// <summary>
		/// True if the Unit was shutdown nicely last time.
		/// </summary>
		[G_STJ_SER.JsonConverter( typeof( G_BF_STJ_CONV.BooleanConverter ) )]
		[DefaultValue(true)]
        public bool PreviousShutdown { get; set; } = true;

		[G_STJ_SER.JsonConverter( typeof( G_BF_STJ_CONV.BooleanConverter ) )]
		[DefaultValue( false )]
        public bool InternetConnection { get; set; }

		[G_STJ_SER.JsonConverter( typeof( G_BF_STJ_CONV.BooleanConverter ) )]
		[DefaultValue( false )]
        public bool IoTConnection { get; set; }
    }
}