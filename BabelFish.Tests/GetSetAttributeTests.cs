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
    public class GetSetAttributeTests
    {
        private static string xApiKey = "wjM7eCb75aa3Okxj4FliXLY0VjHidoE2ei18pdg1";
        private static Dictionary<string, string> clientParams = new Dictionary<string, string>()
        {
            {"UserName", "test_dev_9@shooterstech.net"},
            {"PasssWord", "abcd1234"},
        };
        private readonly GetSetAttributeAPIClient _client = new GetSetAttributeAPIClient(xApiKey, clientParams);

        [TestMethod]
        public void GetSingleAttribute() {

            List<string> MyAttributes = new List<string>()
            {
                "v1.0:orion:Profile Name",
            };
            var response = _client.GetAttributeValueAsync(MyAttributes);
            Assert.IsNotNull(response);

            var taskResult = response.Result;
            var objResponse = taskResult.AttributeValues;
            var msgResponse = taskResult.MessageResponse;

            Assert.IsNotNull(objResponse);
            Assert.IsNotNull(msgResponse);

//            Assert.AreEqual(objResponse.FirstOrDefault().Name, MyAttributes[0]);
            //Assert.AreEqual(objResponse.Type, taskResult.DefinitionType);
            //Assert.AreEqual(objResponse.Fields.Count, 1);
            //Assert.AreEqual(objResponse.Fields[0].FieldName, "Three-Position Air Rifle Type");
        }

        //[TestMethod]
        public void GetMultipleAttribute()
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
            var msgResponse = taskResult.MessageResponse;

            Assert.IsNotNull(objResponse);
            Assert.IsNotNull(msgResponse);

            //Assert.AreEqual(objResponse.SetName, setName.ToString());
            //Assert.AreEqual(objResponse.Type, taskResult.DefinitionType);
            //Assert.IsTrue(objResponse.RangeScripts.Count>=1);
            //Assert.AreEqual(objResponse.Description, setName.ProperName);
        }

        //[TestMethod]
        public void GetNotFoundAttribute()
        {
            List<string> MyAttributes = new List<string>()
            {
                "v1.0:orion:Collegiate Class",
            };

            var response = _client.GetAttributeValueAsync(MyAttributes);
            Assert.IsNotNull(response);

            var taskResult = response.Result;
            var objResponse = taskResult.AttributeValues;
            var msgResponse = taskResult.MessageResponse;

            Assert.IsNotNull(objResponse);
            Assert.IsNotNull(msgResponse);

            //Assert.AreEqual(objResponse.SetName, setName.ToString());
            //Assert.AreEqual(objResponse.Type, taskResult.DefinitionType);
            //Assert.IsTrue(objResponse.RangeScripts.Count >= 1);
            //Assert.AreEqual(objResponse.Description, setName.ProperName);
        }
    }
}
