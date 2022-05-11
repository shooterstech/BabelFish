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
                "v1.0:orion:Profile Name",
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
            string attributeName = "v1.0:orion:Date of Birth";
            DataModel.GetSetAttributeValue.AttributeValue NewAttributeValue = new DataModel.GetSetAttributeValue.AttributeValue(attributeName);
            NewAttributeValue.Visibility = BabelFish.Helpers.VisibilityOption.PROTECTED;
            NewAttributeValue.SetFieldName("DateOfBirth", "1970-01-01"); //orig=1980-03-12

            var response = _client.SetAttributeValueAsync(NewAttributeValue);
            Assert.IsNotNull(response);

            var taskResult = response.Result;
            var objResponse = taskResult.Value;
            var msgResponse = taskResult.MessageResponse;

            Assert.IsNotNull(objResponse);
            Assert.IsNotNull(msgResponse);

            Assert.AreEqual(objResponse.AttributeValue, attributeName);
            Assert.AreEqual(objResponse.StatusCode, "200");
        }
    }
}
