using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BabelFish;
using BabelFish.Helpers;
using BabelFish.DataModel.Definitions;

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
        public void GetSingleAttributeValue() {

            List<string> MyAttributes = new List<string>()
            {
                //"v1.0:orion:Profile Name",
                "v1.0:orion:Device Token",
            };
            var response = _client.GetAttributeValueAsync(MyAttributes);
            Assert.IsNotNull(response);

            var taskResult = response.Result;
            var objResponse = taskResult.AttributeValues;
            Assert.IsTrue(objResponse.Count() == 1);
            var msgResponse = taskResult.MessageResponse;
            Assert.IsNotNull(msgResponse);
            Assert.IsTrue(msgResponse.Message.Count == 0);
        }

        [TestMethod]
        public void GetMultipleAttributeValues()
        {
            List<string> MyAttributes = new List<string>()
            {
                "v1.0:orion:Profile Name",
                "v1.0:orion:Date of Birth",
            };

            var response = _client.GetAttributeValueAsync(MyAttributes);
            Assert.IsNotNull(response);

            var taskResult = response.Result;
            var objResponse = taskResult.AttributeValues;
            Assert.IsTrue(objResponse.Count() == 2);
            foreach ( var checkName in objResponse.ToArray())
                Assert.IsTrue(MyAttributes.Contains(checkName.SetName));
            var msgResponse = taskResult.MessageResponse;
            Assert.IsNotNull(msgResponse);
            Assert.IsTrue(msgResponse.Message.Count == 0);
        }

        [TestMethod]
        public void GetNotFoundAttributeValue()
        {
            List<string> MyAttributes = new List<string>()
            {
                "v1.0:orion:Invalid SetName",
            };

            var response = _client.GetAttributeValueAsync(MyAttributes);
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
        public void GetValidateUserIDValid()
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
        public void GetValidateUserIDInValid()
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
        public void SetAttributeValueDateOfBirth()
        {
            DataModel.GetSetAttributeValue.AttributeValue NewAttributeValue = new DataModel.GetSetAttributeValue.AttributeValue("v1.0:orion:Date of Birth");
            NewAttributeValue.Visibility = BabelFish.Helpers.VisibilityOption.PROTECTED;
            NewAttributeValue.SetFieldName("DateOfBirth", "1970-01-01"); //orig=1980-03-12

            DataModel.GetSetAttributeValue.AttributeValueList NewAttributeValueList = new DataModel.GetSetAttributeValue.AttributeValueList();
            NewAttributeValueList.Attributes.Add(NewAttributeValue);
            var response = _client.SetAttributeValueAsync(NewAttributeValueList);
            //var response = _client.SetAttributeValueAsync(NewAttributeValue); //Alternatively can send in a single AttributeValue

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
        public void SetAttributeValueMultipleAttributes()
        {
            DataModel.GetSetAttributeValue.AttributeValue NewAttributeValue2 = new DataModel.GetSetAttributeValue.AttributeValue("v1.0:orion:Date of Birth");
            NewAttributeValue2.Visibility = BabelFish.Helpers.VisibilityOption.PROTECTED;
            NewAttributeValue2.SetFieldName("DateOfBirth", "1970-01-01"); //orig=1980-03-12

            DataModel.GetSetAttributeValue.AttributeValue NewAttributeValue =
                new DataModel.GetSetAttributeValue.AttributeValue("v1.0:orion:Device Token");
            NewAttributeValue.Visibility = BabelFish.Helpers.VisibilityOption.PRIVATE;
            NewAttributeValue.Action = BabelFish.Helpers.AttributeValueActionEnums.UPDATE;
            NewAttributeValue.SetFieldName("DeviceType", "ios");
            NewAttributeValue.SetFieldName("EventScoresUnofficial", false);
            NewAttributeValue.SetFieldName("LastLoginDate", "2021-05-09");
            NewAttributeValue.SetFieldName("StageScoresUnofficial", true);
            NewAttributeValue.SetFieldName("EventScoresOfficial", true);
            NewAttributeValue.SetFieldName("AthenaAtHome", true);
            NewAttributeValue.SetFieldName("deviceToken", "abcdalahgaihgaiohvahiaugvkuawygaiusgdfiauyfgesf");
            NewAttributeValue.SetFieldName("DeviceName", "A fake ios device");

            DataModel.GetSetAttributeValue.AttributeValueList NewAttributeValueList = new DataModel.GetSetAttributeValue.AttributeValueList();
            NewAttributeValueList.Attributes.Add(NewAttributeValue);
            NewAttributeValueList.Attributes.Add(NewAttributeValue2);
            var response = _client.SetAttributeValueAsync(NewAttributeValueList);

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
        public void SetAttributeValueMultipleAttributeValues()
        {
            DataModel.GetSetAttributeValue.AttributeValue NewAttributeValue =
                new DataModel.GetSetAttributeValue.AttributeValue("v1.0:orion:Device Token");
            NewAttributeValue.Visibility = BabelFish.Helpers.VisibilityOption.PRIVATE;
            NewAttributeValue.SetFieldName("DeviceType", "ios", "abcdalahgaihgaiohvahiaugvkuawygaiusgdfiauyfgesf");
            NewAttributeValue.SetFieldName("EventScoresUnofficial", false, "abcdalahgaihgaiohvahiaugvkuawygaiusgdfiauyfgesf");
            NewAttributeValue.SetFieldName("LastLoginDate", "2021-05-09", "abcdalahgaihgaiohvahiaugvkuawygaiusgdfiauyfgesf");
            NewAttributeValue.SetFieldName("StageScoresUnofficial", true, "abcdalahgaihgaiohvahiaugvkuawygaiusgdfiauyfgesf");
            NewAttributeValue.SetFieldName("EventScoresOfficial", true, "abcdalahgaihgaiohvahiaugvkuawygaiusgdfiauyfgesf");
            NewAttributeValue.SetFieldName("AthenaAtHome", true, "abcdalahgaihgaiohvahiaugvkuawygaiusgdfiauyfgesf");
            NewAttributeValue.SetFieldName("deviceToken", "abcdalahgaihgaiohvahiaugvkuawygaiusgdfiauyfgesf", "abcdalahgaihgaiohvahiaugvkuawygaiusgdfiauyfgesf");
            NewAttributeValue.SetFieldName("DeviceName", "A fake ios device", "abcdalahgaihgaiohvahiaugvkuawygaiusgdfiauyfgesf");
            NewAttributeValue.SetFieldName("DeviceType", "android", "aksuyveaikufgyaksghvausyrgfkasuvygasgauyrgvkauygvraku");
            NewAttributeValue.SetFieldName("EventScoresUnofficial", false, "aksuyveaikufgyaksghvausyrgfkasuvygasgauyrgvkauygvraku");
            NewAttributeValue.SetFieldName("LastLoginDate", "2021-07-19", "aksuyveaikufgyaksghvausyrgfkasuvygasgauyrgvkauygvraku");
            NewAttributeValue.SetFieldName("StageScoresUnofficial", true, "aksuyveaikufgyaksghvausyrgfkasuvygasgauyrgvkauygvraku");
            NewAttributeValue.SetFieldName("EventScoresOfficial", true, "aksuyveaikufgyaksghvausyrgfkasuvygasgauyrgvkauygvraku");
            NewAttributeValue.SetFieldName("AthenaAtHome", true, "aksuyveaikufgyaksghvausyrgfkasuvygasgauyrgvkauygvraku");
            NewAttributeValue.SetFieldName("deviceToken", "aksuyveaikufgyaksghvausyrgfkasuvygasgauyrgvkauygvraku", "aksuyveaikufgyaksghvausyrgfkasuvygasgauyrgvkauygvraku");
            NewAttributeValue.SetFieldName("DeviceName", "Erik's android device", "aksuyveaikufgyaksghvausyrgfkasuvygasgauyrgvkauygvraku");

            DataModel.GetSetAttributeValue.AttributeValueList NewAttributeValueList = new DataModel.GetSetAttributeValue.AttributeValueList();
            NewAttributeValueList.Attributes.Add(NewAttributeValue);
            var response = _client.SetAttributeValueAsync(NewAttributeValue);

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
        public void SetAttributeValueDeleteAttributeValue()
        {
            DataModel.GetSetAttributeValue.AttributeValue NewAttributeValue =
                new DataModel.GetSetAttributeValue.AttributeValue("v1.0:orion:Device Token");
            NewAttributeValue.Visibility = BabelFish.Helpers.VisibilityOption.PRIVATE;
            NewAttributeValue.Action = BabelFish.Helpers.AttributeValueActionEnums.DELETE;
            NewAttributeValue.SetFieldName("DeviceType", "ios", "abcdalahgaihgaiohvahiaugvkuawygaiusgdfiauyfgesf");
            NewAttributeValue.SetFieldName("EventScoresUnofficial", false, "abcdalahgaihgaiohvahiaugvkuawygaiusgdfiauyfgesf");
            NewAttributeValue.SetFieldName("LastLoginDate", "2021-05-09", "abcdalahgaihgaiohvahiaugvkuawygaiusgdfiauyfgesf");
            NewAttributeValue.SetFieldName("StageScoresUnofficial", true, "abcdalahgaihgaiohvahiaugvkuawygaiusgdfiauyfgesf");
            NewAttributeValue.SetFieldName("EventScoresOfficial", true, "abcdalahgaihgaiohvahiaugvkuawygaiusgdfiauyfgesf");
            NewAttributeValue.SetFieldName("AthenaAtHome", true, "abcdalahgaihgaiohvahiaugvkuawygaiusgdfiauyfgesf");
            NewAttributeValue.SetFieldName("deviceToken", "abcdalahgaihgaiohvahiaugvkuawygaiusgdfiauyfgesf", "abcdalahgaihgaiohvahiaugvkuawygaiusgdfiauyfgesf");
            NewAttributeValue.SetFieldName("DeviceName", "A fake ios device", "abcdalahgaihgaiohvahiaugvkuawygaiusgdfiauyfgesf");

            DataModel.GetSetAttributeValue.AttributeValueList NewAttributeValueList = new DataModel.GetSetAttributeValue.AttributeValueList();
            NewAttributeValueList.Attributes.Add(NewAttributeValue);
            var response = _client.SetAttributeValueAsync(NewAttributeValue);

            Assert.IsNotNull(response);

            var taskResult = response.Result;
            var objResponse = taskResult.Value;
            var msgResponse = taskResult.MessageResponse;

            Assert.IsNotNull(objResponse);
            Assert.IsNotNull(msgResponse);

            foreach (var checkResponseName in objResponse.SetAttributeValues)
                Assert.AreEqual(checkResponseName.StatusCode, "200");
        }
    }
}
