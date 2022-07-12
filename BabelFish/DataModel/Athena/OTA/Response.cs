using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShootersTech.BabelFish.DataModel.Athena.OTA
{
    /// <summary>
    /// The OTA.Resonse class is used for two different Orion/Atehan IoT protocols. 
    /// OTA (which Orion sends Network Manger a signal to update the EST Units)
    /// GGGUpdate (which Orion sends IoT Cloud a signal to update the Network Manager)
    /// </summary>
    public class Response
    {

        public string Message { get; set; }
    }
}