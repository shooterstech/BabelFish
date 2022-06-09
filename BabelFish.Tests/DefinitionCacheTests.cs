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
    public class DefinitionCacheTests
    {

        private static string xApiKey = "wjM7eCb75aa3Okxj4FliXLY0VjHidoE2ei18pdg1";

        // DefinitionCacheHelper defaults to Orion dir if exists, then fails over to user's temp dir
        private static string orionDirectory = $"{Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)}\\Documents\\My Matches\\DEFINITIONS";
        private static string defaultDirTemp = $"{Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)}\\appdata\\local\\temp";

        private static string mockDirFile = $"{defaultDirTemp}\\DEFINITIONS\\v1.0 orion Date of Birth.json";
        private static string mockData = "{\"Title\":\"\",\"Message\":[],\"ResponseCodes\":[],\"v1.0:orion:Date of Birth\":{\"Discontinued\":false,\"MaxVisibility\":\"PROTECTED\",\"MultipleValues\":false,\"Fields\":[{\"Validation\":{\"MinValue\":\"1900-01-01\",\"MaxValue\":\"2018-01-01\",\"ValidationErrorList\":[],\"ErrorMessage\":\"DateOfBirthisoutsidetheallowablerange.\"},\"DefaultValue\":\"2018-01-01\",\"Required\":true,\"ValueType\":\"DATE\",\"DisplayName\":\"Date of Birth\",\"MultipleValues\":false,\"FieldName\":\"DateOfBirth\",\"FieldType\":\"OPEN\"}],\"Description\":\"A users date of birth\",\"DisplayName\":\"ProfileName\",\"SetName\":\"v1.0:orion:Date of Birth\",\"Type\":\"ATTRIBUTE\",\"HierarchicalName\":\"orion:Date of Birth\",\"Owner\":\"OrionAcct000001\",\"Version\":\"1.2\",\"Designation\":[\"ATHLETE\",\"CLUB\",\"MATCHOFFICIAL\",\"TEAM\",\"TEAMOFFICIAL\",\"USER\",\"USER\",\"ATHLETE\",\"HIDDEN\"]}}";

        [TestMethod]
        public void NoCurrentParametersSet()
        {
                DefinitionCacheHelper definitionCacheHelperNull = new DefinitionCacheHelper();
                Assert.IsNull(definitionCacheHelperNull.CurrentSetName);
                Assert.IsNull(definitionCacheHelperNull.CurrentType);
        }

        [TestMethod]
        public void DefaultDirectoryCalculatedSet()
        {
            DefinitionCacheHelper definitionCacheHelperNull = new DefinitionCacheHelper();
            Assert.IsTrue(definitionCacheHelperNull.DefaultDirectory != string.Empty);
        }

        [TestMethod]
        public void DefinitionCacheAgeValueRetrieved()
        {
            System.IO.File.WriteAllText(mockDirFile, mockData);
            System.IO.File.SetLastWriteTime(mockDirFile, DateTime.Now.AddDays(-5));
            Assert.IsTrue(System.IO.File.Exists(mockDirFile));

            Dictionary<string, string> clientParams = new Dictionary<string, string>()
            {
                {"Definitions_CacheIgnore", "false"},
                {"Definitions_CacheStorageDirctory", defaultDirTemp },
            };
            DefinitionAPIClient _client = new DefinitionAPIClient(xApiKey, clientParams);

            DefinitionCacheHelper definitionCacheHelperDays = new DefinitionCacheHelper();
            Assert.AreEqual(definitionCacheHelperDays.DefinitionCacheAgeInDays(Definition.DefinitionType.ATTRIBUTE, SetName.Parse("v1.0:orion:Date of Birth")),5);

            System.IO.File.Delete(mockDirFile);
        }

        [TestMethod]
        public void ReturnSpecifiedDefinitionFromFileCache()
        {
            System.IO.File.WriteAllText(mockDirFile, mockData);
            Assert.IsTrue(System.IO.File.Exists(mockDirFile));

            Dictionary<string, string> clientParams = new Dictionary<string, string>()
            {
                {"Definitions_CacheIgnore", "false"},
                {"Definitions_CacheStorageDirctory", defaultDirTemp },
            };
            DefinitionAPIClient _client = new DefinitionAPIClient(xApiKey, clientParams);

            var setName = SetName.Parse("v1.0:orion:Date of Birth");
            string filename = setName.FileName;
            string heirarchyName = setName.ToHierarchicalNameString();
            string fullname = setName.ToString();
            var response = _client.GetAttributeDefinitionAsync(setName);
            Assert.IsNotNull(response);

            var taskResult = response.Result;
            var objResponse = taskResult.Definition;
            var msgResponse = taskResult.MessageResponse;

            Assert.IsNotNull(objResponse);
            Assert.IsNotNull(msgResponse);

            Assert.AreEqual(objResponse.SetName, setName.ToString());
            Assert.AreEqual(objResponse.Type, taskResult.DefinitionType);
            Assert.AreEqual(objResponse.Fields.Count, 1);
            Assert.AreEqual(objResponse.Fields[0].FieldName, "DateOfBirth");

            System.IO.File.Delete(mockDirFile);
        }

        [TestMethod]
        public void ConfirmDefaultDefinitionCacheOrionDirectory()
        {
            // Test assumes existing Orion user with My Matches dir created
            Assert.IsTrue(System.IO.Directory.Exists(orionDirectory));

            Dictionary<string, string> clientParams2 = new Dictionary<string, string>()
            {
                { "Definitions_CacheStorageDirctory", "" },
            };
            DefinitionAPIClient _clienttest = new DefinitionAPIClient(xApiKey, clientParams2);
            DefinitionCacheHelper testCache = new DefinitionCacheHelper();
            Assert.AreEqual(testCache.DefaultDirectory, orionDirectory);
        }

        //[TestMethod]
        public void ConfirmDefaultDefinitionCacheProfileDirectory()
        {
            // Test assumes no Orion user directory so script will bypass; Change dir name to run
            Assert.IsFalse(System.IO.Directory.Exists(orionDirectory));
            Dictionary<string, string> clientParams2 = new Dictionary<string, string>()
            {
                { "Definitions_CacheStorageDirctory", "" },
            };
            DefinitionAPIClient _clienttest = new DefinitionAPIClient(xApiKey, clientParams2);
            DefinitionCacheHelper testCache = new DefinitionCacheHelper();
            Assert.AreEqual(testCache.DefaultDirectory, $"{System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location)}\\DEFINITIONS");
        }

        [TestMethod]
        public void DefinitionSavedToDefaultCacheDirectory()
        {
            Dictionary<string, string> clientParams = new Dictionary<string, string>()
            {
                {"UserName", "test_dev_7@shooterstech.net"},
                {"PassWord", "abcd1234"},
                {"Definitions_CacheIgnore", "false"},
                {"Definitions_CacheExpireTime", "10"},
                {"Definitions_CacheStorageDirctory", defaultDirTemp },
            };
            DefinitionAPIClient _client = new DefinitionAPIClient(xApiKey, clientParams);

            var setName = SetName.Parse("v1.0:orion:Profile Name");
            var response = _client.GetAttributeDefinitionAsync(setName);
            Assert.IsNotNull(response);

            var taskResult = response.Result;
            var objResponse = taskResult.Definition;
            var msgResponse = taskResult.MessageResponse;

            Assert.IsNotNull(objResponse);
            Assert.IsNotNull(msgResponse);

            Assert.AreEqual(objResponse.SetName, setName.ToString());
            Assert.AreEqual(objResponse.Type, taskResult.DefinitionType);
            Assert.IsTrue(objResponse.Fields.Count > 0);

            //System.IO.File.Delete($"{defaultDirTemp}\\DEFINITIONS\\{filename}");
        }

        [TestMethod]
        public void CourseOfFireTypeCache()
        {
            var setName = SetName.Parse("v2.0:ntparc:Three-Position Air Rifle 3x10");
            DefinitionAPIClient _client = new DefinitionAPIClient(xApiKey);

            var response = _client.GetCourseOfFireDefinition(setName);
            Assert.IsNotNull(response);

            var taskResult = response.Result;
            var objResponse = taskResult.Definition;
            var msgResponse = taskResult.MessageResponse;

            Assert.IsNotNull(objResponse);
            Assert.IsNotNull(msgResponse);

            Assert.AreEqual(objResponse.SetName, setName.ToString());
            Assert.AreEqual(objResponse.Type, taskResult.DefinitionType);
            Assert.IsTrue(objResponse.RangeScripts.Count >= 1);
            Assert.AreEqual(objResponse.Description, setName.ProperName);
        }

        [TestMethod]
        public void CompareTimeToRun()
        {
            // Run memory pull first just to prove the point
            System.IO.File.WriteAllText(mockDirFile, mockData);
            Assert.IsTrue(System.IO.File.Exists(mockDirFile));

            Dictionary<string, string> clientParams = new Dictionary<string, string>()
            {
                {"Definitions_CacheIgnore", "false"},
                {"Definitions_CacheStorageDirctory", defaultDirTemp },
            };
            DefinitionAPIClient _client = new DefinitionAPIClient(xApiKey, clientParams);

            var setName = SetName.Parse("v1.0:orion:Date of Birth");
            var response = _client.GetAttributeDefinitionAsync(setName);
            Assert.IsNotNull(response);
            var taskResult = response.Result;
            var runTime1 = taskResult.TimeToRun;

            System.IO.File.Delete(mockDirFile);

            // Run server pull second
            DefinitionAPIClient _client2 = new DefinitionAPIClient(xApiKey, clientParams);
            var setName2 = SetName.Parse("v1.0:orion:Date of Birth");
            var response2 = _client2.GetAttributeDefinitionAsync(setName2);
            Assert.IsNotNull(response2);
            var taskResult2 = response2.Result;
            var runTime2 = taskResult2.TimeToRun;

            Assert.IsTrue(runTime2 > runTime1);
        }

    }
}
