using System;
using System.Collections.Generic;
using System.Text;

namespace Scopos.BabelFish.DataModel.OrionMatch {
	public abstract class RelayInformation {

		/// <summary>
		/// Concrete class identifier.
		/// </summary>
		public SquaddingAssignmentType RelayType { get; protected set; }

		public DateTime CallToLineTime { get; set; } = DateTime.UtcNow;

		public DateTime StartTime { get; set; } = DateTime.UtcNow;

        public virtual string RelayName { get; set; }
    }

	public class RelayInformationFiringPoint : RelayInformation {

		public RelayInformationFiringPoint() {
			this.RelayType = SquaddingAssignmentType.FIRING_POINT;
		}
	}

	public class RelayInformationBank : RelayInformation {

		public RelayInformationBank() {
			this.RelayType = SquaddingAssignmentType.BANK;
		}
	}

	public class RelayInformationSquad : RelayInformation {

		public RelayInformationSquad() {
			this.RelayType = SquaddingAssignmentType.SQUAD;
		}
	}
}
