using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BabelFish.DataModel.Definitions {

    /// <summary>
    /// A "SimpleCOF" defines the type of StageStyles that make it up and the number of 
    /// shots per StageStyle. It does not define any structure, time limites, ranking rules
    /// etc. 
    /// </summary>
    [Serializable]
    public class SimpleCOF {

        public SimpleCOF() {
            Components = new List<SimpleCOFComponent>();
        }

        public string Name { get; set; }

        public List<SimpleCOFComponent> Components { get; set; }
    }

    public class SimpleCOFComponent {
        public string StageStyle { get; set; }

        public int Shots { get; set; }

        public string ScoreFormat { get; set; }
    }
}
