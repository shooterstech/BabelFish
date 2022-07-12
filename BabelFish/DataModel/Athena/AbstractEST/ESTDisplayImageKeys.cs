using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShootersTech.BabelFish.DataModel.Athena.AbstractEST
{
    public class ESTDisplayImageKeys
    {

        public ESTDisplayImageKeys()
        {
            ImageKeys = new Dictionary<string, List<string>>();
        }

        public Dictionary<string, List<string>> ImageKeys { get; set; }
    }
}