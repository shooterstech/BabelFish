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

        [TestInitialize]
        public void InitializeTest() {
            Scopos.BabelFish.Runtime.Settings.XApiKey = Constants.X_API_KEY;
        }

        [TestMethod]
		public async Task BasicGetRest() {
			var client = new DefinitionAPIClient();

			var getDefinitionListResponse = await client.GetDefinitionListPublicAsync( DefinitionType.ATTRIBUTE );

			Assert.AreEqual( HttpStatusCode.OK, getDefinitionListResponse.StatusCode );
			Assert.AreEqual( DefinitionType.ATTRIBUTE, getDefinitionListResponse.DefinitionType );

			var definitionList = getDefinitionListResponse.DefinitionList;

			Assert.IsTrue( definitionList.Items.Count > 0 );

			foreach( var item in definitionList.Items ) {
				Assert.AreEqual( DefinitionType.ATTRIBUTE, item.Type );
				//No search term, so score shold be -1
				Assert.IsTrue( item.SearchScore == -1 ); 
			}
		}

		[TestMethod]
		public async Task CachedResponseTest() {

			var client = new DefinitionAPIClient();

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

		[TestMethod]
		public async Task SearchTermTest() {


			var client = new DefinitionAPIClient( APIStage.PRODTEST );

			var getDefinitionListResponse = await client.GetDefinitionListPublicAsync( DefinitionType.ATTRIBUTE, "Air Rifle Category" );

			Assert.AreEqual( HttpStatusCode.OK, getDefinitionListResponse.StatusCode );
			Assert.AreEqual( DefinitionType.ATTRIBUTE, getDefinitionListResponse.DefinitionType );
			//Caching should always be disabled when doing a search term.
			Assert.IsFalse( getDefinitionListResponse.InMemoryCachedResponse );

			var definitionList = getDefinitionListResponse.DefinitionList;

			//NOTE: The count could be zero, if the serach term didn't match anything
			Assert.IsTrue( definitionList.Items.Count >= 0 );
			//Should always be 20 or less
			Assert.IsTrue( definitionList.Items.Count <= 20 );

			foreach (var item in definitionList.Items) {
				Assert.AreEqual( DefinitionType.ATTRIBUTE, item.Type );
				Assert.IsTrue( item.SearchScore >= 0 );
			}
		}
	}
}
