using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scopos.BabelFish.DataModel.Athena.Range
{
    public class NetworkManagerState
    {

        public NetworkManagerState()
        {

        }

        [DefaultValue("")]
        public string Password { get; set; }

        [DefaultValue("")]
        public string Username { get; set; }
    }
}