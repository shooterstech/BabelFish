﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scopos.BabelFish.DataModel.Athena.Range {
    public class NetworkManager {

        private string envStage = "production";

        public NetworkManager() {

        }

        /// <summary>
        /// IOT Name for the Orion Instance
        /// </summary>
        [Obsolete( "Property moved to RangeState.Orion.OrionName" )]
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
        /// Greengrass group IoT Message Broker address. 
        /// Can be IP or hostname
        /// </summary>
        public string IoTAddress { get; set; }

        /// <summary>
        /// Greengrass group IoT Message Broker port number. Almost always 8883.
        /// </summary>
        public int IoTPort { get; set; }

        /// <summary>
        /// The name of of the Certificate Authority file for the Greengrass Group
        /// It is just the file name, not the full path.
        /// </summary>
        public string CAFile { get; set; }

        /// <summary>
        /// The stage of development to interact with. Values are
        /// production
        /// integrated
        /// beta
        /// alpha
        /// </summary>
        public string EnvStage {
            get {
                if (!string.IsNullOrEmpty( envStage )) {
                    return envStage;
                } else {
                    return "production";
                }
            }
            set {
                switch (value) {
                    case "production":
                    case "integrated":
                    case "alpha":
                    case "beta":
                        envStage = value;
                        break;
                    default:
                        //do nothing
                        break;
                }
            }
        }

        /// <summary>
        /// The AWS Region the Greengrass Group is located in.
        /// </summary>
        [DefaultValue( "us-east-1" )] //NOTE Tried to use AmazonRegion class, however the JSON conversion never quite worked for it.
        public string Region { get; set; } = "us-east-1";

        public string SharedKey { get; set; } = string.Empty;

        [Obsolete( "Property moved to RangeState.Orion.CertificateFile" )]
        public string CertificateFile { get; set; }

        [Obsolete( "Property moved to RangeState.Orion.CertificatePassword" )]
        public string CertificatePassword { get; set; }

        [DefaultValue( 0.1 )]
        [Obsolete( "Property moved to RangeState.Orion.PublishDelay" )]
        public float PublishDelay { get; set; }
    }
}