using System;
using System.Collections.Generic;
using System.Text;

namespace Scopos.BabelFish.DataModel.OrionMatch {
	public interface IRLIFItem : ISquadding, IParticipant {

	}

	public interface IRLIFList {

		List<IRLIFItem> GetAsIRLItemsList();
	}
}
