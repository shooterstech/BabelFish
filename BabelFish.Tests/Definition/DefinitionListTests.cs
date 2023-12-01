using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scopos.BabelFish;
using Scopos.BabelFish.Helpers;
using Scopos.BabelFish.DataModel.Definitions;
using Scopos.BabelFish.APIClients;

namespace Scopos.BabelFish.Tests.Definition {

	[TestClass]
	public class DefinitionListTests {

		[TestMethod]
		public async Task BasicGetRest() {
			var client = new DefinitionAPIClient( Constants.X_API_KEY );

			var getDefinitionListResponse = await client.GetDefinitionListPublicAsync( DefinitionType.ATTRIBUTE );

			Assert.AreEqual( HttpStatusCode.OK, getDefinitionListResponse.StatusCode );
			Assert.AreEqual( DefinitionType.ATTRIBUTE, getDefinitionListResponse.DefinitionType );

			var definitionList = getDefinitionListResponse.DefinitionList;

			Assert.IsTrue( definitionList.Items.Count > 0 );

			foreach( var item in definitionList.Items ) {
				Assert.AreEqual( DefinitionType.ATTRIBUTE, item.Type );
			}
		}

		[TestMethod]
		public async Task CachedResponseTest() {

			var client = new DefinitionAPIClient( Constants.X_API_KEY );

			var getDefinitionListResponse1 = await client.GetDefinitionListPublicAsync( DefinitionType.ATTRIBUTE );

			Assert.AreEqual( HttpStatusCode.OK, getDefinitionListResponse1.StatusCode );
			Assert.AreEqual( DefinitionType.ATTRIBUTE, getDefinitionListResponse1.DefinitionType );
			Assert.IsFalse( getDefinitionListResponse1.InMemoryCachedResponse );


			var getDefinitionListResponse2 = await client.GetDefinitionListPublicAsync( DefinitionType.ATTRIBUTE ); 

			Assert.AreEqual( HttpStatusCode.OK, getDefinitionListResponse2.StatusCode );
			Assert.AreEqual( DefinitionType.ATTRIBUTE, getDefinitionListResponse2.DefinitionType );
			Assert.IsTrue( getDefinitionListResponse2.InMemoryCachedResponse );

			//Check that it is much much faster
			Assert.IsTrue( getDefinitionListResponse1.TimeToRun > 100* getDefinitionListResponse2.TimeToRun );
		}
	}
}
