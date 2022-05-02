using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShootersTech.DataModel.Athena.Range
{
    public class NetworkManager
    {

        public NetworkManager()
        {

        }

        /// <summary>
        /// IOT Name for the Orion Instance
        /// </summary>
        public string OrionName { get; set; }

        /// <summary>
        /// The Green grass group name
        /// </summary>
        public string GGGName { get; set; }

        /// <summary>
        /// IoT Green Grass Core state address.
        /// </summary>
        public string CoreStateAddress { get; set; }

        /// <summary>
        /// Human readable name for the Greengrass Core
        /// </summary>
        public string Nickname { get; set; }

        /// <summary>
        /// IoT Message Broker address. 
        /// Can be IP or hostname
        /// </summary>
        public string IoTAddress { get; set; }

        public int IoTPort { get; set; }

        /// <summary>
        /// The name of of the Certificate Authority file.
        /// It is just the file name, not the full path.
        /// </summary>
        public string CAFile { get; set; }

        public string CertificateFile { get; set; }

        public string CertificatePassword { get; set; }

        [DefaultValue(0.1)]
        public float PublishDelay { get; set; }
    }
}