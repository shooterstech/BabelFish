using Microsoft.VisualStudio.TestTools.UnitTesting;

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
    }
}