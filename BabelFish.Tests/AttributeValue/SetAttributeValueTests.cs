using System;
using System.Collections.Generic;
using System.Configuration;
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
    public class SetAttributeValueTests {

        [TestMethod]
        public void SetAttributeValue_SingleAttribute() {

            var client = new AttributeValueAPIClient( Constants.X_API_KEY, APIStage.BETA );
            AttributeValueDefinitionFetcher.FETCHER.XApiKey = Constants.X_API_KEY;

            var userAuthentication = new UserAuthentication(
                Constants.TestDev7Credentials.Username,
                Constants.TestDev7Credentials.Password );

            //The Test Attribute defines an attribute meant for testing. Do you like the name I gave it? I came up with it myself.
            var setNameTestAttriubte = "v1.0:orion:Test Attribute";

            List<SetName> myAttributes = new List<SetName>()
            {
               SetName.Parse( setNameTestAttriubte )
            };

            //Will use a GetAttributeValueAuthenticatedRequest objectin this unit test, so I can set ReturnDefaultvalues to true (it is by default false).
            var getRequest1 = new GetAttributeValueAuthenticatedRequest( userAuthentication ) {
                AttributeNames = myAttributes,
                ReturnDefaultValues = true
            };

            //Make the call, then retreive the data packet read from the Get API call
            var taskGetResponse1 = client.GetAttributeValueAuthenticatedAsync( getRequest1 );
            var getResponse1 = taskGetResponse1.Result;
            var attributeValueDataPackets1 = getResponse1.AttributeValues;
            var testAttriubuteAttributeValueDataPacket1 = attributeValueDataPackets1[setNameTestAttriubte];

            //To make the code easier to work with, grabbing a pointer to the attribute value from the data packet.
            var testAttributeValue1 = testAttriubuteAttributeValueDataPacket1.AttributeValue;

            //Read the integer value, then use it to set the float, string, and send back to the rest api
            int intValue = (int)testAttributeValue1.GetFieldValue( "AnInteger" );

            //Generate what the new values will be
            int newIntValue = intValue + 1;
            float newFloatValue = newIntValue;
            string newStringValue = newIntValue.ToString();
            bool newBoolean = intValue % 2 == 0;
            DateTime newDate = DateTime.Today;
            DateTime newDateTime = DateTime.UtcNow;

            //Set the values to the attribute value
            testAttributeValue1.SetFieldValue( "AnInteger", newIntValue );
            testAttributeValue1.SetFieldValue( "AFloat", newFloatValue );
            testAttributeValue1.SetFieldValue( "AString", newStringValue );
            testAttributeValue1.SetFieldValue( "ABoolean", newBoolean );
            testAttributeValue1.SetFieldValue( "ADate", newDate );
            testAttributeValue1.SetFieldValue( "ADateTime", newDateTime );

            //Test that the .SetFieldValue worked locally
            Assert.AreEqual( newIntValue,(int) testAttributeValue1.GetFieldValue( "AnInteger" ) );
            Assert.AreEqual( newFloatValue, (float) testAttributeValue1.GetFieldValue( "AFloat" ) );
            Assert.AreEqual( newStringValue, (string) testAttributeValue1.GetFieldValue( "AString" ) );
            Assert.AreEqual( newBoolean, (bool)testAttributeValue1.GetFieldValue( "ABoolean" ) );
            Assert.AreEqual( newDate, (DateTime)testAttributeValue1.GetFieldValue( "ADate" ) );
            Assert.AreEqual( newDateTime, (DateTime)testAttributeValue1.GetFieldValue( "ADateTime" ) );

            //Generate a set attribute value request
            var setRequest = new SetAttributeValueAuthenticatedRequest( userAuthentication );
            //Add the attribute data packet to the request. Note we do not need to create a new AttributeDataPacket, we can re-use the one we got from the .GetAttributeValue call
            setRequest.AttributeValuesToUpdate.Add( testAttriubuteAttributeValueDataPacket1 );

            //Send the request
            var taskSetResponse = client.SetAttributeValueAuthenticatedAsync( setRequest );
            var setResponse = taskSetResponse.Result;
            //Check that the response was successful.
            Assert.AreEqual( System.Net.HttpStatusCode.OK, setResponse.StatusCode );

            //Now, for testing, re-get the attribute value and check it's values.
            //Will use a GetAttributeValueAuthenticatedRequest objectin this unit test, so I can set ReturnDefaultvalues to true (it is by default false).
            var getRequest2 = new GetAttributeValueAuthenticatedRequest( userAuthentication ) {
                AttributeNames = myAttributes,
                ReturnDefaultValues = true
            };

            //Make the second call, then retreive the data packet read from the Get API call
            var taskGetResponse2 = client.GetAttributeValueAuthenticatedAsync( getRequest2 );
            var getResponse2 = taskGetResponse2.Result;
            var attributeValueDataPackets2 = getResponse2.AttributeValues;
            var testAttriubuteAttributeValueDataPacket2 = attributeValueDataPackets2[setNameTestAttriubte];
            var testAttributeValue2 = testAttriubuteAttributeValueDataPacket2.AttributeValue;

            //Test the values that we just pulled, match the values we sent on the Set request
            Assert.AreEqual( newIntValue, (int)testAttributeValue2.GetFieldValue( "AnInteger" ) );
            Assert.AreEqual( newFloatValue, (float)testAttributeValue2.GetFieldValue( "AFloat" ) );
            Assert.AreEqual( newStringValue, (string) testAttributeValue2.GetFieldValue( "AString" ) );
            Assert.AreEqual( newBoolean, (bool)testAttributeValue2.GetFieldValue( "ABoolean" ) );
            Assert.AreEqual( newDate, (DateTime)testAttributeValue2.GetFieldValue( "ADate" ) );
            Assert.AreEqual( newDateTime, (DateTime)testAttributeValue2.GetFieldValue( "ADateTime" ) );
        }
    }
}
