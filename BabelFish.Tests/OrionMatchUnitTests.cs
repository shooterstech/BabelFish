using Microsoft.VisualStudio.TestTools.UnitTesting;
using BabelFish;

namespace BabelFish.Tests
{
    [TestClass]
    public class OrionMatchUnitTests
    {

        [TestMethod]
        public void OrionMatchAPI_Assigns_XApiKey()
        {
            string TestXApiKey = "mock_api_key_value";
            OrionMatchAPIClient orionmatchclient = new OrionMatchAPIClient(TestXApiKey);
            Assert.IsTrue(orionmatchclient.XApiKey.Length > 0);
        }

        [TestMethod]
        public void OrionMatchAPI_GetAMatch() {
            string xApiKey = "wjM7eCb75aa3Okxj4FliXLY0VjHidoE2ei18pdg1";
            var client = new OrionMatchAPIClient(xApiKey);

            var response = client.GetMatchDetailAsync("1.2268.2022021516475240.0");
            Assert.IsNotNull(response);

            var match = response.Result.Match;
            var matchName = match.Name;

            Assert.IsNotNull(matchName);
            Assert.AreNotEqual(matchName, "");

        }
    }
}