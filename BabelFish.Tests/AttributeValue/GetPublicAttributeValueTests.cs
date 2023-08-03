using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scopos.BabelFish.APIClients;
using Scopos.BabelFish.DataModel.AttributeValue;
using Scopos.BabelFish.DataModel.Definitions;
using Scopos.BabelFish.Requests.AttributeValueAPI;
using Scopos.BabelFish.Runtime.Authentication;

namespace Scopos.BabelFish.Tests.AttributeValue {
    [TestClass]
    public class GetPublicAttributeValueTests {

        /// <summary>
        /// Tests the retreival of a single Attribute Value
        /// </summary>
        [TestMethod]
        public async Task GetAttributeValue_SingleValue() {

            var client = new AttributeValueAPIClient( Constants.X_API_KEY, APIStage.PRODUCTION );
            AttributeValueDefinitionFetcher.FETCHER.XApiKey = Constants.X_API_KEY;


            var setNameProfileName = "v1.0:orion:Air Rifle Training Category";
            List<string> myAttributes = new List<string>()
            {
               setNameProfileName
            };
            var taskResponse = client.GetAttributeValuePublicAsync( myAttributes, Constants.TestDev7UserId );
            var response = taskResponse.Result;
            Assert.AreEqual( System.Net.HttpStatusCode.OK, response.StatusCode );

            //The returned data should have one AttriuteValueDataPacket if the Profile Name set name.
            var attributeValueDataPackets = response.AttributeValues;
            Assert.IsTrue( attributeValueDataPackets.Count == 1 );
            Assert.IsTrue( attributeValueDataPackets.ContainsKey( setNameProfileName  ) );

            //Retreive the data packet and check that the status code is success.
            var profileNameAttributeValueDataPacket = attributeValueDataPackets[setNameProfileName];
            Assert.IsTrue( profileNameAttributeValueDataPacket.StatusCode== System.Net.HttpStatusCode.OK );

            //Pull the attribute value back, and check we have reasonable values.
            var profileNameAttributeValue = profileNameAttributeValueDataPacket.AttributeValue;

            Assert.AreEqual( "SH2", (string) profileNameAttributeValue.GetFieldValue( "Rifle Type and Category" ) );
        }

        /// <summary>
        /// Tests the retreival of multiple attributes
        /// </summary>
            [TestMethod]
        public async Task GetAttributeValue_MultipleValue() {

            var client = new AttributeValueAPIClient( Constants.X_API_KEY, APIStage.BETA );
            AttributeValueDefinitionFetcher.FETCHER.XApiKey = Constants.X_API_KEY;

            var userAuthentication = new UserAuthentication(
                Constants.TestDev7Credentials.Username,
                Constants.TestDev7Credentials.Password );
            await userAuthentication.InitializeAsync();

            var setNameProfileName = "v1.0:orion:Profile Name";
            var setNameDOB = "v1.0:orion:Date of Birth";
            var setNameEmail = "v2.0:orion:Email Address";

            List<SetName> myAttributes = new List<SetName>()
            {
               SetName.Parse( setNameProfileName ),
               SetName.Parse( setNameDOB ),
               SetName.Parse( setNameEmail )
            };

            //Will use a GetAttributeValueAuthenticatedRequest objectin this unit test, so I can set ReturnDefaultvalues to true (it is by default false).
            var request = new GetAttributeValueAuthenticatedRequest( userAuthentication ) { 
                AttributeNames = myAttributes,
                ReturnDefaultValues = true
            };

            var taskResponse = client.GetAttributeValueAuthenticatedAsync( request );
            var response = taskResponse.Result;
            //This is the overall status code for the call
            Assert.AreEqual( System.Net.HttpStatusCode.OK, response.StatusCode );

            //The returned data should have three AttriuteValueDataPacket
            var attributeValueDataPackets = response.AttributeValues;
            Assert.IsTrue( attributeValueDataPackets.Count == 3 );
            Assert.IsTrue( attributeValueDataPackets.ContainsKey( setNameProfileName ) );
            Assert.IsTrue( attributeValueDataPackets.ContainsKey( setNameDOB ) );
            Assert.IsTrue( attributeValueDataPackets.ContainsKey( setNameEmail ) );

            //Retreive the three data packet and check that the status code is success.
            var profileNameAttributeValueDataPacket = attributeValueDataPackets[setNameProfileName];
            Assert.IsTrue( profileNameAttributeValueDataPacket.StatusCode == System.Net.HttpStatusCode.OK );

            var dobAttributeValueDataPacket = attributeValueDataPackets[setNameDOB];
            Assert.IsTrue( dobAttributeValueDataPacket.StatusCode == System.Net.HttpStatusCode.OK );

            var emailAttributeValueDataPacket = attributeValueDataPackets[setNameEmail];
            Assert.IsTrue( emailAttributeValueDataPacket.StatusCode == System.Net.HttpStatusCode.OK );
        }

        /// <summary>
        /// The purpose of this test is largely to check in the async call to fetch definitions
        /// is working, especially if the definition is already in cache and a IO bound API call
        /// is not needed.
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task GetAttributeValue_MultipleValueRepeated() {

            var client = new AttributeValueAPIClient( Constants.X_API_KEY, APIStage.BETA );
            AttributeValueDefinitionFetcher.FETCHER.XApiKey = Constants.X_API_KEY;

            var userAuthentication = new UserAuthentication(
                Constants.TestDev7Credentials.Username,
                Constants.TestDev7Credentials.Password );
            await userAuthentication.InitializeAsync();

            var setNameProfileName = "v1.0:orion:Profile Name";
            var setNameDOB = "v1.0:orion:Date of Birth";
            var setNameEmail = "v2.0:orion:Email Address";
            var setNamePhone = "v2.0:orion:Phone Number";

            List<SetName> myAttributes = new List<SetName>()
            {
               SetName.Parse( setNamePhone ),
               SetName.Parse( setNameProfileName ),
               SetName.Parse( setNameDOB ),
               SetName.Parse( setNameEmail )
            };

            //Will use a GetAttributeValueAuthenticatedRequest objectin this unit test, so I can set ReturnDefaultvalues to true (it is by default false).
            var request = new GetAttributeValueAuthenticatedRequest( userAuthentication ) {
                AttributeNames = myAttributes,
                ReturnDefaultValues = true
            };

            var response1 = await client.GetAttributeValueAuthenticatedAsync( request );

            var profile1 = response1.AttributeValues[setNameProfileName].AttributeValue;
            var dob1 = response1.AttributeValues[setNameDOB].AttributeValue;
            var email1 = response1.AttributeValues[setNameEmail].AttributeValue;
            var phone1 = response1.AttributeValues[setNamePhone].AttributeValue;

            Assert.IsNotNull( profile1 );
            Assert.IsNotNull( dob1 );
            Assert.IsNotNull( email1 );
            Assert.IsNotNull( phone1 );

            var response2 = await client.GetAttributeValueAuthenticatedAsync( request );

            var profile2 = response2.AttributeValues[setNameProfileName].AttributeValue;
            var dob2 = response2.AttributeValues[setNameDOB].AttributeValue;
            var email2 = response2.AttributeValues[setNameEmail].AttributeValue;
            var phone2 = response2.AttributeValues[setNamePhone].AttributeValue;

            Assert.IsNotNull( profile2 );
            Assert.IsNotNull( dob2 );
            Assert.IsNotNull( email2 );
            Assert.IsNotNull( phone2 );
        }

        [TestMethod]
        public async Task GetAttributeValue_DoesNotExist() {

            var client = new AttributeValueAPIClient( Constants.X_API_KEY, APIStage.BETA );
            AttributeValueDefinitionFetcher.FETCHER.XApiKey = Constants.X_API_KEY;

            var userAuthentication = new UserAuthentication(
                Constants.TestDev7Credentials.Username,
                Constants.TestDev7Credentials.Password );
            await userAuthentication.InitializeAsync();

            var setNameNotARealAttribute = "v1.0:orion:Not a Real Attribute";
            List<string> myAttributes = new List<string>()
            {
               setNameNotARealAttribute
            };
            var taskResponse = client.GetAttributeValueAuthenticatedAsync( myAttributes, userAuthentication );
            var response = taskResponse.Result;
            //The overall status code for the call should be a 200 (OK).
            Assert.AreEqual( System.Net.HttpStatusCode.OK, response.StatusCode );

            //The returned data should have one AttriuteValueDataPacket.
            var attributeValueDataPackets = response.AttributeValues;
            Assert.IsTrue( attributeValueDataPackets.Count == 1 );
            Assert.IsTrue( attributeValueDataPackets.ContainsKey( setNameNotARealAttribute ) );

            //Retreive the data packet and check that the status code is not found.
            var notARealAttributeValueDataPacket = attributeValueDataPackets[setNameNotARealAttribute];
            Assert.IsTrue( notARealAttributeValueDataPacket.StatusCode == System.Net.HttpStatusCode.NotFound );
        }

        [TestMethod]
        public async Task AttributeValueAppellationTest() {
            AttributeValueDefinitionFetcher.FETCHER.XApiKey = Constants.X_API_KEY;
            var setName = SetName.Parse( "v1.0:ntparc:Three-Position Air Rifle Type" );
            var rifleType = await Scopos.BabelFish.DataModel.AttributeValue.AttributeValue.CreateAsync( setName );

            rifleType.SetFieldValue( "Three-Position Air Rifle Type", "Sporter" );
            Assert.AreEqual( "Sporter", rifleType.AttributeValueAppellation );

            rifleType.SetFieldValue( "Three-Position Air Rifle Type", "Precision" );
            Assert.AreEqual( "Precision", rifleType.AttributeValueAppellation );
        }
    }
}
