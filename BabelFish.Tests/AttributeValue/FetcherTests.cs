using Microsoft.VisualBasic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using Scopos.BabelFish.DataModel.AttributeValue;
using Scopos.BabelFish.DataModel.Definitions;

namespace Scopos.BabelFish.Tests.AttributeValue {
    [TestClass]
    public class FetcherTests {

        /*
         * Because TestMethods are ran alphabetically, prefacing the method names with A, B, C to get them 
         * to run in a specific order. Namely, to test not setting x-api-key before setting it. 
         */

        /// <summary>
        /// Tests that the x api key has to be set before fetching a definition
        /// </summary>
        [TestMethod]
        [ExpectedException( typeof( XApiKeyNotSetException ) )]
        public async Task A_MissingXApiKeyTest() {

            //Without setting the x api key, an exception should be thrown.
            var setNameEmailStr = "v2.3:orion:Email Address";
            var definitionEmail = await AttributeValueDefinitionFetcher.FETCHER.FetchAttributeDefinitionAsync( setNameEmailStr );
        }

        /// <summary>
        /// Tests that the setting of the x api key works as expected.
        /// </summary>
        [TestMethod]
        public void B_XApiKeySettingTests() {

            //To start, no x api key is set
            Assert.IsFalse( AttributeValueDefinitionFetcher.FETCHER.IsXApiKeySet );

            //Set the x api key
            AttributeValueDefinitionFetcher.FETCHER.XApiKey = Constants.X_API_KEY;

            Assert.IsTrue( AttributeValueDefinitionFetcher.FETCHER.IsXApiKeySet );
            Assert.AreEqual( Constants.X_API_KEY, AttributeValueDefinitionFetcher.FETCHER.XApiKey );
        }

        [TestMethod]
        public async Task C_HappyPathFetchDefinitionTest() {

            //Set the x api key
            AttributeValueDefinitionFetcher.FETCHER.XApiKey = Constants.X_API_KEY;

            //Test using with a string as the set name.
            var setNameEmailStr = "v2.3:orion:Email Address";
            var definitionEmail = await AttributeValueDefinitionFetcher.FETCHER.FetchAttributeDefinitionAsync( setNameEmailStr );
            Assert.IsNotNull( definitionEmail );
            Assert.AreEqual( setNameEmailStr, definitionEmail.GetSetName().ToString() );

            //Test using with a string as the set name.
            var setNamePhoneStr = "v2.1:orion:Phone Number";
            var setNamePhone = SetName.Parse( setNamePhoneStr );
            var definitionPhone = await AttributeValueDefinitionFetcher.FETCHER.FetchAttributeDefinitionAsync( setNamePhone );
            Assert.IsNotNull( definitionPhone );
            Assert.AreEqual( setNamePhoneStr, definitionPhone.GetSetName().ToString() );
        }
    }
}
