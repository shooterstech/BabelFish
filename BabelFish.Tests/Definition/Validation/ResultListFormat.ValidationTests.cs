using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Scopos.BabelFish;
using Scopos.BabelFish.Helpers;
using Scopos.BabelFish.DataModel.Definitions;
using Scopos.BabelFish.APIClients;
using Scopos.BabelFish.Requests.DefinitionAPI;
using Scopos.BabelFish.Responses.DefinitionAPI;
using System.Diagnostics;
using Scopos.BabelFish.DataActors.Specification.Definitions;

namespace Scopos.BabelFish.Tests.Definition.Validation {
	[TestClass]
	public class ResultListFormatValidationTests {

		[TestInitialize]
		public void InitializeTest() {
			Scopos.BabelFish.Runtime.Settings.XApiKey = Constants.X_API_KEY;
		}

		[TestMethod]
		public async Task HappyPathAttributeValid() {

			var client = new DefinitionAPIClient();
			var setName = SetName.Parse( "v1.0:orion:3P Qualification" );

			var candidate = (await client.GetResultListFormatDefinitionAsync( setName )).Value;

			var validation = new IsResultListFormatValid();

			var valid = await validation.IsSatisfiedByAsync( candidate );

			Assert.IsTrue( valid, string.Join( ", ", validation.Messages ) );
		}
	}
}
