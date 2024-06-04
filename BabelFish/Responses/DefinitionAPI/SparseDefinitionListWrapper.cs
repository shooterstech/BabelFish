using System;
using System.Collections.Generic;
using System.Text;
using Scopos.BabelFish.DataModel;
using Scopos.BabelFish.DataModel.Definitions;

namespace Scopos.BabelFish.Responses.DefinitionAPI {
	public class SparseDefinitionListWrapper : BaseClass{

		public SparseDefinitionList DefinitionList { get; set; }
	}
}
