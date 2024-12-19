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
	public class CourseOfFireValidationTests {

		[TestInitialize]
		public void InitializeTest() {
			Scopos.BabelFish.Runtime.Settings.XApiKey = Constants.X_API_KEY;
		}

		[TestMethod]
		public async Task HappyPathCourseOfFireValid() {

			var client = new DefinitionAPIClient();
			var setName = SetName.Parse( "v1.0:cmp:Smallbore Rifle 3x20" );

			var candidate = (await client.GetCourseOfFireDefinitionAsync( setName )).Value;

			var validation = new IsCourseOfFireValid();

			var valid = await validation.IsSatisfiedByAsync( candidate );

			Assert.IsTrue( valid, string.Join( ", ", validation.Messages ) );
		}
	}
}
