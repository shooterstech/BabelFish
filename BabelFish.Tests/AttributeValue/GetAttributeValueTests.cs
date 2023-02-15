﻿using System;
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
    public class GetSetAttributeValueTests {

        /// <summary>
        /// Unit test to confirm the Constructors set the api key and API stage as expected.
        /// </summary>
        [TestMethod]
        public void BasicConstructorTests() {

            var defaultConstructorClient = new AttributeValueAPIClient( Constants.X_API_KEY );
            var apiStageConstructorClient = new AttributeValueAPIClient( Constants.X_API_KEY, APIStage.BETA );

            Assert.AreEqual( Constants.X_API_KEY, defaultConstructorClient.XApiKey );
            Assert.AreEqual( APIStage.PRODUCTION, defaultConstructorClient.ApiStage );

            Assert.AreEqual( Constants.X_API_KEY, apiStageConstructorClient.XApiKey );
            Assert.AreEqual( APIStage.BETA, apiStageConstructorClient.ApiStage );
        }

        /// <summary>
        /// Tests the retreival of a single Attribute Value
        /// </summary>
        [TestMethod]
        public void GetAttributeValue_SingleValue() {

            var client = new AttributeValueAPIClient( Constants.X_API_KEY, APIStage.BETA );
            AttributeValueDefinitionFetcher.FETCHER.XApiKey = Constants.X_API_KEY;

            var userAuthentication = new UserAuthentication(
                Constants.TestDev7Credentials.Username,
                Constants.TestDev7Credentials.Password );

            var setNameProfileName = "v1.0:orion:Profile Name";
            List<string> myAttributes = new List<string>()
            {
               setNameProfileName
            };
            var taskResponse = client.GetAttributeValueAuthenticatedAsync( myAttributes, userAuthentication );
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

            Assert.AreEqual( "Chris", (string) profileNameAttributeValue.GetFieldValue( "GivenName" ) );
            Assert.AreEqual( "Jones", (string)profileNameAttributeValue.GetFieldValue( "FamilyName" ) );
        }

        /// <summary>
        /// Tests the retreival of multiple attributes
        /// </summary>
            [TestMethod]
        public void GetAttributeValue_MultipleValue() {

            var client = new AttributeValueAPIClient( Constants.X_API_KEY, APIStage.BETA );
            AttributeValueDefinitionFetcher.FETCHER.XApiKey = Constants.X_API_KEY;

            var userAuthentication = new UserAuthentication(
                Constants.TestDev7Credentials.Username,
                Constants.TestDev7Credentials.Password );

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

        [TestMethod]
        public void GetAttributeValue_DoesNotExist() {

            var client = new AttributeValueAPIClient( Constants.X_API_KEY, APIStage.BETA );
            AttributeValueDefinitionFetcher.FETCHER.XApiKey = Constants.X_API_KEY;

            var userAuthentication = new UserAuthentication(
                Constants.TestDev7Credentials.Username,
                Constants.TestDev7Credentials.Password );

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
    }
}