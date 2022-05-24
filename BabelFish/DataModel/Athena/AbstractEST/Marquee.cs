using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ShootersTech.DataModel.Athena.AbstractEST
{
    public class Marquee
    {

        public Marquee()
        {

            CurrentMessage = "";
            OverrideEnabled = false;
            OverrideDuration = -1;
            OverrideMessages = new List<MarqueeMessage>();
        }

        public string CurrentMessage { get; set; }

        public bool OverrideEnabled { get; set; }
        public int OverrideDuration { get; set; }

        public List<MarqueeMessage> OverrideMessages { get; set; }
    }
}