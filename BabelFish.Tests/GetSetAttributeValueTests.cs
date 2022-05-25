using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BabelFish;
using BabelFish.Helpers;
using BabelFish.DataModel.Definitions;
using ShootersTech.DataModel.Athena;

namespace BabelFish.Tests {
    [TestClass]
    public class GetSetAttributeValueTests
    {
        private static string xApiKey = "wjM7eCb75aa3Okxj4FliXLY0VjHidoE2ei18pdg1";
        private static Dictionary<string, string> clientParams = new Dictionary<string, string>()
        {
            {"UserName", "test_dev_7@shooterstech.net"},
            {"PassWord", "abcd1234"},
        };
        private readonly GetSetAttributeValueAPIClient _client = new GetSetAttributeValueAPIClient(xApiKey, clientParams);

        [TestMethod]
        public void GetAttributeValue_SingleValue() {

            List<string> myAttributes = new List<string>()
            {
                "v1.0:orion:Profile Name",
            };
            var response = _client.GetAttributeValueAsync(myAttributes);
            Assert.IsNotNull(response);

            var taskResult = response.Result;
            var objResponse = taskResult.AttributeValues;
            Assert.IsTrue(objResponse.Count() == 1);
            var msgResponse = taskResult.MessageResponse;
            Assert.IsNotNull(msgResponse);
            Assert.IsTrue(msgResponse.Message.Count == 0);
        }

        [TestMethod]
        public void GetAttributeValue_MultipleValues()
        {
            List<string> myAttributes = new List<string>()
            {
                "v1.0:orion:Profile Name",
                "v1.0:orion:Date of Birth",
            };

            var response = _client.GetAttributeValueAsync(myAttributes);
            Assert.IsNotNull(response);

            var taskResult = response.Result;
            var objResponse = taskResult.AttributeValues;
            Assert.IsTrue(objResponse.Count() == 2);
            foreach ( var checkName in objResponse.ToArray())
                Assert.IsTrue(myAttributes.Contains(checkName.SetName));
            var msgResponse = taskResult.MessageResponse;
            Assert.IsNotNull(msgResponse);
            Assert.IsTrue(msgResponse.Message.Count == 0);
        }

        [TestMethod]
        public void GetAttributeValue_GetAttributeValueSuccess()
        {
            List<string> myAttributes = new List<string>()
            {
                "v1.0:orion:Date of Birth",
            };

            var response = _client.GetAttributeValueAsync(myAttributes);
            Assert.IsNotNull(response);

            var taskResult = response.Result;
            var objResponse = taskResult.AttributeValues;
            var specificObject = taskResult.GetAttributeValue("v1.0:orion:Date of Birth");
            
            Assert.IsNotNull(specificObject);
            Assert.AreEqual(specificObject.SetName, "v1.0:orion:Date of Birth");
        }

        [TestMethod]
        public void GetAttributeValue_GetAttributeValueFail()
        {
            List<string> myAttributes = new List<string>()
            {
                "v1.0:orion:Date of Birth",
            };

            var response = _client.GetAttributeValueAsync(myAttributes);
            Assert.IsNotNull(response);

            var taskResult = response.Result;
            var objResponse = taskResult.AttributeValues;
            var specificObject = taskResult.GetAttributeValue("v1.0:orion:Profile Name");

            Assert.IsNull(specificObject);
        }

        [TestMethod]
        public void GetAttributeValue_NotFound()
        {
            List<string> myAttributes = new List<string>()
            {
                "v1.0:orion:Invalid SetName",
            };

            var response = _client.GetAttributeValueAsync(myAttributes);
            Assert.IsNotNull(response);

            var taskResult = response.Result;
            var objResponse = taskResult.AttributeValues;
            Assert.IsTrue(objResponse.Count() == 0);
            var msgResponse = taskResult.MessageResponse;
            Assert.IsNotNull(msgResponse);
            Assert.IsTrue(msgResponse.Message.Count > 0);
            Assert.IsTrue(msgResponse.Title == "GetAttributeValue API errors encountered");
        }

        [TestMethod]
        public void GetValidateUserID_ValidID()
        {
            string userID = "28489692-0a61-470e-aed8-c71b9cfbfe6e";
            var response = _client.GetValidateUserIDAsync(userID);
            Assert.IsNotNull(response);

            var taskResult = response.Result;
            var objResponse = taskResult.ValidateUserID;
            var msgResponse = taskResult.MessageResponse;

            Assert.IsNotNull(objResponse);
            Assert.IsNotNull(msgResponse);

            Assert.AreEqual(objResponse.UserID, userID);
            Assert.IsTrue(objResponse.Valid);
        }

        [TestMethod]
        public void GetValidateUserID_InValid()
        {
            string userID = "ThisIsAnInvalidUserId";
            var response = _client.GetValidateUserIDAsync(userID);
            Assert.IsNotNull(response);

            var taskResult = response.Result;
            var objResponse = taskResult.ValidateUserID;
            var msgResponse = taskResult.MessageResponse;

            Assert.IsNotNull(objResponse);
            Assert.IsNotNull(msgResponse);

            Assert.AreEqual(objResponse.UserID, userID);
            Assert.IsFalse(objResponse.Valid);
        }

        [TestMethod]
        public void SetAttributeValue_ErrorSingleIntoMultiValue()
        {
            string expectedException = "Field being set is designated MultipleValue needing Key";

            DataModel.GetSetAttributeValue.AttributeValue newAttributeValue =
                new DataModel.GetSetAttributeValue.AttributeValue("v1.0:orion:Device Token");

            try
            {
                //This should throw an error as no key is passed to a MultipleValues AttributeValue
                newAttributeValue.SetFieldValue("DeviceType", "ios");
            }
            catch(Exception ex)
            {
                Assert.IsTrue(ex.Message.Contains(expectedException));
            }
        }

        [TestMethod]
        public void SetAttributeValue_ErrorInvalidFieldName()
        {
            string expectedException = "Invalid Set Field Value";

            DataModel.GetSetAttributeValue.AttributeValue newAttributeValue =
                new DataModel.GetSetAttributeValue.AttributeValue("v1.0:orion:Date of Birth");

            try
            {
                //This should throw an error as no key is passed to a MultipleValues AttributeValue
                newAttributeValue.SetFieldValue("dateOfBirthMispelled", "1970-01-01");
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex.Message.Contains(expectedException));
            }
        }

        [TestMethod]
        public void SetAttributeValue_ErrorMismatchedFieldDefinitionType()
        {
            string expectedException = "Invalid Set Field Value";
            DataModel.GetSetAttributeValue.AttributeValue newAttributeValue = new DataModel.GetSetAttributeValue.AttributeValue("v1.0:orion:Device Token");

            try
            {
                //This should throw an error for EvalToBoolFail should fail convert to bool
                newAttributeValue.SetFieldValue("StageScoresUnofficial", "EvalToBoolFail", "abcd");
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex.Message.Contains(expectedException));
            }
        }

        [TestMethod]
        public void SetAttributeValue_ErrorValueNotInCLOSEDList()
        {
            string expectedException = "Invalid Set Field Value";
            DataModel.GetSetAttributeValue.AttributeValue newAttributeValue = new DataModel.GetSetAttributeValue.AttributeValue("v1.0:orion:Device Token");

            try
            {
                //This should throw an error for deviceType not in predefined list
                newAttributeValue.SetFieldValue("DeviceType", "ThisIsNotValid", "abcd");
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex.Message.Contains(expectedException));
            }
        }

        [TestMethod]
        public void SetAttributeValue_ErrorMinValue()
        {
            string expectedException = "Invalid Set Field Value";
            DataModel.GetSetAttributeValue.AttributeValue newAttributeValue = new DataModel.GetSetAttributeValue.AttributeValue("v1.0:orion:Date of Birth");

            try
            {
                //This should throw an error for < min value
                newAttributeValue.SetFieldValue("DateOfBirth", "1899-12-31");
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex.Message.Contains(expectedException));
            }
        }

        [TestMethod]
        public void SetAttributeValue_ErrorMaxValue()
        {
            string expectedException = "Invalid Set Field Value";
            DataModel.GetSetAttributeValue.AttributeValue newAttributeValue = new DataModel.GetSetAttributeValue.AttributeValue("v1.0:orion:Date of Birth");

            try
            {
                //This should throw an error for > max value
                newAttributeValue.SetFieldValue("DateOfBirth", "2018-01-02");
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex.Message.Contains(expectedException));
            }
        }

        [TestMethod]
        public void SetAttributeValue_ErrorRegExValue()
        {
            string expectedException = "Invalid Set Field Value";
            DataModel.GetSetAttributeValue.AttributeValue newAttributeValue = new DataModel.GetSetAttributeValue.AttributeValue("v1.0:orion:Profile Name");

            try
            {
                //This should throw an error for regex evaluation failure
                newAttributeValue.SetFieldValue("Country", "USofA");
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex.Message.Contains(expectedException));
            }
        }

        [TestMethod]
        public void SetAttributeValue_ErrorMultiIntoSingleValue()
        {
            string expectedException = "Field being set is designated SingleValue not accepting a Key.";

            DataModel.GetSetAttributeValue.AttributeValue newAttributeValue =
                new DataModel.GetSetAttributeValue.AttributeValue("v1.0:orion:Date of Birth");

            try
            {
                //This should throw an error as key is passed to a !MultipleValues AttributeValue
                newAttributeValue.SetFieldValue("DateOfBirth", "1970-01-01", "RandomKeyValue");
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex.Message.Contains(expectedException));
            }
        }

        [TestMethod]
        public void SetAttributeValue_ErrorRequiredFieldMissing()
        {
            string expectedException = "Submission Validation Failed";

            DataModel.GetSetAttributeValue.AttributeValue newAttributeValue =
                new DataModel.GetSetAttributeValue.AttributeValue("v1.0:orion:Device Token");

            try
            {
                //This should throw an error as reuired field is not set
                newAttributeValue.ValidateForSubmit();
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex.Message.Contains(expectedException));
            }
        }

        [TestMethod]
        public void SetAttributeValue_RequiredFieldOnSubmit()
        {
            string expectedException = "Submission Validation Failed";

            DataModel.GetSetAttributeValue.AttributeValue newAttributeValue =
                new DataModel.GetSetAttributeValue.AttributeValue("v1.0:orion:Date of Birth");

            Assert.IsTrue(newAttributeValue.ValidateForSubmit());
        }

        [TestMethod]
        public void SetAttributeValue_OneAttributeOverride()
        {
            DataModel.GetSetAttributeValue.AttributeValue newAttributeValue = new DataModel.GetSetAttributeValue.AttributeValue("v1.0:orion:Date of Birth");
            newAttributeValue.Visibility = BabelFish.Helpers.VisibilityOption.PROTECTED;
            newAttributeValue.SetFieldValue("DateOfBirth", "1970-01-01");

            DataModel.GetSetAttributeValue.AttributeValueList newAttributeValueList = new DataModel.GetSetAttributeValue.AttributeValueList();
            newAttributeValueList.Attributes.Add(newAttributeValue);
            var response = _client.SetAttributeValueAsync(newAttributeValueList);

            Assert.IsNotNull(response);

            var taskResult = response.Result;
            var objResponse = taskResult.Value;
            var msgResponse = taskResult.MessageResponse;

            Assert.IsNotNull(objResponse);
            Assert.IsNotNull(msgResponse);

            foreach ( var checkResponseName in objResponse.SetAttributeValues)
                Assert.AreEqual(checkResponseName.StatusCode, "200");
        }

        [TestMethod]
        public void SetAttributeValue_TwoAttributesOverride()
        {
            DataModel.GetSetAttributeValue.AttributeValue newAttributeValue = new DataModel.GetSetAttributeValue.AttributeValue("v1.0:orion:Date of Birth");
            newAttributeValue.Visibility = BabelFish.Helpers.VisibilityOption.PROTECTED;
            newAttributeValue.SetFieldValue("DateOfBirth", "1970-01-01"); //orig=1980-03-12

            DataModel.GetSetAttributeValue.AttributeValue newAttributeValue2 = new DataModel.GetSetAttributeValue.AttributeValue("v1.0:orion:Device Token");
            newAttributeValue2.Visibility = BabelFish.Helpers.VisibilityOption.PRIVATE;
            newAttributeValue2.Action = BabelFish.Helpers.AttributeValueActionEnums.UPDATE;
            newAttributeValue2.Visibility = BabelFish.Helpers.VisibilityOption.PRIVATE;
            newAttributeValue2.SetFieldValue("deviceToken", "abcd", "abcd");
            newAttributeValue2.SetFieldValue("DeviceType", "ios", "abcd");
            newAttributeValue2.SetFieldValue("DeviceName", "Erik's ios device", "abcd");
            newAttributeValue2.SetFieldValue("LastLoginDate", "2021-08-10", "abcd");
            newAttributeValue2.SetFieldValue("StageScoresUnofficial", true, "abcd");
            newAttributeValue2.SetFieldValue("EventScoresUnofficial", false, "abcd");
            newAttributeValue2.SetFieldValue("EventScoresOfficial", true, "abcd");


            DataModel.GetSetAttributeValue.AttributeValueList newAttributeValueList = new DataModel.GetSetAttributeValue.AttributeValueList();
            newAttributeValueList.Attributes.Add(newAttributeValue);
            newAttributeValueList.Attributes.Add(newAttributeValue2);
            var response = _client.SetAttributeValueAsync(newAttributeValueList);

            Assert.IsNotNull(response);

            var taskResult = response.Result;
            var objResponse = taskResult.Value;
            var msgResponse = taskResult.MessageResponse;

            Assert.IsNotNull(objResponse);
            Assert.IsNotNull(msgResponse);

            foreach (var checkResponseName in objResponse.SetAttributeValues)
                Assert.AreEqual(checkResponseName.StatusCode, "200");
        }

        [TestMethod]
        public void SetAttributeValue_SetAMultiValueAttribute()
        {
            DataModel.GetSetAttributeValue.AttributeValue newAttributeValue =
                new DataModel.GetSetAttributeValue.AttributeValue("v1.0:orion:Device Token");
            newAttributeValue.Visibility = BabelFish.Helpers.VisibilityOption.PRIVATE;
            newAttributeValue.SetFieldValue("DeviceType", "ios", "abcdalahgaihgaiohvahiaugvkuawygaiusgdfiauyfgesf");
            newAttributeValue.SetFieldValue("EventScoresUnofficial", false, "abcdalahgaihgaiohvahiaugvkuawygaiusgdfiauyfgesf");
            newAttributeValue.SetFieldValue("LastLoginDate", "2021-05-09", "abcdalahgaihgaiohvahiaugvkuawygaiusgdfiauyfgesf");
            newAttributeValue.SetFieldValue("StageScoresUnofficial", true, "abcdalahgaihgaiohvahiaugvkuawygaiusgdfiauyfgesf");
            newAttributeValue.SetFieldValue("EventScoresOfficial", true, "abcdalahgaihgaiohvahiaugvkuawygaiusgdfiauyfgesf");
            newAttributeValue.SetFieldValue("AthenaAtHome", true, "abcdalahgaihgaiohvahiaugvkuawygaiusgdfiauyfgesf");
            newAttributeValue.SetFieldValue("deviceToken", "abcdalahgaihgaiohvahiaugvkuawygaiusgdfiauyfgesf", "abcdalahgaihgaiohvahiaugvkuawygaiusgdfiauyfgesf");
            newAttributeValue.SetFieldValue("DeviceName", "A fake ios device", "abcdalahgaihgaiohvahiaugvkuawygaiusgdfiauyfgesf");
            newAttributeValue.SetFieldValue("DeviceType", "android", "aksuyveaikufgyaksghvausyrgfkasuvygasgauyrgvkauygvraku");
            newAttributeValue.SetFieldValue("EventScoresUnofficial", false, "aksuyveaikufgyaksghvausyrgfkasuvygasgauyrgvkauygvraku");
            newAttributeValue.SetFieldValue("LastLoginDate", "2021-07-19", "aksuyveaikufgyaksghvausyrgfkasuvygasgauyrgvkauygvraku");
            newAttributeValue.SetFieldValue("StageScoresUnofficial", true, "aksuyveaikufgyaksghvausyrgfkasuvygasgauyrgvkauygvraku");
            newAttributeValue.SetFieldValue("EventScoresOfficial", true, "aksuyveaikufgyaksghvausyrgfkasuvygasgauyrgvkauygvraku");
            newAttributeValue.SetFieldValue("AthenaAtHome", true, "aksuyveaikufgyaksghvausyrgfkasuvygasgauyrgvkauygvraku");
            newAttributeValue.SetFieldValue("deviceToken", "aksuyveaikufgyaksghvausyrgfkasuvygasgauyrgvkauygvraku", "aksuyveaikufgyaksghvausyrgfkasuvygasgauyrgvkauygvraku");
            newAttributeValue.SetFieldValue("DeviceName", "Erik's android device", "aksuyveaikufgyaksghvausyrgfkasuvygasgauyrgvkauygvraku");

            DataModel.GetSetAttributeValue.AttributeValueList newAttributeValueList = new DataModel.GetSetAttributeValue.AttributeValueList();
            newAttributeValueList.Attributes.Add(newAttributeValue);
            var response = _client.SetAttributeValueAsync(newAttributeValue);

            Assert.IsNotNull(response);

            var taskResult = response.Result;
            var objResponse = taskResult.Value;
            var msgResponse = taskResult.MessageResponse;

            Assert.IsNotNull(objResponse);
            Assert.IsNotNull(msgResponse);

            foreach (var checkResponseName in objResponse.SetAttributeValues)
                Assert.AreEqual(checkResponseName.StatusCode, "200");
        }

        [TestMethod]
        public void SetAttributeValue_DeleteAttributeValue()
        {
            DataModel.GetSetAttributeValue.AttributeValue newAttributeValue =
                new DataModel.GetSetAttributeValue.AttributeValue("v1.0:orion:Device Token");
            newAttributeValue.Visibility = BabelFish.Helpers.VisibilityOption.PRIVATE;
            newAttributeValue.Action = BabelFish.Helpers.AttributeValueActionEnums.DELETE;
            newAttributeValue.SetFieldValue("DeviceType", "ios", "abcdalahgaihgaiohvahiaugvkuawygaiusgdfiauyfgesf");
            newAttributeValue.SetFieldValue("EventScoresUnofficial", false, "abcdalahgaihgaiohvahiaugvkuawygaiusgdfiauyfgesf");
            newAttributeValue.SetFieldValue("LastLoginDate", "2021-05-09", "abcdalahgaihgaiohvahiaugvkuawygaiusgdfiauyfgesf");
            newAttributeValue.SetFieldValue("StageScoresUnofficial", true, "abcdalahgaihgaiohvahiaugvkuawygaiusgdfiauyfgesf");
            newAttributeValue.SetFieldValue("EventScoresOfficial", true, "abcdalahgaihgaiohvahiaugvkuawygaiusgdfiauyfgesf");
            newAttributeValue.SetFieldValue("AthenaAtHome", true, "abcdalahgaihgaiohvahiaugvkuawygaiusgdfiauyfgesf");
            newAttributeValue.SetFieldValue("deviceToken", "abcdalahgaihgaiohvahiaugvkuawygaiusgdfiauyfgesf", "abcdalahgaihgaiohvahiaugvkuawygaiusgdfiauyfgesf");
            newAttributeValue.SetFieldValue("DeviceName", "A fake ios device", "abcdalahgaihgaiohvahiaugvkuawygaiusgdfiauyfgesf");

            DataModel.GetSetAttributeValue.AttributeValueList newAttributeValueList = new DataModel.GetSetAttributeValue.AttributeValueList();
            newAttributeValueList.Attributes.Add(newAttributeValue);
            var response = _client.SetAttributeValueAsync(newAttributeValue);

            Assert.IsNotNull(response);

            var taskResult = response.Result;
            var objResponse = taskResult.Value;
            var msgResponse = taskResult.MessageResponse;

            Assert.IsNotNull(objResponse);
            Assert.IsNotNull(msgResponse);

            foreach (var checkResponseName in objResponse.SetAttributeValues)
                Assert.AreEqual(checkResponseName.StatusCode, "200");
        }

        [TestMethod]
        public void GetThenSetAttributeValue() {

            var dateTimeString = DateTime.UtcNow.ToString( DateTimeFormats.DATETIME_FORMAT );
            var dateString = DateTime.UtcNow.ToString(DateTimeFormats.DATE_FORMAT);

            List<string> myAttributes = new List<string>()
            {
                "v1.0:orion:Test Attribute",
            };
            var initGetResponse = _client.GetAttributeValueAsync( myAttributes );
            Assert.IsNotNull( initGetResponse );

            var attrValueList = initGetResponse.Result.AttributeValues;
            Assert.IsTrue( attrValueList.Count() == 1 );

            var attrValue = attrValueList[0];

            //Learn the current values
            var currentStringValue = attrValue.GetFieldValue( "AString" ).ToString();
            var currentStringAsIntValue = int.Parse(currentStringValue);
            var currentIntegerValue = Convert.ToInt32(attrValue.GetFieldValue( "AnInteger" ));
            var currentFloatValue = float.Parse(attrValue.GetFieldValue( "AFloat" ).ToString());
            var currentBoolean = bool.Parse(attrValue.GetFieldValue("ABoolean").ToString());
            var currentListStrings = attrValue.GetFieldValue( "AListOfStrings" ).ToString();
            var currentDateTime = attrValue.GetFieldValue( "ADateTime" );
            var currentDate = attrValue.GetFieldValue("ADate");

            //Increment the values
            var newStringAsIntValue = currentStringAsIntValue + 1;
            var newStringValue = newStringAsIntValue.ToString();
            var newIntegerValue = currentIntegerValue + 1;
            var newFloatValue = currentFloatValue + 1;
            var newBooleanValue = (currentBoolean) ? false : true;
            var newListStringsValue = new string[] { "1", "2" };
            var newDateTimeValue = dateTimeString;
            var newDateValue = dateString;

            //Set them on the attribute value
            attrValue.SetFieldValue( "AString", newStringValue );
            attrValue.SetFieldValue( "AnInteger", newIntegerValue );
            //float val: attrValue.SetFieldValue( "AFloat", newFloatValue);
            attrValue.SetFieldValue( "ABoolean", newBooleanValue);
            attrValue.SetFieldValue( "AListOfStrings", newListStringsValue);
            attrValue.SetFieldValue( "ADateTime", newDateTimeValue);
            attrValue.SetFieldValue( "ADate", newDateValue);

            //Send the Set request to the API
            var setResponse = _client.SetAttributeValueAsync( attrValue );

            //Check that it got submitted OK.
            var setAttrValueResponse = setResponse.Result.Value.SetAttributeValues[0];
            Assert.AreEqual( setAttrValueResponse.StatusCode, "200" );

            //Do a second get, to see if the returned values are what we just set them to.
            var secondGetResponse = _client.GetAttributeValueAsync( myAttributes );
            Assert.IsNotNull( secondGetResponse );

            var secondAttrValueList = secondGetResponse.Result.AttributeValues;
            Assert.IsTrue( secondAttrValueList.Count() == 1 );

            var secondAttrValue = secondAttrValueList[0];

            //Learn the current values
            var retreivedStringValue = secondAttrValue.GetFieldValue( "AString" ).ToString();
            var retreivedStringAsIntValue = int.Parse( retreivedStringValue );
            var retreivedIntegerValue = Convert.ToInt32(secondAttrValue.GetFieldValue( "AnInteger" ));
            var retreivedFloatValue = float.Parse(secondAttrValue.GetFieldValue("AFloat").ToString());
            var retreivedBoolean = bool.Parse(secondAttrValue.GetFieldValue("ABoolean").ToString());
            var retreivedListStrings = secondAttrValue.GetFieldValue("AListOfStrings").ToString();
            var retreivedDateTime = secondAttrValue.GetFieldValue("ADateTime");
            var retreivedDate = secondAttrValue.GetFieldValue("ADate");

            //Check that the set values equal the get values
            Assert.AreEqual( retreivedStringValue, newStringValue );
            Assert.AreEqual( retreivedIntegerValue, newIntegerValue );
            //float val: Assert.AreEqual(retreivedFloatValue, newFloatValue);
            Assert.AreEqual(retreivedBoolean, newBooleanValue);
            //list array: Assert.AreEqual(retreivedListStrings, newListStringsValue);
            //formatting: Assert.AreEqual(retreivedDateTime, newDateTimeValue);
            Assert.AreEqual(retreivedDate.ToString(), newDateValue);
        }

        [TestMethod]
        public void SetAttributeValue_DefaultValuesAdded()
        {
            DataModel.GetSetAttributeValue.AttributeValue newAttributeValue =
                new DataModel.GetSetAttributeValue.AttributeValue("v1.0:orion:Date of Birth");

            Assert.IsTrue(newAttributeValue.GetFieldValue("DateOfBirth") != string.Empty);

            //Currently don't see any with Default="value" && Required=true for the test below
            //DataModel.GetSetAttributeValue.AttributeValue newAttributeValue2 =
            //    new DataModel.GetSetAttributeValue.AttributeValue("v1.0:orion:Email Address");
            //newAttributeValue2.AddFieldKey("email@mail.com");
            //Assert.IsTrue(newAttributeValue2.GetFieldValue("DeviceType", "abcd") != string.Empty);

        }
    }
}
