using System.Threading.Tasks;
using Scopos.BabelFish.APIClients;
using Scopos.BabelFish.DataModel.AttributeValue;
using Scopos.BabelFish.DataModel.Common;
using Scopos.BabelFish.DataModel.Definitions;
using Scopos.BabelFish.Requests.AttributeValueAPI;
using Scopos.BabelFish.Responses.AttributeValueAPI;
using Scopos.BabelFish.Runtime.Authentication;

namespace Scopos.BabelFish.Tests.AttributeValue {
    /// <summary>
    /// Tests the user of the Public API call for returning Attribute Value on a user.
    /// Which does not use authapi authenticated calls.
    /// </summary>
    [TestClass]
    public class GetPublicAttributeValueTests : BaseTestClass {

        /// <summary>
        /// Tests the retreival of a single Attribute Value who's visibility is PUBLIC
        /// </summary>
        [TestMethod]
        public async Task GetAttributeValue_SingleValue() {

            var client = new AttributeValueAPIClient( APIStage.PRODUCTION );

			//We're first going to make an authenticated call to set the values we expect to later read.
			var userAuthentication = new UserAuthentication(
				Constants.TestDev7Credentials.Username,
				Constants.TestDev7Credentials.Password );
			await userAuthentication.InitializeAsync();

			var setNameTrainingCategory = "v1.0:orion:Air Rifle Training Category";
			List<string> myAttributes = new List<string>()
			{
			   setNameTrainingCategory
			};

			var trainingCategoryAttrValueToSet = await Scopos.BabelFish.DataModel.AttributeValue.AttributeValue.CreateAsync( SetName.Parse( setNameTrainingCategory ) );
			trainingCategoryAttrValueToSet.SetFieldValue( "Rifle Type and Category", "SH1" );

			var trainingCategoryDataPacketToSet = new AttributeValueDataPacketAPIResponse {
				AttributeDef = setNameTrainingCategory,
				AttributeValue = trainingCategoryAttrValueToSet,
				Visibility = VisibilityOption.PUBLIC
			};

			var setSocialMediaRequest = new SetAttributeValueAuthenticatedRequest( userAuthentication ) {
				AttributeValuesToUpdate = new List<AttributeValueDataPacket> { trainingCategoryDataPacketToSet }
			};

			await client.SetAttributeValueAuthenticatedAsync( setSocialMediaRequest );

            //Now the test is to read the value using the PUBLIC api call
            var taskResponse = client.GetAttributeValuePublicAsync( myAttributes, Constants.TestDev7UserId );
            var response = taskResponse.Result;
            Assert.AreEqual( System.Net.HttpStatusCode.OK, response.StatusCode );

            //The returned data should have one AttriuteValueDataPacket if the Profile Name set name.
            var attributeValueDataPackets = response.AttributeValues;
            Assert.IsTrue( attributeValueDataPackets.Count == 1 );
            Assert.IsTrue( attributeValueDataPackets.ContainsKey( setNameTrainingCategory ) );

            //Retreive the data packet and check that the status code is success.
            var trainingCategoryAttributeValueDataPacket = attributeValueDataPackets[setNameTrainingCategory];
            Assert.IsTrue( trainingCategoryAttributeValueDataPacket.StatusCode == System.Net.HttpStatusCode.OK );

            //Pull the attribute value back, and check we have reasonable values.
            var profileNameAttributeValue = trainingCategoryAttributeValueDataPacket.AttributeValue;

            Assert.AreEqual( "SH1", (string) profileNameAttributeValue.GetFieldValue( "Rifle Type and Category" ) );
		}

		/// <summary>
		/// Tests the retreival of a single Attribute Value who's visibility is PRIVATE.
		/// The read response should come back with a NOT FOUND, with no data avaliable.
		/// </summary>
		[TestMethod]
		public async Task GetAttributeValue_PRIVATE_Visibility() {

			var client = new AttributeValueAPIClient( APIStage.PRODUCTION );

			//We're first going to make an authenticated call to set the values we expect to later read.
			var userAuthentication = new UserAuthentication(
				Constants.TestDev7Credentials.Username,
				Constants.TestDev7Credentials.Password );
			await userAuthentication.InitializeAsync();

			var setNameTrainingCategory = "v1.0:orion:Air Pistol Training Category";
			List<string> myAttributes = new List<string>()
			{
			   setNameTrainingCategory
			};

			var trainingCategoryAttrValueToSet = await Scopos.BabelFish.DataModel.AttributeValue.AttributeValue.CreateAsync( SetName.Parse( setNameTrainingCategory ) );
			trainingCategoryAttrValueToSet.SetFieldValue( "Pistol Type and Category", "SH1" );

			var trainingCategoryDataPacketToSet = new AttributeValueDataPacketAPIResponse {
				AttributeDef = setNameTrainingCategory,
				AttributeValue = trainingCategoryAttrValueToSet,
				Visibility = VisibilityOption.PRIVATE
			};

			var setSocialMediaRequest = new SetAttributeValueAuthenticatedRequest( userAuthentication ) {
				AttributeValuesToUpdate = new List<AttributeValueDataPacket> { trainingCategoryDataPacketToSet }
			};

			await client.SetAttributeValueAuthenticatedAsync( setSocialMediaRequest );

			//Now the test is to read the value using the PUBLIC api call
			var taskResponse = client.GetAttributeValuePublicAsync( myAttributes, Constants.TestDev7UserId );
			var response = taskResponse.Result;
			//The overall status code for the call should be .OK (only the status code for reading the specific attribure value is .NotFound)
			Assert.AreEqual( System.Net.HttpStatusCode.OK, response.StatusCode );

			//The returned data should have one AttriuteValueDataPacket if the Profile Name set name.
			var attributeValueDataPackets = response.AttributeValues;
			Assert.IsTrue( attributeValueDataPackets.Count == 1 );
			Assert.IsTrue( attributeValueDataPackets.ContainsKey( setNameTrainingCategory ) );

			//Retreive the data packet and check that the status code is success.
			var trainingCategoryAttributeValueDataPacket = attributeValueDataPackets[setNameTrainingCategory];
			Assert.IsTrue( trainingCategoryAttributeValueDataPacket.StatusCode == System.Net.HttpStatusCode.NotFound );

			//Because it is not found, the .AttributeValue should also be NULL
			Assert.IsNull( trainingCategoryAttributeValueDataPacket.AttributeValue ); 
		}

		[TestMethod]
        public async Task GetAttributeValue_DoesNotExist() {

            var client = new AttributeValueAPIClient( APIStage.BETA );

            var setNameNotARealAttribute = "v1.0:orion:Not a Real Attribute";
            List<string> myAttributes = new List<string>()
            {
               setNameNotARealAttribute
            };
            var taskResponse = client.GetAttributeValuePublicAsync( myAttributes, Constants.TestDev7UserId );
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

        /// <summary>
        /// Tests if publicly reading a Attribute Value with multiple values gets returned and interpreted properly.
        /// </summary>
        /// <returns></returns>
		[TestMethod]
		public async Task GetAttributeValue_MultipleValueAttributeValue() {

			var client = new AttributeValueAPIClient( APIStage.BETA );

            //We're first going to make an authenticated call to set the values we expect to later read.
            var userAuthentication = new UserAuthentication(
				Constants.TestDev7Credentials.Username,
				Constants.TestDev7Credentials.Password );
			await userAuthentication.InitializeAsync();

			var setNameSocialMedia = "v1.0:orion:Social Media Accounts";
			List<string> myAttributes = new List<string>()
			{
			   setNameSocialMedia
			};

            var socialMediaAttrValueToSet = await Scopos.BabelFish.DataModel.AttributeValue.AttributeValue.CreateAsync( SetName.Parse( setNameSocialMedia ) );
            socialMediaAttrValueToSet.SetFieldValue( "ProfileName", "myFacebookAccount", "FACEBOOK" );
			socialMediaAttrValueToSet.SetFieldValue( "ProfileName", "myInstagramAccount", "INSTAGRAM" );

			var socialMediaDataPacketToSet = new AttributeValueDataPacketAPIResponse {
                AttributeDef = setNameSocialMedia,
                AttributeValue = socialMediaAttrValueToSet,
                Visibility = VisibilityOption.PUBLIC
            };

            var setSocialMediaRequest = new SetAttributeValueAuthenticatedRequest( userAuthentication ) {
                AttributeValuesToUpdate = new List<AttributeValueDataPacket> { socialMediaDataPacketToSet }
            };

            await client.SetAttributeValueAuthenticatedAsync( setSocialMediaRequest );


            //Now read the attribute value we just set, using the public API
			var taskResponse = client.GetAttributeValuePublicAsync( myAttributes, Constants.TestDev7UserId );
			var response = taskResponse.Result;

			//The overall status code for the call should be a 200 (OK).
			Assert.AreEqual( System.Net.HttpStatusCode.OK, response.StatusCode );

			//The returned data should have one AttriuteValueDataPacket.
			var attributeValueDataPackets = response.AttributeValues;
			Assert.IsTrue( attributeValueDataPackets.Count == 1 );
			Assert.IsTrue( attributeValueDataPackets.ContainsKey( setNameSocialMedia ) );

			//Retreive the data packet and check that the status code is not found.
			var socialMediaAttributeValueDataPacket = attributeValueDataPackets[setNameSocialMedia];
			Assert.IsTrue( socialMediaAttributeValueDataPacket.StatusCode == System.Net.HttpStatusCode.OK );

            var facebook = socialMediaAttributeValueDataPacket.AttributeValue.GetFieldValue( "ProfileName", "FACEBOOK" );
            var instagram = socialMediaAttributeValueDataPacket.AttributeValue.GetFieldValue( "ProfileName", "INSTAGRAM" );

            Assert.AreEqual( "myFacebookAccount", facebook );
            Assert.AreEqual( "myInstagramAccount", instagram );
		}
    }
}
