using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scopos.BabelFish.DataModel.Definitions;
using Scopos.BabelFish.APIClients;
using Scopos.BabelFish.DataActors.Specification.Definitions;

namespace Scopos.BabelFish.Tests.Definition.Validation {
	[TestClass]
	public class TargetValidationTests {

		[TestInitialize]
		public void InitializeTest() {
			Scopos.BabelFish.Runtime.Settings.XApiKey = Constants.X_API_KEY;
		}

		[TestMethod]
		public async Task HappyPathAttributeValid() {

			var client = new DefinitionAPIClient();
			var setName = SetName.Parse( "v1.0:issf:10m Air Rifle" );

			var candidate = (await client.GetTargetDefinitionAsync( setName )).Value;

			var validation = new IsTargetValid();

			var valid = await validation.IsSatisfiedByAsync( candidate );

			Assert.IsTrue( valid, string.Join( ", ", validation.Messages ) );
		}
	}
}
