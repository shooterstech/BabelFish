using System;
using System.Collections.Generic;
using System.Text;
using Scopos.BabelFish.Runtime;

namespace Scopos.BabelFish.Runtime {


	/// <summary>
	/// Thrown when a user tries to instantiate a API Client and the X API Key has not been set.
	/// </summary>
	public class XApiKeyNotSetException : ScoposException {

		public XApiKeyNotSetException()
			: base( "The X Api Key has not been set. The value may be set in Scopos.BabelFish.Runtime.Settings" ) {
		}
	}
}
