using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Scopos.BabelFish.DataModel.OrionMatch {
    [Serializable]
    public class SquaddingEvent {

        public SquaddingEvent() {
        }

		/// <summary>
		/// A unique name giving to this Squadding Event. Usually (not always) cooresponds to an Event Name.
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// A Human readable description of this Squadding Event.
		/// </summary>
		public string Description { get; set; }
	}
}
