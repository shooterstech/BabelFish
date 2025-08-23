using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scopos.BabelFish.DataModel.Athena.Range {
    public class OrionState {

        public OrionState() { }

        /// <summary>
        /// IOT Name for this Orion Instance.
        /// this property was previously stored in RangeConfiguration.NetworkManager
        /// </summary>
        [DefaultValue( "" )]
        public string OrionName { get; set; }

        /// <summary>
        /// Name (not hte full path) of the certificate file to use when connecting to GGC.
        /// This property was previously stored in RangeConfiguration.NetworkManager
        /// </summary>
        public string CertificateFile { get; set; }

        /// <summary>
        /// Password to use when accessing the CertificateFile. An empty string is the traditional value.
        /// This property was previously stored in RangeConfiguration.NetworkManager
        /// </summary>
        public string CertificatePassword { get; set; }

        /// <summary>
        /// How long to wait, in seconds, between publishing iot messages
        /// This property was previously stored in RangeConfiguration.NetworkManager
        /// </summary>
        [DefaultValue( 5 )]
        public int PublishDelay { get; set; }
    }
}
