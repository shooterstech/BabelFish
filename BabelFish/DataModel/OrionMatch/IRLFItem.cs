using System;
using System.Collections.Generic;
using System.Text;

namespace Scopos.BabelFish.DataModel.OrionMatch {
	public interface IRLIFItem : ISquadding, IParticipant {

	}

	public interface IRLIFList {

		List<IRLIFItem> GetAsIRLItemsList();

		string ResultListFormatDef { get; }

		string Name { get; }

		DateTime StartDate { get; }

		DateTime EndDate { get; }

		ResultStatus Status { get; }
	}
}
