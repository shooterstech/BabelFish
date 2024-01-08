using System;
using System.Collections.Generic;
using System.Text;

namespace Scopos.BabelFish.DataModel.Definitions {

	/// <summary>
	/// Classes that reference StageStyle Definitions should implement this interface.
	/// </summary>
	public interface IGetStageStyleDefinition {

		/// <summary>
		/// Retreives the StageStyle Definition referenced by the instantiating class.
		/// </summary>
		/// <returns></returns>
		Task<StageStyle> GetStageStyleDefinitionAsync();
	}
}
