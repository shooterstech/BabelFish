using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShootersTech.BabelFish.DataModel.Athena.AbstractEST
{
    public class StripControl
    {

        public StripControl()
        {

            //As of Dec 2020 the values for TopStrip and BottomStrip are being statically set, since there are
            //only two Display Strip entities, MatchInforStrip and Marquee
            TopStrip = new DisplayStrip() { DisplayEntityName = "MatchInfoStrip" };
            BottomStrip = new DisplayStrip() { DisplayEntityName = "Marquee" };
        }

        public DisplayStrip TopStrip { get; set; }

        public DisplayStrip BottomStrip { get; set; }
    }
}