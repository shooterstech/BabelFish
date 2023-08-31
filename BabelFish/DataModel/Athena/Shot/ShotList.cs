using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scopos.BabelFish.DataModel.Athena.Shot
{
	[Serializable]
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