using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scopos.BabelFish.DataModel;

namespace Scopos.BabelFish.DataModel.OrionMatch {
    public class Squadding : IRLFItem {

        public Squadding() { }

		public Participant Participant { get; set; }

		public SquaddingAssignment SquaddingAssignment { get; set; }

		public override string ToString() {
            return $"Squadding for {Participant.DisplayName}: {SquaddingAssignment}";
        }
    }
}
