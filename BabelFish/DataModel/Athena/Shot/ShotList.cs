using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShootersTech.BabelFish.DataModel.Athena.Shot
{
    public class ShotList
    {

        public ShotList()
        {

            Shots = new List<Shot>();
        }

        public string MatchID { get; set; }

        public List<Shot> Shots { get; set; }
    }
}