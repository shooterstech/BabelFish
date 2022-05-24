using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ShootersTech.DataModel.Athena.AbstractEST
{
    public class MarqueeMessage
    {

        public MarqueeMessage()
        {
            Message = "";
            Duration = 1;
        }

        public MarqueeMessage(MarqueeMessage mm)
        {
            this.Message = mm.Message;
            this.Duration = mm.Duration;
        }

        public string Message { get; set; }


        [DefaultValue(1)]
        public int Duration { get; set; }
    }
}