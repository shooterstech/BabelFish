using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scopos.BabelFish.APIClients;
using Scopos.BabelFish.DataModel.AttributeValue;
using Scopos.BabelFish.DataModel.Common;
using Scopos.BabelFish.DataModel.Definitions;
using Scopos.BabelFish.Helpers;
using Scopos.BabelFish.Requests.AttributeValueAPI;
using Scopos.BabelFish.Responses.AttributeValueAPI;
using Scopos.BabelFish.Runtime.Authentication;

namespace Scopos.BabelFish.Tests.AttributeValue {
    [TestClass]
    public class SetMultipleValuesTest {

        [TestInitialize]
        public void InitializeTest() {
            Scopos.BabelFish.Runtime.Settings.XApiKey = Constants.X_API_KEY;
        }


        [TestMethod]
        public async Task SetMultiplePhoneNumbers() {

            var setNamePhoneNumber = "v2.0:orion:Phone Number";

            /*
             * The Phone Number Attribute Value allows for setting multiple phone numbers to the user (MultipleValues is set to true).
             * Each saved phone number is uniquly identified by the field "PhoneNumberName." This field is identified because
             * it has Key set to true in the definition.
             * 
             * When getting or setting the phone number, use the value of PhoneNumberName.
             */


            var client = new AttributeValueAPIClient( APIStage.PRODTEST );

            var userAuthentication = new UserAuthentication(
                Constants.TestDev7Credentials.Username,
                Constants.TestDev7Credentials.Password );
            await userAuthentication.InitializeAsync();

            //Retreive the user's current values.            
            //Will use a GetAttributeValueAuthenticatedRequest object in this unit test, so I can set ReturnDefaultvalues to true (it is by default false).
            List<SetName> myAttributes = new List<SetName>() { SetName.Parse(setNamePhoneNumber) };

            //For the purpose of this unit test, I am starting with a clean / brand new Attribute Value. Normally, in real life
            //The user's attribute value should be read first
            var phoneNumberAttrValue = Scopos.BabelFish.DataModel.AttributeValue.AttributeValue.CreateAsync( SetName.Parse( setNamePhoneNumber ) ).Result;

            //Work
            var workFieldKey = "MyWorkNumber";
            var workNumber = $"+1 {RandomStringGenerator.RandomNumericString(3)}-867-5309";
            phoneNumberAttrValue.SetFieldValue( "PhoneNumber", workNumber, workFieldKey );
            phoneNumberAttrValue.SetFieldValue( "Type", new List<string>() { "WORK" }, workFieldKey );
            
            //Mobile
            var mobileFieldKey = "MyMobileNumber";
            var mobileNumber = $"+1 {RandomStringGenerator.RandomNumericString( 3 )}-867-5309";
            phoneNumberAttrValue.SetFieldValue( "PhoneNumber", mobileNumber, mobileFieldKey );
            phoneNumberAttrValue.SetFieldValue( "Type", new List<string>() { "MOBILE" }, mobileFieldKey );

            var sendPhoneNumberAttrValueDataPacket = new AttributeValueDataPacketAPIResponse() {
                AttributeDef = setNamePhoneNumber,
                AttributeValue = phoneNumberAttrValue,
                Visibility = VisibilityOption.PROTECTED
            };
            var setRequest = new SetAttributeValueAuthenticatedRequest( userAuthentication ) {
                AttributeValuesToUpdate = new List<AttributeValueDataPacket>() { sendPhoneNumberAttrValueDataPacket }
            };
            var taskSetResponse = client.SetAttributeValueAuthenticatedAsync( setRequest );
            var setResponse = taskSetResponse.Result;

            //Check the status of the overall call
            Assert.AreEqual( System.Net.HttpStatusCode.OK, setResponse.StatusCode );

            var getRequest = new GetAttributeValueAuthenticatedRequest( userAuthentication ) {
                AttributeNames = myAttributes,
                ReturnDefaultValues = true
            };
            var taskGetResponse = client.GetAttributeValueAuthenticatedAsync( getRequest );
            var getResponse = taskGetResponse.Result;

            //Check the status of the overall call
            Assert.AreEqual( System.Net.HttpStatusCode.OK, getResponse.StatusCode );

            //Pull the Attribute Value Data Packet for Phone Number
            var readPhoneNumberAttrValueDataPacket = getResponse.AttributeValues[setNamePhoneNumber];

            //Check the status of the phone number data packet
            //NOTE: If ReturnDefaultValues wasn't set to true, the .StatusCode could return a Not Found error if the data does not yet exist in database.
            Assert.AreEqual( System.Net.HttpStatusCode.OK, readPhoneNumberAttrValueDataPacket.StatusCode );

            var readPhoneNumberAttrValue = readPhoneNumberAttrValueDataPacket.AttributeValue;

            //Check that the read values are the same as the values we set.
            Assert.AreEqual( workNumber, readPhoneNumberAttrValue.GetFieldValue( "PhoneNumber", workFieldKey ) );
            Assert.AreEqual( mobileNumber, readPhoneNumberAttrValue.GetFieldValue( "PhoneNumber", mobileFieldKey ) );
		}

		[TestMethod]
		public async Task SetMultipleSocialMediaAccounts() {

			var setNameSocialMedia = "v1.0:orion:Social Media Accounts";

			var client = new AttributeValueAPIClient( APIStage.PRODTEST );

			var userAuthentication = new UserAuthentication(
				Constants.TestDev7Credentials.Username,
				Constants.TestDev7Credentials.Password );
			await userAuthentication.InitializeAsync();

			//Retreive the user's current values.            
			//Will use a GetAttributeValueAuthenticatedRequest object in this unit test, so I can set ReturnDefaultvalues to true (it is by default false).
			List<SetName> myAttributes = new List<SetName>() { SetName.Parse( setNameSocialMedia ) };

            //For the purpose of this unit test, I am starting with a clean / brand new Attribute Value. Normally, in real life
            //The user's attribute value should be read first
            var socialMediaAttrValue = await Scopos.BabelFish.DataModel.AttributeValue.AttributeValue.CreateAsync( SetName.Parse( setNameSocialMedia ) );

            socialMediaAttrValue.SetFieldValue( "ProfileName", "facebook_test", "FACEBOOK" );
			socialMediaAttrValue.SetFieldValue( "ProfileName", "instagram_test", "INSTAGRAM" );

			var sendSocialmediaAttrValueDataPacket = new AttributeValueDataPacketAPIResponse() {
				AttributeDef = setNameSocialMedia,
				AttributeValue = socialMediaAttrValue,
				Visibility = VisibilityOption.PUBLIC
			};
			var setRequest = new SetAttributeValueAuthenticatedRequest( userAuthentication ) {
				AttributeValuesToUpdate = new List<AttributeValueDataPacket>() { sendSocialmediaAttrValueDataPacket }
			};
			var taskSetResponse = client.SetAttributeValueAuthenticatedAsync( setRequest );
			var setResponse = taskSetResponse.Result;

			//Check the status of the overall call
			Assert.AreEqual( System.Net.HttpStatusCode.OK, setResponse.StatusCode );

			var getRequest = new GetAttributeValueAuthenticatedRequest( userAuthentication ) {
				AttributeNames = myAttributes,
				ReturnDefaultValues = true
			};
			var taskGetResponse = client.GetAttributeValueAuthenticatedAsync( getRequest );
			var getResponse = taskGetResponse.Result;

			//Check the status of the overall call
			Assert.AreEqual( System.Net.HttpStatusCode.OK, getResponse.StatusCode );

			//Pull the Attribute Value Data Packet for Phone Number
			var readSocialMediaAttrValueDataPacket = getResponse.AttributeValues[setNameSocialMedia];

			//Check the status of the phone number data packet
			//NOTE: If ReturnDefaultValues wasn't set to true, the .StatusCode could return a Not Found error if the data does not yet exist in database.
			Assert.AreEqual( System.Net.HttpStatusCode.OK, readSocialMediaAttrValueDataPacket.StatusCode );

			var readPhoneNumberAttrValue = readSocialMediaAttrValueDataPacket.AttributeValue;

			//Check that the read values are the same as the values we set.
			Assert.AreEqual( "facebook_test", readPhoneNumberAttrValue.GetFieldValue( "ProfileName", "FACEBOOK" ) );
			Assert.AreEqual( "instagram_test", readPhoneNumberAttrValue.GetFieldValue( "ProfileName", "INSTAGRAM" ) );
			Assert.AreEqual( "", readPhoneNumberAttrValue.GetFieldValue( "ProfileName", "SNAPCHAT" ) );
		}
	}
}
