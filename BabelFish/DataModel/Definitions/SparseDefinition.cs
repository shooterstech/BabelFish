using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Scopos.BabelFish.DataModel.Definitions {
	/// <summary>
	/// Provides a description of a Definition, without the technical details. Intended for look up and searching.
	/// </summary>
	public class SparseDefinition : Definition {

		/// <summary>
		/// If the user specified a search term in the REST API call, SearchScore returns the relavenacy
		/// of this Definition compared to the search term. The higher the value the better the match.
		/// A value of -1 means no search term comparision was made.
		/// </summary>
		[DefaultValue( -1 )]
		public int SearchScore { get; set; } = -1;
	}
}
