using System;
using System.Collections.Generic;
using System.Text;

namespace Scopos.BabelFish.DataModel.OrionMatch {
	public abstract class Relay {

		/// <summary>
		/// Concrete class identifier.
		/// </summary>
		public SquaddingAssignmentType RelayType { get; protected set; }

		public DateTime CallToLineTime { get; set; }

		public DateTime StartTime { get; set; }
	}

	public class RelayFiringPoint : Relay {

		public RelayFiringPoint() {
			this.RelayType = SquaddingAssignmentType.FIRING_POINT;
		}

		public string RelayName { get; set; }
	}

	public class RelayBank : Relay {

		public RelayBank() {
			this.RelayType = SquaddingAssignmentType.BANK;
		}

		public string RelayName { get; set; }
	}

	public class RelaySquad : Relay {

		public RelaySquad() {
			this.RelayType = SquaddingAssignmentType.SQUAD;
		}

		public string RelayName { get; set; }
	}
}
