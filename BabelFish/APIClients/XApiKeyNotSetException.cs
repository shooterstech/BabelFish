using System;
using System.Collections.Generic;
using System.Text;
using Scopos.BabelFish.Runtime;

namespace Scopos.BabelFish.APIClients {


	/// <summary>
	/// Thrown when a user tries to instantiate a AttributeValue but the x-api-key in the 
	/// DefinitionFetcher is not yet set.
	/// </summary>
	public class XApiKeyNotSetException : ScoposAPIException {

		public XApiKeyNotSetException()
			: base( "X Api Key on the DefinitionFetcher is not set. Can not read any Definition." ) {
		}
	}
}
