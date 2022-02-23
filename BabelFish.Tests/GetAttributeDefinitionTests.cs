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
    public class GetAttributeDefinitionTests {

        [TestMethod]
        public void GetAttributeAirRifleType() {
            string xApiKey = "wjM7eCb75aa3Okxj4FliXLY0VjHidoE2ei18pdg1";

            var setName = SetName.Parse("v1.0:ntparc:Three-Position Air Rifle Type");

            var client = new DefinitionAPIClient(xApiKey);

            var response = client.GetAttributeDefinitionAsync(setName);
            Assert.IsNotNull(response);

            var taskResult = response.Result;
            var attribute = taskResult.Definition;

            Assert.AreEqual(attribute.SetName, setName.ToString());
            Assert.AreEqual(attribute.Type, taskResult.DefinitionType);
            Assert.AreEqual(attribute.Fields.Count, 1);
            Assert.AreEqual(attribute.Fields[0].FieldName, "Three-Position Air Rifle Type");
        }
    }
}
