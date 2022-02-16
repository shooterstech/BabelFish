using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace BabelFish.DataModel.Definitions {
    [Serializable]
    public class DisplayScoreFormat {

        List<string> errorList = new List<string>();

        public DisplayScoreFormat() {

        }

        public string Name { get; set; }

        public string ScoreFormat { get; set; }

        public float MaxShotValue { get; set; }

    }
}
